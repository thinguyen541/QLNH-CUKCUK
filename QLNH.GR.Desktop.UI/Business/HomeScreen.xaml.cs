using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.BO.Entity;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : BaseUserControl, INotifyPropertyChanged
    {
        public HomeScreen()
        {
            InitializeComponent();
            setTimer();
            LoadListAppFeatures();
            CurrentTime = DateTime.Now;
            DataContext = this;
        }

        List<FeatureApp> ListFeatureApp = new List<FeatureApp>();

        private DispatcherTimer timer;

        private DateTime? currentTime;
        public DateTime? CurrentTime
        {
            get { return currentTime; }
            set
            {
                if (currentTime != value)
                {
                    currentTime = value;
                    OnPropertyChanged(nameof(CurrentTime));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void setTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the tim
            CurrentTime = DateTime.Now;
            OnPropertyChanged(nameof(CurrentTime));
        }


        private void LoadListAppFeatures()
        {
            if(Session.ListFeatureApp != null)
            {
                ListFeatureApp = Session.ListFeatureApp;
            }
            else
            {
                ListFeatureApp.Add(new FeatureApp {FeatureKey="Orders",FeatureName="Orders",SortOrder=0,IconName="Orders" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Table", FeatureName = "Bàn", SortOrder = 0, IconName = "Table" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Menu", FeatureName = "Menu", SortOrder = 0, IconName = "Menu" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Employee", FeatureName = "Nhân viên", SortOrder = 0, IconName = "Employee" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Reciept", FeatureName = "Hóa đơn", SortOrder = 0, IconName = "Reciept" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Transaction", FeatureName = "Giao dịch", SortOrder = 0, IconName = "Transaction" });
                Session.ListFeatureApp = ListFeatureApp;
            }
                itemsControl.ItemsSource = ListFeatureApp;
        }

        private void DineInClick(object sender, MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(AppPage.Table, previousPage:AppPage.MainScreen);
        }

        private void DeliveryClick(object sender, MouseButtonEventArgs e)
        {
            Dictionary<string, object> navigateDictionary = new Dictionary<string, object>();
            Session.IsCreateNewOrder = true;
            Session.SelectingOrderType = EnumOrderType.Delivery;
            CommonFunctionUI.NavigateToPage(AppPage.Order, previousPage: AppPage.MainScreen, navigateDictionary);
        }

        private void TogoClick(object sender, MouseButtonEventArgs e)
        {
            Dictionary<string, object> navigateDictionary = new Dictionary<string, object>();
            Session.IsCreateNewOrder = true;
            Session.SelectingOrderType = EnumOrderType.Pickup;
            CommonFunctionUI.NavigateToPage(AppPage.Order, previousPage: AppPage.MainScreen, navigateDictionary);
        }

        private void LogOut_click(object sender, MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(AppPage.Login);
        }

        private void FetureApp_click(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as FeatureApp;
                if (clickedItem != null)
                {
                    if(clickedItem.FeatureKey == "Orders")
                    {
                        CommonFunctionUI.NavigateToPage(AppPage.OrderList, previousPage: AppPage.MainScreen);
                    }
                    else if (clickedItem.FeatureKey == "Transaction")
                    {
                        CommonFunctionUI.NavigateToPage(AppPage.Transaction, previousPage: AppPage.MainScreen);
                    }
                    else if (clickedItem.FeatureKey == "Reciept")
                    {
                        CommonFunctionUI.NavigateToPage(AppPage.Receipt, previousPage: AppPage.MainScreen);
                    }
                    else if (clickedItem.FeatureKey == "Table")
                    {
                        CommonFunctionUI.NavigateToPage(AppPage.Table, previousPage: AppPage.MainScreen);
                    }
                }
            }
        }
    }
}
