using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
using CakeGUI.classes.entity.rest;

namespace CakeGUI.classes.service
{
    interface ProductService
    {
        List<ProductEntity> getProducts();
        List<ProductEntity> getProducts(ProductTypeEntity type);
        List<ProductEntity> getProducts(List<KeyValue> listFilter);
        ProductEntity getProduct(String id);
        void saveProduct(ProductEntity product);
        bool deleteProduct(ProductEntity product);
        ProductEntity getProductByBarcode(string barcode);
        List<ProductStockEntity> getStocks(ProductTypeEntity type, string barcode);
    }
}
