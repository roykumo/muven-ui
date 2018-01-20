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
    public partial class Product : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private CommonPage commonPage;

        public Product()
        {
            InitializeComponent();
            product = new ProductEntity();
            //product.Type = productTypeService.getProductType("2");
            txtExpiryRed.Text = "0";
            txtExpiryYellow.Text = "30";
            txtExpiryGreen.Text = "60";
            txtExpiryBlue.Text = "180";
            init();
        }

        public Product(ProductEntity product)
        {
            InitializeComponent();
            this.product = product;
            txtCode.Text = this.product.Code;
            txtBarcode.Text = this.product.BarCode;
            txtName.Text = this.product.Name;
            txtExpiryRed.Text = this.product.AlertRed.ToString();
            txtExpiryYellow.Text = this.product.AlertYellow.ToString();
            txtExpiryGreen.Text = this.product.AlertGreen.ToString();
            txtExpiryBlue.Text = this.product.AlertBlue.ToString();
            lblTitle.Text += " (ubah)";
            //product.Type = productTypeService.getProductType("2");
            init();
        }

        private void init()
        {
            try
            {
                commonPage = new CommonPage();
                commonPage.Title = (this.product != null && !string.IsNullOrEmpty(this.product.Id)) ? "Edit Barang" : "Penambahan Barang";
                productTypes = productTypeService.getProductTypes();
                cmbType.ItemsSource = productTypes;
            }
            catch (Exception e)
            {
                MessageBox.Show("failed init : "+e.Message);
            }
        }
        
        private ProductEntity product;
        public ProductEntity _Product { get { return this.product; } }
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();

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
            }catch(Exception ex)
            {
                MessageBox.Show("failed cancel : "+ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbType.SelectedIndex >= 0)
                {
                    product.Type = (ProductTypeEntity)cmbType.SelectedItem;

                    MessageBoxResult messageBoxResult = MessageBox.Show("Konfirmasi?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        product.Code = txtCode.Text;
                        product.BarCode = txtBarcode.Text;
                        product.Name = txtName.Text;
                        product.AlertRed = Int32.Parse(txtExpiryRed.Text);
                        product.AlertYellow = Int32.Parse(txtExpiryYellow.Text);
                        product.AlertGreen = Int32.Parse(txtExpiryGreen.Text);

                        productService.saveProduct(product);
                        
                        GenericWindow genericWindow = ((GenericWindow)this.Parent);
                        //((MainWindow)genericWindow.Owner).refreshFrame();
                        genericWindow.Close();


                    }

                }
                else
                {
                    MessageBox.Show("Pilih jenis Barang!");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("failed save : "+ex.Message);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    lblNotif.Text = type.Expiration ? "Batasan Kadaluarsa" : "Batasan Aging";
                }
            }catch(Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
            }
        }

        public void SetProductType(ProductTypeEntity productType)
        {
            cmbType.SelectedItem = productType;
            if(product!=null)
                product.Type = productType;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //(this.Tag as MainWindow).setLabelTitle(commonPage.TitleSiteMap);
            lblSiteMap.Content = commonPage.TitleSiteMap;
            if (product.Type != null)
            {
                int idx = productTypes.FindIndex(t => t.Id == product.Type.Id);
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
