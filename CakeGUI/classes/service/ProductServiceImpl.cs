using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
using CakeGUI.classes.entity.rest;

namespace CakeGUI.classes.service
{
    public class ProductServiceImpl : ProductService
    {
        private static ProductServiceImpl instance;
        static ProductTypeService productTypeService = ProductTypeServiceImpl.Instance;
        private static int counter = 0;

        private ProductServiceImpl() { }

        public static ProductServiceImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductServiceImpl();
                }
                return instance;
            }
        }

        static Dictionary<String, ProductEntity> mapProduct = null; 

        static ProductServiceImpl()
        {
            fillMap();            
        }
        
        static void fillMap()
        {
            if (mapProduct == null)
            {
                mapProduct = new Dictionary<String, ProductEntity>();
            }

            ProductEntity prd1 = new ProductEntity();
            prd1.Id = (++counter).ToString();
            prd1.AlertGreen = 90;
            prd1.AlertRed = 10;
            prd1.AlertYellow = 30;
            prd1.BarCode = "123123123";
            prd1.Name = "Beras Topi Koki @25Kg";
            prd1.Category = new ProductCategoryEntity();
            prd1.Category.Type = productTypeService.getProductType("1");

            mapProduct.Add(prd1.Id, prd1);

            ProductEntity prd2 = new ProductEntity();
            prd2.Id = (++counter).ToString();
            prd2.AlertGreen = 30;
            prd2.AlertRed = 3;
            prd2.AlertYellow = 10;
            prd2.BarCode = "234234234";
            prd2.Name = "Susu Bayi @1Kg";
            prd2.Category = new ProductCategoryEntity();
            prd2.Category.Type = productTypeService.getProductType("2");

            mapProduct.Add(prd2.Id, prd2);

        }

        public ProductEntity getProduct(String id)
        {
            return mapProduct[id];
        }

        public List<ProductEntity> getProducts()
        {
            return new List<ProductEntity>(mapProduct.Values);
        }

        public void saveProduct(ProductEntity product)
        {
            if (product != null)
            {
                if (String.IsNullOrEmpty(product.Id))
                {
                    product.Id = (++counter).ToString();
                }

                if (!mapProduct.ContainsKey(product.Id))
                {
                    mapProduct.Add(product.Id, product);
                }
                else
                {
                    mapProduct[product.Id] = product;
                }
            }
        }

        public bool deleteProduct(ProductEntity product)
        {
            if (product != null)
            {
                if (!String.IsNullOrEmpty(product.Id) && mapProduct.ContainsKey(product.Id))
                {
                    mapProduct.Remove(product.Id);
                    return true;
                }
            }

            return false;
        }

        public ProductEntity getProductByBarcode(String barcode)
        {
            if(mapProduct != null && mapProduct.Count > 0)
            {
                foreach(ProductEntity p in mapProduct.Values)
                {
                    if (p.BarCode.Equals(barcode))
                    {
                        return p;
                    }
                }
                
            }
            return null;
        }

        public List<ProductEntity> getProducts(ProductTypeEntity type)
        {
            throw new NotImplementedException();
        }

        public List<ProductStockEntity> getStocks(ProductTypeEntity type, string barcode)
        {
            throw new NotImplementedException();
        }

        public List<ProductEntity> getProducts(List<KeyValue> listFilter)
        {
            throw new NotImplementedException();
        }

        public List<ProductStockEntity> getStocks(ProductTypeEntity type, ProductCategoryEntity category, string barcode)
        {
            throw new NotImplementedException();
        }

        public List<ProductStockEntity> getStocks(ProductTypeEntity type, ProductCategoryEntity category, string barcode, string group)
        {
            throw new NotImplementedException();
        }
    }
}
