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

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for ProductStock.xaml
    /// </summary>
    public partial class SaleNotification : Page
    {
        private static NotificationService notificationService = NotificationServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;
        
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();

        private CommonPage commonPage;

        public SaleNotification()
        {
            InitializeComponent();

            commonPage = new CommonPage();
            commonPage.Title = "Sale Notification";
            lblSiteMap.Content = commonPage.Title;

            init();
        }
        
        public List<CakeGUI.classes.entity.SaleNotificationEntity> saleNotifications { get; set; }

        private void init()
        {
            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedIndex = 0;

            loadData();
        }

        private void loadData()
        {
            saleNotifications = notificationService.getSaleNotification((ProductTypeEntity)cmbType.SelectedItem);
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = saleNotifications;
        }

        private void ProductNameClicked(object sender, MouseButtonEventArgs e)
        {
            ProductStockEntity cellContent = (ProductStockEntity)dataGrid.SelectedItem;
            GenericWindow windowInventoryList = new GenericWindow();
            ProductInventoryList inventoryListPage = new ProductInventoryList(cellContent.Product);
            inventoryListPage.SetParent(commonPage);

            windowInventoryList.Content = inventoryListPage;
            windowInventoryList.ShowDialog();
        }

        private void SellPriceClicked(object sender, MouseButtonEventArgs e)
        {
            SaleNotificationEntity cellContent = (SaleNotificationEntity)dataGrid.SelectedItem;
            
            GenericWindow windowSellPriceHistory = new GenericWindow();
            SellPriceHistory sellPriceHistoryPage = new SellPriceHistory(cellContent.Product);
            sellPriceHistoryPage.SetParent(commonPage);
            sellPriceHistoryPage.setAvgBuyPrice(cellContent.BuyPrice);

            windowSellPriceHistory.Content = sellPriceHistoryPage;
            windowSellPriceHistory.ShowDialog();
            loadData();
        }

        private string headerAlertDate = "Date";
        public string HeaderAlertDate
        {
            get { return headerAlertDate; }
            set { headerAlertDate = value; }
        }
        
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
            if (type != null)
            {
                dataGrid.Columns[2].Header = type.Expiration ? "Expired Date" : "Aging Date";
                //dataGrid.Columns[6].Header = type.Expiration ? "Expiry Notif" : "Aging Notif";
                loadData();
            }
        }

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
                lblSiteMap.Content = commonPage.TitleSiteMap;
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblSiteMap.Content = commonPage.TitleSiteMap;
        }
    }
}
