﻿using Newtonsoft.Json.Linq;
using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using QLNH.GR.Desktop.UI.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ChooseTable.xaml
    /// </summary>
    public partial class ChooseTable : BaseUserControl
    {
        public List<Order> ListOrder { get; set; }

        public OrderService orderService = new OrderService();

        public TableService tableService = new TableService();

        public AreaService areaService = new AreaService();
        public List<BO.Table> ListTable { get; set; }
        public List<Area> ListArea { get; set; }

        public Area RecentArea { get; set; }

        public ChooseTable()
        {
            InitializeComponent();

        }


        public override async void ProcessDataAsync()
        {
            await LoadArea();
            await LoadOrder();
            await LoadTable();
        }

        async private Task LoadArea()
        {
            var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
            List<SortObject> lstsort = new List<SortObject>() {
            new SortObject() { Property = "createdDate", SortBy = 0 }
            };

            pag.SortObjects = lstsort;
            HttpResponseMessage response = await areaService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListArea = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Area>>(result.Data.ToString());
                    lvArea.ItemsSource = ListArea;
                    RecentArea = ListArea.FirstOrDefault();
                    lvArea.SelectedIndex = 0;
                }
            }
        }

        async private Task LoadTable()
        {
            var pag = new PaginationObject() { PageSize = 100, RecentPage = 1 };
            pag.FilterObjects = new List<FilterObject>() { new FilterObject() { Property = "AreaID", Value = RecentArea?.AreaID, PropertyType = 4, Operator = 1, RelationType = 0 } };
            HttpResponseMessage response = await tableService.Filter(pag);
            if (response != null && response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the response body to the specified type
                PagingHttpResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<PagingHttpResponse>(responseBody);
                if (result != null)
                {
                    ListTable = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BO.Table>>(result.Data.ToString());
                    foreach (var table in ListTable)
                    {
                        var order = ListOrder.Find(item=>item.TableID == table.TableID);
                        if(order != null)
                        {
                            table.IsServing = true;
                            // Current time
                            DateTime now = DateTime.Now;

                            // Calculate the time difference
                            TimeSpan timeDifference = now - order.CreatedDate.GetValueOrDefault();

                            // Format the time difference to HH:mm
                            string formattedTimeDifference = $"{(int)timeDifference.TotalHours}h:{timeDifference.Minutes:D2}m";
                            
                            table.ServingTime = formattedTimeDifference;
                        }
                    }
                    lvTable.ItemsSource = ListTable;
                }
            }
        }

        private void Return_Click(object sender, MouseButtonEventArgs e)
        {
            CommonFunctionUI.NavigateToPage(AppPage.MainScreen, previousPage: AppPage.Table);
        }

        private void Table_click(object sender, MouseButtonEventArgs e)
        {

            var listView = sender as ListView;
            var clickedItem = (BO.Table)listView?.SelectedItem;

            // If the clickedItem is null, it means no ListViewItem was clicked.
            if (clickedItem != null)
            {
                if (clickedItem.IsServing)
                {
                    CommonFunctionUI.ShowToast("Table is serving by other order.",ToastType.Warning);
                    return;
                }
                Dictionary<string, object> navigateDictionary = new Dictionary<string, object>();
                navigateDictionary.Add("TableID", clickedItem.TableID);
                navigateDictionary.Add("TableName", clickedItem.TableName);
                navigateDictionary.Add("IsCreateNewOrder", true);
                Session.IsCreateNewOrder = true;
                Session.TableName = clickedItem.TableName;
                Session.TableID = clickedItem.TableID;
                Session.SelectingOrderType = EnumOrderType.DineIn;
                CommonFunctionUI.NavigateToPage(AppPage.Order, previousPage: AppPage.Table, navigateDictionary);
            }
        }

        private async void Area_click(object sender, MouseButtonEventArgs e)
        {

            var listView = sender as ListView;
            var clickedItem = (Area)listView?.SelectedItem;

            // If the clickedItem is null, it means no ListViewItem was clicked.
            if (clickedItem != null)
            {
                RecentArea = clickedItem;
                await LoadTable();
            }

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
                }
            }
        }

    }
}
