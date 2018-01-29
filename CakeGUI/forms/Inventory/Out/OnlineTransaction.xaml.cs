using System;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ProductInventoryList.xaml
    /// </summary>
    public partial class OnlineTransaction : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        ProductInventoryService inventoryService = ProductInventoryServiceRestImpl.Instance;
        ProductInventoryItemService inventoryItemService = ProductInventoryItemServiceRestImpl.Instance;
        ProductInventoryOutService inventoryOutService = ProductInventoryOutServiceRestImpl.Instance;
        ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
        private CommonPage commonPage;
        private CommonPage commonPageIn;
        private CommonPage commonPageOut;
        private string type="SA";
        private bool IsReadOnly;
        //private PaymentEntity payment;

        public OnlineTransaction()
        {
            InitializeComponent();
            init();
        }

        public OnlineTransaction(string type)
        {
            InitializeComponent();
            this.type = type;
            init();

            DataObject.AddPastingHandler(txtBarcode, onPaste);
        }

        public OnlineTransaction(InventoryOutEntity inventoryOut, bool readOnly)
        {
            InitializeComponent();
            IsReadOnly = readOnly;

            commonPage = new CommonPage();
            if (type != null)
            {
                commonPage.Title = "Penjualan Online";
            }

            lblTitle.Text = commonPage.Title;
            
            lblSiteMap.Content = commonPage.Title;

            txtTransactionCode.IsEnabled = false;
            dateOut.IsEnabled = false;
            txtTotalPrice.IsEnabled = false;
            txtTotalPriceRemark.IsEnabled = false;
            btnAddOut.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnConfirm.IsEnabled = false;

            if (inventoryOut != null)
            {
                txtTransactionCode.Text = inventoryOut.TransactionCode;
                dateOut.SelectedDate = inventoryOut.Date;
                txtTotalPrice.Text = inventoryOut.TotalPrice.ToString("0,0");
                txtTotalPriceRemark.Text = inventoryOut.Remarks;

                if(inventoryOut.Items!=null && inventoryOut.Items.Count > 0)
                {
                    dataGridOut.ItemsSource = inventoryOut.Items;
                }
            }
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

        private DateTime trxDate = DateTime.Now;
        public List<CakeGUI.classes.entity.InventoryItemEntity> inventories { get; set; }
        public List<CakeGUI.classes.entity.InventoryItemOutEntity> outInventories { get; set; }

        private void init()
        {
            try
            {
                CommonPage parent = new CommonPage();
                parent.Title = "Stock In";

                commonPage = new CommonPage();
                if (type != null)
                {
                    commonPage.Title = "Penjualan Online / Grosir";
                    //if (type.Equals("RP"))
                    //{
                    //    commonPage.Title = "Repacking";
                    //}
                    //else if (type.Equals("SO"))
                    //{
                    //    commonPage.Title = "Stock Opname";
                    //}
                    //else if (type.Equals("SA"))
                    //{
                    //    commonPage.Title = "Penjualan Online / Grosir";
                    //}
                    //else if (type.Equals("CR"))
                    //{
                    //    commonPage.Title = "Cash Register";
                    //}
                }

                //payment = new PaymentEntity();

                lblTitle.Text = commonPage.Title;

                commonPage.ParentPage = parent;
                lblSiteMap.Content = commonPage.Title;

                commonPageIn = new CommonPage();
                commonPageIn.Title = "Barang Masuk";
                commonPageIn.ParentPage = commonPage;

                commonPageOut = new CommonPage();
                commonPageOut.Title = "Stock Out";
                commonPageOut.ParentPage = commonPage;

                //lblTitle.Text += trxDate.ToString("yyyy-MM-dd");
                inventories = new List<InventoryItemEntity>();
                outInventories = new List<InventoryItemOutEntity>();

                //date.SelectedDate = DateTime.Now;
                dateOut.SelectedDate = DateTime.Now;

                loadData();

                txtTransactionCode.Text = inventoryService.getTrxCode(this.type, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed init : "+ex.Message);
            }
        }

        private void loadData()
        {
            
        }

        private void loadDataOut()
        {
            try
            {
                this.dataGridOut.ItemsSource = null;
                this.dataGridOut.ItemsSource = outInventories;
                txtBarcode.Text = "";

                if (outInventories != null && outInventories.Count > 0)
                {
                    try
                    {
                        //payment.TotalAmount = this.outInventories.Sum(o => o.Quantity * o.SellPriceTrx);
                        //txtTotalPrice.Text = payment.TotalAmount.ToString("0,0");
                        txtTotalPrice.Text = this.outInventories.Sum(o => o.Quantity * o.SellPriceTrx).ToString("0,0");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("gagal kalkulasi harga: " + e.Message);
                    }
                }
                else
                {
                    //payment.TotalAmount = 0;
                    txtTotalPrice.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed loadData : "+ex.Message);
            }
        }
        
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    loadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (String.IsNullOrEmpty(txtTransactionCode.Text))
                //{
                //    MessageBox.Show("Kode transaksi harus diisi!");
                //}else if (String.IsNullOrEmpty(txtTotalBuyPrice.Text))
                if (inventories.Count == 0 && outInventories.Count == 0)
                {
                    MessageBox.Show("Input barang keluar / masuk dulu!");
                }//else if(cmbType.SelectedIndex== -1)
                 //{
                 //    MessageBox.Show("Pilih jenis barang dulu!");
                 //    cmbType.Focus();
                 //}else if (string.IsNullOrEmpty(txtTransactionCode.Text))
                 //{
                 //    MessageBox.Show("Kode Transaksi harus diisi");
                 //    txtTransactionCode.Focus();
                 //}
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Selesaikan Pembelian?", "Konfirmasi Beli", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        InventoryOutEntity outInventory = new InventoryOutEntity();
                        outInventory.Date = dateOut.SelectedDate.Value;
                        outInventory.Items = outInventories;
                        outInventory.Type = type;
                        outInventory.Remarks = txtTotalPriceRemark.Text;
                        outInventory.TotalPrice = decimal.Parse(txtTotalPrice.Text);
                        outInventory.TransactionCode = txtTransactionCode.Text;


                        //InventoryEntity inventory = new InventoryEntity();
                        //inventory.Date = date.SelectedDate.Value;
                        //inventory.Items = inventories;
                        //inventory.TransactionCode = txtTransactionCode.Text;
                        //inventory.TotalPrice = inventories.Sum(i => i.PurchasePrice * i.Quantity);

                        //outInventory.InventoryIn = inventory;

                        inventoryOutService.saveProductInventory(outInventory);

                        //inventories.Clear();
                        //loadData();
                        outInventories.Clear();
                        txtTransactionCode.Text = inventoryService.getTrxCode(this.type, null);
                        loadDataOut();
                        txtTotalPrice.Text = "";
                        txtTotalPriceRemark.Text = "";
                        //txtTransactionCode.Text = "";
                        //date.SelectedDate = DateTime.Now;
                        dateOut.SelectedDate = DateTime.Now;

                        MessageBox.Show("Penjualan Online berhasil disimpan");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed save : " +ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin batalkan Pembelian?", "Konfirmasi Batal", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    inventories.Clear();
                    outInventories.Clear();
                    //txtTransactionCode.Text = "";
                    txtTransactionCode.Text = inventoryService.getTrxCode(this.type, null);
                    loadData();
                    loadDataOut();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed cancel : "+ex.Message);
            }
        }
        
        private void txtTotalBuyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }
        
        private void btnAddOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBarcode.Text))
                {
                    MessageBox.Show("Isi barcode dahulu!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                }
                else
                {
                    openWindowAddItem(txtBarcode.Text);
                }
                //GenericWindow windowAdd = new GenericWindow();
                //InventoryItemOut inventoryPage = new InventoryItemOut(outInventories);
                //inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;

                //inventoryPage.SetParent(commonPageOut);
                //inventoryPage.Tag = this;

                //windowAdd.Content = inventoryPage;
                //windowAdd.Owner = (this.Tag as MainWindow);
                //windowAdd.ShowDialog();
                //loadDataOut();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed add : "+ex.Message);
            }
        }

        private void openWindowAddItem(string barcode)
        {
            try
            {
                isPaste = false;
                GenericWindow windowAdd = new GenericWindow();
                AddItemOut addItemOut = new AddItemOut(barcode, outInventories);

                addItemOut.IsJoinSameProduct = false;
                addItemOut.SetParent(commonPageOut);
                addItemOut.Tag = this;
                addItemOut.TrxType = this.type;

                windowAdd.Content = addItemOut;
                windowAdd.Owner = (this.Tag as MainWindow);
                windowAdd.ShowDialog();
                loadDataOut();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed open add : "+ex.Message);
            }
        }

        private void EditItemOutClicked(object sender, RoutedEventArgs e)
        {
            //InventoryItemOutEntity cellContent = (InventoryItemOutEntity)dataGridOut.SelectedItem;

            //GenericWindow windowAdd = new GenericWindow();
            //InventoryItemOut inventoryItemOutPage = new InventoryItemOut(cellContent);
            //inventoryItemOutPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
            //inventoryItemOutPage.SetParent(commonPage);
            //inventoryItemOutPage.Tag = this;

            //windowAdd.Content = inventoryItemOutPage;
            //windowAdd.Owner = (this.Tag as MainWindow);
            //windowAdd.ShowDialog();
            //loadDataOut();
        }

        private void EditSellPriceClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsReadOnly)
                {
                    InventoryItemOutEntity cellContent = (InventoryItemOutEntity)dataGridOut.SelectedItem;
                    GenericWindow editSellPrice = new GenericWindow();

                    SellPrice currentSellPrice = new SellPrice();
                    currentSellPrice.SellingPrice = cellContent.SellPrice.SellingPrice;

                    SellPriceOverride sellPriceOverride = new SellPriceOverride(currentSellPrice, cellContent.SellPriceTrx);
                    sellPriceOverride.SetParent(commonPageOut);
                    sellPriceOverride.Tag = this;
                    editSellPrice.Content = sellPriceOverride;
                    editSellPrice.Owner = (this.Tag as MainWindow);
                    editSellPrice.ShowDialog();

                    cellContent.SellPriceTrx = currentSellPrice.SellingPrice;

                    loadDataOut();

                    //InventoryItemOutEntity cellContent = (InventoryItemOutEntity)dataGridOut.SelectedItem;

                    //GenericWindow windowAdd = new GenericWindow();
                    //InventoryItemOut inventoryItemOutPage = new InventoryItemOut(cellContent);
                    //inventoryItemOutPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
                    //inventoryItemOutPage.SetParent(commonPage);
                    //inventoryItemOutPage.Tag = this;

                    //windowAdd.Content = inventoryItemOutPage;
                    //windowAdd.Owner = (this.Tag as MainWindow);
                    //windowAdd.ShowDialog();
                    //loadDataOut();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed edit : "+ex.Message);
            }
        }

        private void DeleteItemOutClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsReadOnly)
                {
                    InventoryItemOutEntity cellContent = (InventoryItemOutEntity)dataGridOut.SelectedItem;
                    if (cellContent != null)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin hapus Barang?", "Konfirmasi Hapus", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            outInventories.Remove(cellContent);
                            loadDataOut();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed delete : "+ex.Message);
            }
        }

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
                lblSiteMap.Content = commonPage.TitleSiteMap;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
    }
}
