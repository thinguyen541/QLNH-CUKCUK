using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLNH.GR.Desktop.UI
{
    public class CustomTexbox: TextBox
    {
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
         nameof(PlaceholderText), typeof(string), typeof(CustomTexbox), new PropertyMetadata(string.Empty));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }



        static CustomTexbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTexbox), new FrameworkPropertyMetadata(typeof(CustomTexbox)));
        }
    }
}
