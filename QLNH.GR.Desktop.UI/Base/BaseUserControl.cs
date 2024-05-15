using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLNH.GR.Desktop.UI
{
    public class BaseUserControl: UserControl
    {

        public AppPage PreviousPage { get; set; } = AppPage.MainScreen;

        private bool _isDataLoaded = false; // Flag to track whether data is loaded

        public BaseUserControl()
        {
            Loaded += BaseUserControl_Loaded; // Subscribe to the Loaded event
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isDataLoaded)
            {
                // Process data here
                ProcessDataAsync();

                // Set flag to indicate data is loaded
                _isDataLoaded = true;

                // Make the control visible after data loading
                Visibility = Visibility.Visible;
            }
        }

        // Method to process data
        public virtual void ProcessDataAsync()
        {
            // Implement data processing logic in derived classes
        }
    }
}
