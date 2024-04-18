using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common.Service.HttpConfig
{

    public class HttpClientConfigurator
    {
        public static HttpClient ConfigureHttpClient(string token)
        {
            var httpClient = new HttpClient(new TokenHandler(token));

            // Configure HttpClient settings (if needed)
            // For example:
            // httpClient.Timeout = TimeSpan.FromSeconds(30);

            return httpClient;
        }
    }
}
