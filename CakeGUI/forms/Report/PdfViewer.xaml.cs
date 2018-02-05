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

namespace CakeGUI.forms.Report
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Page
    {
        public PdfViewer()
        {
            InitializeComponent();
            commonPage = new CommonPage();
            commonPage.Title = "Pdf Viewer";
        }

        public PdfViewer(string url)
        {
            InitializeComponent();
            commonPage = new CommonPage();
            commonPage.Title = "Pdf Viewer";
            setUrl(url);
        }

        public void setUrl(string url)
        {
            webBrowser.Source = new Uri(url);
            this.webBrowser.Navigating += new NavigatingCancelEventHandler(webBrowser_Navigating);
        }

        private CommonPage commonPage;

        public void SetParent(CommonPage page)
        {
            if (commonPage != null)
            {
                commonPage.ParentPage = page;
            }
        }

        private void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
