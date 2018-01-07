using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class InventoryItemOutEntity
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        //public String TransactionCode { get; set; }
        [JsonProperty("product")]
        public ProductEntity Product { get; set; }
        [JsonProperty("quantity")]
        public Int32 Quantity { get; set; }
        [JsonProperty("purchasePrice")]
        public Decimal PurchasePrice { get; set; }
        [JsonProperty("sellPrice")]
        public SellPrice SellPrice { get; set; }
        [JsonProperty("remarks")]
        public String Remarks { get; set; }
        [JsonProperty("inventoryOut")]
        public InventoryOutEntity InventoryOut { get; set; }


        public Decimal Price
        {
            get
            {
                if (InventoryOut.Type.Equals("RP")){
                    return PurchasePrice;
                }
                else
                {
                    if (SellPrice != null)
                        return SellPrice.SellingPrice;
                    else
                        return 0;
                }
            }
        }
    }
}
