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
using CakeGUI.classes.util;

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
            txtOrderNo.Text = this.productCategory.OrderNo.ToString();
            lblTitle.Text += " (ubah)";
            //product.Type = productTypeService.getProductType("2");
            init();
            if (this.productCategory.Parent==null)
            {
                //initParent();
            }
            //if (this.productCategory.Parent != null)
            //{
            //    cmbParent.SelectedItem = this.productCategory.Parent;
            //}
        }

        private void init()
        {
            try
            {
                commonPage = new CommonPage();
                commonPage.Title = (this.productCategory != null && !string.IsNullOrEmpty(this.productCategory.Id)) ? "Edit Barang" : "Penambahan Barang";
                initProductTypes();
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void initProductTypes()
        {
            try
            {
                productTypes = productTypeService.getProductTypes();
                cmbType.ItemsSource = productTypes;

                if (this.productCategory.Type != null)
                {
                    cmbType.SelectedItem = this.productCategory.Type;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("failed init product type: "+e.Message);
            }
        }
    

        private void initParent()
        {
            try
            {
                parents = productCategoryService.getProductCategoriesByType((ProductTypeEntity)cmbType.SelectedItem, true);
                cmbParent.ItemsSource = parents;

                if (this.productCategory.Parent != null)
                {
                    cmbParent.SelectedValue = this.productCategory.Parent.Id;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("failed init parent: "+e.Message);
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
            try
            {
                ((GenericWindow)this.Parent).Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed cancel : "+ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    MessageBox.Show("Kode harus diisi");
                }else if (string.IsNullOrEmpty(txtOrderNo.Text))
                {
                    MessageBox.Show("Order Number harus diisi");
                }
                else if (cmbType.SelectedIndex >= 0)
                {
                    productCategory.Type = (ProductTypeEntity)cmbType.SelectedItem;
                    productCategory.Parent = (ProductCategoryEntity)cmbParent.SelectedItem;

                    MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan Barang?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        productCategory.Code = txtCode.Text;
                        productCategory.OrderNo = Int32.Parse(txtOrderNo.Text);

                        productCategoryService.saveProductCategory(productCategory);

                        //MessageBox.Show("Barang berhasil disimpan");

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
            catch (Exception ex)
            {
                MessageBox.Show("failed save : "+ex.Message);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                initParent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
            }
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
        
        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }

    }
}
