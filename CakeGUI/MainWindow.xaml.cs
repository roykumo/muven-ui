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
        }

        ProductList productList = new ProductList();
        Product product = new Product();
        ProductStock productStock = new ProductStock();
        NewInventory newInventory = new NewInventory();

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
            productList.Tag = this;
            this.mainFrame.Content = productList;
        }

        private void loadProductStock()
        {
            productStock.Tag = this;
            this.mainFrame.Content = productStock;
        }

        private void loadNewInventory()
        {
            newInventory.Tag = this;
            this.mainFrame.Content = newInventory;
        }
    }
}
