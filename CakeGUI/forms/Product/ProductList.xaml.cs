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
    /// Interaction logic for ProductInventoryList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        public ProductList()
        {
            InitializeComponent();
            init();
        }
        
        public ProductEntity product;        
        public List<CakeGUI.classes.entity.ProductEntity> products { get; set; }
        public List<CakeGUI.classes.entity.ProductTypeEntity> productTypes { get; set; }


        private void init()
        {
            if (productTypes == null)
            {
                productTypes = productTypeService.getProductTypes();
            }

            if (productTypes.Count > 0)
            {
                cmbType.ItemsSource = productTypes;
                cmbType.SelectedItem = 0;
            }

            loadData();
        }

        private void loadData()
        {
            this.dataGrid.ItemsSource = null;
            if (cmbType.SelectedItem == null)
            {
                products = productService.getProducts();
            }
            else
            {
                products = productService.getProducts((ProductTypeEntity)cmbType.SelectedItem);
            }
            this.dataGrid.ItemsSource = products;
        }
      
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GenericWindow windowAdd = new GenericWindow();
            Product productPage = new Product();

            windowAdd.Content = productPage;
            windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
            init();
        }

        private void EditProductClicked(object sender, RoutedEventArgs e)
        {
            ProductEntity cellContent = (ProductEntity)dataGrid.SelectedItem;
            //MessageBox.Show(cellContent.Product.Name);

            GenericWindow windowInventoryList = new GenericWindow();
            Page productPage = new Product(cellContent);

            windowInventoryList.Content = productPage;
            windowInventoryList.ShowDialog();
            init();
        }
        private void DeleteProductClicked(object sender, RoutedEventArgs e)
        {
            ProductEntity cellContent = (ProductEntity)dataGrid.SelectedItem;
            if (cellContent != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin hapus Barang?", "Konfirmasi Hapus", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (productService.deleteProduct(cellContent))
                    {
                        MessageBox.Show("Barang berhasil dihapus");
                        init();
                    }
                }
            }
            
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
            if (type != null)
            {
                //dataGrid.Columns[3].Header = type.Expiration ? "Expired Date" : "Aging Date";
                dataGrid.Columns[3].Header = type.Expiration ? "Expiry Notif" : "Aging Notif";
                loadData();
                /*dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type != null && string.Equals(x.Product.Type.Id, type.Id));*/
                //MessageBox.Show("change");
                //lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
            }
        }
    }
}
