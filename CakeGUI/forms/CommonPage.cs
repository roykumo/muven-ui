using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CakeGUI.forms
{
    public class CommonPage
    {
        public string Title { get; set; }
        public CommonPage ParentPage { get; set; } 
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
