using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class LoginResponse
    {
        public string token { get; set; }
        public Employee user { get; set; }

        public List<FeatureApp> ListFeatureApp { get; set; }

    }
}
