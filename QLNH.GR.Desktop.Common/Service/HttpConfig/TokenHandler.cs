using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common.Service.HttpConfig
{

    public class TokenHandler : DelegatingHandler
    {
        private readonly string _token;

        public TokenHandler(string token)
        {
            _token = token;
            // Ensure the inner handler is assigned
            InnerHandler = new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Add authorization token to request headers
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            // Call the inner handler to continue the request pipeline
            return base.SendAsync(request, cancellationToken);
        }
    }
}
