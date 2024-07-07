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



        private void CustomTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            var placeholder = GetTemplateChild("Placeholder") as TextBlock;
            var textBox = GetTemplateChild("TextBox") as TextBox;
            if (placeholder != null && textBox != null)
            {
                this.Text = textBox.Text;
                placeholder.Visibility = string.IsNullOrEmpty(textBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public CustomTexbox()
        {
            this.TextChanged += CustomTextbox_TextChanged;
            UpdatePlaceholderVisibility();
        }
    }
}
