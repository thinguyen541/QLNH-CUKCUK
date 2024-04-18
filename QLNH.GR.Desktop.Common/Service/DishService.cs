using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLNH.GR.Desktop.Common
{
    public class DishService: BaseService
    {
        public DishService(): base("Dish") { }

        public async Task<HttpResponseMessage> getNewCode(string name)
        {
            return await this.GetAsync($"/GetNewCode?inputString={name}");

        }
    }
}
