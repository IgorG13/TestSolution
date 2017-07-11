using CarsOrders.Models.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CarsOrders.Controllers
{
    public abstract class BaseController : Controller
    {
        #region Members

        protected readonly DbContext dbContext;
        protected IWebKey WebKey { get; set; }

        #endregion

        #region Constructors

        public BaseController(DbContext context)
        {
            dbContext = context;
        }

        #endregion

        #region Internal Functions

        protected IActionResult HandleException(Func<IActionResult> exec)
        {
            try
            {
                return exec();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        protected async Task<IActionResult> HandleExceptionAsync(Func<Task<IActionResult>> exec)
        {
            try
            {
                return await exec();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        protected bool KeyValid(string key)
        {
            return WebKey.IsKeyValid(key);
        }

        #endregion
    }
}
