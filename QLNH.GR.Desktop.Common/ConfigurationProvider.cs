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
                .AddJsonFile("E:\\Documents\\git_local\\QLNH.GR.Desktop\\QLNH.GR.Desktop\\appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
