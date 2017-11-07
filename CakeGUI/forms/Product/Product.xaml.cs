using System;
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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Page
    {
        private static ProductService productService = ProductServiceImpl.Instance;
        
        public Product()
        {
            InitializeComponent();
            product = new ProductEntity();
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
        }

        private ProductEntity product;
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((GenericWindow)this.Parent).Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
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
}
