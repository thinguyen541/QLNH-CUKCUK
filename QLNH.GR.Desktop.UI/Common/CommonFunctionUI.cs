using QLNH.GR.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using QLNH.GR.Desktop.UI;
using System.Windows.Input;
using QLNH.GR.Desktop.BO;

namespace QLNH.GR.Desktop.UI.Common
{
    public class CommonFunctionUI
    {
        public static void NavigateToPage(AppPage page, AppPage previousPage = AppPage.MainScreen,Dictionary<string, object> arguments = null)
        {
            Window window = Application.Current.MainWindow;
            Frame frame = window.FindName("MainFrame") as Frame;

            switch (page)
            {
                case AppPage.MainScreen:
                    var homePage = new HomeScreen();
                    homePage.PreviousPage = previousPage;
                    frame.Navigate(homePage);
                    
                    if (arguments != null && arguments.ContainsKey("SomeArgument"))
                    {
                        
                    }
                    frame.Navigate(homePage);
                    break;
                 
                case AppPage.Login:
                    var loginPage = new Login();
                  
                    frame.Navigate(loginPage);
                    break;
                case AppPage.LoginBranch:
                    var loginBranchPage = new LoginBranch();
                    frame.Navigate(loginBranchPage);
                    break;
                case AppPage.Table:
                    
                    var chooseTable = new ChooseTable();
                    chooseTable.PreviousPage = previousPage;
                    frame.Navigate(chooseTable);
                    break;
                default:
                    throw new ArgumentException("Invalid page enum value");
            }
        }

        public static void ShowToast(string message, ToastType type = ToastType.Success)
        {
            EventManager.RaiseShowToastEvent(null, new ToastEventArgs(message, type));
        }
    }
}
