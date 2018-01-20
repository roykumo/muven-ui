using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class PaymentEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("receiptNo")]
        public string ReceiptNo { get; set; }
        [JsonProperty("cardReffNo")]
        public string CardReffNo { get; set; }
        [JsonProperty("cardType")]
        public string CardType { get; set; }
        [JsonProperty("payAmount")]
        public decimal PayAmount { get; set; }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }

    }
}
