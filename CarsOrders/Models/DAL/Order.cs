using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsOrders.Models
{
    [Table("orders")]
	public class Order : BaseModel
	{
        #region Fields

        public const string OrderDateField = "OrderDate";
        public const string CarIDField = "CarID";

        #endregion

        #region Properties

        public string OrderNumber { get; set; }

        [JsonIgnore]
        [Column(OrderDateField)]
        public DateTime OrderDate { get; set; }


        [NotMapped]
        public string OrderDateStr
        {
            get { return OrderDate.ToString("dd.MM.yyyy HH:mm"); }
            set { OrderDate = DateTime.Parse(value); }
        }

        [Column(CarIDField)]
        public Guid CarID { get; set; }

        [Required]
        [ForeignKey("CarID")]
        [JsonIgnore]
        public Car Car { get; set; }

        [NotMapped]
        public string CarInfo => Car?.Info ?? string.Empty;

        #endregion

        #region Constructors

        public Order()
        {
        }

        public Order(Car car, string orderNumber)
        {
            OrderDate = DateTime.Now;
            OrderNumber = orderNumber;
            CarID = car?.ID ?? Guid.Empty;
            Car = car;
        }

        #endregion
    }
}
