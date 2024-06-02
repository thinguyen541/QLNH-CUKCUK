
using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        private string currentTime;
        public string CurrentTime
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

        public MainWindow()
        {

            InitializeComponent();
            // Subscribe to the ShowToastEvent
            SwitchFram();
            setTimer();
            MyPopup.Closed += MyPopup_Closed;
        }

        private void setTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            timer.Start();
        }

        private void SwitchFram()
        {
            Navigator.Initialize(MainFrame);
            if (!string.IsNullOrEmpty(Session.Token))
            {
                Navigator.NavigateToPage(typeof(HomeScreen));
            }
            else if (IsCacheData())
            {
                Navigator.NavigateToPage(typeof(HomeScreen));
            }
            else
            {
                Navigator.NavigateToPage(typeof(LoginBranch));
            }
            EventManager.ShowToastEvent += HandleShowToastEvent; 
            EventManager.ShowDialogEvent += HandleShowDialogEvent;
        }

        private void HandleShowToastEvent(object sender, ToastEventArgs e)
        {
            // Show toast notification
            myToast.ShowToast(e.Message, e.Type);
        }
        private void HandleShowDialogEvent(object sender, DialogEventArgs e)
        {
            // Assign the UserControl to the Popup
            MyPopup.Child = e.dialog;

            // Blur the main window content
            this.Effect = new BlurEffect { Radius = 10 };

            // Show the overlay
            BackgroundOverlay.Visibility = Visibility.Visible;
            // Set the Popup to open
            MyPopup.IsOpen = true;
        }

        private bool IsCacheData()
        {
            var cacheLoginData = JsonFileManager.ReadFromJson<LoginResponse>("E:\\Documents\\git_local\\QLNH.GR.Desktop\\QLNH.GR.Desktop.UI\\FileRerource\\account.json");
            if(cacheLoginData != null && !string.IsNullOrEmpty(cacheLoginData.token))
            {
                Session.UserID = cacheLoginData.user?.EmployeeId;
                Session.UserName = cacheLoginData.user?.AccountName;
                Session.Token = cacheLoginData.token;
                return true;
            }
            return false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the time
            OnPropertyChanged(nameof(CurrentTime));
        }
        private void MyPopup_Closed(object sender, EventArgs e)
        {
            // Remove the blur effect when the popup is closed
            this.Effect = null;

            // Hide the overlay
            BackgroundOverlay.Visibility = Visibility.Collapsed;
        }

    }
}
