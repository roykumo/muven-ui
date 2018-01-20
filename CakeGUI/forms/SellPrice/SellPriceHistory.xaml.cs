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
    public partial class SellPriceHistory : Page
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static SellPriceService sellPriceService = SellPriceServiceRestImpl.Instance;

        private CommonPage commonPage;
        private decimal avgBuyPrice;

        public SellPriceHistory()
        {
            InitializeComponent();
            init();
        }

        public SellPriceHistory(ProductEntity product)
        {
            InitializeComponent();
            this.product = product;
            init();
        }

        public ProductEntity product;     
        public List<SellPrice> sellPrices { get; set; }

        private void init()
        {
            try
            {
                commonPage = new CommonPage();
                commonPage.Title = "Penetapan Harga Jual";

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
                if (product != null)
                {
                    sellPrices = sellPriceService.getSellPrices(product);
                    this.dataGrid.ItemsSource = null;
                    this.dataGrid.ItemsSource = sellPrices;
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
                SellPriceMaintenance sellPricePage = new SellPriceMaintenance(product);
                sellPricePage.SetParent(commonPage);
                sellPricePage.setAvgBuyPrice(avgBuyPrice);

                windowAdd.Content = sellPricePage;
                windowAdd.Owner = (this.Tag as MainWindow);
                windowAdd.ShowDialog();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed add : "+ex.Message);
            }
        }

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblSiteMap.Content = commonPage.TitleSiteMap;
            lblTitle.Text = product.Name + " [" + product.BarCode + "]";
        }

        public void setAvgBuyPrice(decimal price)
        {
            avgBuyPrice = price;
        }
    }
}
