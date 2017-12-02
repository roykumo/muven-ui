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

        public NewInventory()
        {
            InitializeComponent();
            init();
        }

        private DateTime trxDate = DateTime.Now;
        public List<CakeGUI.classes.entity.InventoryItemEntity> inventories { get; set; }
        
        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Pembelian";
            lblSiteMap.Content = commonPage.Title;

            //lblTitle.Text += trxDate.ToString("yyyy-MM-dd");
            inventories = new List<InventoryItemEntity>();

            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedIndex = 0;

            date.SelectedDate = DateTime.Now;

            loadData();
        }

        private void loadData()
        {
            this.dataGrid.ItemsSource = null;
            this.dataGrid.ItemsSource = inventories;

            if(inventories != null && inventories.Count > 0)
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
            GenericWindow windowAdd = new GenericWindow();
            Inventory inventoryPage = new Inventory(inventories);
            inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;
            inventoryPage.SetParent(commonPage);
            inventoryPage.Tag = this;

            windowAdd.Content = inventoryPage;
            windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
            loadData();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
            if (type != null)
            {
                dataGrid.Columns[3].Header = type.Expiration ? "Expired Date" : "Aging Date";
                loadData();
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTransactionCode.Text))
            {
                MessageBox.Show("Kode transaksi harus diisi!");
            }else if (String.IsNullOrEmpty(txtTotalBuyPrice.Text))
            {
                MessageBox.Show("Total harga harus diisi!");
            }else if (inventories.Count == 0)
            {
                MessageBox.Show("Input barang beli dulu!");
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Selesaikan Pembelian?", "Konfirmasi Beli", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    InventoryEntity inventory = new InventoryEntity();
                    inventory.TransactionCode = txtTransactionCode.Text;
                    inventory.Date = date.SelectedDate.Value;
                    inventory.TotalPrice = Int32.Parse(txtTotalBuyPrice.Text);
                    inventory.Items = inventories;

                    inventoryService.saveProductInventory(inventory);

                    inventories.Clear();
                    txtTransactionCode.Text = "";
                    txtTotalBuyPrice.Text = "";
                    date.SelectedDate = DateTime.Now;
                    
                    MessageBox.Show("Pembelian berhasil disimpan");
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin batalkan Pembelian?", "Konfirmasi Batal", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                inventories.Clear();
                txtTotalBuyPrice.Text = "";
                loadData();
            }
        }

        private void txtTotalBuyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }

        private void EditItemClicked(object sender, RoutedEventArgs e)
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
        private void DeleteItemClicked(object sender, RoutedEventArgs e)
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
}
