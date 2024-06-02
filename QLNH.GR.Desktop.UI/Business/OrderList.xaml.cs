using AssignmentGPBL.Domain.Object;
using Newtonsoft.Json.Linq;
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
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : BaseUserControl
    {
        public List<Order> ListOrder { get; set; }

        public OrderService orderService = new OrderService();

        public OrderDetailService orderDetailService = new OrderDetailService();

        public DetailItemService detailItemService = new DetailItemService();
        public OrderList()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(AppPage.MainScreen, previousPage: AppPage.Order);
        }

        public override async void ProcessDataAsync()
        {
            await LoadOrder();
        }

        async private Task LoadOrder()
        {
            var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
            List<BO.FilterObject> lstFilter = new List<BO.FilterObject>()
            {
                new BO.FilterObject(){ Property = "OrderStatus", Value=(int)EnumOrderStatus.Done, PropertyType = (int)EnumPropertyType.isInt, Operator = (int)EnumOperator.NOTEQUAL,RelationType= (int)EnumRelationType.AND }
            };
            pag.FilterObjects = lstFilter;
            HttpResponseMessage response = await orderService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(result.Data.ToString());
                    lvOrderList.ItemsSource = ListOrder;
                }
            }
        }



        private void btnPay_click(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var clickedItem = button.DataContext as Order;
                if (clickedItem != null)
                {
                    Session.IsCreateNewOrder = false;
                    Session.CurrenOrderId = clickedItem.OrderId;
                    CommonFunctionUI.NavigateToPage(AppPage.PaymentScreen, previousPage: AppPage.OrderList);
                }
            }
        }

        private void order_click(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as Order;
                if (clickedItem != null)
                {
                    Session.IsCreateNewOrder = false;
                    Session.CurrenOrderId = clickedItem.OrderId;
                    CommonFunctionUI.NavigateToPage(AppPage.Order, previousPage: AppPage.OrderList);
                }
            }
        }
    }
}
