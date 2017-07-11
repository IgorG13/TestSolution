using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CarsOrders.Models;

namespace CarsOrders.Migrations
{
    [DbContext(typeof(MainDBContext))]
    [Migration("20170704170650_SeedData")]
    partial class SeedData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarsOrders.Models.Car", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("EngineCapacity");

                    b.Property<string>("Model");

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.ToTable("cars");
                });

            modelBuilder.Entity("CarsOrders.Models.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CarID");

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("OrderNumber");

                    b.HasKey("ID");

                    b.HasIndex("CarID")
                        .IsUnique();

                    b.ToTable("orders");
                });

            modelBuilder.Entity("CarsOrders.Models.Order", b =>
                {
                    b.HasOne("CarsOrders.Models.Car", "Car")
                        .WithOne("Order")
                        .HasForeignKey("CarsOrders.Models.Order", "CarID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
