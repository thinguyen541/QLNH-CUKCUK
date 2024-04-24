using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
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
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Table", FeatureName = "Table", SortOrder = 0, IconName = "Table" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Menu", FeatureName = "Menu", SortOrder = 0, IconName = "Menu" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Employee", FeatureName = "Employee", SortOrder = 0, IconName = "Employee" });
                ListFeatureApp.Add(new FeatureApp { FeatureKey = "Reciept", FeatureName = "Reciept", SortOrder = 0, IconName = "Reciept" });
                Session.ListFeatureApp = ListFeatureApp;
                itemsControl.ItemsSource = ListFeatureApp;
            }
        }
    }
}
