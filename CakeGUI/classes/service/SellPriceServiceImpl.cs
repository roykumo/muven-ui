using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    class SellPriceServiceImpl: SellPriceService
    {
        private static ProductService productService = ProductServiceImpl.Instance;

        public List<SellPrice> getSellPrices()
        {
            return new List<SellPrice>(mapSellPrice.Values);
        }

        public SellPrice getSellPrice(string id)
        {
            return mapSellPrice[id];
        }

        private static SellPriceServiceImpl instance;
        private SellPriceServiceImpl() { }

        public static SellPriceServiceImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SellPriceServiceImpl();
                }
                return instance;
            }
        }

        static Dictionary<String, SellPrice> mapSellPrice = null;

        static SellPriceServiceImpl()
        {
            fillMap();
        }

        static void fillMap()
        {
            if (mapSellPrice == null)
            {
                mapSellPrice = new Dictionary<String, SellPrice>();
            }

            SellPrice price1 = new SellPrice();
            price1.BuyPrice = 95000;
            price1.Date = new DateTime(2017, 10, 02);
            price1.Id = "1";
            price1.Remarks = "baru beli";
            price1.SellingPrice = 127500;
            price1.Sale = false;
            price1.Product = productService.getProduct("1");

            SellPrice price2 = new SellPrice();
            price2.BuyPrice = 85000;
            price2.Date = new DateTime(2017, 10, 31);
            price2.Id = "2";
            price2.Remarks = "Sale";
            price2.SellingPrice = 100000;
            price2.Sale = true;
            price2.Product = productService.getProduct("1");

            mapSellPrice.Add("1", price1);
            mapSellPrice.Add("2", price2);

        }


    }
}
