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
    public partial class TransactionList : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TransactionService transactionService = TransactionServiceRestImpl.Instance;
        private static ProductInventoryOutService productInventoryOutService = ProductInventoryOutServiceRestImpl.Instance;
        private static ProductInventoryService productInventoryService = ProductInventoryServiceRestImpl.Instance;

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
                cmbMonth.ItemsSource = Utils.ListMonth;

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

        private void TransactionCodeClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                /*ProductStockEntity cellContent = (ProductStockEntity)dataGrid.SelectedItem;
                GenericWindow windowInventoryList = new GenericWindow();
                ProductInventoryList inventoryListPage = new ProductInventoryList(cellContent.Product);
                inventoryListPage.SetParent(commonPage);

                windowInventoryList.Content = inventoryListPage;
                windowInventoryList.ShowDialog();*/



                TransactionEntity transactionEntity = (TransactionEntity)dataGrid.SelectedItem;
                if (transactionEntity.Type.Equals("CASH REGISTER"))
                {
                    InventoryOutEntity inventoryOut = productInventoryOutService.getById(transactionEntity.Id);
                    if (inventoryOut != null)
                    {
                        CashRegister cashRegister = new CashRegister(inventoryOut, true);
                        
                        GenericWindow windowInventoryList = new GenericWindow();
                        cashRegister.SetParent(commonPage);
                        windowInventoryList.Content = cashRegister;
                        windowInventoryList.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Transaction data");
                    }
                }
                else if(transactionEntity.Type.Equals("PEMBELIAN"))
                {
                    InventoryEntity inventory = productInventoryService.getById(transactionEntity.Id);
                    if (inventory != null)
                    {
                        NewInventory newInventory = new NewInventory(inventory, true);

                        GenericWindow windowInventoryList = new GenericWindow();
                        newInventory.SetParent(commonPage);
                        windowInventoryList.Content = newInventory;
                        windowInventoryList.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Transaction data");
                    }
                }
                else if (transactionEntity.Type.Equals("REPACKING") || transactionEntity.Type.Equals("STOCK OPNAME"))
                {
                    InventoryOutEntity inventoryOut = productInventoryOutService.getById(transactionEntity.Id);
                    if (inventoryOut != null)
                    {
                        InventoryOut dataOut = new InventoryOut(inventoryOut, true);

                        GenericWindow windowInventoryList = new GenericWindow();
                        dataOut.SetParent(commonPage);
                        windowInventoryList.Content = dataOut;
                        windowInventoryList.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Transaction data");
                    }
                }
                else if (transactionEntity.Type.Equals("PENJUALAN ONLINE"))
                {
                    InventoryOutEntity inventoryOut = productInventoryOutService.getById(transactionEntity.Id);
                    if (inventoryOut != null)
                    {
                        OnlineTransaction dataOut = new OnlineTransaction(inventoryOut, true);

                        GenericWindow windowInventoryList = new GenericWindow();
                        dataOut.SetParent(commonPage);
                        windowInventoryList.Content = dataOut;
                        windowInventoryList.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Transaction data");
                    }
                }
                else if (transactionEntity.Type.Equals("PENGHAPUSAN"))
                {
                    InventoryOutEntity inventoryOut = productInventoryOutService.getById(transactionEntity.Id);
                    if (inventoryOut != null)
                    {
                        Damage dataOut = new Damage(inventoryOut, true);

                        GenericWindow windowInventoryList = new GenericWindow();
                        dataOut.SetParent(commonPage);
                        windowInventoryList.Content = dataOut;
                        windowInventoryList.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("No Transaction data");
                    }
                }
                else
                {
                    MessageBox.Show("Not implemented yet");
                }

            }
            catch (Exception ex)
            {
                log.Error("error");
                log.Error(ex);
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
        
        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                /*ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    dataGrid.Columns[2].Header = type.Expiration ? "Expired Date" : "Aging Date";
                    //dataGrid.Columns[6].Header = type.Expiration ? "Expiry Notif" : "Aging Notif";
                    loadData();
                }*/
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
