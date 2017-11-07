using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    class ProductStockServiceImpl: ProductStockService
    {
        List<ProductStockEntity> productStocks = null;
        public List<ProductStockEntity> getProductStock()
        {
            if(productStocks == null)
            {
                productStocks = new List<ProductStockEntity>();

                ProductEntity prd1 = new ProductEntity();
                prd1.AlertGreen = 90;
                prd1.AlertRed = 10;
                prd1.AlertYellow = 30;
                prd1.BarCode = "123123123";
                prd1.Name = "Beras Topi Koki @25Kg";

                ProductEntity prd2 = new ProductEntity();
                prd2.AlertGreen = 30;
                prd2.AlertRed = 3;
                prd2.AlertYellow = 10;
                prd2.BarCode = "234234234";
                prd2.Name = "Susu Bayi @1Kg";

                ProductStockEntity prdStock = new ProductStockEntity();
                prdStock.BuyPrice = 95000;
                prdStock.ExpiredDate = DateTime.Now;
                prdStock.Product = prd1;
                prdStock.Quantity = 5;
                prdStock.SellPrice = 115000;

                ProductStockEntity prdStock2 = new ProductStockEntity();
                prdStock2.BuyPrice = 50000;
                prdStock2.ExpiredDate = DateTime.Now;
                prdStock2.Product = prd2;
                prdStock2.Quantity = 35;
                prdStock2.SellPrice = 115000;

                productStocks.Add(prdStock);
                productStocks.Add(prdStock2);

                return productStocks;
            }
            else { return productStocks; }
        }
        
    }
}
