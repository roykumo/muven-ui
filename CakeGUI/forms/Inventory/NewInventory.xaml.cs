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
    public partial class NewInventory : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        ProductInventoryService inventoryService = ProductInventoryServiceRestImpl.Instance;
        ProductInventoryItemService inventoryItemService = ProductInventoryItemServiceRestImpl.Instance;
        ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
        private CommonPage commonPage;
        public bool IsReadOnly { get; set; }

        public NewInventory()
        {
            InitializeComponent();
            init();
        }

        public NewInventory(InventoryEntity inventory, bool readOnly)
        {
            InitializeComponent();
            IsReadOnly = readOnly;
            btnCancel.IsEnabled = false;
            btnConfirm.IsEnabled = false;
            btnAdd.IsEnabled = false;
            cmbType.IsEnabled = false;
            txtInvoice.IsEnabled = false;
            txtSupplier.IsEnabled = false;
            txtTransactionCode.IsEnabled = false;
            date.IsEnabled = false;
            //txtTotalBuyPrice.IsEnabled = false;
            //txtTotalBuyPriceRemark.IsEnabled = false;

            commonPage = new CommonPage();
            commonPage.Title = "Pembelian";
            lblSiteMap.Content = commonPage.Title;

            if (inventory != null)
            {
                if (readOnly)
                {
                    if (inventory.ProductType != null)
                    {
                        List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
                        productTypes.Add(inventory.ProductType);
                        cmbType.ItemsSource = productTypes;
                        cmbType.SelectedIndex = 0;
                    }

                    txtInvoice.Text = inventory.Invoice;
                    txtSupplier.Text = inventory.Supplier;
                    txtTransactionCode.Text = inventory.TransactionCode;
                    date.SelectedDate = inventory.Date;

                    if(inventory.Items!=null && inventory.Items.Count > 0)
                    {
                        dataGrid.ItemsSource = inventory.Items;
                    }

                }
            }
        }
        
        public DateTime TrxDate { get; set; }
        public List<CakeGUI.classes.entity.InventoryItemEntity> inventories { get; set; }
        
        private void init()
        {
            try
            {
                commonPage = new CommonPage();
                commonPage.Title = "Pembelian";
                lblSiteMap.Content = commonPage.Title;

                //lblTitle.Text += trxDate.ToString("yyyy-MM-dd");
                inventories = new List<InventoryItemEntity>();

                productTypes = productTypeService.getProductTypes();
                cmbType.ItemsSource = productTypes;
                cmbType.SelectedIndex = 0;

                TrxDate = DateTime.Now;
                date.SelectedDate = TrxDate;
                date.Text = TrxDate.ToString("yyyy/MM/dd");

                txtTransactionCode.Text = inventoryService.getTrxCode("PU", null);

                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed init : "+ex.Message);
            }
        }

        private void loadData()
        {
            try
            {
                this.dataGrid.ItemsSource = null;
                this.dataGrid.ItemsSource = inventories;

                if (inventories != null && inventories.Count > 0)
                {
                    cmbType.IsEnabled = false;
                    //txtTotalBuyPrice.Text = inventories.Sum(i => i.PurchasePrice).ToString();
                }
                else
                {
                    cmbType.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed loadData : "+ex.Message);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GenericWindow windowAdd = new GenericWindow();
                Inventory inventoryPage = new Inventory(inventories);
                inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
                inventoryPage.SetParent(commonPage);
                inventoryPage.Tag = this;
                inventoryPage.TrxType = "PU";

                windowAdd.Content = inventoryPage;
                windowAdd.Owner = (this.Tag as MainWindow);
                windowAdd.ShowDialog();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed add : "+ex.Message);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
                if (type != null)
                {
                    dataGrid.Columns[3].Header = type.Expiration ? "Tanggal Kadaluarsa" : "Tanggal Aging";
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
                if (String.IsNullOrEmpty(txtSupplier.Text))
                {
                    MessageBox.Show("Supplier harus diisi!");
                }
                else if (String.IsNullOrEmpty(txtTransactionCode.Text))
                {
                    MessageBox.Show("Kode transaksi harus diisi!");
                }
                //else if (String.IsNullOrEmpty(txtTotalBuyPrice.Text))
                //{
                //    MessageBox.Show("Total harga harus diisi!");
                //}
                else if (inventories.Count == 0)
                {
                    MessageBox.Show("Input barang beli dulu!");
                }
                else
                {
                    string confirmation = "Jenis Barang \t\t: " + (cmbType.SelectedItem as ProductTypeEntity).Description + "\n\r";
                    confirmation += "Nomor Invoice \t\t: " + txtInvoice.Text + "\n\r";
                    confirmation += "Supplier \t\t\t: " + txtSupplier.Text + "\n\r";
                    confirmation += "Kode Transaksi \t\t: " + txtTransactionCode.Text + "\n\r";
                    confirmation += "Tanggal Barang Masuk \t: " + date.SelectedDate.Value.ToString("yyyy/MM/dd") + "\n\r";
                    //confirmation += "\n\rTotal Pembayaran \t\t: " + decimal.Parse(txtTotalBuyPrice.Text).ToString("0,0") + "\n\r";
                    confirmation += "\n\rSelesaikan pembelian?";

                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(confirmation, "Konfirmasi Beli", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        InventoryEntity inventory = new InventoryEntity();
                        inventory.TransactionCode = txtTransactionCode.Text;
                        inventory.Invoice = txtInvoice.Text;
                        inventory.Supplier = txtSupplier.Text;
                        inventory.Date = date.SelectedDate.Value;
                        //inventory.TotalPrice = Int32.Parse(txtTotalBuyPrice.Text);
                        inventory.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
                        inventory.Items = inventories;

                        inventoryService.saveProductInventory(inventory);

                        inventories.Clear();
                        loadData();
                        txtTransactionCode.Text = "";
                        txtInvoice.Text = "";
                        txtSupplier.Text = "";
                        //txtTotalBuyPrice.Text = "";
                        //txtTotalBuyPriceRemark.Text = "";
                        date.SelectedDate = DateTime.Now;

                        //MessageBox.Show("Pembelian berhasil disimpan");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed confirm : " +ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin batalkan Pembelian?", "Konfirmasi Batal", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    (this.Tag as MainWindow).loadProductStock();

                    inventories.Clear();
                    txtTransactionCode.Text = inventoryService.getTrxCode("PU", null);
                    //txtTotalBuyPrice.Text = "";
                    loadData();
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

        private void EditItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsReadOnly)
                {
                    InventoryItemEntity cellContent = (InventoryItemEntity)dataGrid.SelectedItem;
                    //MessageBox.Show(cellContent.Product.Name);

                    GenericWindow windowAdd = new GenericWindow();
                    Inventory inventoryPage = new Inventory(cellContent);
                    inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
                    inventoryPage.SetParent(commonPage);
                    inventoryPage.Tag = this;

                    windowAdd.Content = inventoryPage;
                    windowAdd.Owner = (this.Tag as MainWindow);
                    windowAdd.ShowDialog();
                    loadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed edit : "+ex.Message);
            }
        }
        private void DeleteItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsReadOnly)
                {
                    InventoryItemEntity cellContent = (InventoryItemEntity)dataGrid.SelectedItem;
                    if (cellContent != null)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin hapus Barang?", "Konfirmasi Hapus", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            inventories.Remove(cellContent);
                            loadData();
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
    }
}
