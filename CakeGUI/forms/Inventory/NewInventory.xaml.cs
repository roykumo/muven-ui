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

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for ProductInventoryList.xaml
    /// </summary>
    public partial class NewInventory : Page
    {
        private static ProductService productService = ProductServiceImpl.Instance;

        ProductInventoryItemService inventoryService = ProductInventoryItemServiceRestImpl.Instance;
        
        public NewInventory()
        {
            InitializeComponent();
            init();
        }

        private DateTime trxDate = DateTime.Now;
        public List<CakeGUI.classes.entity.InventoryItemEntity> inventories { get; set; }
        
        private void init()
        {
            lblTitle.Text += trxDate.ToString("yyyy-MM-dd");
            inventories = new List<InventoryItemEntity>();
            loadData();
        }

        private void loadData()
        {
            this.dataGrid.ItemsSource = null;
            this.dataGrid.ItemsSource = inventories;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            GenericWindow windowAdd = new GenericWindow();
            Inventory inventoryPage = new Inventory(inventories);

            windowAdd.Content = inventoryPage;
            windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
            loadData();
        }
    }
}
