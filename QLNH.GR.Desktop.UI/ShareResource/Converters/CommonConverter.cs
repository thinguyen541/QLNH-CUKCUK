using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using QLNH.GR.Desktop.BO;

namespace QLNH.GR.Desktop.UI.Converter
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (boolValue)
                {
                    // If true, return Visible
                    return Visibility.Visible;
                }
                else
                {
                    // If false, return Collapsed or Hidden, based on the converter parameter
                    if (Enum.TryParse(parameter?.ToString(), out Visibility visibility))
                    {
                        return visibility;
                    }
                }
            }

            // By default, return Collapsed
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }



    public class NotBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Invert the boolean value
                bool invertedValue = !boolValue;

                // Convert the inverted boolean value to visibility
                return invertedValue ? Visibility.Visible : Visibility.Collapsed;
            }

            // If the value is not a boolean, default to Collapsed
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    public class DateTimeToDateString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime date)
            {
                // Invert the boolean value
                return date.ToString("MM/dd/yyyy");
            }

            // If the value is not a boolean, default to Collapsed
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class DateTimeToTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime date)
            {
                // Invert the boolean value
                return date.ToString("hh:mm:ss");
            }

            // If the value is not a boolean, default to Collapsed
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string iconName)
            {
                // Construct the path to the PNG file based on the iconName
                return $"pack://application:,,,/FileRerource/Resources/Icon/{iconName}.png";

                // Load the PNG file as a BitmapImage

            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class DecimalToQuantityStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                //return String.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:c}", decimalValue);
                return decimalValue.ToString("G29",culture); // "G29" removes unnecessary trailing zeros while preserving precision
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            return 0m;
        }
    }


    public class DecimalToAmountStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            if (value is decimal decimalValue)
            {
                return String.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:c}", decimalValue); // "G29" removes unnecessary trailing zeros while preserving precision
            }
            return "0.0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            return 0m;
        }
    }

    
    public class AmountToPayContents : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return $"Trả - {decimalValue.ToString(CultureInfo.GetCultureInfo("vi-VN"))}"; // "G29" removes unnecessary trailing zeros while preserving precision
            }
            return "Trả - 0.0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            return 0m;
        }
    }


    public class RandomBrushConverter : IValueConverter
    {
        private static Random random = new Random();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a Brush");

            Color randomColor = Color.FromRgb(
                (byte)random.Next(256),
                (byte)random.Next(256),
                (byte)random.Next(256)
            );

            return new SolidColorBrush(randomColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }

    public class EnumOrderTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is EnumOrderType OrderType)
            {
                if (OrderType == EnumOrderType.DineIn)
                {
                    return "Tại bàn";
                }
                if (OrderType == EnumOrderType.Delivery)
                {
                    return "Giao hàng";
                }
                if (OrderType == EnumOrderType.Pickup)
                {
                    return "Mang về";
                }
                return OrderType;

            }

            // If the value is not a boolean, default to Collapsed
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
