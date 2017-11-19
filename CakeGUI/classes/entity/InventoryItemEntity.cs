using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class InventoryItemEntity
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        //public String TransactionCode { get; set; }
        [JsonProperty("product")]
        public ProductEntity Product { get; set; }
        //public DateTime PurchaseDate { get; set; }
        [JsonProperty("expiredDate")]
        [JsonConverter(typeof (ISODateConverter))]
        public DateTime ExpiredDate { get; set; }
        [JsonProperty("quantity")]
        public Int32 Quantity { get; set; }
        [JsonProperty("purchasePrice")]
        public Decimal PurchasePrice { get; set; }
        [JsonProperty("remarks")]
        public String Remarks { get; set; }
        [JsonProperty("inventory")]
        public InventoryEntity Inventory { get; set; }
    }
}
