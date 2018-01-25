using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class InventoryOutEntity
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("date")]
        [JsonConverter(typeof(ISODateConverter))]
        public DateTime Date { get; set; }
        [JsonProperty("totalPrice")]
        public Decimal TotalPrice { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("inventoryIn")]
        public InventoryEntity InventoryIn { get; set; }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }
        [JsonProperty("payment")]
        public PaymentEntity Payment { get; set; }
        [JsonProperty("productType")]
        public ProductTypeEntity ProductType { get; set; }

        [JsonProperty("items")]
        public List<InventoryItemOutEntity> Items { get; set; }
    }
}
