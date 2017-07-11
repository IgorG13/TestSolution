using System.ComponentModel;

namespace CarsOrders.Models.BLL
{
    public class CarVM
    {
        #region Properties

        public string Model { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Engine capacity")]
        public decimal EngineCapacity { get; set; }

        #endregion

        #region Constructors

        public CarVM()
        {

        }

        public CarVM(Car c)
        {
            Model = c.Model;
            Price = c.Price;
            EngineCapacity = c.EngineCapacity;
        }

        #endregion

        #region Public Functions

        public Car GetCar()
        {
            return new Car
            {
                Model = Model,
                Price = Price,
                EngineCapacity = EngineCapacity
            };
        }

        #endregion
    }
}
