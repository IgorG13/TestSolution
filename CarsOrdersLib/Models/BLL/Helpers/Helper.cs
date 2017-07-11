using System;
using System.Globalization;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsOrders.Models.Helpers
{
    public static class Helper
    {
        #region Public Functions

        public static string MonthName(int month)
        {
            if (month < 1 || month > 12)
                return string.Empty;
            return new DateTime(2010, month, 1).ToString("MMMM", CultureInfo.InvariantCulture);
        }

        public static string GetTableName<T>() where T : BaseModel
        {
            var type = typeof(T);
            var tableAttribute = type.GetTypeInfo().GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
                throw new ArgumentNullException($"Attribute 'Table' is missing in class '{type}'");
            return tableAttribute.Name;
        }

        #endregion
    }
}
