using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public class DishGroupService : BaseService
    {

        public DishGroupService() : base("DishGroup") { }

        public async Task<HttpResponseMessage> getNewCode(string name)
        {
            return await this.GetAsync($"/GetNewCode?inputString={name}");

        }
    }
}
