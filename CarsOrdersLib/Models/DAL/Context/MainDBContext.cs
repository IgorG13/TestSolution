using CarsOrders.Models.Helpers;
using CarsOrders.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CarsOrders.Models
{
	public class MainDBContext : DbContext, ITestDataFiller
    {
        #region Members

        private static MainDBContext instance;

		#endregion

		#region Properties

		public static MainDBContext Instance => instance ?? (instance = new MainDBContext(new DbContextOptions<MainDBContext>()));

		#endregion

		#region Constructors

		public MainDBContext(DbContextOptions<MainDBContext> options)
			: base(options)
		{

		}

		#endregion

		#region Sets

		public DbSet<Car> Cars { get; set; }
		public DbSet<Order> Orders { get; set; }

        #endregion

        #region Virtual Functions

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Car)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CarID);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
            var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "local";
            optionsBuilder.UseSqlServer(new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(GetType().GetTypeInfo().Assembly.Location))
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: false)
                    .Build()
                    .GetConnectionString("CarsOrdersCS")
             );
        }

        #endregion

        #region Public Functions

        public void FillDBWithTestData()
        {
            var carsList = new List<Car> {
                new Car { Model = "VW Passat", Price = 15000, EngineCapacity = (decimal)2.2 }
                , new Car { Model = "Mersedes CLR", Price = 80000, EngineCapacity = (decimal)3.2 }
                , new Car { Model = "Chevrolet Camaro", Price = 300000, EngineCapacity = (decimal)6.0 }
                , new Car { Model = "Suzuki Vitara", Price = 20000, EngineCapacity = (decimal)2.4 }
                , new Car { Model = "Porsche Panamera", Price = 100000, EngineCapacity = (decimal)3.0 }
                , new Car { Model = "Kia Sportage", Price = 25000, EngineCapacity = (decimal)2.6 }
            };

            foreach (var car in carsList)
                Cars.Add(car);

            var ordersList = new List<Order> {
                new Order(carsList[1], "AA 345")
                , new Order(carsList[1], "AA 346") { OrderDate = new DateTime(2017, 5, 10, 15, 18, 0) }
                , new Order(carsList[1], "AA 347") { OrderDate = new DateTime(2017, 5, 10, 15, 17, 0) }
                , new Order(carsList[3], "AA 348") { OrderDate = new DateTime(2017, 5, 10, 15, 16, 0) }
                , new Order(carsList[4], "AA 349") { OrderDate = new DateTime(2017, 3, 10, 15, 18, 0) }
            };

            Orders.AddRange(ordersList);
            SaveChanges();
        }

        public void Delete<T>(Guid id) where T : BaseModel
        {
            var dbSet = Set<T>();
            var item = dbSet.Find(id);
            if (item == null)
                throw new KeyNotFoundException($"Entity of type {typeof(T)} by id {id} was not found!");
            dbSet.Remove(item);
            SaveChanges();
        }

        public Guid AddNew<T>(T obj) where T : BaseModel
        {
            Add(obj);
            SaveChanges();
            return obj.ID;
        }

        #endregion
    }
}
