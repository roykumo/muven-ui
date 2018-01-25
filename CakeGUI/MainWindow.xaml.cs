using System;
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
using CakeGUI.forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CakeGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            initMenu();
            commonPageOut.Title = "Barang Keluar";
            commonPageIn.Title = "Barang Masuk";
            commonPageReport.Title = "Ringkasan Laporan";
            commonPageItem.Title = "Daftar Barang";
        }

        CommonPage commonPageOut = new CommonPage();
        CommonPage commonPageIn = new CommonPage();
        CommonPage commonPageReport = new CommonPage();
        CommonPage commonPageItem = new CommonPage();

        private void initMenu()
        {
            try
            {
                var actions = new Dictionary<string, Func<MenuItem, RoutedEventHandler>>()
                {
                    { "Exit", mi => (s, e) => { closeApp(); }},
                    { "Product", mi => (s, e) => { loadProductList(); }},
                    { "ProductCategory", mi => (s, e) => { loadProductCategory(); }},
                    { "ProductStock", mi => (s, e) => { loadProductStock(); }},
                    { "Inventory", mi => (s, e) => { loadNewInventory(); }},
                    { "SaleNotification", mi => (s, e) => { loadSaleNotification(); }},
                    { "Repacking", mi => (s, e) => { loadInventoryOut("RE"); }},
                    { "StockOpname", mi => (s, e) => { loadInventoryOut("ST"); }},
                    { "CashRegister", mi => (s, e) => { loadCashRegister(); }},
                    { "ProductDump", mi => (s, e) => { loadProductDump(); }},                
                    { "OnlineTransaction", mi => (s, e) => { loadOnlineTransaction(); }},
                    { "TransactionList", mi => (s, e) => { loadTransactionList(); }},
                    { "Alert", mi => (s, e) => { loadAlertNotification(); }}
                };

                foreach (MenuItem mi in mainMenu.Items)
                {
                    if (actions.ContainsKey(mi.Name))
                    {
                        mi.Click += actions[mi.Name](mi);
                    }
                    if (mi.Items != null && !mi.Items.IsEmpty)
                    {
                        foreach (MenuItem miChild in mi.Items)
                        {
                            if (actions.ContainsKey(miChild.Name))
                            {
                                miChild.Click += actions[miChild.Name](miChild);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
                
        private void ContextMenuClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string menuName = ((MenuItem)sender).Name;
                if (menuName.Equals("Exit"))
                {
                    closeApp();
                }
                else if (menuName.Equals("Product"))
                {
                    loadProductList();
                }
                else if (menuName.Equals("ProductStock"))
                {
                    loadProductStock();
                }
                else if (menuName.Equals("Inventory"))
                {
                    loadNewInventory();
                }
                //MessageBox.Show(string.Format("MyContent:{0}", ((MenuItem)sender).Name));
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void closeApp()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void refreshFrame()
        {
            Page currentPage = (Page)mainFrame.Content;
            Type typeProductList = typeof(ProductList);
            Type currentType = currentPage.GetType();
            if(currentType.Equals(typeProductList)){
                mainFrame.Content = null;
                loadProductList();
                mainFrame.NavigationService.Refresh();
            }
            mainFrame.Refresh();
        }
        
        private void loadProductList()
        {
            ProductList productList = new ProductList();
            productList.Tag = this;
            productList.SetParent(commonPageItem);
            this.mainFrame.Content = productList;
        }

        private void loadProductCategory()
        {
            ProductCategoryList productCategoryList = new ProductCategoryList();
            productCategoryList.Tag = this;
            productCategoryList.SetParent(commonPageItem);
            this.mainFrame.Content = productCategoryList;
        }

        public void loadProductStock()
        {
            ProductStock productStock = new ProductStock();
            productStock.Tag = this;
            productStock.SetParent(commonPageItem);
            this.mainFrame.Content = productStock;
        }

        private void loadNewInventory()
        {
            NewInventory newInventory = new NewInventory();
            newInventory.SetParent(commonPageIn);
            newInventory.Tag = this;
            this.mainFrame.Content = newInventory;
        }

        private void loadSaleNotification()
        {
            SaleNotification saleNotification = new SaleNotification();
            saleNotification.SetParent(commonPageReport);
            saleNotification.Tag = this;
            this.mainFrame.Content = saleNotification;
        }

        private void loadInventoryOut(string type)
        {
            InventoryOut inventoryOut = new InventoryOut(type);
            inventoryOut.Tag = this;
            inventoryOut.SetParent(commonPageIn);

            this.mainFrame.Content = inventoryOut;
        }

        private void loadOnlineTransaction()
        {
            OnlineTransaction onlineTrx = new OnlineTransaction("SA");
            onlineTrx.Tag = this;
            onlineTrx.SetParent(commonPageOut);

            this.mainFrame.Content = onlineTrx;
        }

        private void loadCashRegister()
        {

            CashRegister cashRegister = new CashRegister("CR");
            cashRegister.Tag = this;
            cashRegister.SetParent(commonPageOut);

            this.mainFrame.Content = cashRegister;
        }

        private void loadProductDump()
        {
            Damage cashRegister = new Damage("EX");
            cashRegister.Tag = this;
            cashRegister.SetParent(commonPageOut);

            this.mainFrame.Content = cashRegister;
        }

        private void loadAlertNotification()
        {
            StatusNotification statusNotification = new StatusNotification();
            statusNotification.SetParent(commonPageReport);
            statusNotification.Tag = this;
            this.mainFrame.Content = statusNotification;
        }

        private void loadTransactionList()
        {
            TransactionList trxList = new TransactionList();
            trxList.SetParent(commonPageReport);
            trxList.Tag = this;
            this.mainFrame.Content = trxList;
        }

        public void setLabelTitle(string title)
        {
            //lblSiteMap.Content = title;
        }
        
    }
}
