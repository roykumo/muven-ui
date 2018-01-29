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
using CakeGUI.classes.util;

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductInventoryItemService inventoryService = ProductInventoryItemServiceRestImpl.Instance;
        private ProductTypeEntity productType;
        public ProductTypeEntity ProductType
        {
            set
            {
                productType = value;
                if(productType!=null)
                    lblExpired.Text = productType.Expiration ? "Tanggal Kadaluarsa" : "Tanggal Aging";
            }
        }
        private CommonPage commonPage;
        public bool EditMode { get; set; }
        public String TrxType { get; set; } = "";

        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Add New";
            lblSiteMap.Content = commonPage.Title;
        }

        public Inventory()
        {
            InitializeComponent();
            inventory = new InventoryItemEntity();
            dtExpired.SelectedDate = DateTime.Now;
            init();
        }

        public Inventory(List<InventoryItemEntity> inventories)
        {
            InitializeComponent();
            inventory = new InventoryItemEntity();
            this.inventories = inventories;
            dtExpired.SelectedDate = DateTime.Now;
            init();
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
            init();
        }

        private InventoryItemEntity inventory;
        private ProductEntity product;
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
                if (product == null)
                {
                    MessageBox.Show("Harus pilih barang dulu!");
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        inventory.Product = product;
                        //inventory.TransactionCode = txtTransactionCode.Text;
                        //inventory.PurchaseDate = dtPurchase.SelectedDate.Value;
                        inventory.PurchasePrice = Int32.Parse(txtPurchasePrice.Text);
                        inventory.Quantity = Int32.Parse(txtQuantity.Text);
                        inventory.ExpiredDate = dtExpired.SelectedDate.Value;
                        inventory.Remarks = txtRemarks.Text;

                        //inventoryService.saveProductInventory(inventory);
                        if (inventories != null)
                            inventories.Add(inventory);

                        GenericWindow genericWindow = ((GenericWindow)this.Parent);
                        //((MainWindow)genericWindow.Owner).refreshFrame();
                        genericWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed save : " +ex.Message);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            seachProduct(txtBarcode.Text);
        }

        private void seachProduct(string barcode)
        {
            try
            {
                if (String.IsNullOrEmpty(barcode))
                {
                    MessageBox.Show("Isi barcode!");
                }
                else
                {
                    ProductEntity p = productService.getProductByBarcode(barcode);
                    if (p == null || (productType != null && !p.Category.Type.Id.Equals(productType.Id)))
                    {
                        MessageBox.Show("Barang tidak ditemukan");
                        txtName.Text = "";
                        product = null;
                    }
                    else
                    {
                        if (TrxType.Equals("PU") && p.ProductGroup.Equals("RPCK"))
                        {
                            MessageBox.Show("Barang Repacking tidak valid untuk pembelian");
                            txtName.Text = "";
                            product = null;
                        }else if(TrxType.Equals("RE") && !p.ProductGroup.Equals("RPCK"))
                        {
                            MessageBox.Show("Hanya bisa barang Repacking");
                            txtName.Text = "";
                            product = null;
                        }
                        else
                        {
                            product = p;
                            txtBarcode.Text = product.BarCode;
                            txtName.Text = product.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed search : " + ex.Message);
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
            //(this.Tag as MainWindow).setLabelTitle(commonPage.TitleSiteMap);
            commonPage.Title = (EditMode ? "Edit" : "Add");
            lblSiteMap.Content = commonPage.TitleSiteMap;

            this.KeyDown += new KeyEventHandler(Page_KeyDown);
        }

        DateTime _lastKeystroke = new DateTime(0);
        List<char> _barcode = new List<char>(20);
        void Page_KeyDown(object sender, KeyEventArgs e)
        {
            TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
            if (elapsed.TotalMilliseconds > 100)
                _barcode.Clear();

            // process barcode
            if (e.Key == Key.Enter)
            {
                if (_barcode.Count > 0)
                {
                    string msg = new String(_barcode.ToArray());
                    _barcode.Clear();
                    seachProduct(msg);
                }
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                // record keystroke & timestamp
                _barcode.Add(Convert.ToChar(e.Key.ToString().Substring(1, 1)));
                _lastKeystroke = DateTime.Now;
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                // record keystroke & timestamp
                _barcode.Add(Convert.ToChar(e.Key.ToString().Substring(6, 1)));
                _lastKeystroke = DateTime.Now;
            }
        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }

        private void txtPurchasePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }
    }
}
