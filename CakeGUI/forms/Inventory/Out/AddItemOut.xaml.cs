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
    public partial class AddItemOut : Page
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
        public bool IsJoinSameProduct = true;

        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Add New";
            lblSiteMap.Content = commonPage.Title;
            txtQuantity.Text = "1";

            loadLatestPurchasePrice();
            loadCurrentSellPrice();
        }

        private void loadLatestPurchasePrice()
        {
            try
            {
                if (product != null)
                {
                    List<ProductStockEntity> listStock = productService.getStocks(product.Category.Type, "");
                    if (listStock != null || listStock.Count > 0)
                    {
                        ProductStockEntity stock = listStock.Find(p => p.Product.Id == product.Id);
                        if (stock != null)
                        {
                            //txtPurchasePrice.Text = stock.BuyPrice.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed loadLatest harga beli : " +ex.Message);
            }
        }

        private void loadCurrentSellPrice()
        {
            try
            {
                if (product != null)
                {
                    SellPrice = sellPriceService.getCurrentSellPrice(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed load harga jual : " +ex.Message);
            }
        }

        public AddItemOut()
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            init();
        }

        public AddItemOut(string barcode, List<InventoryItemOutEntity> outInventories)
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            this.outInventories = outInventories;

            try
            {
                this.product = productService.getProductByBarcode(barcode);
                if (this.product != null)
                {
                    List<ProductStockEntity> stock = productService.getStocks(this.product.Category.Type, barcode);
                    if (stock == null || stock.Count == 0)
                    {
                        MessageBox.Show("Barang tidak ada stock!");
                        btnSave.IsEnabled = false;
                        txtQuantity.IsEnabled = false;
                    }
                    else
                    {
                        txtBarcode.Text = this.product.BarCode;
                        txtName.Text = this.product.Name;
                    }
                }
                else
                {
                    MessageBox.Show("Barang tidak ditemukan");
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            init();
        }

        public AddItemOut(List<InventoryItemOutEntity> outInventories)
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            this.outInventories = outInventories;
            init();
        }

        List<InventoryItemOutEntity> outInventories;
        public AddItemOut(InventoryItemOutEntity outInventory)
        {
            InitializeComponent();
            this.outInventory = outInventory;
            this.product = outInventory.Product;

            if (outInventory != null)
                this.SellPrice = outInventory.SellPrice;

            if (this.product != null)
            {
                txtBarcode.Text = this.product.BarCode;
                txtName.Text = this.product.Name;
            }
            //txtPurchasePrice.Text = this.outInventory.PurchasePrice.ToString();
            txtQuantity.Text = this.outInventory.Quantity.ToString();
            lblTitle.Text += " (ubah)";
            init();
        }

        private InventoryItemOutEntity outInventory;
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
                    MessageBox.Show("Tidak ada barang!");
                }
                else
                {
                    if (string.IsNullOrEmpty(txtQuantity.Text))
                    {
                        MessageBox.Show("Isi jumlah barang!");
                    }
                    else
                    {
                        //MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                        //if (messageBoxResult == MessageBoxResult.Yes)
                        //{
                        outInventory.Product = product;
                        //outInventory.PurchasePrice = decimal.Parse(!string.IsNullOrEmpty(txtPurchasePrice.Text) ? txtPurchasePrice.Text : "0");
                        outInventory.Quantity = Int32.Parse(!string.IsNullOrEmpty(txtQuantity.Text) ? txtQuantity.Text : "0");
                        outInventory.SellPrice = SellPrice;
                        if (SellPrice != null)
                        {
                            outInventory.SellPriceTrx = SellPrice.SellingPrice;
                        }

                        if (outInventories != null)
                        {
                            if (IsJoinSameProduct)
                            {
                                InventoryItemOutEntity checkItem = outInventories.Find(o => o.Product.Id == this.product.Id);
                                if (checkItem == null)
                                {
                                    outInventories.Add(outInventory);
                                }
                                else
                                {
                                    checkItem.Quantity += outInventory.Quantity;
                                }
                            }
                            else
                            {
                                outInventories.Add(outInventory);
                            }
                        }
                        else
                        {
                            outInventories = new List<InventoryItemOutEntity>();
                            outInventories.Add(outInventory);
                        }

                        GenericWindow genericWindow = ((GenericWindow)this.Parent);
                        genericWindow.Close();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed save : "+ex.Message);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtBarcode.Text))
                {
                    MessageBox.Show("Isi barcode!");
                }
                else
                {
                    ProductEntity p = productService.getProductByBarcode(txtBarcode.Text);
                    if (p == null || (productType != null && !p.Category.Type.Id.Equals(productType.Id)))
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
            catch (Exception ex)
            {
                MessageBox.Show("failed search : "+ex.Message);
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
