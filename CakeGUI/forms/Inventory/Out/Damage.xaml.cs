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
        private bool IsReadOnly { get; set; }

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

        public Damage(InventoryOutEntity inventoryOut, bool readOnly)
        {
            InitializeComponent();
            IsReadOnly = readOnly;

            commonPage = new CommonPage();
            if (type != null)
            {
                commonPage.Title = "Penghapusan Barang";
            }

            lblTitle.Text = commonPage.Title;

            lblSiteMap.Content = commonPage.Title;

            cmbType.IsEnabled = false;
            txtTransactionCode.IsEnabled = false;
            dateOut.IsEnabled = false;
            btnAddOut.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnConfirm.IsEnabled = false;

            if (inventoryOut != null)
            {
                if (inventoryOut.ProductType != null)
                {
                    List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();
                    productTypes.Add(inventoryOut.ProductType);
                    cmbType.ItemsSource = productTypes;
                    cmbType.SelectedIndex = 0;
                }

                txtTransactionCode.Text = inventoryOut.TransactionCode;
                dateOut.SelectedDate = inventoryOut.Date;
                
                if(inventoryOut.Items!=null && inventoryOut.Items.Count > 0)
                {
                    dataGridOut.ItemsSource = inventoryOut.Items;
                }
            }
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
            catch (Exception ex)
            {
                MessageBox.Show("failed init : "+ex.Message);
            }
        }

        private void clearData()
        {
            try
            {
                txtTransactionCode.Text = inventoryService.getTrxCode("EX", null);
                btnAddOut.IsEnabled = true;
                btnConfirm.IsEnabled = true;
                outInventories.Clear();
                inventories.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed clear data : "+ex.Message);
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

                if (outInventories != null && outInventories.Count > 0)
                {
                    cmbType.IsEnabled = false;
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
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Selesaikan Penghapusan?", "Konfirmasi Penghapusan", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        InventoryOutEntity outInventory = new InventoryOutEntity();
                        outInventory.Date = dateOut.SelectedDate.Value;
                        outInventory.Items = outInventories;
                        outInventory.Type = type;
                        outInventory.TransactionCode = txtTransactionCode.Text;
                        outInventory.ProductType = (ProductTypeEntity)cmbType.SelectedItem;

                        inventoryOutService.saveProductInventory(outInventory);

                        clearData();

                        dateOut.SelectedDate = DateTime.Now;

                        MessageBox.Show("Penghapusan Barang Berhasil");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed confirm : "+ex.Message);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
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
                if (cmbType.SelectedItem != null)
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
            catch (Exception ex)
            {
                MessageBox.Show("failed add out : "+ex.Message);
            }
        }

        private void EditItemOutClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsReadOnly)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed edit : " + ex.Message);
            }
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
                    sellPriceOverride.SetParent(commonPage);
                    sellPriceOverride.Tag = this;
                    editSellPrice.Content = sellPriceOverride;
                    editSellPrice.Owner = (this.Tag as MainWindow);
                    editSellPrice.ShowDialog();

                    cellContent.SellPriceTrx = currentSellPrice.SellingPrice;

                    loadDataOut();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed edit harga jual : "+ex.Message);
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

    }
}
