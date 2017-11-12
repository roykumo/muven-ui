using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class SellPrice
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Profit { get {return SellingPrice - BuyPrice; } }
        public string Remarks { get; set; }
        public bool Sale { get; set; }
        public ProductEntity Product { get; set; }
    }
}
