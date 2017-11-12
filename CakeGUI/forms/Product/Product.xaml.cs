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
        private static ProductService productService = ProductServiceImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceImpl.Instance;

        public Product()
        {
            InitializeComponent();
            product = new ProductEntity();
            //product.Type = productTypeService.getProductType("2");
            init();
        }

        public Product(ProductEntity product)
        {
            InitializeComponent();
            this.product = product;
            txtBarcode.Text = this.product.BarCode;
            txtName.Text = this.product.Name;
            txtExpiryRed.Text = this.product.AlertRed.ToString();
            txtExpiryYellow.Text = this.product.AlertYellow.ToString();
            txtExpiryGreen.Text = this.product.AlertGreen.ToString();
            lblTitle.Text += " (ubah)";
            //product.Type = productTypeService.getProductType("2");
            init();
        }

        private void init()
        {
            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedItem = product.Type;
        }
        
        private ProductEntity product;
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
            ((GenericWindow)this.Parent).Close();
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedIndex >= 0)
            {
                product.Type = (ProductTypeEntity)cmbType.SelectedItem;

                MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan Barang?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    product.BarCode = txtBarcode.Text;
                    product.Name = txtName.Text;
                    product.AlertRed = Int32.Parse(txtExpiryRed.Text);
                    product.AlertYellow = Int32.Parse(txtExpiryYellow.Text);
                    product.AlertGreen = Int32.Parse(txtExpiryGreen.Text);

                    productService.saveProduct(product);

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
            if (type != null)
            {
                lblNotif.Text = type.Expiration ? "Expire Notification" : "Aging Notification";
            }
        }
    }
}
