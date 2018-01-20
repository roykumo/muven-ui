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
    public partial class CashRegister : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        ProductInventoryService inventoryService = ProductInventoryServiceRestImpl.Instance;
        ProductInventoryItemService inventoryItemService = ProductInventoryItemServiceRestImpl.Instance;
        ProductInventoryOutService inventoryOutService = ProductInventoryOutServiceRestImpl.Instance;
        ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
        private CommonPage commonPage;
        //private CommonPage commonPageIn;
        //private CommonPage commonPageOut;
        private string type="CR";
        public bool IsFinish { get; set; }
        private bool IsPrint { get; set; }

        PaymentEntity payment;

        public CashRegister()
        {
            InitializeComponent();
            init();
            DataObject.AddPastingHandler(txtBarcode, onPaste);
        }

        public CashRegister(string type)
        {
            InitializeComponent();
            this.type = type;
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
                    commonPage.Title = "Cash Register";
                }

                outInventories = new List<InventoryItemOutEntity>();
                inventories = new List<InventoryItemEntity>();

                clearData();

                lblTitle.Text = commonPage.Title;

                commonPage.ParentPage = parent;
                lblSiteMap.Content = commonPage.Title;

                payment = new PaymentEntity();

                inventories = new List<InventoryItemEntity>();
                outInventories = new List<InventoryItemOutEntity>();

                dateOut.SelectedDate = DateTime.Now;

                loadData();

                txtTransactionCode.Text = inventoryService.getTrxCode("CR", null);
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
                        payment.TotalAmount = this.outInventories.Sum(o => o.Quantity * o.SellPriceTrx);
                        txtTotalPrice.Text = payment.TotalAmount.ToString("0,0");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("gagal kalkulasi harga: " + e.Message);
                    }
                }
                else
                {
                    payment.TotalAmount = 0;
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
                if (inventories.Count == 0 && outInventories.Count == 0)
                {
                    MessageBox.Show("Input barang dulu!");
                }
                else
                {
                    MessageBoxResult messageBoxResult;
                    if (string.IsNullOrEmpty(payment.Type))
                    {
                        messageBoxResult = System.Windows.MessageBox.Show("Selesaikan Transaksi?", "Konfirmasi Bayar", System.Windows.MessageBoxButton.YesNo);
                    }
                    else
                    {
                        messageBoxResult = MessageBoxResult.Yes;
                    }

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        GenericWindow windowAdd = new GenericWindow();
                        Payment paymentForm = new Payment(payment);

                        paymentForm.SetParent(commonPage);
                        paymentForm.Tag = this;

                        windowAdd.Content = paymentForm;
                        windowAdd.Owner = (this.Tag as MainWindow);
                        windowAdd.ShowDialog();

                        if (!string.IsNullOrEmpty(payment.Type))
                        {
                            txtPaymentType.Text = payment.Type;
                            txtPayAmount.Text = payment.PayAmount.ToString("0,0");
                            if (payment.Type.Equals(Payment.TYPE_CASH))
                            {
                                txtExchange.Text = (payment.PayAmount - payment.TotalAmount).ToString("0,0");
                            }
                            else if (payment.Type.Equals(Payment.TYPE_EDC))
                            {
                                txtExchange.Text = payment.ReceiptNo;
                            }
                            if (payment.PayAmount > 0 || !string.IsNullOrEmpty(payment.ReceiptNo))
                            {
                                InventoryOutEntity outInventory = new InventoryOutEntity();
                                outInventory.Date = dateOut.SelectedDate.Value;
                                outInventory.Items = outInventories;
                                outInventory.Type = type;
                                outInventory.Remarks = txtPaymentType.Text + " - " + payment.PayAmount.ToString("0,0") + " - " + payment.ReceiptNo;
                                outInventory.TotalPrice = decimal.Parse(txtTotalPrice.Text);
                                outInventory.TransactionCode = txtTransactionCode.Text;
                                outInventory.Payment = payment;

                                inventoryOutService.saveProductInventory(outInventory);

                                IsFinish = true;
                                btnAddOut.IsEnabled = false;
                                btnConfirm.IsEnabled = false;
                                btnPrint.IsEnabled = true;

                                loadDataOut();

                                MessageBox.Show("Cash Register berhasil disimpan");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed confirm : "+ex.Message);
            }
        }

        private void clearData()
        {
            try
            {
                payment = new PaymentEntity();
                txtTransactionCode.Text = inventoryService.getTrxCode("CR", null);
                txtTotalPrice.Text = "";
                txtPaymentType.Text = "";
                txtPayAmount.Text = "";
                txtExchange.Text = "";
                btnAddOut.IsEnabled = true;
                btnConfirm.IsEnabled = true;
                btnPrint.IsEnabled = false;
                IsFinish = false;
                IsPrint = false;
                outInventories.Clear();
                inventories.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed clear data : "+ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show((IsFinish && !IsPrint ? "Struk belum dicetak, " : "") + "Yakin batalkan Transaksi?", "Konfirmasi Batal", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    clearData();
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
                    //GenericWindow windowAdd = new GenericWindow();
                    //AddItemOut addItemOut = new AddItemOut(txtBarcode.Text, outInventories);

                    //addItemOut.SetParent(commonPage);
                    //addItemOut.Tag = this;

                    //windowAdd.Content = addItemOut;
                    //windowAdd.Owner = (this.Tag as MainWindow);
                    //windowAdd.ShowDialog();
                    //loadDataOut();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed add : "+ex.Message);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearData();
                loadDataOut();
                dateOut.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed print receipt : " +ex.Message);
            }
        }

        private void EditSellPriceClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                InventoryItemOutEntity cellContent = (InventoryItemOutEntity)dataGridOut.SelectedItem;
                GenericWindow editSellPrice = new GenericWindow();

                SellPrice currentSellPrice = new SellPrice();
                currentSellPrice.SellingPrice = cellContent.SellPrice.SellingPrice;

                SellPriceOverride sellPriceOverride = new SellPriceOverride(currentSellPrice, cellContent.SellPriceTrx);
                sellPriceOverride.SetParent(commonPage);
                sellPriceOverride.Tag = this;
                editSellPrice.Content = sellPriceOverride;
                editSellPrice.Owner = (this.Tag as MainWindow);
                editSellPrice.ShowDialog();

                cellContent.SellPriceTrx = currentSellPrice.SellingPrice;

                loadDataOut();
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
                if (!IsFinish)
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

        private void openWindowAddItem(string barcode)
        {
            try
            {
                isPaste = false;
                GenericWindow windowAdd = new GenericWindow();
                AddItemOut addItemOut = new AddItemOut(txtBarcode.Text, outInventories);

                addItemOut.SetParent(commonPage);
                addItemOut.Tag = this;

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
    }
}
