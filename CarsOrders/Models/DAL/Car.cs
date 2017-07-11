using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsOrders.Models
{
    [Table("cars")]
    public class Car : BaseModel
    {
        #region Fields

        public const string ModelField = "Model";

        #endregion

        #region Properties

        [Column(ModelField)]
        public string Model { get; set; }

        public decimal Price { get; set; }

        public decimal EngineCapacity { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Order> Orders { get; set; }

        [NotMapped]
        public string Info => $"{Model}, {EngineCapacity.ToString("F1")}l, ${Price.ToString("F2")}";

        #endregion
    }
}
