
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class OrderDetail: BaseEntity
    {
        public Guid OrderDetailID { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountBeforeTax { get; set; }

        public decimal AmountAfterTax{ get; set; }

        public EnumOrderDetailStatus OrderDetailStatus { get; set; }

    }
}
