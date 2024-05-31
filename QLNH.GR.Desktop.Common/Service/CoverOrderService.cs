using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public class CoverOrderService:BaseService
    {
        static string Route { get; set; } = "CoverOrder";
        public CoverOrderService() : base(Route)
        {
        }
    }
}
