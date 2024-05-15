using QLNH.GR.Desktop.BO;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for OrderScreen.xaml
    /// </summary>
    public partial class OrderScreen : BaseUserControl
    {
        public Order CurrentOrder { get; set; }
        public OrderScreen()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
