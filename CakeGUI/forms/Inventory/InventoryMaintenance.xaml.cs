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
    public partial class InventoryMaintenance : Page
    {
        private static ProductService productService = ProductServiceImpl.Instance;
        private static ProductInventoryItemService inventoryService = ProductInventoryItemServiceRestImpl.Instance;

        private CommonPage commonPage;

        private void initCommonPage()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Barang Masuk";
        }

        public InventoryMaintenance()
        {
            InitializeComponent();
            initCommonPage();
            inventory = new InventoryItemEntity();
        }
        
        public InventoryMaintenance(InventoryItemEntity inventory)
        {
            InitializeComponent();
            initCommonPage();
            this.inventory = inventory;
            txtInvoice.Text = this.inventory.Inventory.Invoice;
            txtTransactionCode.Text = this.inventory.Inventory.TransactionCode;
            txtSupplier.Text = this.inventory.Inventory.Supplier;
            //txtTransactionCode.Text = this.inventory.TransactionCode;
            //txtPurchaseDate.Text = this.inventory.PurchaseDate;
            txtDate.Text = this.inventory.Inventory.Date.ToShortDateString();
            txtPurchasePrice.Text = this.inventory.PurchasePrice.ToString();
            txtQuantity.Text = this.inventory.Quantity.ToString();
            //txtExpiredDate.Text = this.inventory.ExpiredDate;
            dtExpired.SelectedDate = this.inventory.ExpiredDate;
            txtRemarks.Text = this.inventory.Remarks;
        }

        private InventoryItemEntity inventory;
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((GenericWindow)this.Parent).Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan Barang?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                //inventory.TransactionCode = txtTransactionCode.Text;
                inventory.Inventory.Invoice = txtInvoice.Text;
                inventory.PurchasePrice = Int32.Parse(txtPurchasePrice.Text);
                inventory.Quantity = Int32.Parse(txtQuantity.Text);
                inventory.ExpiredDate = dtExpired.SelectedDate.Value;
                inventory.Remarks = txtRemarks.Text;

                inventoryService.saveProductInventory(inventory);

                GenericWindow genericWindow = ((GenericWindow)this.Parent);
                //((MainWindow)genericWindow.Owner).refreshFrame();
                genericWindow.Close();
            }
        }

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitle.Text = inventory.Product.Name;
            lblSiteMap.Content = commonPage.TitleSiteMap + (string.IsNullOrEmpty(inventory.Id) ? "" : " > edit");
        }
    }
}
