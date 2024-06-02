using Newtonsoft.Json;
using QLNH.GR.Desktop.BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public class FavoriteServiceService : BaseService
    {
        public FavoriteServiceService() : base("FavoriteService") { }

        public async Task<List<FavoriteService>> GetFavoriteServiceByDishID(Guid dishId)
        {
            List<FavoriteService> ListFavoriteService = new List<FavoriteService>();
            string Url = $"GetFavoriteServiceByDishID/{dishId}";
            HttpResponseMessage response = await GetAsync(Url);
            if (response != null && response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                ListFavoriteService = JsonConvert.DeserializeObject<List<FavoriteService>>(responseBody);
            }
            return ListFavoriteService;
        }
    }
}
