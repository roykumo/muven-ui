using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.forms
{
    public interface ICommonPage
    {
        void SetParent(CommonPage page);
        CommonPage GetParent();
    }
}
