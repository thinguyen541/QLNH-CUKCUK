using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class FavoriteService: BaseEntity
    {
        /// <summary>
        /// Picture's id
        /// </summary>
        public Guid? FavoriteServiceId { get; set; }

        /// <summary>
        /// FavoriteService's name
        /// </summary>
        [MaxLength(255)]
        public string? FavoriteServiceName { get; set; }

        /// <summary>
        /// FavoriteService's Cost
        /// </summary>
        [DefaultValue(0)]
        public int? FavoriteServiceCost { get; set; }
    }
}
