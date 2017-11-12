using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CakeGUI.classes.entity
{
    public class ProductStockEntity
    {
        public ProductEntity Product { get; set; }
        public Int32 Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
        public Decimal BuyPrice { get; set; }
        public SellPrice SellPrice { get; set; }

        public Color AlertColor
        {
            get
            {
                if(ExpiredDate!=null && Product!=null && Product.Type != null)
                {
                    int dateDiff = (ExpiredDate - DateTime.Now).Days;
                    if (dateDiff > Product.AlertGreen)
                    {
                        return Colors.Transparent;
                    }else if(dateDiff > Product.AlertYellow)
                    {
                        return Colors.Green;
                    }else if(dateDiff > Product.AlertRed)
                    {
                        return Colors.Yellow;
                    }
                    else
                    {
                        return Colors.Red;
                    }
                }
                else
                {
                    return Colors.Transparent;
                }
            }
        }
    }
}
