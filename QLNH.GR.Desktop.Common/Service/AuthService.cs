using QLNH.GR.Desktop.BO;
using System;
using System.Runtime.CompilerServices;

namespace QLNH.GR.Desktop.Common
{
    public class AuthService: BaseService
    {
        static string Route { get; set; } = "Auth";
        public AuthService(): base(Route)    { 
        }

        public async Task<HttpResponseMessage> Login(string email, string password)
        {
            var auth = new { email= email, password= password };
            return await this.PostAsync(auth, "");
        }

        public async Task<HttpResponseMessage> GetEmployee(string email, string password)
        {
            return await this.GetAsync("/Getuser");
        }


        public async Task<HttpResponseMessage> GetListEmployeeFilter(PaginationObject paginationObject)
        {
            return await this.PostAsync(paginationObject);
        }

        public async Task<HttpResponseMessage> SaveUser(Employee employee)
        {
            return await this.PostAsync(employee, "SaveUser");
        }



    }
}
