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
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductInventoryItemService inventoryService = ProductInventoryItemServiceRestImpl.Instance;
        
        public Inventory()
        {
            InitializeComponent();
            inventory = new InventoryItemEntity();
        }

        public Inventory(List<InventoryItemEntity> inventories)
        {
            InitializeComponent();
            inventory = new InventoryItemEntity();
            this.inventories = inventories;
        }

        List<InventoryItemEntity> inventories;
        public Inventory(InventoryItemEntity inventory)
        {
            InitializeComponent();
            this.inventory = inventory;
            this.product = inventory.Product;
            txtBarcode.Text = this.inventory.Product.BarCode;
            txtName.Text = this.inventory.Product.Name;
            //txtTransactionCode.Text = this.inventory.TransactionCode;
            //txtPurchaseDate.Text = this.inventory.PurchaseDate;
            //dtPurchase.SelectedDate = this.inventory.PurchaseDate;
            txtPurchasePrice.Text = this.inventory.PurchasePrice.ToString();
            txtQuantity.Text = this.inventory.Quantity.ToString();
            //txtExpiredDate.Text = this.inventory.ExpiredDate;
            dtExpired.SelectedDate = this.inventory.ExpiredDate;
            txtRemarks.Text = this.inventory.Remarks;
            lblTitle.Text += " (ubah)";
        }

        private InventoryItemEntity inventory;
        private ProductEntity product;
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((GenericWindow)this.Parent).Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (product == null)
            {
                MessageBox.Show("Harus pilih barang dulu!");
            }
            else
            {
                inventory.Product = product;
                //inventory.TransactionCode = txtTransactionCode.Text;
                //inventory.PurchaseDate = dtPurchase.SelectedDate.Value;
                inventory.PurchasePrice = Int32.Parse(txtPurchasePrice.Text);
                inventory.Quantity = Int32.Parse(txtQuantity.Text);
                inventory.ExpiredDate = dtExpired.SelectedDate.Value;
                inventory.Remarks = txtRemarks.Text;

                //inventoryService.saveProductInventory(inventory);
                inventories.Add(inventory);

                GenericWindow genericWindow = ((GenericWindow)this.Parent);
                //((MainWindow)genericWindow.Owner).refreshFrame();
                genericWindow.Close();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBarcode.Text))
            {
                MessageBox.Show("Isi barcode!");
            }
            else
            {
                product = productService.getProductByBarcode(txtBarcode.Text);
                if (product == null)
                {
                    MessageBox.Show("Barang tidak ditemukan");
                    txtName.Text = "";
                }
                else
                {
                    txtBarcode.Text = product.BarCode;
                    txtName.Text = product.Name;
                }
            }
        }
    }
}
