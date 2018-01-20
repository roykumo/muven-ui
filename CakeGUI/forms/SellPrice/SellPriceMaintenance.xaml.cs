using System;
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
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class SellPriceMaintenance : Page
    {
        private ProductService productService = ProductServiceImpl.Instance;
        private SellPriceService sellPriceService = SellPriceServiceRestImpl.Instance;
        private SellPrice SellPrice { get; set; }

        private decimal AvgBuyPrice { get; set; }

        private CommonPage commonPage;

        private void iniCommonPage()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Penetapan";
        }

        public SellPriceMaintenance()
        {
            InitializeComponent();
            iniCommonPage();
        }

        public Decimal Profit
        {
            get
            {
                try
                {
                    return (string.IsNullOrEmpty(txtSellPrice.Text) ? new decimal(0) : decimal.Parse(txtSellPrice.Text)) - AvgBuyPrice;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("failed calculate profit: "+ex.Message);
                    return 0;
                }
            }
        }

        public SellPriceMaintenance(ProductEntity product)
        {
            InitializeComponent();
            iniCommonPage();
            SellPrice = new SellPrice();
            SellPrice.Product = product;
            SellPrice.Date = DateTime.Now;
            SellPrice.Sale = false;
            radioTrue.IsChecked = SellPrice.Sale;
            radioFalse.IsChecked = !SellPrice.Sale;

            dtExpired.SelectedDate = SellPrice.Date;
        }
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((GenericWindow)this.Parent).Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed cancel : "+ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan Harga Jual?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SellPrice.SellingPrice = decimal.Parse(txtSellPrice.Text);
                    SellPrice.Sale = (bool)radioTrue.IsChecked;
                    SellPrice.Remarks = txtRemarks.Text;

                    sellPriceService.saveSellPrice(SellPrice);

                    GenericWindow genericWindow = ((GenericWindow)this.Parent);
                    //((MainWindow)genericWindow.Owner).refreshFrame();
                    genericWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed save : "+ex.Message);
            }
        }

        public void SetParent(CommonPage page)
        {
            if (page != null)
            {
                commonPage.ParentPage = page;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblSiteMap.Content = commonPage.TitleSiteMap;
            lblTitle.Text = SellPrice.Product.Name + " [" + SellPrice.Product.BarCode + "]";
            txtBuyPrice.Text = SellPrice.BuyPrice.ToString("0,0");
        }

        public void setAvgBuyPrice(decimal price)
        {
            AvgBuyPrice = price;
            SellPrice.BuyPrice = AvgBuyPrice;
        }

        private void txtSellPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtProfit.Text = Profit.ToString("0,0");
        }
    }
}
