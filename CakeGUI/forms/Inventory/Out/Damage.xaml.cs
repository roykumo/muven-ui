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
    public partial class Damage : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        ProductInventoryService inventoryService = ProductInventoryServiceRestImpl.Instance;
        ProductInventoryItemService inventoryItemService = ProductInventoryItemServiceRestImpl.Instance;
        ProductInventoryOutService inventoryOutService = ProductInventoryOutServiceRestImpl.Instance;
        ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
        private CommonPage commonPage;
        //private CommonPage commonPageOut;
        private string type="EX";

        public Damage()
        {
            InitializeComponent();
            init();
        }

        public Damage(string type)
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
                commonPage.Title = "Penghapusan Barang";
            }

            lblTitle.Text = commonPage.Title;

            commonPage.ParentPage = parent;
            lblSiteMap.Content = commonPage.Title;
            
            //commonPageOut = new CommonPage();
            //commonPageOut.Title = "Penghapusan Barang";
            //commonPageOut.ParentPage = commonPage;
            
            inventories = new List<InventoryItemEntity>();
            outInventories = new List<InventoryItemOutEntity>();

            productTypes = productTypeService.getProductTypes();
            cmbType.ItemsSource = productTypes;
            cmbType.SelectedIndex = 0;

            dateOut.SelectedDate = DateTime.Now;

            loadData();

            txtTransactionCode.Text = inventoryService.getTrxCode("EX", null);
        }

        private void clearData()
        {
            txtTransactionCode.Text = inventoryService.getTrxCode("EX", null);
            btnAddOut.IsEnabled = true;
            btnConfirm.IsEnabled = true;
            outInventories.Clear();
            inventories.Clear();
        }

        private void loadData()
        {
            
        }

        private void loadDataOut()
        {
            this.dataGridOut.ItemsSource = null;
            this.dataGridOut.ItemsSource = outInventories;

            if (outInventories != null && outInventories.Count > 0)
            {
                cmbType.IsEnabled = false;
            }
            else
            {
                cmbType.IsEnabled = true;
            }
            
        }
        
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeEntity type = ((sender as ComboBox)).SelectedItem as ProductTypeEntity;
            if (type != null)
            {
                loadData();
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (inventories.Count == 0 && outInventories.Count == 0)
            {
                MessageBox.Show("Input barang dulu!");
            }
            else
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Selesaikan Penghapusan?", "Konfirmasi Penghapusan", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    InventoryOutEntity outInventory = new InventoryOutEntity();
                    outInventory.Date = dateOut.SelectedDate.Value;
                    outInventory.Items = outInventories;
                    outInventory.Type = type;
                    
                    inventoryOutService.saveProductInventory(outInventory);

                    clearData();

                    dateOut.SelectedDate = DateTime.Now;

                    MessageBox.Show("Penghapusan Barang Berhasil");
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Yakin batalkan Penghapusan?", "Konfirmasi Batal", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                inventories.Clear();
                outInventories.Clear();
                loadData();
                loadDataOut();
            }
        }
        
        private void txtTotalBuyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }
        
        private void btnAddOut_Click(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedItem != null )
            {
                GenericWindow windowAdd = new GenericWindow();
                InventoryItemOut inventoryPage = new InventoryItemOut(outInventories);
                inventoryPage.LabelTitle = "Hapus Barang";
                inventoryPage.ProductType = (ProductTypeEntity)cmbType.SelectedItem;

                inventoryPage.SetParent(commonPage);
                inventoryPage.Tag = this;

                windowAdd.Content = inventoryPage;
                windowAdd.Owner = (this.Tag as MainWindow);
                windowAdd.ShowDialog();
                loadDataOut();
            }
        }

        private void EditItemOutClicked(object sender, RoutedEventArgs e)
        {

        }

        private void EditSellPriceClicked(object sender, RoutedEventArgs e)
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
