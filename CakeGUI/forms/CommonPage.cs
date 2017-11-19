using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CakeGUI.forms
{
    public class CommonPage
    {
        private string title;
        private CommonPage commonPage;

        public string Title { get { return title; } set { title = value; } }
        public CommonPage ParentPage { get { return commonPage; } set { commonPage = value;} } 
        public string TitleSiteMap
        {
            get
            {
                if (ParentPage != null && !string.IsNullOrEmpty(ParentPage.TitleSiteMap))
                {
                    return ParentPage.TitleSiteMap + " > " + Title;
                }
                else
                {
                    return Title;
                }
            }
        }
    }
}
