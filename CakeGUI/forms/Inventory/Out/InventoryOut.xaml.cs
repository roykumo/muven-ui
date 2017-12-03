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
    public partial class InventoryOut : Page
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
        private string type;

        public InventoryOut()
        {
            InitializeComponent();
            init();
        }

        public InventoryOut(string type)
        {
            InitializeComponent();
            this.type = type;
            init();
        }

        private DateTime trxDate = DateTime.Now;
        public List<CakeGUI.classes.entity.InventoryItemEntity> inventories { get; set; }
        public List<CakeGUI.classes.entity.InventoryItemOutEntity> outInventories { get; set; }

        private void init()
        {
            CommonPage parent = new CommonPage();
            parent.Title = "Stock In";

            commonPage = new CommonPage();
            if (type != null)
            {
                if (type.Equals("RP"))
                {
                    commonPage.Title = "Repacking";
                }
                else if (type.Equals("SO"))
                {
                    commonPage.Title = "Stock Opname";
                }
            }

            lblTitle.Text = commonPage.Title;

            commonPage.ParentPage = parent;
            lblSiteMap.Content = commonPage.Title;

            commonPageIn = new CommonPage();
            commonPageIn.Title = "Barang Masuk";
            commonPageIn.ParentPage = commonPage;

            commonPageOut = new CommonPage();
            commonPageOut.Title = "Barang Keluar";
            commonPageOut.ParentPage = commonPage;

            //lblTitle.Text += trxDate.ToString("yyyy-MM-dd");
            inventories = new List<InventoryItemEntity>();
            outInventories = new List<InventoryItemOutEntity>();

            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedIndex = 0;

            date.SelectedDate = DateTime.Now;
            dateOut.SelectedDate = DateTime.Now;

            loadData();
        }

        private void loadData()
        {
            this.dataGridIn.ItemsSource = null;
            this.dataGridIn.ItemsSource = inventories;
            
        }

        private void loadDataOut()
        {
            this.dataGridOut.ItemsSource = null;
            this.dataGridOut.ItemsSource = outInventories;

            if (outInventories != null && outInventories.Count > 0)
            {
                cmbType.IsEnabled = false;
                //txtTotalBuyPrice.Text = inventories.Sum(i => i.PurchasePrice).ToString();
            }
            else
            {
                cmbType.IsEnabled = true;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih jenis barang dulu!");
                cmbType.Focus();
            }
            else
            {
                GenericWindow windowAdd = new GenericWindow();
                Inventory inventoryPage = new Inventory(inventories);
                inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
                inventoryPage.SetParent(commonPageIn);
                inventoryPage.Tag = this;

                windowAdd.Content = inventoryPage;
                windowAdd.Owner = (this.Tag as MainWindow);
                windowAdd.ShowDialog();
                loadData();
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
            if (type != null)
            {
                dataGridIn.Columns[3].Header = type.Expiration ? "Expired Date" : "Aging Date";
                loadData();
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            //if (String.IsNullOrEmpty(txtTransactionCode.Text))
            //{
            //    MessageBox.Show("Kode transaksi harus diisi!");
            //}else if (String.IsNullOrEmpty(txtTotalBuyPrice.Text))
            if (inventories.Count == 0 && outInventories.Count ==0)
            {
                MessageBox.Show("Input barang keluar / masuk dulu!");
            }else if(cmbType.SelectedIndex== -1)
            {
                MessageBox.Show("Pilih jenis barang dulu!");
                cmbType.Focus();
            }else if (string.IsNullOrEmpty(txtTransactionCode.Text))
            {
                MessageBox.Show("Kode Transaksi harus diisi");
                txtTransactionCode.Focus();
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Selesaikan Pembelian?", "Konfirmasi Beli", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    InventoryOutEntity outInventory = new InventoryOutEntity();
                    outInventory.Date = dateOut.SelectedDate.Value;
                    outInventory.Items = outInventories;
                    outInventory.Type = type;

                    InventoryEntity inventory = new InventoryEntity();
                    inventory.Date = date.SelectedDate.Value;
                    inventory.Items = inventories;
                    inventory.TransactionCode = txtTransactionCode.Text;
                    inventory.TotalPrice = inventories.Sum(i => i.PurchasePrice * i.Quantity);

                    outInventory.InventoryIn = inventory;

                    inventoryOutService.saveProductInventory(outInventory);

                    inventories.Clear();
                    loadData();
                    outInventories.Clear();
                    loadDataOut();
                    txtTransactionCode.Text = "";
                    date.SelectedDate = DateTime.Now;
                    dateOut.SelectedDate = DateTime.Now;

                    MessageBox.Show((type.Equals("RP") ?"Repacking" : "Stock Opname") +" berhasil disimpan");
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin batalkan Pembelian?", "Konfirmasi Batal", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                inventories.Clear();
                outInventories.Clear();
                txtTransactionCode.Text = "";
                loadData();
                loadDataOut();
            }
        }

        private void txtTotalBuyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }

        private void EditItemClicked(object sender, RoutedEventArgs e)
        {
            InventoryItemEntity cellContent = (InventoryItemEntity)dataGridIn.SelectedItem;
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
        private void DeleteItemClicked(object sender, RoutedEventArgs e)
        {
            InventoryItemEntity cellContent = (InventoryItemEntity)dataGridIn.SelectedItem;
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

        private void btnAddOut_Click(object sender, RoutedEventArgs e)
        {
            GenericWindow windowAdd = new GenericWindow();
            InventoryItemOut inventoryPage = new InventoryItemOut(outInventories);
            inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
            
            inventoryPage.SetParent(commonPageOut);
            inventoryPage.Tag = this;

            windowAdd.Content = inventoryPage;
            windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
            loadDataOut();
        }

        private void EditItemOutClicked(object sender, RoutedEventArgs e)
        {
            InventoryItemOutEntity cellContent = (InventoryItemOutEntity)dataGridOut.SelectedItem;

            GenericWindow windowAdd = new GenericWindow();
            InventoryItemOut inventoryItemOutPage = new InventoryItemOut(cellContent);
            inventoryItemOutPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
            inventoryItemOutPage.SetParent(commonPage);
            inventoryItemOutPage.Tag = this;

            windowAdd.Content = inventoryItemOutPage;
            windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
            loadDataOut();
        }
        private void DeleteItemOutClicked(object sender, RoutedEventArgs e)
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
