using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public class PictureService : BaseService
    {
        public PictureService() : base("Picture") { }

        public async Task<byte[]> GetImageAsync(string id)
        {
            try
            {
                // Make the GET request to download the image
                var response = await this.GetAsync($"/getImage/{id}");

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // Read the content as a byte array
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                return imageBytes;
            }
            catch (HttpRequestException ex)
            {
                // Handle request exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
