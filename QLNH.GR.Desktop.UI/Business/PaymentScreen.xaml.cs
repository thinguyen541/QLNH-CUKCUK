﻿using Newtonsoft.Json.Linq;
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


        public CoverOrderService coverOrderService = new CoverOrderService();

        public InvoiceService invoiceService = new InvoiceService();

        public DishGroupService dishGroupService = new DishGroupService();

        public OrderService orderService = new OrderService();

        public OrderDetailService orderDetailService = new OrderDetailService();

        public DetailItemService detailItemService = new DetailItemService();

        public OrderService rrderService = new OrderService();


        // Register the dependency property
        public static readonly DependencyProperty AmountToPayProperty =
            DependencyProperty.Register(
                nameof(AmountToPay),       // The name of the dependency property
                typeof(decimal),           // The type of the property
                typeof(PaymentScreen),    // The owner type
                new PropertyMetadata(default(decimal)));  // Default value

        // CLR property wrapper
        public decimal AmountToPay
        {
            get => (decimal)GetValue(AmountToPayProperty);
            set => SetValue(AmountToPayProperty, value);
        }

        public static readonly DependencyProperty ChoosePaymentAmountProperty =
          DependencyProperty.Register(
              nameof(ChoosePaymentAmount),       // The name of the dependency property
              typeof(decimal),           // The type of the property
              typeof(PaymentScreen),    // The owner type
              new PropertyMetadata(default(decimal)));  // Default value

        // CLR property wrapper
        public decimal ChoosePaymentAmount
        {
            get => (decimal)GetValue(ChoosePaymentAmountProperty);
            set => SetValue(ChoosePaymentAmountProperty, value);
        }
        public List<Card> ListCard { get; set; }
        public List<SuggestMoney> ListSuggestMoney { get; set; }

        public Card SelectedCard { get; set; }

        public SuggestMoney SelectedSuggestMoney { get; set; }
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
            this.DataContext = this;
        }
        async public Task LoadData()
        {
            await LoadCurrenOrder();
            await LoadOrderDetail();
            LoadListCard();
            CalculateAmountToPay();

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
            SelectedCard = listCard.FirstOrDefault();
            BuidListSuggestMoney();
            lvCard.SelectedIndex = 0;
        }

        public void BuidListSuggestMoney()
        {
            var listSuggest = new List<SuggestMoney>();
            switch (SelectedCard.CardType)
            {
                
                case EnumCardType.Cash:
                    var listMoney = CommonFunction.SuggestPayment(CurrentOrder.RemainAmount.GetValueOrDefault(),5);
                    foreach (var money in listMoney)
                    {
                        if (money == CurrentOrder.RemainAmount.GetValueOrDefault())
                        {
                            listSuggest.Add(new SuggestMoney() { Amount = money ,IsSelected=true});
                        }
                        else
                        {
                            listSuggest.Add(new SuggestMoney() { Amount = money });
                        }
                    }
                   
                  
                    break;
                case EnumCardType.Card:
                    listSuggest.Add(new SuggestMoney() { Amount = CurrentOrder?.RemainAmount.GetValueOrDefault() , IsSelected = true });
                    break;
                case EnumCardType.Vemmo:
                    listSuggest.Add(new SuggestMoney() { Amount = CurrentOrder?.RemainAmount.GetValueOrDefault() , IsSelected = true });
                    break;
                case EnumCardType.OtherCard:
                    listSuggest.Add(new SuggestMoney() { Amount = CurrentOrder?.RemainAmount.GetValueOrDefault(), IsSelected = true });
                    break;
            }
            ListSuggestMoney = listSuggest;
            lvSuggestMoney.ItemsSource = null;
            SelectedSuggestMoney = ListSuggestMoney.FirstOrDefault();
            CalculateChoosePaymentAmount();
            lvSuggestMoney.ItemsSource = ListSuggestMoney;
        }


        private async void btnPay_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ProcessPayment();
        }

        public async Task ProcessPayment()
        {
            var isBuildCoverOK = await BuidCoverOrder();
            var isBuildinvoiceOK = await BuidInvoice();
            if (isBuildCoverOK && isBuildinvoiceOK)
            {
                List<object> lstSaveOrder = new List<object>();
                lstSaveOrder.Add(CurrentOrder);
                CurrentOrder.OrderStatus = EnumOrderStatus.Fired;
                HttpResponseMessage response = await orderService.SaveAndUpdateData(lstSaveOrder);
                if (response != null && response.IsSuccessStatusCode)
                {
                    CommonFunctionUI.ShowToast("Payment success.");
                    CommonFunctionUI.NavigateToPage(AppPage.MainScreen, previousPage: AppPage.MainScreen);
                }
            }

        }

        public async Task<bool> BuidCoverOrder()
        {
            List<object> lstSaveOrder = new List<object>();
            CoverOrder coverOrder = new CoverOrder();
            coverOrder.EntityMode = 0;
            coverOrder.CoverOrderId = Guid.NewGuid();
            coverOrder.CoverAmount = SelectedSuggestMoney.Amount;
            coverOrder.OrderId = CurrentOrder.OrderId;
            coverOrder.OrderNo = CurrentOrder.OrderNo;
            coverOrder.UserId = Session.UserID.GetValueOrDefault();
            coverOrder.UserName = Session.UserName;
            coverOrder.TransactionID = Guid.NewGuid().ToString();
            coverOrder.CardType = SelectedCard.CardType;
            coverOrder.CardName = SelectedCard.CardName;

            lstSaveOrder.Add(coverOrder);
            HttpResponseMessage response = await coverOrderService.SaveAndUpdateData(lstSaveOrder);
            if (response != null && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> BuidInvoice()
        {
            if (SelectedSuggestMoney.Amount == CurrentOrder.Amount)
            {
                CurrentOrder.PaymentStatus = EnumPaymentStatus.PaidAll;
                List<object> lstSaveOrder = new List<object>();
                Invoice invoice = new Invoice();
                invoice.EntityMode = 0;
                invoice.InvoiceId = Guid.NewGuid();
                invoice.Amount = SelectedSuggestMoney.Amount.GetValueOrDefault();
                invoice.OrderId = CurrentOrder.OrderId.GetValueOrDefault();
                invoice.OrderNo = CurrentOrder.OrderNo;
                invoice.UserId = Session.UserID.GetValueOrDefault();
                invoice.UserName = Session.UserName;
                invoice.OrderType = CurrentOrder.OrderType;
                invoice.TableID = CurrentOrder.TableID;
                invoice.TableName = CurrentOrder.TableName;



                lstSaveOrder.Add(invoice);
                HttpResponseMessage response = await invoiceService.SaveAndUpdateData(lstSaveOrder);
                if (response != null && response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            return true;
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
                    BuidListSuggestMoney();
                }
            }
        }

        private void lvCard_Loaded(object sender, RoutedEventArgs e)
        {
            if (lvCard.Items?.Count > 0)
            {
                // Get the first item in the ListView
                var firstItem = lvCard.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                if (firstItem != null)
                {
                    // Set focus to the first item
                    firstItem.Focus();
                    BuidListSuggestMoney();

                }
            }
        }

        private void lvSuggestMoney__Loaded(object sender, RoutedEventArgs e)
        {
            if (lvSuggestMoney.Items?.Count > 0)
            {
                // Get the first item in the ListView
                var firstItem = lvSuggestMoney.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                if (firstItem != null)
                {
                    // Set focus to the first item
                    firstItem.Focus();

                }
            }
        }

        private void lvSuggestMoney_click(object sender, MouseButtonEventArgs e)
        {
            // Find the clicked item
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as SuggestMoney;
                if (clickedItem != null)
                {
                    SelectedSuggestMoney = clickedItem;
                    foreach (var item in ListSuggestMoney)
                    {
                        item.IsSelected = false;
                    }
                    clickedItem.IsSelected = true;
                    CalculateChoosePaymentAmount();
                }
            }
        }
        private void CalculateAmountToPay()
        {
            if (CurrentOrder != null)
            {
                AmountToPay = CurrentOrder.RemainAmount.GetValueOrDefault();
            }
        }

        private void CalculateChoosePaymentAmount()
        {
            if (SelectedSuggestMoney != null)
            {
                ChoosePaymentAmount = SelectedSuggestMoney.Amount.GetValueOrDefault();
            }
        }
    }
}
