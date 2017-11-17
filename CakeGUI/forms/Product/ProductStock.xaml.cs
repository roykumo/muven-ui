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
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        ProductStockService stockService = ProductStockServiceRestImpl.Instance;
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();

        public ProductStock()
        {
            InitializeComponent();
            init();
        }
        
        public List<CakeGUI.classes.entity.ProductStockEntity> productStocks { get; set; }
        public classes.entity.ProductEntity product { get; set; }

        private void init()
        {
            //productStocks = getListProduct();

            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedIndex = 0;

            loadData();
            //if(cmbType.SelectedIndex >= 0 )
            //    this.dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type != null && string.Equals(x.Product.Type.Id, ((ProductTypeEntity)cmbType.SelectedItem).Id));

            //this.dataGrid.ItemsSource = productStocks;
        }

        private void loadData()
        {
            productStocks = stockService.getProductStock((ProductTypeEntity)cmbType.SelectedItem);
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = productStocks;
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

        private void SellPriceClicked(object sender, MouseButtonEventArgs e)
        {
            ProductStockEntity cellContent = (ProductStockEntity)dataGrid.SelectedItem;
            
            GenericWindow windowSellPriceHistory = new GenericWindow();
            Page sellPriceHistoryPage = new SellPriceHistory(cellContent.Product);

            windowSellPriceHistory.Content = sellPriceHistoryPage;
            windowSellPriceHistory.ShowDialog();
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
                dataGrid.Columns[7].Header = type.Expiration ? "Expiry Notif" : "Aging Notif";
                loadData();
                //dataGrid.ItemsSource = null;
                //dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type!=null && string.Equals(x.Product.Type.Id, type.Id));
                //MessageBox.Show("change");
                //lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
            }
        }

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            /*try
            {
                DataGridRow gridRow = e.Row;
                ProductStockEntity stock = (ProductStockEntity)gridRow.Item;

                dataGrid.Columns[4].CellStyle = new Style(typeof(DataGridCell));
                dataGrid.Columns[4].CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(stock != null ? stock.AlertColor : Colors.Transparent)));

                if (stock != null && stock.SellPrice != null)
                {
                    dataGrid.Columns[7].CellStyle = new Style(typeof(DataGridCell));
                    dataGrid.Columns[7].CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(stock.SellPrice.Sale ? Colors.Blue : Colors.Transparent)));
                }
            }catch {  }*/
        }
        
    }
}
