using Newtonsoft.Json.Linq;
using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.BO.Entity;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using QLNH.GR.Desktop.UI.Converter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for OrderScreen.xaml
    /// </summary>
    public partial class OrderScreen : BaseUserControl
    {
        public Order CurrentOrder { get; set; }

        public DishService dishService = new DishService();

        public DishGroupService dishGroupService = new DishGroupService();

        public OrderService orderService = new OrderService();

        public OrderDetailService orderDetailService = new OrderDetailService();

        public DetailItemService detailItemService = new DetailItemService();

        public OrderService rrderService = new OrderService();

        public List<Dish> ListDish { get; set; }

        public List<DishGroup> ListDishGroup { get; set; }

        public DishGroup CurrentDishGroup { get; set; }

        public bool IsCreateNewOrder { get; set; }

        private readonly DecimalToAmountStringConverter _decimalconverter = new DecimalToAmountStringConverter();


        public OrderScreen()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(this.PreviousPage, previousPage: AppPage.Order);
        }

        public override async void ProcessDataAsync()
        {
            await LoadData();
            this.DataContext = CurrentOrder;
        }
        async public Task LoadData()
        {
            await LoadCurrenOrder();
            await LoadDish();
            await LoadDishGroup();
            await LoadOrderDetail();

        }


        async private Task LoadOrderDetail()
        {
            //load orderDetail
            lvOrderDetail.ItemsSource = CurrentOrder?.ListOrderDetail?.Where(item => item.EntityMode != 2);
        }

        async private Task ReloadLoadOrderDetail()
        {
            //load orderDetail
            lvOrderDetail.ItemsSource = null;
            lvOrderDetail.ItemsSource = CurrentOrder?.ListOrderDetail?.Where(item => item.EntityMode != 2);

        }

        async private Task LoadCurrenOrder()
        {
            if(PreviousPage == AppPage.PaymentScreen)
            {
                PreviousPage = Session.PreviousOrderPage.Value;
                CurrentOrder = Session.CurrentOrderPayment;
            }
            else if (Session.IsCreateNewOrder == true)
            {
                CurrentOrder = new Order();
                CurrentOrder.OrderId = Guid.NewGuid();
                CurrentOrder.EntityMode = 0;
                CurrentOrder.TableID = Session.TableID;
                CurrentOrder.TableName = Session.TableName;
                CurrentOrder.OrderStatus = EnumOrderStatus.Serving;
                CurrentOrder.OrderType = Session.SelectingOrderType;
                CurrentOrder.OrderNo = CommonFunction.GenerateUniqueOrderNumber();
            }
            else
            {
                if (Session.CurrenOrderId != null)
                {
                    HttpResponseMessage response = await orderService.GetById(Session.CurrenOrderId.ToString());
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
            }
            CalculateOrderAmount();
        }
        async private Task LoadDish()
        {
            var pag = new PaginationObject() { PageSize = 40, RecentPage = 1 };
            HttpResponseMessage response = await dishService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListDish = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dish>>(result.Data.ToString());
                    lvDish.ItemsSource = ListDish;


                }
            }
        }

        async private Task LoadDishGroup()
        {
            var pag = new PaginationObject() { PageSize = 15, RecentPage = 1 };
            HttpResponseMessage response = await dishGroupService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListDishGroup = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DishGroup>>(result.Data.ToString());
                    ListDishGroup.Insert(0, new DishGroup() { DishGroupId = Guid.Empty, DishGroupName = "All Items" });
                    lvDishGroup.ItemsSource = ListDishGroup;
                }
            }
        }

        private void lvDish_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as Dish;
                if (clickedItem != null)
                {
                    AddItemToOrder(clickedItem);
                }
            }
        }

        public void AddItemToOrder(Dish dishItem)
        {
            OrderDetail orderDetail = new OrderDetail();
            DetailItem detailItem = new DetailItem();
            detailItem.DetailItemId = Guid.NewGuid();
            orderDetail.OrderDetailId = Guid.NewGuid();
            orderDetail.OrderId = CurrentOrder.OrderId;
            orderDetail.OrderDetailStatus = EnumOrderDetailStatus.NotSentKitchen;
            detailItem.DishId = dishItem.DishId;
            detailItem.OrderDetailId = orderDetail.OrderDetailId;
            orderDetail.Quantity = 1;
            orderDetail.Amount += dishItem.Price.GetValueOrDefault();
            detailItem.DishName = dishItem.DishName;
            detailItem.EntityMode = 0;
            orderDetail.EntityMode = 0;

            //Check xem có item nào giống item vừa thêm không
            //tạo order detail mới
            if (orderDetail.ListDetailItem == null)
            {
                orderDetail.ListDetailItem = new List<DetailItem>
                        {
                            detailItem
                        };
            }
            else
            {
                orderDetail.ListDetailItem.Add(detailItem);
            }
            if (CurrentOrder.ListOrderDetail == null)
            {
                CurrentOrder.ListOrderDetail = new List<OrderDetail>
                        {
                            orderDetail
                        };
            }
            else
            {
                var existOrderDetail = CurrentOrder?.ListOrderDetail?.FirstOrDefault(detail => detail.ListDetailItem != null && detail.ListDetailItem.Any(item => item.DishId == dishItem.DishId) && detail.OrderDetailStatus == EnumOrderDetailStatus.NotSentKitchen && detail.EntityMode != 2);

                if (existOrderDetail != null)
                {
                    existOrderDetail.Quantity += 1;
                    existOrderDetail.EntityMode = 1;
                    existOrderDetail.Amount += dishItem.Price.GetValueOrDefault();
                }
                else
                {
                    CurrentOrder.ListOrderDetail.Add(orderDetail);
                }

            }
            ReloadLoadOrderDetail();
            CalculateOrderAmount();


        }

        private void btnDelete_Dish(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get the clicked button
            var button = sender as Button;
            if (button != null)
            {
                // Get the item associated with the button
                var item = button.DataContext as OrderDetail;
                if (item != null)
                {
                    // Find the category that contains the item
                    item.EntityMode = 2;
                    foreach (var detailItem in item.ListDetailItem)
                    {
                        detailItem.EntityMode = 2;
                    }
                }
                ReloadLoadOrderDetail();
                CalculateOrderAmount();
            }
        }

        private async void Group_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as DishGroup;
                if (clickedItem != null)
                {
                    var pag = new PaginationObject() { PageSize = 40, RecentPage = 1 };
                    if (clickedItem.DishGroupId != Guid.Empty)
                    {
                        pag.FilterObjects = new List<FilterObject>() { new FilterObject() { Property = "Dishs.DishGroupId", Value = clickedItem?.DishGroupId.ToString(), PropertyType = 4, Operator = 1, RelationType = 0 } };
                    }
                    HttpResponseMessage response = await dishService.Filter(pag);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserialize the response body to the specified type
                        PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                        if (result != null)
                        {
                            ListDish = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dish>>(result.Data.ToString());
                            lvDish.ItemsSource = null;
                            lvDish.ItemsSource = ListDish;
                            lvDish.UpdateLayout();

                        }
                    }
                }
            }
        }

        private async void btnFire_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CurrentOrder != null && CurrentOrder.ListOrderDetail != null)
            {
                if (CurrentOrder.ListOrderDetail.Any(item => item.EntityMode != 2))
                {
                    await ProcessFireAndSave();
                }
                else
                {
                    CommonFunctionUI.ShowToast("Order have at least one item.", type: ToastType.Warning);
                }
            }
            else
            {
                CommonFunctionUI.ShowToast("Order have at least one item.", type: ToastType.Warning);
            }
        }

        private async void btnPay_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CurrentOrder != null && CurrentOrder.ListOrderDetail != null)
            {
                if (CurrentOrder.ListOrderDetail.Any(item => item.EntityMode != 2))
                {
                    Session.PreviousOrderPage = PreviousPage;
                    Session.IsCreateNewOrder = false;
                    Session.CurrentOrderPayment = CurrentOrder;
                    CommonFunctionUI.NavigateToPage(AppPage.PaymentScreen, previousPage: AppPage.Order);
                }
                else
                {
                    CommonFunctionUI.ShowToast("Order have at least one item.", type: ToastType.Warning);
                }
            }
            else
            {
                CommonFunctionUI.ShowToast("Order have at least one item.", type: ToastType.Warning);
            }
          
        }

        public async Task ProcessFireAndSave()
        {
            List<object> lstSaveOrder = new List<object>();
            lstSaveOrder.Add(CurrentOrder);
            CurrentOrder.OrderStatus = EnumOrderStatus.Fired;
            HttpResponseMessage response = await orderService.SaveAndUpdateData(lstSaveOrder);
            if (response != null && response.IsSuccessStatusCode)
            {
                CommonFunctionUI.ShowToast("Fire successfully.");
                CommonFunctionUI.NavigateToPage(AppPage.MainScreen, previousPage: AppPage.Order);
            }
        }

        public void CalculateOrderAmount()
        {
            if (CurrentOrder != null && CurrentOrder.ListOrderDetail != null)
            {
                CurrentOrder.Amount = CurrentOrder.ListOrderDetail.Sum(item => { if (item.EntityMode != 2) return item.Amount; return 0; });
            }
            else
            {
                CurrentOrder.Amount = 0;
               
            }
            string convertedValue = (string)_decimalconverter.Convert(CurrentOrder.Amount, typeof(string), null, CultureInfo.InvariantCulture);
            CurrentOrder.RemainAmount = CurrentOrder.Amount;
            txtTotalAmount.Text = convertedValue;
        }
    }
}
