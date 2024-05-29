using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public class OrderDetailService : BaseService
    {
        static string Route { get; set; } = "OrderDetail";
        public OrderDetailService() : base(Route)
        {
        }

    }
}
