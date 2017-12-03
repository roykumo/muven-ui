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
        }

        private void initMenu()
        {
            var actions = new Dictionary<string, Func<MenuItem, RoutedEventHandler>>()
            {
                { "Exit", mi => (s, e) => { closeApp(); }},
                { "Product", mi => (s, e) => { loadProductList(); }},
                { "ProductStock", mi => (s, e) => { loadProductStock(); }},
                { "Inventory", mi => (s, e) => { loadNewInventory(); }},
                { "SaleNotification", mi => (s, e) => { loadSaleNotification(); }},
                { "Repacking", mi => (s, e) => { loadInventoryOut("RP"); }},
                { "StockOpname", mi => (s, e) => { loadInventoryOut("SO"); }}
            };

            foreach (MenuItem mi in mainMenu.Items)
            {
                if (actions.ContainsKey(mi.Name))
                {
                    mi.Click += actions[mi.Name](mi);
                }
                if(mi.Items!=null && !mi.Items.IsEmpty)
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
                
        private void ContextMenuClick(object sender, RoutedEventArgs e)
        {
            string menuName = ((MenuItem)sender).Name;
            if (menuName.Equals("Exit"))
            {
                closeApp();
            }
            else if(menuName.Equals("Product"))
            {
                loadProductList();
            }
            else if(menuName.Equals("ProductStock"))
            {
                loadProductStock();
            }
            else if (menuName.Equals("Inventory"))
            {
                loadNewInventory();
            }
            //MessageBox.Show(string.Format("MyContent:{0}", ((MenuItem)sender).Name));
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
            CommonPage commonPage = new CommonPage();
            commonPage.Title = "List Item";

            ProductList productList = new ProductList();
            productList.Tag = this;
            productList.SetParent(commonPage);
            this.mainFrame.Content = productList;
        }

        private void loadProductStock()
        {
            CommonPage commonPage = new CommonPage();
            commonPage.Title = "List Item";

            ProductStock productStock = new ProductStock();
            productStock.Tag = this;
            productStock.SetParent(commonPage);
            this.mainFrame.Content = productStock;
        }

        private void loadNewInventory()
        {
            CommonPage commonPage = new CommonPage();
            commonPage.Title = "Stock In";

            NewInventory newInventory = new NewInventory();
            newInventory.SetParent(commonPage);
            newInventory.Tag = this;
            this.mainFrame.Content = newInventory;
        }

        private void loadSaleNotification()
        {
            CommonPage commonPage = new CommonPage();
            commonPage.Title = "Notification";

            SaleNotification saleNotification = new SaleNotification();
            saleNotification.SetParent(commonPage);
            saleNotification.Tag = this;
            this.mainFrame.Content = saleNotification;
        }

        private void loadInventoryOut(string type)
        {
            CommonPage commonPage = new CommonPage();
            commonPage.Title = "Stock In";

            InventoryOut inventoryOut = new InventoryOut(type);
            inventoryOut.Tag = this;
            inventoryOut.SetParent(commonPage);

            this.mainFrame.Content = inventoryOut;
        }

        public void setLabelTitle(string title)
        {
            //lblSiteMap.Content = title;
        }
        
    }
}
