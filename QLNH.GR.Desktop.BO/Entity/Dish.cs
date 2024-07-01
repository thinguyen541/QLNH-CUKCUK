using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class Dish : BaseEntity
    {
        /// <summary>
        /// Dish's id
        /// </summary>
        public Guid? DishId { get; set; }

        /// <summary>
        /// Dish's code
        /// </summary>
        [Required]
        public string? DishCode { get; set; }

        /// <summary>
        /// Dish's name
        /// </summary>
        [MaxLength(255)]
        public string? DishName { get; set; }

        /// <summary>
        /// Dish's group id
        /// </summary>

        public Guid? DishGroupId { get; set; }

        /// <summary>
        /// Dish's unit id
        /// </summary>

        [Required]
        public Guid? UnitId { get; set; }

        /// <summary>
        /// Dish's cookplace id
        /// </summary>
        public Guid? CookPlaceId { get; set; }

        /// <summary>
        /// Dish's picture id
        /// </summary>
        public Guid? PictureId { get; set; }

        /// <summary>
        /// Dish's parent id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Dish's cost - gia mua
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// Dish's price - gia ban
        /// </summary>
        [Required]
        public decimal? Price { get; set; }

        /// <summary>
        /// Dish's dissciption - mo ta
        /// </summary>
        [MaxLength(5000)]
        public string? Description { get; set; }

        /// <summary>
        /// Dish's tinh trang
        /// </summary>
        [Range(0, 10)]
        public int? Status { get; set; }

        /// <summary>
        /// Dish's code
        /// </summary>
        [MaxLength(255)]
        public string? DishGroupCode { get; set; }

        /// <summary>
        /// Dish's name
        /// </summary>
        [MaxLength(255)]
        public string? DishGroupName { get; set; }

        /// <summary>
        /// CookPlace's name
        /// </summary>
        [MaxLength(255)]
        public string? CookPlaceName { get; set; }

        /// <summary>
        /// Unit's name
        /// </summary>
        [MaxLength(255)]
        public string? UnitName { get; set; }

        /// <summary>
        /// Unit's DisplayOnMenu
        /// </summary>
        public int? DisplayOnMenu { get; set; }

        /// <summary>
        /// Unit's FavoriteServiceDishListDto
        /// </summary>
        [MaxLength(255)]
        public List<FavoriteServiceDish>? FavoriteServiceDishListDto { get; set; }

        public string ImageSource
        {
            get
            {
                if (PictureId != null && PictureId != Guid.Empty) { return $"https://localhost:7206/api/v1/Picture/getImage/{PictureId}"; }
                else
                {
                    return "C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\dish.png";
                }
            }
        }
    }
}
