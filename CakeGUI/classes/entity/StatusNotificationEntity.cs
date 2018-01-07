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
    public class StatusNotificationEntity
    {
        [JsonProperty("product")]
        public ProductEntity Product { get; set; }
        [JsonProperty("quantityRed")]
        public Int32 QuantityRed { get; set; }
        [JsonProperty("quantityYellow")]
        public Int32 QuantityYellow { get; set; }
        [JsonProperty("quantityGreen")]
        public Int32 QuantityGreen { get; set; }
        [JsonProperty("quantityBlue")]
        public Int32 QuantityBlue { get; set; }
        [JsonProperty("quantityOut")]
        public Int32 QuantityOut { get; set; }
        [JsonProperty("expiredDate")]
        [JsonConverter(typeof(ISODateConverter))]
        public DateTime ExpiredDate { get; set; }
        [JsonProperty("buyPrice")]
        public Decimal _BuyPrice { get; set; }
        [JsonProperty("sellPrice")]
        public SellPrice SellPrice { get; set; }

        public Decimal Profit
        {
            get
            {
                if(SellPrice != null)
                {
                    return SellPrice.SellingPrice - BuyPrice;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Brush ProfitColor
        {
            get
            {
                if (Profit < 0) return new SolidColorBrush(Colors.Red);
                else return new SolidColorBrush(Colors.Black);
            }
        }

        public Decimal BuyPrice
        {
            get { return _BuyPrice / ((QuantityRed+QuantityYellow+QuantityGreen+QuantityBlue)-QuantityOut); }
            set { _BuyPrice = value; }
        }

        public Int32 RecalculateQuantity(string color)
        {
            Int32 qOut = QuantityOut;
            Int32 qRed = QuantityRed;
            Int32 qYellow = QuantityYellow;
            Int32 qGreen = QuantityGreen;
            Int32 qBlue = QuantityBlue;

            if (qRed >= qOut)
            {
                qRed -= qOut;
                qOut = 0;
            }
            else
            {
                qOut -= qRed;
                qRed = 0;
            }

            if (color.Equals("RED"))
            {
                return qRed;
            }

            if (qYellow >= qOut)
            {
                qYellow -= qOut;
                qOut = 0;
            }
            else
            {
                qOut -= qYellow;
                qYellow = 0;
            }

            if (color.Equals("YELLOW"))
            {
                return qYellow;
            }

            if (qGreen >= qOut)
            {
                qGreen -= qOut;
                qOut = 0;
            }
            else
            {
                qOut -= qGreen;
                qGreen = 0;
            }

            if (color.Equals("GREEN"))
            {
                return qGreen;
            }

            if (qBlue >= qOut)
            {
                qBlue -= qOut;
                qOut = 0;
            }
            else
            {
                qOut -= qBlue;
                qBlue = 0;
            }

            if (color.Equals("BLUE"))
            {
                return qBlue;
            }

            return 0;
        }

        public Int32 QuantityRedCount
        {
            get
            {
                return RecalculateQuantity("RED");
            }
        }

        public Int32 QuantityYellowCount
        {
            get
            {
                return RecalculateQuantity("YELLOW");
            }
        }

        public Int32 QuantityGreenCount
        {
            get
            {
                return RecalculateQuantity("GREEN");
            }
        }

        public Int32 QuantityBlueCount
        {
            get
            {
                return RecalculateQuantity("BLUE");
            }
        }

        public Brush AlertColor
        {
            get
            {
                if(ExpiredDate!=null && Product!=null && Product.Type != null)
                {
                    if (Product.Type.Expiration)
                    {
                        int dateDiff = (ExpiredDate - DateTime.Now).Days;
                        if (dateDiff > Product.AlertGreen)
                        {
                            return new SolidColorBrush(Colors.Transparent);
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
                            return new SolidColorBrush(Colors.Transparent);
                        }
                        else if (dateDiff > Product.AlertYellow)
                        {
                            return new SolidColorBrush(Colors.Transparent);
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
