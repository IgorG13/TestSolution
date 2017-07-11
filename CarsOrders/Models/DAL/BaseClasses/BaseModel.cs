using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsOrders.Models
{
    public abstract class BaseModel
    {
        #region Fields

        public const string IDField = "ID";

        #endregion

        #region Properties

        [Column(IDField)]
        public Guid ID { get; set; }

        #endregion

        #region Constructors

        public BaseModel()
        {
            ID = Guid.NewGuid();
        }

        #endregion
    }
}
