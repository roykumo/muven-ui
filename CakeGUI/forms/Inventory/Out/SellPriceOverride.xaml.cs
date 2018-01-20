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
using CakeGUI.classes.util;

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class SellPriceOverride : Page
    {
        
        private CommonPage commonPage;
        public bool EditMode { get; set; }
        public SellPrice SellPrice { get; set; }
        public decimal SellPriceTrx{ get; set; }

        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Add New";
            lblSiteMap.Content = commonPage.Title;
            
        }
        
        public SellPriceOverride(SellPrice sellPrice, decimal sellPriceTrx)
        {
            InitializeComponent();
            this.SellPrice = sellPrice;
            if (this.SellPrice != null && this.SellPrice.SellingPrice>0)
            {
                txtSellPrice.Text = this.SellPrice.SellingPrice.ToString("0,0");
            }

            this.SellPriceTrx = sellPriceTrx;
            if(this.SellPriceTrx >0)
            {
                txtSellPriceTrx.Text = this.SellPriceTrx.ToString("0,0");
            }

            init();
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
                if (string.IsNullOrEmpty(txtSellPriceTrx.Text))
                {
                    MessageBox.Show("Isi jumlah barang!");
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Yakin simpan?", "Konfirmasi Simpan", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        SellPriceTrx = decimal.Parse(txtSellPriceTrx.Text);
                        SellPrice.SellingPrice = SellPriceTrx;

                        GenericWindow genericWindow = ((GenericWindow)this.Parent);
                        genericWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed save : "+ex.Message);
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
            commonPage.Title = (EditMode ? "Edit" : "Add");
            lblSiteMap.Content = commonPage.TitleSiteMap;
        }
        
        private void txtSellPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }
    }
}
