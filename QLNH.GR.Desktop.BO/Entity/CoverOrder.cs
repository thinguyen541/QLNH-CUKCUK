using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class CoverOrder : BaseEntity
    {

        public Guid CoverOrderId { get; set; }
        public Guid? OrderId { get; set; }
        public string OrderNo { get; set; }
        public string TransactionID { get; set; }
        public decimal? CoverAmount { get; set; }
        public decimal? TipAmount { get; set; }
        public bool? IsTipCover { get; set; }
        public DateTime? CoverDate { get; set; }
        public EnumCardType? CardType { get; set; }
        public string? CardName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
