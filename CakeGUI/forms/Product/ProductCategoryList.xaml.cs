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
    public partial class ProductCategoryList : Page
    {
        private static ProductCategoryService productCategoryService = ProductCategoryServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private CommonPage commonPage;

        public ProductCategoryList()
        {
            InitializeComponent();

            commonPage = new CommonPage();
            commonPage.Title = "Kategori Barang";
            lblTItle.Text = commonPage.Title;
            
            init();
        }
       
        public List<CakeGUI.classes.entity.ProductCategoryEntity> productCategories { get; set; }
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
                keyValueBarcode.Key = "code";
                keyValueBarcode.Value = txtBarcode.Text;
                listFilter.Add(keyValueBarcode);
            }

            productCategories = productCategoryService.getProductCategories(listFilter);
            
            this.dataGrid.ItemsSource = productCategories;
        }
      
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GenericWindow windowAdd = new GenericWindow();

            windowAdd.Content = createProductCategoryPage(null);
            windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
            init();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ProductTypeEntity type = (ProductTypeEntity)cmbType.SelectedItem;
            
            loadData();
        }

        private void EditProductCategoryClicked(object sender, RoutedEventArgs e)
        {
            ProductCategoryEntity cellContent = (ProductCategoryEntity)dataGrid.SelectedItem;
            //MessageBox.Show(cellContent.Product.Name);

            GenericWindow windowInventoryList = new GenericWindow();
            windowInventoryList.Content = createProductCategoryPage(cellContent);
            windowInventoryList.ShowDialog();
            init();
        }
        private void DeleteProductCategoryClicked(object sender, RoutedEventArgs e)
        {
            ProductCategoryEntity cellContent = (ProductCategoryEntity)dataGrid.SelectedItem;
            if (cellContent != null)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin hapus Barang?", "Konfirmasi Hapus", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (productCategoryService.deleteProductCategory(cellContent))
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
                dataGrid.Columns[4].Header = type.Expiration ? "Notifikasi Kadaluarsa" : "Notifikasi Aging";
                loadData();
                /*dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = productStocks.Where(x => x.Product.Type != null && string.Equals(x.Product.Type.Id, type.Id));*/
                //MessageBox.Show("change");
                //lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
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

        private  ProductCategory createProductCategoryPage(ProductCategoryEntity productCategory)
        {
            ProductCategory productPage;
            if (productCategory == null)
            {
                productPage = new ProductCategory();
                productPage.SetProductType((ProductTypeEntity)cmbType.SelectedItem);
            }
            else
            {
                productPage = new ProductCategory(productCategory);
            }
            productPage.SetParent(commonPage);
            productPage.Tag = this.Tag;

            return productPage;
        }
    }
}
