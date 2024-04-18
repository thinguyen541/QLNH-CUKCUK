using QLNH.GR.Desktop.Common.Service.HttpConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace QLNH.GR.Desktop.Common
{
    public class BaseService

    {
        private readonly IConfiguration _configuration;
        public BaseService() { }

        public BaseService(string endPoint) {
            this.Route = endPoint;
            _configuration = ConfigurationProvider.LoadConfiguration();
        }
        public string Route { get; set; }

        // Your authentication token
        public static string Token { get; set; } = Session.Token;

        // Create HttpClient with TokenHandler configured
        public HttpClient httpClient = HttpClientConfigurator.ConfigureHttpClient(Token);

        public async Task<HttpResponseMessage> GetAsync(string endpoint) {
            return await httpClient.GetAsync(_configuration["BaseUrl"].ToString() + this.Route + "/" + endpoint);

        }


        public async Task<HttpResponseMessage> PostAsync(object objToSend,string endpont = "") {
            // Serialize object to JSON
            try
            {
                string jsonBody = JsonConvert.SerializeObject(objToSend);

                // Create the request content
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                if (!string.IsNullOrEmpty(endpont))
                {
                    endpont = "\"" + endpont;
                }
                return await httpClient.PostAsync(_configuration["BaseUrl"].ToString() + this.Route + endpont, content);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> PutAsync(object objToSend, string endpont = "")
        {
            // Serialize object to JSON
            string jsonBody = JsonConvert.SerializeObject(objToSend);

            // Create the request content
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(endpont))
            {
                endpont = "\"" + endpont;
            }

            return await httpClient.PutAsync(_configuration["BaseUrl"].ToString() + this.Route + endpont, content);
        }


        public async Task<HttpResponseMessage> DeleteAsync(string endpoint) {
            return await httpClient.DeleteAsync(_configuration["BaseUrl"].ToString() + this.Route + "/" + endpoint);
        }

        public async Task<HttpResponseMessage> Filter(object pagination)
        {
            return await this.PostAsync(pagination, endpont: "/Filter");
        }

        public async Task<HttpResponseMessage> SaveData(object data)
        {
            return await this.PostAsync(data, endpont: "/Savedata");
        }

        public async Task<HttpResponseMessage> GetById(string id)
        {
            return await this.GetAsync(id);
        }  
        
        public async Task<HttpResponseMessage> DeleteById(string id)
        {
            return await this.DeleteAsync(id);
        } 
        
        public async Task<HttpResponseMessage> DeleteByIds(List<Guid> ids)
        {
            var content = new StringContent(JsonConvert.SerializeObject(ids), System.Text.Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(_configuration["BaseUrl"].ToString() + this.Route, content);
        }

    }
    
}
