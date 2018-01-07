using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class ProductCategory : Page
    {
        private static ProductCategoryService productCategoryService = ProductCategoryServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private CommonPage commonPage;

        public ProductCategory()
        {
            InitializeComponent();
            productCategory = new ProductCategoryEntity();
            init();
        }

        public ProductCategory(ProductCategoryEntity productCategory)
        {
            InitializeComponent();
            this.productCategory = productCategory;
            txtCode.Text = this.productCategory.Code;
            txtDescription.Text = this.productCategory.Description;
            if(this.productCategory.Parent !=null)
            {
                cmbParent.SelectedItem = this.productCategory.Parent;
            }
            lblTitle.Text += " (ubah)";
            //product.Type = productTypeService.getProductType("2");
            init();
        }

        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = (this.productCategory!=null && !string.IsNullOrEmpty(this.productCategory.Id)) ? "Edit Barang" : "Penambahan Barang" ;
            initProductTypes();

        }

        private void initProductTypes()
        {
            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;

            if (this.productCategory.Type != null)
            {
                cmbType.SelectedItem = this.productCategory.Type;
            }
        }

        private void initParent()
        {
            parents = productCategoryService.getProductCategoriesByType((ProductTypeEntity)cmbType.SelectedItem);
            cmbParent.ItemsSource = parents;

            if (this.productCategory.Parent != null)
            {
                cmbParent.SelectedItem = this.productCategory.Parent;
            }
        }

        private ProductCategoryEntity productCategory;
        public ProductCategoryEntity _Product { get { return this.productCategory; } }
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
        private List<ProductCategoryEntity> parents = new List<ProductCategoryEntity>();

        //public ProductEntity ProductSelected
        //{
        //    get { return product; }
        //    set { product = value; }
        //}

        //public ObservableCollection<ProductTypeEntity> ProductTypeList
        //{
        //    get { return new ObservableCollection<ProductTypeEntity>(productTypes); }
        //}

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((GenericWindow)this.Parent).Close();
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedIndex >= 0)
            {
                productCategory.Type = (ProductTypeEntity)cmbType.SelectedItem;
                productCategory.Parent = (ProductCategoryEntity)cmbParent.SelectedItem;

                MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan Barang?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    productCategory.Code = txtCode.Text;
                    productCategory.Description = txtDescription.Text;

                    productCategoryService.saveProductCategory(productCategory);

                    MessageBox.Show("Barang berhasil disimpan");

                    GenericWindow genericWindow = ((GenericWindow)this.Parent);
                    //((MainWindow)genericWindow.Owner).refreshFrame();
                    genericWindow.Close();
                    

                }

            }
            else
            {
                MessageBox.Show("Pilih jenis Barang!");
            }

        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
            initParent();
        }

        public void SetProductType(ProductTypeEntity productType)
        {
            cmbType.SelectedItem = productType;
            productCategory.Type = productType;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //(this.Tag as MainWindow).setLabelTitle(commonPage.TitleSiteMap);
            lblSiteMap.Content = commonPage.TitleSiteMap;
            if (productCategory.Type != null)
            {
                int idx = productTypes.FindIndex(t => t.Id == productCategory.Type.Id);
                cmbType.SelectedIndex = idx;
            }
        }

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
            }
        }
    }
}
