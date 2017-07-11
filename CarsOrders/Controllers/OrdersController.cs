using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarsOrders.Models;
using CarsOrders.Models.Statistic;
using CarsOrders.Models.Interfaces;
using System.Net;
using CarsOrders.Models.BLL;

namespace CarsOrders.Controllers
{
    [Produces("application/json")]
    public class OrdersController : BaseController
    {
        #region Properties

        private IStatisticManager Statistic { get; set; }

        #endregion

        #region Constructors

        public OrdersController(MainDBContext context)
            : base(context)
        {
            Statistic = new StatisticManager(dbContext);
        }

        #endregion

        #region Public Functions

        // GET: orders
        [HttpGet]
        [Route("orders")]
        public IEnumerable<OrderVM> GetOrders()
        {
            return dbContext.Set<Order>().Include(o => o.Car).ToList().Select(o => new OrderVM(o));
        }

        // GET: orders/cars
        [HttpGet]
        [Route("orders/cars")]
        public IEnumerable<OrderCarVM> GetCarsForSale()
        {
            return dbContext.Set<Car>().OrderBy(c => c.Model).ThenBy(c => c.Price).ToList().Select(c => new OrderCarVM(c));
        }

        // GET: orders/:id
        [HttpGet("{id}")]
        [Route("orders")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var order = await dbContext.Set<Order>().SingleOrDefaultAsync(m => m.ID == id);

                if (order == null)
                    return NotFound();

                return Ok(order);
            });
        }

        // POST: orders/add
        [HttpPost]
        [Route("orders/add")]
        public async Task<IActionResult> AddOrder([FromBody] OrderAddVM order)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var newOrder = order.GetOrder();
                dbContext.Set<Order>().Add(newOrder);
                await dbContext.SaveChangesAsync();
                newOrder.Car = dbContext.Set<Car>().FirstOrDefault(c => c.ID == newOrder.CarID);
                if (newOrder.Car == null)
                    return NotFound("Car for order " + order.OrderNumber + " was not found");

                return CreatedAtAction("AddOrder", new { id = newOrder.ID }, new OrderVM(newOrder));
            });
        }

        // Get: orders/delete/:id
        [HttpGet("{id}")]
        [Route("orders/delete/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var order = await dbContext.Set<Order>().SingleOrDefaultAsync(m => m.ID == id);
                if (order == null)
                    return NotFound();

                dbContext.Set<Order>().Remove(order);
                await dbContext.SaveChangesAsync();

                return Ok(order);
            });
        }

        // Get: orders/statistic
        [HttpGet]
        [Route("orders/statistic")]
        public IActionResult GetStatistic()
        {
            return HandleException(() =>
            {
                var retVal = Statistic?.GetStatisticResult();
                if (string.IsNullOrWhiteSpace(retVal))
                    return StatusCode((int)HttpStatusCode.NotFound);
                else
                    return Json(retVal);
            });
        }

        #endregion

        #region Internal Functions

        private bool OrderExists(Guid id)
        {
            return dbContext.Set<Order>().Any(e => e.ID == id);
        } 

        #endregion
    }
}