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
    public partial class InventoryItemOut : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static ProductInventoryOutService inventoryOutService = ProductInventoryOutServiceRestImpl.Instance;
        private static SellPriceService sellPriceService = SellPriceServiceRestImpl.Instance;

        private ProductTypeEntity productType;
        public ProductTypeEntity ProductType
        {
            set
            {
                productType = value;
            }
        }
        private CommonPage commonPage;
        public bool EditMode { get; set; }
        public SellPrice SellPrice { get; set; }

        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Add New";
            lblSiteMap.Content = commonPage.Title;
            loadLatestPurchasePrice();
        }

        private void loadLatestPurchasePrice()
        {
            if (product != null)
            {
                List<ProductStockEntity> listStock = productService.getStocks(product.Type);
                if(listStock!=null || listStock.Count > 0)
                {
                    ProductStockEntity stock = listStock.Find(p => p.Product.Id == product.Id);
                    if (stock != null)
                    {
                        txtPurchasePrice.Text = stock.BuyPrice.ToString();
                    }
                }
            }
        }

        private void loadCurrentSellPrice()
        {
            if (product != null)
            {
                SellPrice = sellPriceService.getCurrentSellPrice(product);
                if (SellPrice == null)
                {
                    txtSellingPrice.Text = "";
                    radioFalse.IsChecked = false;
                    radioTrue.IsChecked = false;
                }
                else
                {
                    txtSellingPrice.Text = SellPrice.SellingPrice.ToString();
                    radioTrue.IsChecked = SellPrice.Sale;
                    radioFalse.IsChecked = !SellPrice.Sale;
                }
            }
        }

        public InventoryItemOut()
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            init();
        }

        public InventoryItemOut(List<InventoryItemOutEntity> outInventories)
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            this.outInventories = outInventories;
            init();
        }

        List<InventoryItemOutEntity> outInventories;
        public InventoryItemOut(InventoryItemOutEntity outInventory)
        {
            InitializeComponent();
            this.outInventory = outInventory;
            this.product = outInventory.Product;

            if (outInventory != null)
                this.SellPrice = outInventory.SellPrice;

            txtBarcode.Text = this.outInventory.Product.BarCode;
            txtName.Text = this.outInventory.Product.Name;
            txtPurchasePrice.Text = this.outInventory.PurchasePrice.ToString();
            txtQuantity.Text = this.outInventory.Quantity.ToString();
            lblTitle.Text += " (ubah)";
            init();
        }

        private InventoryItemOutEntity outInventory;
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
                MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    outInventory.Product = product;
                    outInventory.PurchasePrice = decimal.Parse(!string.IsNullOrEmpty(txtPurchasePrice.Text) ? txtPurchasePrice.Text : "0");
                    outInventory.Quantity = Int32.Parse(!string.IsNullOrEmpty(txtQuantity.Text) ? txtQuantity.Text : "0");
                    outInventory.SellPrice = SellPrice;
                    
                    if(outInventories!=null)
                        outInventories.Add(outInventory);

                    GenericWindow genericWindow = ((GenericWindow)this.Parent);
                    genericWindow.Close();
                }
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
                ProductEntity p = productService.getProductByBarcode(txtBarcode.Text);
                if (p == null || (productType!=null && !p.Type.Id.Equals(productType.Id)))
                {
                    MessageBox.Show("Barang tidak ditemukan");
                    txtName.Text = "";
                    product = null;
                }
                else
                {
                    product = p;
                    txtBarcode.Text = product.BarCode;
                    txtName.Text = product.Name;
                }
                loadLatestPurchasePrice();
                loadCurrentSellPrice();
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
            commonPage.Title = (EditMode ? "Edit" : "Add");
            lblSiteMap.Content = commonPage.TitleSiteMap;
            if (SellPrice != null)
            {
                txtSellingPrice.Text = SellPrice.SellingPrice.ToString();
                radioFalse.IsChecked = !SellPrice.Sale;
                radioTrue.IsChecked = SellPrice.Sale;
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
