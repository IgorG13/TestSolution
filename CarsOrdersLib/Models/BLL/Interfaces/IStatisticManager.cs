using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsOrders.Models.Interfaces
{
    public interface IStatisticManager
    {
        string GetStatisticResult();

        Task<string> GetStatisticResultAsync();
    }
}
