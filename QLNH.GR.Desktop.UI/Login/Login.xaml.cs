using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Principal;
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

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        AuthService authService = new AuthService();
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validateInput())
                {
                    SecureString securePassword = txtPassword.SecurePassword;

                    // Convert the SecureString to a regular string (not recommended for security reasons)
                    string password = new System.Net.NetworkCredential(string.Empty, securePassword).Password;
                    HttpResponseMessage response = await authService.Login(txtUserName.Text, password);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserialize the response body to the specified type
                        LoginResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                        if(result != null && result.token != null)
                        {
                            Session.UserID = result.user?.EmployeeId;
                            Session.UserName = result.user?.AccountName;
                            Session.Token = result.token;
                            WriteToFile(result);
                        }
                        CommonFunctionUI.NavigateToPage(AppPage.MainScreen);
                    }
                    else
                    {
                        // Handle unsuccessful response
                        CommonFunctionUI.ShowToast("Sai thôn tin!", ToastType.Warning);

                    }
                }
            }
            catch (Exception)
            {
                CommonFunctionUI.ShowToast("Lỗi kết nối!", ToastType.Warning);
            }
        }

        private static void WriteToFile(LoginResponse result)
        {
            JsonFileManager.WriteToJson<LoginResponse>(result, "C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\FileRerource\\account.json");
        }

        public bool validateInput()
        {
            return true;
        }

      
    }
}
