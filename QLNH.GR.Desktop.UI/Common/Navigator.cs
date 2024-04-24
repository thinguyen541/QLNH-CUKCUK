using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QLNH.GR.Desktop.UI.Common
{
    public static class Navigator
    {
        private static Frame _mainFrame;

        public static void Initialize(Frame mainFrame)
        {
            _mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
        }

        public static void NavigateToPage(Type pageType)
        {
            if (_mainFrame == null)
                throw new InvalidOperationException("NavigationHelper is not initialized. Call Initialize method first.");

            // You can add additional logic or checks before navigating if needed

            _mainFrame.Navigate(Activator.CreateInstance(pageType));
        }
    }
}
