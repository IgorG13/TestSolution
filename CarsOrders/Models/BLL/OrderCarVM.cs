using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsOrders.Models.BLL
{
    public class OrderCarVM
    {
        #region Properties

        public Guid ID { get; set; }

        public string Info { get; set; }

        #endregion

        #region Constructors

        public OrderCarVM(Car car)
        {
            ID = car.ID;
            Info = car.Info;
        }

        #endregion
    }
}
