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
using CakeGUI.classes.entity.rest;

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for ProductInventoryList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private CommonPage commonPage;

        public ProductList()
        {
            InitializeComponent();

            commonPage = new CommonPage();
            commonPage.Title = "Master Barang";
            lblTItle.Text = commonPage.Title;
            
            init();
        }

        public ProductEntity product;        
        public List<CakeGUI.classes.entity.ProductEntity> products { get; set; }
        public List<CakeGUI.classes.entity.ProductTypeEntity> productTypes { get; set; }


        private void init()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("failed init : "+ex.Message);
            }
        }

        private void loadData()
        {
            try
            {
                this.dataGrid.ItemsSource = null;

                List<KeyValue> listFilter = new List<KeyValue>();


                if (cmbType.SelectedItem != null)
                {
                    KeyValue keyValue = new KeyValue();
                    keyValue.Key = "productType";
                    keyValue.Value = ((ProductTypeEntity)cmbType.SelectedItem).Id;
                    listFilter.Add(keyValue);
                }

                if (!string.IsNullOrEmpty(txtBarcode.Text))
                {
                    KeyValue keyValueBarcode = new KeyValue();
                    keyValueBarcode.Key = "productBarcode";
                    keyValueBarcode.Value = txtBarcode.Text;
                    listFilter.Add(keyValueBarcode);
                }

                products = productService.getProducts(listFilter);

                this.dataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed loadData : "+ex.Message);
            }
        }
      
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GenericWindow windowAdd = new GenericWindow();

                windowAdd.Content = createProductPage(null);
                windowAdd.Owner = (this.Tag as MainWindow);
                windowAdd.ShowDialog();
                init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed add : "+ex.Message);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = (ProductTypeEntity)cmbType.SelectedItem;
                if (type != null)
                {
                    dataGrid.Columns[4].Header = type.Expiration ? "Notifikasi Kadaluarsa" : "Notifikasi Aging";
                }
                else
                {
                    dataGrid.Columns[4].Header = "Expired / Aging";
                }
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed search : "+ex.Message);
            }
        }

        private void EditProductClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductEntity cellContent = (ProductEntity)dataGrid.SelectedItem;
                //MessageBox.Show(cellContent.Product.Name);

                GenericWindow windowInventoryList = new GenericWindow();
                windowInventoryList.Content = createProductPage(cellContent);
                windowInventoryList.ShowDialog();
                init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed edit : "+ex.Message);
            }
        }
        private void DeleteProductClicked(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("failed delete : "+ex.Message);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    //dataGrid.Columns[3].Header = type.Expiration ? "Expired Date" : "Aging Date";
                    dataGrid.Columns[4].Header = type.Expiration ? "Notifikasi Kadaluarsa" : "Notifikasi Aging";
                    loadData();
                    /*dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type != null && string.Equals(x.Product.Type.Id, type.Id));*/
                    //MessageBox.Show("change");
                    //lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
            }
        }

        public void SetParent(CommonPage page)
        {
            if(commonPage != null)
            {
                commonPage.ParentPage = page;
            }
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //(this.Tag as MainWindow).setLabelTitle(commonPage.TitleSiteMap);
            lblSiteMap.Content = commonPage.TitleSiteMap;
        }

        private  Product createProductPage(ProductEntity product)
        {
            Product productPage;
            if (product == null)
            {
                productPage = new Product();
                productPage.SetProductType((ProductTypeEntity)cmbType.SelectedItem);
            }
            else
            {
                productPage = new Product(product);
            }
            productPage.SetParent(commonPage);
            productPage.Tag = this.Tag;

            return productPage;
        }
        
    }
}
