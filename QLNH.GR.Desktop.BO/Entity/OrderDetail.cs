
using QLNH.GR.Desktop.BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class OrderDetail: BaseEntity
    {
        public Guid? OrderDetailId { get; set; }
        public Guid? OrderId { get; set; }

        public List<DetailItem> ListDetailItem { get; set; }   

        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; } = 0;

        public decimal? AmountBeforeTax { get; set; } = 0;

        public decimal? AmountAfterTax { get; set; } = 0;

        public EnumOrderDetailStatus OrderDetailStatus { get; set; }

    }
}
