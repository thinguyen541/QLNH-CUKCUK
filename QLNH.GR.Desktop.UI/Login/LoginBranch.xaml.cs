

using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using System.Windows.Controls;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for LoginBranch.xaml
    /// </summary>
    public partial class LoginBranch : Page
    {

        public LoginBranch()
        {
            InitializeComponent();
        }

        private void tbChooseyourBranch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Move focus to the next control (textBox2)
            if (!string.IsNullOrEmpty(tbChooseyourBranch.Text) && tbChooseyourBranch.Text.Contains(".cukcuk.vn"))
            {
                tbChooseyourBranch.Text = tbChooseyourBranch.Text + ".cukcuk.vn";
            }

            // Mark the event as handled so the TextBox doesn't insert a tab character
            e.Handled = true;
        }

        private void btnOk_click(object sender, System.Windows.RoutedEventArgs e)
        {
            Session.ReccentDomain = tbChooseyourBranch.Text;
            CommonFunctionUI.NavigateToPage(AppPage.Login);
        }
    }
}
