using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class InventoryEntity
    {
        public String Id { get; set; }
        public String TransactionCode { get; set; }
        public ProductEntity Product { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public Int32 Quantity { get; set; }
        public Decimal PurchasePrice { get; set; }
        public String Remarks { get; set; }
    }
}
