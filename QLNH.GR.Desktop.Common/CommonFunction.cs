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

        public static List<decimal> SuggestPayment(decimal amount, int maxSuggestions)
        {
            // Define the denominations available in VND (greater than or equal to 500 VND)
            decimal[] denominations = new decimal[]
            {
            500000, 200000, 100000, 50000, 20000, 10000, 5000, 2000, 1000, 500
            };

            // List to store the suggested amounts
            List<decimal> suggestions = new List<decimal>();

            // Add the original amount to the suggestions list
            suggestions.Add(amount);

            // Calculate the suggested payment amounts
            decimal currentAmount = amount;

            // Add increments of the smallest denomination greater than or equal to 500
            decimal smallestDenomination = 500;

            // Suggest payments that minimize the number of denominations used
            while (currentAmount % smallestDenomination != 0 && suggestions.Count < maxSuggestions)
            {
                currentAmount += smallestDenomination;
                suggestions.Add(currentAmount);
            }

            return suggestions;
        }
        public static string GeneratePaymentId()
        {
            string datePart = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string randomPart = _random.Next(1000, 9999).ToString();
            return $"{datePart}{randomPart}";
        }
    }


}
