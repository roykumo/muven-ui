using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface SellPriceService
    {
        List<SellPrice> getSellPrices(ProductEntity product);
        SellPrice getSellPrice(string id);
        void saveSellPrice(SellPrice sellPrice);
        SellPrice getCurrentSellPrice(ProductEntity product);
    }
}
