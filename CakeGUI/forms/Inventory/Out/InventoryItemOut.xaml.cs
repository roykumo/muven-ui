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
using CakeGUI.classes.view_model;

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

        public bool IsLoadPurchasePrice { get; set; } = true;
        public bool IsLoadSellPrice { get; set; } = true;

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

        public String TrxType { get; set; } = "";

        private void init()
        {
            InventoryItemOutViewModel viewModel = new InventoryItemOutViewModel();
            DataContext = viewModel;

            commonPage = new CommonPage();
            commonPage.Title = "Add New";
            lblSiteMap.Content = commonPage.Title;
            loadLatestPurchasePrice();
        }

        private void loadLatestPurchasePrice()
        {
            try
            {
                if (IsLoadPurchasePrice)
                {
                    if (product != null)
                    {
                        List<ProductStockEntity> listStock = productService.getStocks(product.Category.Type, "");
                        if (listStock != null || listStock.Count > 0)
                        {
                            ProductStockEntity stock = listStock.Find(p => p.Product.Id == product.Id);
                            if (stock != null)
                            {
                                txtPurchasePrice.Text = stock.BuyPrice.ToString();
                            }
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed load harga beli : "+ex.Message);
            }
        }

        private void loadCurrentSellPrice()
        {
            try
            {
                if (IsLoadSellPrice)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed load harga jual : "+ex.Message);
            }
        }

        public InventoryItemOut()
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            init();
            DataObject.AddPastingHandler(txtBarcode, onPaste);
        }

        public InventoryItemOut(List<InventoryItemOutEntity> outInventories)
        {
            InitializeComponent();
            outInventory = new InventoryItemOutEntity();
            this.outInventories = outInventories;
            init();
            DataObject.AddPastingHandler(txtBarcode, onPaste);
        }

        bool isPaste = false;
        private void onPaste(object sender, DataObjectPastingEventArgs e)
        {
            var istext = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!istext) return;

            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            isPaste = true;

            //new task(()=> { openwindowadditem(text); }).start();

        }

        List<InventoryItemOutEntity> outInventories;
        public InventoryItemOut(InventoryItemOutEntity outInventory)
        {
            try
            {
                InitializeComponent();
                this.outInventory = outInventory;
                this.product = outInventory.Product;

                if (outInventory != null)
                    this.SellPrice = outInventory.SellPrice;

                txtRemarks.Text = this.outInventory.Remarks;
                if (this.product != null)
                {
                    txtBarcode.Text = this.product.BarCode;
                    txtName.Text = this.product.Name;
                }
                txtPurchasePrice.Text = this.outInventory.PurchasePrice.ToString();
                txtQuantity.Text = this.outInventory.Quantity.ToString();
                lblTitle.Text += " (ubah)";
                init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed construct : "+ex.Message);
            }
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
                        outInventory.Remarks = txtRemarks.Text;

                        if (outInventories != null)
                            outInventories.Add(outInventory);

                        GenericWindow genericWindow = ((GenericWindow)this.Parent);
                        genericWindow.Close();
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
            openWindowAddItem(txtBarcode.Text);
            /*try
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
                MessageBox.Show("failed search : " +ex.Message);
            }*/
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
                    openWindowAddItem(msg);
                }
            }
            else if((e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                // record keystroke & timestamp
                _barcode.Add(Convert.ToChar(e.Key.ToString().Substring(1, 1)));
                _lastKeystroke = DateTime.Now;
            }else if(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
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

        public string LabelTitle {
            set
            {
                this.lblTitle.Text = value;
            }
        }


        private void txtBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPaste)
            {
                openWindowAddItem(txtBarcode.Text);
            }
            isPaste = false;
        }

        private void openWindowAddItem(string barcode)
        {
            try
            {
                isPaste = false;
                ProductEntity p = productService.getProductByBarcode(barcode);
                if (p == null || (productType != null && !p.Category.Type.Id.Equals(productType.Id)))
                {
                    MessageBox.Show("Barang tidak ditemukan");
                    txtName.Text = "";
                    txtPurchasePrice.Text = "";
                    txtSellingPrice.Text = "";
                    product = null;
                }
                else
                {
                    if (TrxType.Equals("RE") && !p.ProductGroup.Equals("BULK"))
                    {
                        MessageBox.Show("Hanya bisa barang Bulk");
                        txtName.Text = "";
                        txtPurchasePrice.Text = "";
                        txtSellingPrice.Text = "";
                        product = null;
                    }
                    else
                    {
                        product = p;
                        txtBarcode.Text = product.BarCode;
                        txtName.Text = product.Name;
                    }
                }
                loadLatestPurchasePrice();
                loadCurrentSellPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed open add : "+ex.Message);
            }
        }

    }
}
