using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class Order: BaseEntity
    {
        public Guid? OrderID { get; set; }  
        public List<OrderDetail>? ListOrderdetail { get; set; }  
        public decimal? AmountBeforeTax { get; set; }
        public decimal? AmountAfterTax { get; set; }
        public decimal? Amount { get; set; }
        
        public EnumPaymentStatus? PaymentStatus { get; set; }

        public EnumOrderStatus? OrderStatus { get; set; }

        public Guid? EmployeeID { get; set; }

        public string? EmployeeName { get; set; }

        public Guid? TableID { get; set; }

    }
}
