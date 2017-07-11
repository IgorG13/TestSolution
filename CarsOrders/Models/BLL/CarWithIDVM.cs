using System;

namespace CarsOrders.Models.BLL
{
    public class CarWithIDVM : CarVM
    {
        #region Properties

        public Guid ID { get; set; }

        #endregion

        #region Constructors

        public CarWithIDVM()
        {

        }

        public CarWithIDVM(Car c)
            : base(c)
        {
            ID = c.ID;
        }

        #endregion
    }
}
