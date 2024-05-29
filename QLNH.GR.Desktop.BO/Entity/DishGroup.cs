using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class DishGroup: BaseEntity
    {
        /// <summary>
        /// Dish's id
        /// </summary>
        public Guid DishGroupId { get; set; }

        /// <summary>
        /// Dish's code
        /// </summary>
        [Required]
        public string DishGroupCode { get; set; }


        /// <summary>
        /// Dish's name
        /// </summary>
        [MaxLength(255)]
        public string DishGroupName { get; set; }

        /// <summary>
        /// Dish's name
        /// </summary>
        [MaxLength(5000)]
        public string? DishGroupDescription { get; set; }
    }
}
