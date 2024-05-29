using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class FavoriteServiceDish: BaseEntity
    {
        /// <summary>
        /// FavoriteServiceDish's id
        /// </summary>

        public Guid? FavoriteServiceDishId { get; set; }

        /// <summary>
        /// Picture's id
        /// </summary>


        public Guid? FavoriteServiceId { get; set; }

        /// <summary>
        /// Dish's id
        /// </summary>

        public Guid? DishId { get; set; }

        /// <summary>
        /// FavoriteService's Cost
        /// </summary>
        public int? ServiceCost { get; set; }

    }
}
