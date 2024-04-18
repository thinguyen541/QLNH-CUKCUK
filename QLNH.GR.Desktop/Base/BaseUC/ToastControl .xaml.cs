using QLNH.GR.Desktop.Common;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for ToastControl.xaml
    /// </summary>
    public partial class ToastControl : UserControl
    {
        private Timer _timer;
        public ToastControl()
        {
            InitializeComponent();
            _timer = new Timer(3000); // 3000 milliseconds = 3 seconds
            _timer.Elapsed += TimerElapsed;
        }

        // Dependency property for message
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastControl), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Dependency property for toast type
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ToastType), typeof(ToastControl), new PropertyMetadata(ToastType.Success));

        public ToastType Type
        {
            get { return (ToastType)GetValue(TypeProperty); }
            set
            {
                SetValue(TypeProperty, value);
                SetTypeStyle();
            }
        }

        private void SetTypeStyle()
        {
            switch (Type)
            {
                case ToastType.Success:
                    border.Background = Brushes.LightGreen;
                    break;
                case ToastType.Warning:
                    border.Background = Brushes.Orange;
                    break;
            }
        }

        // ShowToast method to display the toast message
        public void ShowToast(string message, ToastType type = ToastType.Success)
        {
            // Set the message
            Message = message;

            Type = type;

            // Show the toast
            Visibility = Visibility.Visible;

            // Start the timer
            _timer.Start();
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            Application.Current.Dispatcher.Invoke(() => Visibility = Visibility.Collapsed);
        }
    }

   

}
