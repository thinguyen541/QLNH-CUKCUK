using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO.Entity
{
    public class Invoice: BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? RefID { get; set; }
        public string OrderNo { get; set; }
        public decimal Amount { get;set; }
        public decimal Tipamount { get;set; }
        public Guid UserId { get;set; }
        public string UserName { get; set; }   
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Guid? TableID { get; set; }
        public string? TableName { get; set; }
        public EnumOrderType? OrderType { get; set; }
        public string? PromotionName { get; set; }
        public decimal? PromotionAmount { get; set; }

    }
}
