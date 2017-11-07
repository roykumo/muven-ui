using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class ProductStockEntity
    {
        public ProductEntity Product { get; set; }
        public Int32 Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
        public Decimal BuyPrice { get; set; }
        public Decimal SellPrice { get; set; }
    }
}
