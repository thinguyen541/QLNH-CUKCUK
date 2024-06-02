using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ModifierDialog : BaseUserControl
    {
        public Order CurrentOrder { get;set; } 

        public Dish SelectedDish { get;set; } 
        public List<FavoriteService> ListFavoriteService { get; set; }

        public List<FavoriteService> ListSelectdFavoriteService { get; set; }

        public event EventHandler<bool?> DialogResultEvent;
        public ModifierDialog()
        {
            InitializeComponent();
        }

        public override void ProcessDataAsync()
        {
            base.ProcessDataAsync();
            txtTilte.Text = DialogTitle;
            lvFavoriteService.ItemsSource = ListFavoriteService;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent Popup and close it
            if (this.Parent is Popup popup)
            {

                DialogResult = false;
                DialogResultEvent?.Invoke(this, false);
                popup.IsOpen = false;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent Popup and close it
            if (this.Parent is Popup popup)
            {
                List<FavoriteService> selectedListFavor = ListFavoriteService.Where(item => item.IsSelected == true).ToList();
                DialogResult = true;
                AddItemToOrder(SelectedDish, selectedListFavor);
                DialogResultEvent?.Invoke(this, true);
                popup.IsOpen = false;
            }

        
        }

        private void favoriteService_click(object sender, MouseButtonEventArgs e)
        {

        }


        public void AddItemToOrder(Dish dishItem, List<FavoriteService> favoriteServices)
        {
            OrderDetail orderDetail = new OrderDetail();
            DetailItem detailItem = new DetailItem();
            detailItem.DetailItemId = Guid.NewGuid();
            orderDetail.OrderDetailId = Guid.NewGuid();
            orderDetail.OrderId = CurrentOrder.OrderId;
            orderDetail.OrderDetailStatus = EnumOrderDetailStatus.NotSentKitchen;
            detailItem.DishId = dishItem.DishId;
            orderDetail.Quantity = 1;
            detailItem.OrderDetailId = orderDetail.OrderDetailId;
            detailItem.DetailItemType = EnumDetailItemType.Normal;
            orderDetail.Amount += dishItem.Price.GetValueOrDefault();
            detailItem.DishName = dishItem.DishName;
            if (detailItem.Amount == null)
            {
                detailItem.Amount += dishItem.Price.GetValueOrDefault();
            }
            detailItem.Amount = dishItem.Price;
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
                var existOrderDetail = CurrentOrder?.ListOrderDetail?.FirstOrDefault(detail => detail.ListDetailItem != null && detail.ListDetailItem.Any(item => item.DishId == dishItem.DishId) && detail.OrderDetailStatus == EnumOrderDetailStatus.NotSentKitchen && detail.EntityMode != 2 );

                if (existOrderDetail != null && (favoriteServices == null || favoriteServices?.Count == 0))
                {
                    existOrderDetail.Quantity += 1;
                    existOrderDetail.EntityMode = 1;
                    existOrderDetail.Amount += dishItem.Price.GetValueOrDefault();
                    foreach (var detail in existOrderDetail.ListDetailItem)
                    {
                        detail.Amount += dishItem.Price.GetValueOrDefault();
                    }
                }
                else
                {
                    CurrentOrder.ListOrderDetail.Add(orderDetail);
                }

            }

            foreach(var item in favoriteServices)
            {
                var newFavor = new DetailItem();
                newFavor.EntityMode = 0;
                newFavor.DetailItemId = Guid.NewGuid();
                newFavor.DishId = item.FavoriteServiceId;
                newFavor.OrderDetailId = orderDetail.OrderDetailId;
                newFavor.DishName = item.FavoriteServiceName;
                orderDetail.Amount += item.FavoriteServiceCost;
                newFavor.Amount = item.FavoriteServiceCost;
                newFavor.DetailItemType = EnumDetailItemType.Modifier;
                orderDetail.ListDetailItem.Add(newFavor);
            }
        }
    }
}
