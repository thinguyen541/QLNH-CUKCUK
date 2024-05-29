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
using System.Windows.Input;

namespace QLNH.GR.Desktop.UI
{
    /// <summary>
    /// Interaction logic for OrderScreen.xaml
    /// </summary>
    public partial class PaymentScreen : BaseUserControl
    {
        public Order CurrentOrder { get; set; }

        public DishService dishService = new DishService();

        public DishGroupService dishGroupService = new DishGroupService();

        public OrderService orderService = new OrderService();

        public OrderDetailService orderDetailService = new OrderDetailService();

        public DetailItemService detailItemService = new DetailItemService();

        public OrderService rrderService = new OrderService();

        public List<Card> ListCard;


        public Card SelectedCard;
        public List<Dish> ListDish { get; set; }

        public List<DishGroup> ListDishGroup { get; set; }

        public DishGroup CurrentDishGroup { get; set; }

        public bool IsCreateNewOrder { get; set; }

        private readonly DecimalToAmountStringConverter _decimalconverter = new DecimalToAmountStringConverter();


        public PaymentScreen()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(this.PreviousPage, previousPage: AppPage.PaymentScreen);
        }

        public override async void ProcessDataAsync()
        {
            await LoadData();
            this.DataContext = CurrentOrder;
        }
        async public Task LoadData()
        {
            await LoadCurrenOrder();
            await LoadOrderDetail();
            LoadListCard();

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
            if (Session.CurrentOrderPayment != null && PreviousPage == AppPage.Order)
            {
                CurrentOrder = Session.CurrentOrderPayment;
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


        public void LoadListCard()
        {
            var listCard = new List<Card>
            {
                new Card() { CardID = Guid.NewGuid(), CardName = "Cash", CardType = EnumCardType.Cash, ImagePath = "E:\\Documents\\git_local\\QLNH.GR.Desktop\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\money.png" },
                new Card() { CardID = Guid.NewGuid(), CardName = "Card", CardType = EnumCardType.Card, ImagePath = "E:\\Documents\\git_local\\QLNH.GR.Desktop\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\credit-card.png" } ,
                new Card() { CardID = Guid.NewGuid(), CardName = "Vemmo", CardType = EnumCardType.Vemmo, ImagePath = "E:\\Documents\\git_local\\QLNH.GR.Desktop\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\Vemmo.png" } ,
                new Card() { CardID = Guid.NewGuid(), CardName = "Other Card", CardType = EnumCardType.OtherCard, ImagePath = "E:\\Documents\\git_local\\QLNH.GR.Desktop\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\atm-card.png" }
            };
            ListCard = listCard;
            lvCard.ItemsSource = ListCard;
        }



        private async void btnPay_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        public async Task ProcessFireAndSave()
        {
            List<object> lstSaveOrder = new List<object>();
            lstSaveOrder.Add(CurrentOrder);
            CurrentOrder.OrderStatus = EnumOrderStatus.Fired;
            CurrentOrder.OrderNo = CommonFunction.GenerateUniqueOrderNumber();
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

            }
            string convertedValue = (string)_decimalconverter.Convert(CurrentOrder.Amount, typeof(string), null, CultureInfo.InvariantCulture);
            txtTotalAmount.Text = convertedValue;
        }
        private void card_click(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as Card;
                if (clickedItem != null)
                {
                    SelectedCard = clickedItem;
                }
            }
        }

    }
}
