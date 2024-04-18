using QLNH.GR.Desktop.UI.Common;
using System.Windows;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Subscribe to the ShowToastEvent
            Navigator.Initialize(MainFrame);
            Navigator.NavigateToPage(typeof(LoginBranch));
            EventManager.ShowToastEvent += HandleShowToastEvent;
        }
        private void HandleShowToastEvent(object sender, ToastEventArgs e)
        {
            // Show toast notification
            myToast.ShowToast(e.Message, e.Type);
        }

    }
}
