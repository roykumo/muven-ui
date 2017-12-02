using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class InventoryEntity
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("transactionCode")]
        public String TransactionCode { get; set; }
        [JsonProperty("date")]
        [JsonConverter(typeof(ISODateConverter))]
        public DateTime Date { get; set; }
        [JsonProperty("totalPrice")]
        public Decimal TotalPrice { get; set; }

        [JsonProperty("items")]
        public List<InventoryItemEntity> Items { get; set; }
    }
}
