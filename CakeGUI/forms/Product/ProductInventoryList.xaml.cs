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
        private static ProductService productService = ProductServiceImpl.Instance;

        ProductInventoryService inventoryService = ProductInventoryServiceImpl.Instance;
        
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
        
        public List<CakeGUI.classes.entity.InventoryEntity> inventories { get; set; }
        
        private void init()
        {
            if(product == null)
                product = productService.getProduct("1");

            inventories = inventoryService.getProductInventories(product);

            this.dataGrid.ItemsSource = inventories;

            lblProduct.Text = product.Name + " [" + product.BarCode + "]";
        }
        
    }
}
