using System;

namespace CarsOrders.Models.BLL
{
    public class OrderVM
    {
        #region Properties

        public Guid ID { get; set; }

        public string OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public string CarInfo { get; set; }

        #endregion

        #region Constructors

        public OrderVM()
        {

        }

        public OrderVM(Order order)
        {
            ID = order.ID;
            OrderDate = order.OrderDateStr;
            OrderNumber = order.OrderNumber;
            CarInfo = order.CarInfo;
        }

        #endregion
    }
}
