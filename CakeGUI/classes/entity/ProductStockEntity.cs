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
    public class ProductStockEntity
    {
        [JsonProperty("product")]
        public ProductEntity Product { get; set; }
        [JsonProperty("quantity")]
        public Int32 Quantity { get; set; }
        [JsonProperty("expiredDate")]
        [JsonConverter(typeof(ISODateConverter))]
        public DateTime ExpiredDate { get; set; }
        [JsonProperty("buyPrice")]
        public Decimal _BuyPrice { get; set; }
        [JsonProperty("sellPrice")]
        public SellPrice SellPrice { get; set; }


        public Decimal BuyPrice
        {
            get { return _BuyPrice / Quantity; }
            set { _BuyPrice = value; }
        }

        public Color AlertColor
        {
            get
            {
                if(ExpiredDate!=null && Product!=null && Product.Type != null)
                {
                    int dateDiff = (ExpiredDate - DateTime.Now).Days;
                    if (dateDiff > Product.AlertGreen)
                    {
                        return Colors.Transparent;
                    }else if(dateDiff > Product.AlertYellow)
                    {
                        return Colors.Green;
                    }else if(dateDiff > Product.AlertRed)
                    {
                        return Colors.Yellow;
                    }
                    else
                    {
                        return Colors.Red;
                    }
                }
                else
                {
                    return Colors.Transparent;
                }
            }
        }
    }
}
