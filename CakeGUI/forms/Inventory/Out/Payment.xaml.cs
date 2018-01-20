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
    public partial class Payment : Page
    {
        public const string TYPE_CASH = "CASH";
        public const string TYPE_EDC = "EDC";

        private CommonPage commonPage;
        public bool EditMode { get; set; }
        public PaymentEntity payment { get; set; }
        public decimal SellPriceTrx{ get; set; }
        public bool IsCash { get; set; }

        private void init()
        {
            commonPage = new CommonPage();
            commonPage.Title = "Add New";
            lblSiteMap.Content = commonPage.Title;
        }
        
        public Payment(PaymentEntity payment)
        {
            InitializeComponent();
            IsCash = true;
            radioTrue.IsChecked = IsCash;
            radioFalse.IsChecked = !IsCash;
            this.payment = payment;
            if (this.payment != null)
            {
                txtPayAmount.Text = this.payment.PayAmount > 0 ? this.payment.PayAmount.ToString("0,0") : "";
                if (!string.IsNullOrEmpty(this.payment.Type))
                {
                    if (this.payment.Type.Equals(TYPE_CASH))
                    {
                        IsCash = true;
                    }else if (this.payment.Type.Equals(TYPE_EDC))
                    {
                        IsCash = false;
                    }
                    updateRadio();
                }
            }

            init();
        }

        private void updateRadio()
        {
            radioTrue.IsChecked = IsCash;
            radioFalse.IsChecked = !IsCash;
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
                if (radioTrue.IsChecked == true && string.IsNullOrEmpty(txtPayAmount.Text))
                {
                    MessageBox.Show("Isi jumlah pembayaran!");
                }
                else if (radioTrue.IsChecked == true && (decimal.Parse(txtPayAmount.Text) - payment.TotalAmount) < 0)
                {
                    MessageBox.Show("Uang bayar kurang!");
                }
                else if (radioFalse.IsChecked == true && string.IsNullOrEmpty(txtReffNo.Text))
                {
                    MessageBox.Show("Isi nomor struk!");
                }
                else
                {
                    IsCash = (radioTrue.IsChecked == true);
                    if (IsCash)
                    {
                        payment.PayAmount = decimal.Parse(txtPayAmount.Text);
                        payment.Type = TYPE_CASH;
                    }
                    else
                    {
                        payment.ReceiptNo = txtReffNo.Text;
                        payment.PayAmount = payment.TotalAmount;
                        payment.Type = TYPE_EDC;
                    }

                    GenericWindow genericWindow = ((GenericWindow)this.Parent);
                    genericWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed save : " +ex.Message);
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
            updateRadio();
        }
        
        private void txtAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }

        private void radioTrue_Checked(object sender, RoutedEventArgs e)
        {
            IsCash = true;
            radioTrue.IsChecked = true;
            txtPayAmount.IsEnabled = true;
            txtReffNo.Text = "";
            txtReffNo.IsEnabled = false;
        }

        private void radioFalse_Checked(object sender, RoutedEventArgs e)
        {
            IsCash = false;
            radioFalse.IsChecked = true;
            txtPayAmount.IsEnabled = false;
            txtPayAmount.Text = "";
            txtReffNo.IsEnabled = true;
        }
    }
}
