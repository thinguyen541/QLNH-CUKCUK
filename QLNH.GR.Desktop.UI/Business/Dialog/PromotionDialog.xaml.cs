
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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ModifierDialog.xaml
    /// </summary>
    public partial class PromotionDialog : BaseUserControl
    {
        public Order CurrentOrder { get; set; }

        public Dish SelectedDish { get; set; }

        public List<Promotion> ListPromtion { get; set; }  
        public Promotion SelectedPromtion { get; set; }  

        public PromotionService promotionService = new PromotionService();

        public event EventHandler<bool?> DialogResultEvent;
        public PromotionDialog()
        {
            InitializeComponent();
            txtTilte.Text = DialogTitle;
        }

        private void lvPromotion_click(object sender, MouseButtonEventArgs e)
        {

        }

        public async override void ProcessDataAsync()
        {
            base.ProcessDataAsync();
            lvPromotion.ItemsSource = ListPromtion; 
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
                        foreach(var promotion in lstPro)
                        {
                            if (CheckValidPromotion(promotion))
                            {
                                ListPromtion.Add(promotion);
                            }
                        }
                    }
                   
                    lvPromotion.ItemsSource = ListPromtion;
                }
            }
        }

        public bool CheckValidPromotion(Promotion promotion)
        {
            var result = false;
            if(promotion != null)
            {
                if(promotion.AmountConditionType  == EnumAmountConditionType.GreaterThan && promotion.AmountCondition >= 0)
                {
                    if(CurrentOrder.Amount >= promotion.AmountCondition)
                    {
                        return true;
                    }
                }
                else if (promotion.AmountConditionType == EnumAmountConditionType.LessThan && promotion.AmountCondition >= 0)
                {
                    if (CurrentOrder.Amount <= promotion.AmountCondition)
                    {
                        return true;
                    }
                }
                else if (promotion.AmountConditionType == EnumAmountConditionType.Equal && promotion.AmountCondition >= 0)
                {
                    if (CurrentOrder.Amount == promotion.AmountCondition)
                    {
                        return true;
                    }
                }
            }
            return result;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Popup popup)
            {
                BuildPromotion();
                CalculateAmountToPay();
                DialogResult = true;
                DialogResultEvent?.Invoke(this, true);
                popup.IsOpen = false;
            }
        }

        private void BuildPromotion()
        {
            var selectedPromotion = ListPromtion.FirstOrDefault(item => item.IsSelected == true);
            var oldPromotion = CurrentOrder.ListOrderDetail.FirstOrDefault(item => item.OrderDetailType == EnumOrderDetailType.Promotion);
            if (oldPromotion != null)
            {
                CurrentOrder.ListOrderDetail.Remove(oldPromotion);
            }
            CalculateAmountToPay();
            if (selectedPromotion != null)
            {
                var promotionOrderDetail = new OrderDetail();
                promotionOrderDetail.OrderDetailType = EnumOrderDetailType.Promotion;
                if (selectedPromotion.PromotionValueType == EnumPromotionValueType.Amount)
                {
         
                    if (CurrentOrder != null && CurrentOrder.Amount >= 0)
                    {
                        if (CurrentOrder.Amount >= selectedPromotion.AmountValue)
                        {
                            promotionOrderDetail.Amount = 0 - selectedPromotion.AmountValue.GetValueOrDefault();
                        }
                        else
                        {
                            promotionOrderDetail.Amount = 0 - CurrentOrder.Amount;
                        }
                    }
                  
                }
                else if (selectedPromotion.PromotionValueType == EnumPromotionValueType.Percentage)
                {
                    if (CurrentOrder.Amount >= 0)
                    {
                        var AmountValue = 0 - (selectedPromotion.AmountValue.GetValueOrDefault() / 100 * CurrentOrder.Amount.GetValueOrDefault());
                        if (CurrentOrder.Amount >= AmountValue)
                        {
                            promotionOrderDetail.Amount = AmountValue;
                        }
                        else
                        {
                            promotionOrderDetail.Amount = 0 - CurrentOrder.Amount;
                        }
                    }
                }
                if(CurrentOrder != null && CurrentOrder.ListOrderDetail != null)
                {
                    CurrentOrder.PromotionId = selectedPromotion.PromotionId;
                    CurrentOrder.PromotionAmount = promotionOrderDetail.Amount;
                    CurrentOrder.PromotionName = selectedPromotion.PromotionName;
                    CurrentOrder.ListOrderDetail.Add(promotionOrderDetail);
                }
            }
        }
        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Popup popup)
            {
                DialogResult = false;
                DialogResultEvent?.Invoke(this, false);
                popup.IsOpen = false;
            }
        }

        private void check_promotion(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Promotion selectedPromotion)
            {
                if (ListPromtion.Count(item => item.IsSelected) > 1)
                {
                  
                    myToast.ShowToast("Hãy bỏ chọn nếu muốn chọn khuyến mại khác", ToastType.Warning);
                    selectedPromotion.IsSelected = false;
                    return;
                }


            }
        }

        private void CalculateAmountToPay()
        {
            if (CurrentOrder != null && CurrentOrder.ListOrderDetail != null)
            {
                CurrentOrder.Amount = CurrentOrder.ListOrderDetail.Sum(item=>item.Amount.GetValueOrDefault());
                CurrentOrder.RemainAmount = CurrentOrder.Amount;
            }
        }
    }
}
