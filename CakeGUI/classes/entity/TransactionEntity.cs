using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CakeGUI.classes.entity
{
    public class TransactionEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("date")]
        [JsonConverter(typeof(ISODateConverter))]
        public DateTime Date { get; set; }
        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("transactionCode")]
        public string TransactionCode { get; set; }
        [JsonProperty("productType")]
        public string ProductType { get; set; }

        [JsonIgnore]
        public decimal Revenue
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                {
                    return 0;
                }
                else
                {
                    if(Type.Equals("PENJUALAN ONLINE") || Type.Equals("CASH REGISTER"))
                    {
                        return TotalPrice;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        [JsonIgnore]
        public decimal Expense
        {
            get
            {
                if (string.IsNullOrEmpty(Type))
                {
                    return 0;
                }
                else
                {
                    if (Type.Equals("PEMBELIAN") || Type.Equals("REPACKING") || Type.Equals("STOCK OPNAME") || Type.Equals("PENGHAPUSAN"))
                    {
                        return TotalPrice;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
