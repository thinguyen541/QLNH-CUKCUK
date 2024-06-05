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


        public OrderDetailService orderDetailService = new OrderDetailService();

        public DetailItemService detailItemService = new DetailItemService();
        public Order CurrentOrder { get; set; }

        public OrderService orderService = new OrderService();

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
                    lvInvoiceList.SelectedIndex = 0;
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

        private async void ReceiptsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvInvoiceList.SelectedItem is Invoice selectedReceipt)
            {
                HttpResponseMessage response = await orderService.GetById(selectedReceipt.OrderId.ToString());
                if (response != null && response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the response body to the specified type
                    Order result = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(responseBody);
                    if (result != null)
                    {
                        CurrentOrder = result;
                        CurrentOrder.EntityMode = 1;
                    }
                }

                if (CurrentOrder != null)
                {

                    var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
                    pag.FilterObjects = new List<FilterObject>() { new FilterObject() { Property = "OrderID", Value = CurrentOrder?.OrderId.ToString(), PropertyType = 4, Operator = 1, RelationType = 0 } };
                    HttpResponseMessage orderDetailResponse = await orderDetailService.Filter(pag);
                    if (orderDetailResponse != null && orderDetailResponse.IsSuccessStatusCode)
                    {
                        string orderDetailResponseBody = await orderDetailResponse.Content.ReadAsStringAsync();

                        PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(orderDetailResponseBody);
                        if (result != null)
                        {
                            var listOrderDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderDetail>>(result.Data.ToString());
                            if (listOrderDetail != null && listOrderDetail.Any())
                            {
                                CurrentOrder.ListOrderDetail = listOrderDetail;
                                foreach (var deatil in listOrderDetail)
                                {
                                    var pag1 = new PaginationObject() { PageSize = 1000, RecentPage = 1 };
                                    pag1.FilterObjects = new List<FilterObject>() { new FilterObject() { Property = "DetailItems.OrderDetailId", Value = deatil?.OrderDetailId.ToString(), PropertyType = 4, Operator = 1, RelationType = 0 } };
                                    HttpResponseMessage detailItemResponse = await detailItemService.Filter(pag1);
                                    if (detailItemResponse != null && detailItemResponse.IsSuccessStatusCode)
                                    {
                                        // Read the detailItemResponse content as a string
                                        string detailItemResponseBody = await detailItemResponse.Content.ReadAsStringAsync();

                                        // Deserialize the detailItemResponse body to the specified type
                                        PagingHttpResponse result1 = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(detailItemResponseBody);
                                        if (result1 != null)
                                        {
                                            var detailItem = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DetailItem>>(result1.Data.ToString());
                                            deatil.EntityMode = 1;
                                            deatil.ListDetailItem = detailItem;
                                            foreach (var item in detailItem)
                                            {
                                                item.EntityMode = 1;
                                            }


                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                GenerateAndDisplayPdf(CurrentOrder, selectedReceipt);
            }
           
        }



        private void GenerateAndDisplayPdf(Order CurrentOrde, Invoice selectedReceiptr)
        {
            string storeName = "My Store";
            string storeAddress = "123 Main St, Anytown, USA";

            // Define custom page size (5 inches wide by 7 inches tall)
            float width = 5 * 72; // 5 inches
            float height = 7 * 72; // 7 inches
            iTextSharp.text.Rectangle customPageSize = new iTextSharp.text.Rectangle(width, height);

            string pdfFilePath = "E:\\Downloads\\" + $"{Guid.NewGuid().ToString()}.pdf";

            Printer.PrintReceipt(storeName, storeAddress, CurrentOrder, pdfFilePath, customPageSize, selectedReceiptr);

            DisplayPdf(pdfFilePath, customPageSize);
        }



        private void DisplayPdf(string pdfFilePath, iTextSharp.text.Rectangle pageSize)
        {
            PdfWebViewer.Navigate(new Uri(pdfFilePath));

            // Display PDF size
            string sizeText = $"Width: {pageSize.Width / 72} inches ({pageSize.Width} points)\n" +
                              $"Height: {pageSize.Height / 72} inches ({pageSize.Height} points)";
           
        }



    }
}
