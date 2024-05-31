using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class Order: BaseEntity
    {
        public Guid? OrderId { get; set; }
        public List<OrderDetail>? ListOrderDetail { get; set; }  
        public decimal? AmountBeforeTax { get; set; } = 0;
        public decimal? AmountAfterTax { get; set; } = 0;
        public decimal? Amount { get; set; } = 0;
        public decimal? RemainAmount { get; set; } = 0;
        public EnumPaymentStatus? PaymentStatus { get; set; }
        public EnumOrderStatus? OrderStatus { get; set; }
        public EnumOrderType? OrderType { get; set; }

        public Guid? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public Guid? TableID { get; set; }
        public string? OrderNo { get; set; }
        public string? TableName { get; set; }

    }
}
