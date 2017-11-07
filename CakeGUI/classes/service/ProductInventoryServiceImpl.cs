using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    public class ProductInventoryServiceImpl : ProductInventoryService
    {
        private static ProductService productService = ProductServiceImpl.Instance;

        private static ProductInventoryServiceImpl instance;
        private static int counter = 0;

        private ProductInventoryServiceImpl() { }

        public static ProductInventoryServiceImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductInventoryServiceImpl();
                }
                return instance;
            }
        }

        static List<InventoryEntity> inventories = null;
        static Dictionary<String, List<InventoryEntity>> mapInventory = null; 

        static ProductInventoryServiceImpl()
        {
            fillMap();            
        }

        static void fillMap()
        {
            if (mapInventory == null)
            {
                mapInventory = new Dictionary<String, List<InventoryEntity>>();
            }

            List<InventoryEntity> list1 = new List<InventoryEntity>();
            ProductEntity prd1 = productService.getProduct("1");

            InventoryEntity inventory1 = new InventoryEntity();
            inventory1.Id = (++counter).ToString();
            inventory1.ExpiredDate = new DateTime(2018, 3, 1);
            inventory1.Product = prd1;
            inventory1.PurchaseDate = new DateTime(2017, 11, 1);
            inventory1.PurchasePrice = 95000;
            inventory1.Quantity = 15;
            inventory1.Remarks = "Re-stock";
            inventory1.TransactionCode = "20171101-P-01";

            InventoryEntity inventory2 = new InventoryEntity();
            inventory2.Id = (++counter).ToString();
            inventory2.ExpiredDate = new DateTime(2017, 12, 30);
            inventory2.Product = prd1;
            inventory2.PurchaseDate = new DateTime(2017, 5, 16);
            inventory2.PurchasePrice = 90000;
            inventory2.Quantity = 2;
            inventory2.Remarks = "Keterangan";
            inventory2.TransactionCode = "20170516-P-04";

            list1.Add(inventory1);
            list1.Add(inventory2);
            mapInventory.Add(prd1.Id, list1);


            List<InventoryEntity> list2 = new List<InventoryEntity>();
            ProductEntity prd2 = productService.getProduct("2");

            InventoryEntity inventory3 = new InventoryEntity();
            inventory3.Id = (++counter).ToString();
            inventory3.ExpiredDate = new DateTime(2018, 12, 1);
            inventory3.Product = prd2;
            inventory3.PurchaseDate = new DateTime(2017, 9, 1);
            inventory3.PurchasePrice = 215000;
            inventory3.Quantity = 20;
            inventory3.Remarks = "Promo";
            inventory3.TransactionCode = "20170901-P-01";

            InventoryEntity inventory4 = new InventoryEntity();
            inventory4.Id = (++counter).ToString();
            inventory4.ExpiredDate = new DateTime(2018, 4, 30);
            inventory4.Product = prd2;
            inventory4.PurchaseDate = new DateTime(2017, 1, 31);
            inventory4.PurchasePrice = 200000;
            inventory4.Quantity = 2;
            inventory4.Remarks = "Baru";
            inventory4.TransactionCode = "20170131-P-02";

            list2.Add(inventory3);
            list2.Add(inventory4);
            mapInventory.Add(prd2.Id, list2);

        }

        public List<InventoryEntity> getProductInventories(ProductEntity product)
        {
            if(inventories == null)
            {
                return mapInventory[product.Id];
            }
            else { return inventories; }
        }

        public void saveProductInventory(InventoryEntity inventory)
        {
            if (inventory != null)
            {
                if (String.IsNullOrEmpty(inventory.Id))
                {
                    inventory.Id = (++counter).ToString();
                }

                if (!mapInventory.ContainsKey(inventory.Product.Id))
                {
                    List<InventoryEntity> list = new List<InventoryEntity>();
                    list.Add(inventory);
                    mapInventory.Add(inventory.Product.Id, list);
                }
                else
                {
                    List<InventoryEntity> existingList = mapInventory[inventory.Product.Id];
                    foreach(InventoryEntity i in existingList)
                    {
                        if (i.Id.Equals(inventory.Id))
                        {
                            existingList.Remove(i);
                        }
                    }
                    existingList.Add(inventory);
                }
            }
        }

    }
}
