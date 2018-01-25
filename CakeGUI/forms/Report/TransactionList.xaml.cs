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
    public partial class TransactionList : Page
    {
        private static TransactionService transactionService = TransactionServiceRestImpl.Instance;
        
        private List<TransactionEntity> productTypes = new List<TransactionEntity>();

        private CommonPage commonPage;

        public TransactionList()
        {
            InitializeComponent();

            commonPage = new CommonPage();
            commonPage.Title = "Daftar Transaksi";
            lblSiteMap.Content = commonPage.Title;

            init();
        }
        
        public List<CakeGUI.classes.entity.TransactionEntity> transactionList { get; set; }

        private void init()
        {
            try
            {
                

                loadData();
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
                transactionList = transactionService.getTransactionList();
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = transactionList;
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
                ProductStockEntity cellContent = (ProductStockEntity)dataGrid.SelectedItem;
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

        private void SellPriceClicked(object sender, MouseButtonEventArgs e)
        {
            try
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
                    dataGrid.Columns[2].Header = type.Expiration ? "Expired Date" : "Aging Date";
                    //dataGrid.Columns[6].Header = type.Expiration ? "Expiry Notif" : "Aging Notif";
                    loadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
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
