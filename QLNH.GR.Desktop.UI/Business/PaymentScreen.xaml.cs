using Microsoft.Win32;
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

        public List<Promotion> ListPromtion { get; set; } = new List<Promotion>();

        public CoverOrderService coverOrderService = new CoverOrderService();

        public InvoiceService invoiceService = new InvoiceService();

        public DishGroupService dishGroupService = new DishGroupService();

        public OrderService orderService = new OrderService();

        public OrderDetailService orderDetailService = new OrderDetailService();

        public DetailItemService detailItemService = new DetailItemService();

        public OrderService rrderService = new OrderService();
        public PromotionService promotionService = new PromotionService();


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
        public List<SuggestMoney> ListTip { get; set; }

        public Card SelectedCard { get; set; }

        public SuggestMoney SelectedSuggestMoney { get; set; }
        public SuggestMoney SelectedTip { get; set; }
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
            await LoadPromotion();
            LoadListCard();
            CalculateAmountToPay();

        }


        async private Task LoadOrderDetail()
        {
            //load orderDetail
            lvOrderDetail.ItemsSource = CurrentOrder?.ListOrderDetail?.Where(item => item.EntityMode != 2);
        }

        async private Task LoadPromotion()
        {
            var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
            List<SortObject> lstsort = new List<SortObject>() {
            new SortObject() { Property = "createdDate", SortBy = 0 }
            };

            pag.SortObjects = lstsort;
            HttpResponseMessage response = await promotionService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    var lstPro = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Promotion>>(result.Data.ToString());
                    if (lstPro != null)
                    {
                        foreach (var promotion in lstPro)
                        {
                            if (CheckValidPromotion(promotion))
                            {
                                ListPromtion.Add(promotion);
                                foreach (var promo in ListPromtion)
                                {
                                    if(promo.PromotionId == CurrentOrder.PromotionId){
                                        promo.IsSelected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            CalculateOrderAmount();
        }

        public bool CheckValidPromotion(Promotion promotion)
        {
            var result = false;
            if ((promotion.StartTime !=  null && promotion.StartTime > DateTime.Now) || (promotion.EndTime != null && promotion.EndTime< DateTime.Now))
            {
                return false;
            }
            if (promotion != null)
            {
                if (promotion.AmountConditionType == EnumAmountConditionType.GreaterThan)
                {
                    if (CurrentOrder.Amount >= promotion.AmountCondition || promotion.AmountCondition == null)
                    {
                        return true;
                    }
                }
                else if (promotion.AmountConditionType == EnumAmountConditionType.LessThan)
                {
                    if (CurrentOrder.Amount <= promotion.AmountCondition || promotion.AmountCondition == null)
                    {
                        return true;
                    }
                }
                else if (promotion.AmountConditionType == EnumAmountConditionType.Equal)
                {
                    if (CurrentOrder.Amount == promotion.AmountCondition || promotion.AmountCondition == null)
                    {
                        return true;
                    }
                }
            }
            return result;
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
            if (CurrentOrder != null && CurrentOrder.ListOrderDetail != null)
            {
               CurrentOrder.Amount = CurrentOrder.ListOrderDetail.Sum(item => { if (item.EntityMode != 2) return item.Amount; return 0; });
                string convertedValue = (string)_decimalconverter.Convert(CurrentOrder.Amount, typeof(string), null, CultureInfo.InvariantCulture);
                txtSubtotal.Text = convertedValue;
            }
            CalculateOrderAmount();
        }


        public void LoadListCard()
        {
            var listCard = new List<Card>
            {
                new Card() { CardID = Guid.NewGuid(), CardName = "Cash", CardType = EnumCardType.Cash, ImagePath = "C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\money.png" },
                new Card() { CardID = Guid.NewGuid(), CardName = "Card", CardType = EnumCardType.Card, ImagePath = "C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\credit-card.png" } ,
                new Card() { CardID = Guid.NewGuid(), CardName = "Vemmo", CardType = EnumCardType.Vemmo, ImagePath = "C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\Vemmo.png" } ,
                new Card() { CardID = Guid.NewGuid(), CardName = "Other Card", CardType = EnumCardType.OtherCard, ImagePath = "C:\\Đồ án\\QLNH-Thesis\\QLNH.GR.Desktop.UI\\FileRerource\\Resources\\Icon\\atm-card.png" }
            };
            ListCard = listCard;
            lvCard.ItemsSource = ListCard;
            SelectedCard = listCard.FirstOrDefault();
            BuidListSuggestMoney();
            BuidListTipMoney();
            lvCard.SelectedIndex = 0;
        }

        public void BuidListSuggestMoney()
        {
            var listSuggest = new List<SuggestMoney>();
            switch (SelectedCard?.CardType)
            {
                
                case EnumCardType.Cash:
                    listSuggest.Add(new SuggestMoney() { Amount = CurrentOrder?.RemainAmount.GetValueOrDefault(), IsSelected = true });

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

        public void BuidListTipMoney()
        {
            var lstTip = new List<SuggestMoney>();
          
            lstTip.Add(new SuggestMoney() { Amount =0});
            lstTip.Add(new SuggestMoney() { Amount =CurrentOrder?.RemainAmount.GetValueOrDefault()/100*5});
            lstTip.Add(new SuggestMoney() { Amount =CurrentOrder?.RemainAmount.GetValueOrDefault()/100*10 });
            lstTip.Add(new SuggestMoney() { Amount =CurrentOrder?.RemainAmount.GetValueOrDefault()/100*15 });
            lstTip.Add(new SuggestMoney() { Amount =CurrentOrder?.RemainAmount.GetValueOrDefault()/100*20 });
            lstTip.Add(new SuggestMoney() { Amount =CurrentOrder?.RemainAmount.GetValueOrDefault()/100*30 });
            lstTip.Add(new SuggestMoney() { Amount = -1 });
            ListTip = lstTip;
            lvTip.ItemsSource = ListTip;
        }



        private async void btnPay_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await ProcessPayment();
        }

        public async Task ProcessPayment()
        {
            CurrentOrder.OrderStatus = EnumOrderStatus.Fired;
            var isBuildCoverOK = await BuidCoverOrder();
            var isBuildinvoiceOK = await BuidInvoice();
            if (isBuildCoverOK && isBuildinvoiceOK)
            {
                List<object> lstSaveOrder = new List<object>();
                lstSaveOrder.Add(CurrentOrder);
                HttpResponseMessage response = await orderService.SaveAndUpdateData(lstSaveOrder);
                if (response != null && response.IsSuccessStatusCode)
                {
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
            if (SelectedSuggestMoney.Amount == CurrentOrder.RemainAmount)
            {
                CurrentOrder.PaymentStatus = EnumPaymentStatus.PaidAll;
                CurrentOrder.OrderStatus = EnumOrderStatus.Done;
                List<object> lstSaveOrder = new List<object>();
                CommonFunctionUI.ShowToast("Thanh toán thành công");
                SaveFileDialog oDlg = new SaveFileDialog();
                oDlg.Filter = "Pdf files (*.pdf)|*.pdf";
                if (true == oDlg.ShowDialog())
                {
                    string oSelectedFile = "";
                    oSelectedFile = oDlg.FileName;

                    Printer.PrintSendKitchen(CurrentOrder, oSelectedFile);

                }
                foreach (var detail in CurrentOrder.ListOrderDetail)
                {
                    detail.OrderDetailStatus = EnumOrderDetailStatus.Send;
                }
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
                invoice.PromotionName = CurrentOrder.PromotionName;
                invoice.PromotionAmount = CurrentOrder.PromotionAmount;
                




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

                var oldPromotion = CurrentOrder.ListOrderDetail.FirstOrDefault(item => item.OrderDetailType == EnumOrderDetailType.Promotion);
                if (oldPromotion != null)
                {
                    gdTotalDiscount.Visibility = Visibility.Visible;
                    txtTotalDiscount.Text = (string)_decimalconverter.Convert(oldPromotion.Amount, typeof(string), null, CultureInfo.InvariantCulture);
                }
                else
                {
                    gdTotalDiscount.Visibility = Visibility.Collapsed;
                }

                decimal tipAmout = 0;
                if (SelectedTip != null && SelectedTip.Amount > 0)
                {
                    gdTipAmount.Visibility = Visibility.Visible;
                    tipAmout = SelectedTip.Amount.GetValueOrDefault();
                    txtTipAmount.Text = (string)_decimalconverter.Convert(SelectedTip.Amount, typeof(string), null, CultureInfo.InvariantCulture);
                }
                else
                {
                    gdTipAmount.Visibility = Visibility.Collapsed;
                }
                string convertedValue = (string)_decimalconverter.Convert(CurrentOrder.Amount + tipAmout, typeof(string), null, CultureInfo.InvariantCulture);
                txtTotalAmount.Text = convertedValue;
            }
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
                var oldPromotion = CurrentOrder.ListOrderDetail.FirstOrDefault(item => item.OrderDetailType == EnumOrderDetailType.Promotion);
                if (oldPromotion != null)
                {
                    txtTotalDiscount.Text = (string)_decimalconverter.Convert(oldPromotion.Amount, typeof(string), null, CultureInfo.InvariantCulture);
                }
            }
        }

        private void CalculateChoosePaymentAmount()
        {
            if (SelectedSuggestMoney != null)
            {
                ChoosePaymentAmount = SelectedSuggestMoney.Amount.GetValueOrDefault();
            }
            if (SelectedTip != null)
            {
                ChoosePaymentAmount = SelectedSuggestMoney.Amount.GetValueOrDefault() + SelectedTip.Amount.GetValueOrDefault();
            }
        }

        private void btnPromotion_click(object sender, MouseButtonEventArgs e)
        {
            var dialog = new PromotionDialog();
            dialog.ListPromtion = ListPromtion;
            dialog.DialogTitle = "Promotions";
            dialog.DialogResultEvent += PopupContent_DialogResult;
            dialog.CurrentOrder = CurrentOrder;
            CommonFunctionUI.ShowDialog(dialog);
            if (dialog.DialogResult)
            {
            
            }
        }

        private void PopupContent_DialogResult(object? sender, bool? e)
        {
            CalculateAmountToPay();
            CalculateOrderAmount();
            BuidListSuggestMoney();
        }

        private void lvTip_click(object sender, MouseButtonEventArgs e)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                var clickedItem = originalSource.DataContext as SuggestMoney;
                if (clickedItem != null)
                {
                    SelectedTip = clickedItem;
                    foreach (var item in ListTip)
                    {
                        item.IsSelected = false;
                    }
                    clickedItem.IsSelected = true;
                    CalculateChoosePaymentAmount();
                    CalculateOrderAmount();
                }
            }
        }
    }
}
