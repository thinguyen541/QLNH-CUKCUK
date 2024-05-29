using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.Common
{
    public static class CommonFunction
    {
        private static readonly Random _random = new Random();
        public static string GenerateUniqueOrderNumber()
        {
            // Use the current UTC date and time to ensure uniqueness
            DateTime now = DateTime.UtcNow;

            // Generate a random number between 1000 and 9999
            int randomPart = _random.Next(1, 100);

            // Format the date and time as a string, using fewer elements for a shorter order number
            string orderNumber = now.ToString("MMddHHmmff") + randomPart;

            return orderNumber;
        }
    }
}
