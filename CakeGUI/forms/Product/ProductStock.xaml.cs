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
        private static ProductTypeService productTypeService = ProductTypeServiceImpl.Instance;

        ProductStockService stockService = ProductStockServiceImpl.Instance;
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();

        public ProductStock()
        {
            InitializeComponent();
            init();
        }
        
        /*private List<CakeGUI.classes.entity.ProductStockEntity> getListProduct()
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
        }*/
        
        public List<CakeGUI.classes.entity.ProductStockEntity> productStocks { get; set; }
        public classes.entity.ProductEntity product { get; set; }

        private void init()
        {
            productStocks = stockService.getProductStock();

            //productStocks = getListProduct();

            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedIndex = 0;

            if(cmbType.SelectedIndex >= 0 )
                this.dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type != null && string.Equals(x.Product.Type.Id, ((ProductTypeEntity)cmbType.SelectedItem).Id));

            //this.dataGrid.ItemsSource = productStocks;
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
                dataGrid.Columns[3].Header = type.Expiration ? "Expired Date" : "Aging Date";
                dataGrid.Columns[8].Header = type.Expiration ? "Expiry Notif" : "Aging Notif";
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type!=null && string.Equals(x.Product.Type.Id, type.Id));
                //MessageBox.Show("change");
                //lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow gridRow = e.Row;
            ProductStockEntity stock = (ProductStockEntity)gridRow.Item;
            
            dataGrid.Columns[4].CellStyle = new Style(typeof(DataGridCell));
            dataGrid.Columns[4].CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(stock!=null?stock.AlertColor:Colors.Transparent)));

            if(stock != null && stock.SellPrice != null)
            {
                dataGrid.Columns[7].CellStyle = new Style(typeof(DataGridCell));
                dataGrid.Columns[7].CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(stock.SellPrice.Sale ? Colors.Blue : Colors.Transparent)));
            }

        }
    }
}
