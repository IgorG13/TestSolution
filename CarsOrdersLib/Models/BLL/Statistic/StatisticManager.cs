using System;
using CarsOrders.Models.Interfaces;
using System.Linq;
using CarsOrders.Models.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Text;

namespace CarsOrders.Models.Statistic
{
    public class StatisticManager : IStatisticManager
    {
        #region Properties

        private DbContext DbContext { get; set; }

        #endregion

        #region Constructors

        public StatisticManager(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Public Functions

        public string GetStatisticResult()
        {
            return GetStatisticResultAsync().GetAwaiter().GetResult();
        }

        public async Task<string> GetStatisticResultAsync()
        {
            var retVal = new StringBuilder();
            var conn = DbContext.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                retVal.Append(await GetMonthStatistic(conn));
                var popularModel = await GetMostPopular(conn);
                if (!string.IsNullOrWhiteSpace(popularModel))
                {
                    retVal.Append(Environment.NewLine);
                    retVal.Append(popularModel);
                }
            }
            finally
            {
                conn.Close();
            }
            return retVal.ToString();
        }

        public async Task<string> GetMonthStatistic(DbConnection conn)
        {
            var strSQL = $"SELECT CONCAT('Month: ', DATENAME(MONTH, DATEADD(MONTH, MONTH(t1.{Order.OrderDateField})-1, CAST('2017-01-01' AS DATETIME))), ' Quantity: '"
                    + $", COUNT(t1.{Order.IDField}), ' Sum: $', SUM(t2.Price)) as Result"
                + $" FROM {Helper.GetTableName<Order>()} t1"
                + $" LEFT JOIN {Helper.GetTableName<Car>()} t2 ON t1.{Order.CarIDField}=t2.{Car.IDField}"
                + $" WHERE YEAR(t1.{Order.OrderDateField}) = {DateTime.Now.Year} "
                + $" GROUP BY MONTH(t1.{Order.OrderDateField})"
                + $" ORDER BY MONTH(t1.{Order.OrderDateField})";
            return string.Join(Environment.NewLine, await SqlQueryAsync(conn, strSQL));
        }

        public async Task<string> GetMostPopular(DbConnection conn)
        {
            var strSQL = $"SELECT TOP 1 COUNT(t1.{Order.IDField}) as Count"
                + $" FROM {Helper.GetTableName<Order>()} t1"
                + $" LEFT JOIN {Helper.GetTableName<Car>()} t2 ON t1.{Order.CarIDField}=t2.{Car.IDField}"
                + $" GROUP BY t1.{Order.CarIDField}"
                + $" ORDER BY COUNT(t1.{Order.IDField}) DESC";
            var maxOrdersCountList = await SqlQueryAsync(conn, strSQL);
            if (maxOrdersCountList.Count == 0 || string.IsNullOrWhiteSpace(maxOrdersCountList[0]))
                return string.Empty;
            var maxOrdersCount = maxOrdersCountList[0];
            strSQL = $"SELECT t2.{Car.ModelField}"
                + $" FROM {Helper.GetTableName<Order>()} t1"
                + $" LEFT JOIN {Helper.GetTableName<Car>()} t2 ON t1.{Order.CarIDField}=t2.{Car.IDField}"
                + $" GROUP BY t2.{Car.ModelField}"
                + $" HAVING COUNT(t1.{Order.IDField})={maxOrdersCount}"
                + $" ORDER BY t2.{Car.ModelField}";
            return $"Most popular car is/are {string.Join(", ", await SqlQueryAsync(conn, strSQL))}";
        }

        public async Task<List<string>> SqlQueryAsync(DbConnection conn, string strSQL)
        {
            var retVal = new List<string>();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = strSQL;
                using (DbDataReader reader = await command.ExecuteReaderAsync())
                    if (reader.HasRows)
                        while (await reader.ReadAsync())
                        {
                            var val = reader.GetValue(0).ToString();
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                retVal.Add(val);
                            }
                        }
            }
            return retVal;
        }

        #endregion
    }
}
