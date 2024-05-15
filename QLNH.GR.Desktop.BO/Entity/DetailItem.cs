
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class DetailItem : BaseEntity
    {
        public Guid? DetailItemID { get; set; }
        public Guid? DishId { get; set; }
        public string? DishName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public EnumDetailItemType DetailItemType { get; set; }
}
}
