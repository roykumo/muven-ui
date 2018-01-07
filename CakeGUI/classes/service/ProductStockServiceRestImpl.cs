using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    class ProductStockServiceRestImpl: ProductStockService
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;
        private static SellPriceService sellPriceService = SellPriceServiceImpl.Instance;

        public List<ProductStockEntity> getProductStock(ProductTypeEntity type, string barcode)
        {
            return productService.getStocks(type, barcode);
        }

        private static ProductStockServiceRestImpl instance;
        private ProductStockServiceRestImpl() { }

        public static ProductStockServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductStockServiceRestImpl();
                }
                return instance;
            }
        }
        

    }
}
