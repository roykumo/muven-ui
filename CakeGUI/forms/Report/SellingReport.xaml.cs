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
using CakeGUI.forms.Report;

namespace CakeGUI.forms
{
    /// <summary>
    /// Interaction logic for ProductStock.xaml
    /// </summary>
    public partial class SellingReport : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TransactionService transactionService = TransactionServiceRestImpl.Instance;
        private static ProductInventoryOutService productInventoryOutService = ProductInventoryOutServiceRestImpl.Instance;
        private static ProductInventoryService productInventoryService = ProductInventoryServiceRestImpl.Instance;
        private static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;
        
        private List<ProductTypeEntity> productTypes = new List<ProductTypeEntity>();

        private CommonPage commonPage;

        public SellingReport()
        {
            InitializeComponent();

            commonPage = new CommonPage();
            commonPage.Title = "Laporan Penjualan";
            lblSiteMap.Content = commonPage.Title;

            init();
        }
        
        private void init()
        {
            try
            {
                cmbMonth.ItemsSource = Utils.ListMonth;
                cmbMonth.SelectedIndex = DateTime.Now.Month-1;

                productTypes = productTypeService.getProductTypes();
                cmbType.ItemsSource = productTypes;
                cmbType.SelectedIndex = 1;

                txtYear.Text = DateTime.Now.Year.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed init : "+ex.Message);
            }
        }
        
        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show("failed combo change : "+ex.Message);
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
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblSiteMap.Content = commonPage.TitleSiteMap;
        }

        private void txtYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Utils.IsTextAllowed(e.Text);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GenericWindow windowAdd = new GenericWindow();
            String url = "http://localhost:8908/transaction/sell/report/print?type={0}&month={1}&year={2}";

            PdfViewer pdfViewer = new PdfViewer(String.Format(url, ((ProductTypeEntity)cmbType.SelectedItem).Id, ((MonthEntity)cmbMonth.SelectedItem).Id, txtYear.Text));

            pdfViewer.SetParent(commonPage);
            pdfViewer.Tag = this;

            windowAdd.Content = pdfViewer;
            //windowAdd.Owner = (this.Tag as MainWindow);
            windowAdd.ShowDialog();
        }
    }
}
