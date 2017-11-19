using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
namespace CakeGUI.classes.service
{
    public class ProductTypeServiceImpl : ProductTypeService
    {
        private static ProductTypeServiceImpl instance;
        private ProductTypeServiceImpl() { }

        public static ProductTypeServiceImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductTypeServiceImpl();
                }
                return instance;
            }
        }

        static Dictionary<String, ProductTypeEntity> mapProductType = null; 

        static ProductTypeServiceImpl()
        {
            fillMap();            
        }
        
        static void fillMap()
        {
            if (mapProductType == null)
            {
                mapProductType = new Dictionary<String, ProductTypeEntity>();
            }

            ProductTypeEntity type1 = new ProductTypeEntity();
            type1.Id = "1";
            type1.Code = "B";
            type1.Description = "Bahan";
            type1.Expiration = false;

            mapProductType.Add(type1.Id, type1);

            ProductTypeEntity type2 = new ProductTypeEntity();
            type2.Id = "2";
            type2.Code = "A";
            type2.Description = "Alat";
            type2.Expiration = true;

            mapProductType.Add(type2.Id, type2);

        }

        public ProductTypeEntity getProductType(String id)
        {
            return mapProductType[id];
        }

        public List<ProductTypeEntity> getProductTypes()
        {
            return new List<ProductTypeEntity>(mapProductType.Values);
        }
        
    }
}
