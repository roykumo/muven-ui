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
    public partial class ProductStock : Page
    {
        private static ProductService productService = ProductServiceImpl.Instance;

        ProductStockService stockService = new ProductStockServiceImpl();

        public ProductStock()
        {
            InitializeComponent();
            init();
        }
        
        private List<CakeGUI.classes.entity.ProductStockEntity> getListProduct()
        {
            if (productStocks == null)
            {
                productStocks = new List<classes.entity.ProductStockEntity>();

                classes.entity.ProductStockEntity stock1 = new classes.entity.ProductStockEntity();
                stock1.Product = productService.getProduct("1");
                stock1.BuyPrice = 9000;
                stock1.ExpiredDate = new DateTime(2017, 8, 31);
                stock1.Quantity = 5;
                stock1.SellPrice = 12500;
                productStocks.Add(stock1);

                classes.entity.ProductStockEntity stock2 = new classes.entity.ProductStockEntity();
                stock2.Product = productService.getProduct("2");
                stock2.BuyPrice = 10000;
                stock2.ExpiredDate = new DateTime(2018, 3, 28);
                stock2.Quantity = 10;
                stock2.SellPrice = 12500;
                productStocks.Add(stock2);

            }

            return productStocks;
        }
        
        public List<CakeGUI.classes.entity.ProductStockEntity> productStocks { get; set; }
        public classes.entity.ProductEntity product { get; set; }

        private void init()
        {
            //productStocks = stockService.getProductStock();
            productStocks = getListProduct();

            this.dataGrid.ItemsSource = productStocks;
        }

        private void ProductNameClicked(object sender, MouseButtonEventArgs e)
        {
            ProductStockEntity cellContent = (ProductStockEntity)dataGrid.SelectedItem;
            //MessageBox.Show(cellContent.Product.Name);

            GenericWindow windowInventoryList = new GenericWindow();
            Page inventoryListPage = new ProductInventoryList(cellContent.Product);

            windowInventoryList.Content = inventoryListPage;
            windowInventoryList.ShowDialog();
        }
    }
}
