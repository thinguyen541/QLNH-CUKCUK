using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

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
    
}
