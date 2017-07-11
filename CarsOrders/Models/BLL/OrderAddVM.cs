using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsOrders.Models.BLL
{
    public class OrderAddVM
    {
        #region Properites

        public Guid CarID { get; set; }

        public string OrderDate { get; set; }

        public string OrderNumber { get; set; }

        #endregion

        #region Constructors

        public OrderAddVM()
        {

        }

        public OrderAddVM(Order o)
        {
            CarID = o.CarID;
            OrderDate = o.OrderDateStr;
            OrderNumber = o.OrderNumber;
        }

        #endregion

        #region Public Functions

        public Order GetOrder()
        {
            return new Order
            {
                CarID = CarID,
                OrderDateStr = OrderDate,
                OrderNumber = OrderNumber
            };
        }

        #endregion
    }
}
