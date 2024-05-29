using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public class OrderService : BaseService
    {
        static string Route { get; set; } = "Order";
        public OrderService() : base(Route)
        {
        }
     

    }
}
