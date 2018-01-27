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

        public Brush AlertColor
        {
            get
            {
                if(ExpiredDate!=null && Product!=null && Product.Category.Type != null)
                {
                    if (Product.Category.Type.Expiration)
                    {
                        int dateDiff = (ExpiredDate - DateTime.Now).Days;
                        if (dateDiff > Product.AlertGreen)
                        {
                            return new SolidColorBrush(Colors.Blue);
                        }
                        else if (dateDiff > Product.AlertYellow)
                        {
                            return new SolidColorBrush(Colors.Green);
                        }
                        else if (dateDiff > Product.AlertRed)
                        {
                            return new SolidColorBrush(Colors.Yellow);
                        }
                        else
                        {
                            return new SolidColorBrush(Colors.Red);
                        }
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        int dateDiff = ((ExpiredDate.Year - now.Year) * 12 )+(ExpiredDate.Month - now.Month);
                        if (dateDiff > Product.AlertGreen)
                        {
                            return new SolidColorBrush(Colors.Blue);
                        }
                        else if (dateDiff > Product.AlertYellow)
                        {
                            return new SolidColorBrush(Colors.Green);
                        }
                        else if (dateDiff > Product.AlertRed)
                        {
                            return new SolidColorBrush(Colors.Yellow);
                        }
                        else
                        {
                            return new SolidColorBrush(Colors.Red);
                        }
                    }
                }
                else
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
        }
    }
}
