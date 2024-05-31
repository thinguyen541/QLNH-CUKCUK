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
    /// Interaction logic for Reciept.xaml
    /// </summary>
    public partial class Receipt : BaseUserControl
    {
        public List<Invoice> ListInvoice { get; set; }

        public InvoiceService invoiceService = new InvoiceService();

        public Receipt()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(this.PreviousPage, previousPage: AppPage.Receipt);
        }

        public override async void ProcessDataAsync()
        {
            await LoadInvoice();
        }

        async private Task LoadInvoice()
        {
            var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
            HttpResponseMessage response = await invoiceService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListInvoice = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Invoice>>(result.Data.ToString());
                    lvInvoiceList.ItemsSource = ListInvoice;
                }
            }
        }



        private void invoice_click(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as Invoice;
                if (clickedItem != null)
                {
                   
                }
            }
        }
    }
}
