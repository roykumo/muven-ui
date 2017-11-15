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
    public partial class ProductInventoryList : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        ProductInventoryItemService inventoryService = ProductInventoryItemServiceRestImpl.Instance;
        
        public ProductInventoryList()
        {
            InitializeComponent();
            init();
        }

        public ProductInventoryList(ProductEntity product)
        {
            InitializeComponent();
            this.product = product;
            init();
        }

        public ProductEntity product;
        
        public List<CakeGUI.classes.entity.InventoryItemEntity> inventories { get; set; }
        
        private void init()
        {
            if (product != null)
            {
                inventories = inventoryService.getProductInventories(product);
                dataGrid.Columns[3].Header = product.Type.Expiration ? "Expired Date" : "Aging Date";

                this.dataGrid.ItemsSource = inventories;

                lblProduct.Text = product.Name + " [" + product.BarCode + "]";
            }
        }
        private void EditProductClicked(object sender, RoutedEventArgs e)
        {
            InventoryItemEntity cellContent = (InventoryItemEntity)dataGrid.SelectedItem;
            //MessageBox.Show(cellContent.Product.Name);

            GenericWindow windowInventoryList = new GenericWindow();
            InventoryMaintenance inventoryMaintenancePage = new InventoryMaintenance(cellContent);

            windowInventoryList.Content = inventoryMaintenancePage;
            windowInventoryList.ShowDialog();
            init();
        }
    }
}
