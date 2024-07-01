using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{

    public static class ConfigurationProvider
    {
        public static IConfiguration LoadConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
