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
        private static ProductService productService = ProductServiceImpl.Instance;
        private static SellPriceService sellPriceService = SellPriceServiceImpl.Instance;

        public List<ProductStockEntity> getProductStock()
        {
            return new List<ProductStockEntity>(mapProductStock.Values);
        }

        private static ProductStockServiceImpl instance;
        private ProductStockServiceImpl() { }

        public static ProductStockServiceImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductStockServiceImpl();
                }
                return instance;
            }
        }

        static Dictionary<String, ProductStockEntity> mapProductStock = null;

        static ProductStockServiceImpl()
        {
            fillMap();
        }

        static void fillMap()
        {
            if (mapProductStock == null)
            {
                mapProductStock = new Dictionary<String, ProductStockEntity>();
            }

            /*ProductEntity prd1 = new ProductEntity();
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
            prd2.Name = "Susu Bayi @1Kg";*/

            ProductStockEntity prdStock = new ProductStockEntity();
            prdStock.BuyPrice = 95000;
            prdStock.ExpiredDate = new DateTime(2017, 10, 10);
            prdStock.Product = productService.getProduct("1");
            prdStock.Quantity = 5;
            prdStock.SellPrice = sellPriceService.getSellPrice("1");

            ProductStockEntity prdStock2 = new ProductStockEntity();
            prdStock2.BuyPrice = 50000;
            prdStock2.ExpiredDate = new DateTime(2018, 3, 24);
            prdStock2.Product = productService.getProduct("2");
            prdStock2.Quantity = 35;
            prdStock2.SellPrice = sellPriceService.getSellPrice("2");

            mapProductStock.Add("1", prdStock);
            mapProductStock.Add("2", prdStock2);

        }

        public List<ProductStockEntity> getProductStock(ProductTypeEntity type, string barcode)
        {
            throw new NotImplementedException();
        }

        public List<ProductStockEntity> getProductStock(ProductTypeEntity type, ProductCategoryEntity category, string barcode)
        {
            throw new NotImplementedException();
        }

        public List<ProductStockEntity> getProductStock(ProductTypeEntity type, ProductCategoryEntity category, string barcode, string group)
        {
            throw new NotImplementedException();
        }
    }
}
