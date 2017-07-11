using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarsOrders.Models;
using CarsOrders.Models.BLL.Interfaces;
using Microsoft.Extensions.Options;
using CarsOrders.Models.BLL;

namespace CarsOrders.Controllers
{
    [Produces("application/json")]
    public class CarsController : BaseController
    {
        #region Constructors

        public CarsController(MainDBContext context, IOptions<WebKey> options)
            : base(context)
        {
            WebKey = options.Value;
        }

        #endregion

        #region Public Functions

        // GET: cars
        [HttpGet]
        [Route("cars")]
        public IEnumerable<CarWithIDVM> GetCars()
        {
            return dbContext.Set<Car>().ToList().Select(c => new CarWithIDVM(c));
        }

        // GET: cars/:id
        [HttpGet("{id}")]
        [Route("cars/:id")]
        public async Task<IActionResult> GetCar([FromRoute] Guid id)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var car = await dbContext.Set<Car>().SingleOrDefaultAsync(m => m.ID == id);

                if (car == null)
                    return NotFound();

                return Ok(car);
            });
        }


        // POST: cars/add/:key
        [HttpPost]
        [Route("cars/add/{key}")]
        public async Task<IActionResult> AddCar([FromRoute] string key, [FromBody] CarVM car)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!KeyValid(key))
                    return Unauthorized();

                var newCar = car.GetCar();
                dbContext.Set<Car>().Add(newCar);
                await dbContext.SaveChangesAsync();

                return CreatedAtAction("AddCar", new { id = newCar.ID }, new CarWithIDVM(newCar));
            });
        }

        // PUT: cars/edit/:id?key=
        [HttpPut("{id}")]
        [Route("cars/edit/{id}")]
        public async Task<IActionResult> PutCar([FromRoute] Guid id, string key, [FromBody] Car car)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != car.ID)
                    return BadRequest();

                dbContext.Entry(car).State = EntityState.Modified;

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(id))
                        return NotFound();
                    else
                        throw;
                }

                return NoContent();
            });
        }

        // GET: cars/delete/:id?key=
        [HttpGet("{id}")]
        [Route("cars/delete/{id}")]
        public async Task<IActionResult> DeleteCar(Guid id, string key)
        {
            return await HandleExceptionAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!KeyValid(key))
                    return Unauthorized();

                var car = await dbContext.Set<Car>().SingleOrDefaultAsync(m => m.ID == id);
                if (car == null)
                    return NotFound();

                dbContext.Set<Car>().Remove(car);
                await dbContext.SaveChangesAsync();

                return Ok(car);
            });
        }

        #endregion

        #region Internal Functions

        private bool CarExists(Guid id)
        {
            return dbContext.Set<Car>().Any(e => e.ID == id);
        } 

        #endregion
    }
}