using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using CarsOrders.Models.Interfaces;
using CarsOrders.Models;

namespace CarsOrders.Migrations
{
    public partial class SeedData : Migration
    {
        ITestDataFiller testDataFiller = MainDBContext.Instance;

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            testDataFiller.FillDBWithTestData();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
