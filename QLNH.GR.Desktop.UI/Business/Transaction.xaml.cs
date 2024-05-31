using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.BO.Entity;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for Transaction.xaml
    /// </summary>
    public partial class Transaction : BaseUserControl 
    {
        public List<CoverOrder> ListCoverOrder { get; set; }

        public CoverOrderService coverOrderService = new CoverOrderService();

        public Transaction()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(this.PreviousPage, previousPage: AppPage.Transaction);
        }


        public override async void ProcessDataAsync()
        {
            await LoadTransaction();
        }

        async private Task LoadTransaction()
        {
            var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
            HttpResponseMessage response = await coverOrderService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListCoverOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CoverOrder>>(result.Data.ToString());
                    lvTransactionList.ItemsSource = ListCoverOrder;
                }
            }
        }



        private void transaction_click(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as CoverOrder;
                if (clickedItem != null)
                {

                }
            }
        }
    }
}
