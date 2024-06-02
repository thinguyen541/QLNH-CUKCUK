
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class DetailItem : BaseEntity
    {
        public Guid? DetailItemId { get; set; }
        public Guid? DishId { get; set; }

        public Guid? OrderDetailId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountBeforeTax { get; set; }
        public decimal? AmountAfterTax { get; set; }
        public string? DishName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public EnumDetailItemType DetailItemType { get; set; }
}
}
