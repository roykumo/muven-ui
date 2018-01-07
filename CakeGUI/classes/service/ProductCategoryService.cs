using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
using CakeGUI.classes.entity.rest;

namespace CakeGUI.classes.service
{
    interface ProductCategoryService
    {
        List<ProductCategoryEntity> getProductCategories();
        List<ProductCategoryEntity> getProductCategoriesByType(ProductTypeEntity type);
        List<ProductCategoryEntity> getProductCategories(List<KeyValue> listFilter);
        ProductCategoryEntity getProductCategory(String id);
        void saveProductCategory(ProductCategoryEntity productCategory);
        bool deleteProductCategory(ProductCategoryEntity productCategory);
        
    }
}
