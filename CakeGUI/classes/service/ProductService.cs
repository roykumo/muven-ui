using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface ProductService
    {
        List<ProductEntity> getProducts();
        ProductEntity getProduct(String id);
        void saveProduct(ProductEntity product);
        bool deleteProduct(ProductEntity product);
        ProductEntity getProductByBarcode(string barcode);
    }
}
