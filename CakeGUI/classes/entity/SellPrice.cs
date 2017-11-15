using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class SellPrice
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("date")]
        [JsonConverter(typeof(ISODateConverter))]
        public DateTime Date { get; set; }
        [JsonProperty("buyPrice")]
        public decimal BuyPrice { get; set; }
        [JsonProperty("sellingPrice")]
        public decimal SellingPrice { get; set; }
        public decimal Profit { get {return SellingPrice - BuyPrice; } }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
        [JsonProperty("sale")]
        public bool Sale { get; set; }
        [JsonProperty("product")]
        public ProductEntity Product { get; set; }
    }
}
