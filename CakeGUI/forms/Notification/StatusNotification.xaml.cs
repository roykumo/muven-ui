using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
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
using CakeGUI.classes.service;
using CakeGUI.classes.entity;
using CakeGUI.classes.util;

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for ProductStock.xaml
    /// </summary>
    public partial class StatusNotification : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;
        private static ProductCategoryService productCategoryService = ProductCategoryServiceRestImpl.Instance;

        NotificationService notificationService = NotificationServiceRestImpl.Instance;
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
        private List<ProductCategoryEntity> productCategories = new List<ProductCategoryEntity>();

        private CommonPage commonPage;

        public StatusNotification()
        {
            InitializeComponent();

            commonPage = new CommonPage();
            commonPage.Title = "Stock Barang";
            lblSiteMap.Content = commonPage.Title;

            init();
        }
        
        public List<CakeGUI.classes.entity.StatusNotificationEntity> productStocks { get; set; }
        public classes.entity.ProductEntity product { get; set; }

        private void init()
        {
            try
            {
                //productStocks = getListProduct();

                productTypes = productTypeService.getProductTypes();
                cmbType.ItemsSource = productTypes;
                cmbType.SelectedIndex = 0;

                cmbGroup.ItemsSource = Utils.ListProductGroupWithBlank;

                loadData();
                //if(cmbType.SelectedIndex >= 0 )
                //    this.dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type != null && string.Equals(x.Product.Type.Id, ((ProductTypeEntity)cmbType.SelectedItem).Id));

                //this.dataGrid.ItemsSource = productStocks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed init : "+ex.Message);
            }
        }

        private void loadData()
        {
            try
            {
                productStocks = notificationService.getStatusNotification((ProductTypeEntity)cmbType.SelectedItem, (string)cmbCategory.SelectedValue, txtBarcode.Text, (string)cmbGroup.SelectedValue);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = productStocks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed loadData : "+ex.Message);
            }
        }

        private void ProductNameClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StatusNotificationEntity cellContent = (StatusNotificationEntity)dataGrid.SelectedItem;
                //MessageBox.Show(cellContent.Product.Name);

                GenericWindow windowInventoryList = new GenericWindow();
                ProductInventoryList inventoryListPage = new ProductInventoryList(cellContent.Product);
                inventoryListPage.SetParent(commonPage);

                windowInventoryList.Content = inventoryListPage;
                windowInventoryList.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed show detail : "+ex.Message);
            }
        }

        private void StockClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StatusNotificationEntity cellContent = (StatusNotificationEntity)dataGrid.SelectedItem;

                GenericWindow windowInventoryList = new GenericWindow();
                ProductInventoryList inventoryListPage = new ProductInventoryList(cellContent.Product);
                inventoryListPage.SetParent(commonPage);

                windowInventoryList.Content = inventoryListPage;
                windowInventoryList.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed show stock : "+ex.Message);
            }
        }

        private void BuyPriceClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StatusNotificationEntity cellContent = (StatusNotificationEntity)dataGrid.SelectedItem;
                //MessageBox.Show(cellContent.Product.Name);

                GenericWindow windowInventoryList = new GenericWindow();
                InventoryHistory inventoryHistoryPage = new InventoryHistory(cellContent.Product);
                inventoryHistoryPage.SetParent(commonPage);

                windowInventoryList.Content = inventoryHistoryPage;
                windowInventoryList.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed show harga beli : "+ex.Message);
            }
        }

        private void SellPriceClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StatusNotificationEntity cellContent = (StatusNotificationEntity)dataGrid.SelectedItem;

                GenericWindow windowSellPriceHistory = new GenericWindow();
                SellPriceHistory sellPriceHistoryPage = new SellPriceHistory(cellContent.Product);
                sellPriceHistoryPage.SetParent(commonPage);
                sellPriceHistoryPage.setAvgBuyPrice(cellContent.BuyPrice);

                windowSellPriceHistory.Content = sellPriceHistoryPage;
                windowSellPriceHistory.ShowDialog();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed show harga jual : "+ex.Message);
            }
        }

        private string headerAlertDate = "Date";
        public string HeaderAlertDate
        {
            get { return headerAlertDate; }
            set { headerAlertDate = value; }
        }
        
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    initCategories(type);
                    dataGrid.Columns[2].Header = type.Expiration ? "Batasan Kadaluarsa" : "Batasan Aging";
                    //dataGrid.Columns[6].Header = type.Expiration ? "Batasan Kadaluarsa" : "Batasan Aging";
                    loadData();
                    //dataGrid.ItemsSource = null;
                    //dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type!=null && string.Equals(x.Product.Type.Id, type.Id));
                    //MessageBox.Show("change");
                    //lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
            }
        }
        
        private void initCategories(ProductTypeEntity type)
        {
            productCategories = productCategoryService.getProductCategoriesByType(type, true);
            if (productCategories != null && productCategories.Count > 0)
            {
                ProductCategoryEntity blankCategory = new ProductCategoryEntity();
                blankCategory.Id = "";
                blankCategory.Code = "ALL";
                productCategories.Insert(0, blankCategory);
            }
            cmbCategory.ItemsSource = productCategories;
        }

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            /*try
            {
                DataGridRow gridRow = e.Row;
                StatusNotificationEntity stock = (StatusNotificationEntity)gridRow.Item;

                dataGrid.Columns[4].CellStyle = new Style(typeof(DataGridCell));
                dataGrid.Columns[4].CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(stock != null ? stock.AlertColor : Colors.Transparent)));

                if (stock != null && stock.SellPrice != null)
                {
                    dataGrid.Columns[7].CellStyle = new Style(typeof(DataGridCell));
                    dataGrid.Columns[7].CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(stock.SellPrice.Sale ? Colors.Blue : Colors.Transparent)));
                }
            }catch {  }*/
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblSiteMap.Content = commonPage.TitleSiteMap;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = cmbType.SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    dataGrid.Columns[2].Header = type.Expiration ? "Batasan Kadaluarsa" : "Batasan Aging";
                    //dataGrid.Columns[6].Header = type.Expiration ? "Batasan Kadaluarsa" : "Batasan Aging";
                }
                else
                {
                    dataGrid.Columns[2].Header = "Expired / Aging Date";
                    //dataGrid.Columns[6].Header = "Expiry / Aging Notif";
                }
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed search : "+ex.Message);
            }
        }
        
    }
}
