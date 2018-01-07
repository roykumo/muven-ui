using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    public class ProductInventoryItemServiceImpl : ProductInventoryItemService
    {
        private static ProductService productService = ProductServiceImpl.Instance;

        private static ProductInventoryItemServiceImpl instance;
        private static int counter = 0;

        private ProductInventoryItemServiceImpl() { }

        public static ProductInventoryItemServiceImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductInventoryItemServiceImpl();
                }
                return instance;
            }
        }

        static List<InventoryItemEntity> inventories = null;
        static Dictionary<String, List<InventoryItemEntity>> mapInventory = null; 

        static ProductInventoryItemServiceImpl()
        {
            fillMap();            
        }

        static void fillMap()
        {
            if (mapInventory == null)
            {
                mapInventory = new Dictionary<String, List<InventoryItemEntity>>();
            }

            List<InventoryItemEntity> list1 = new List<InventoryItemEntity>();
            ProductEntity prd1 = productService.getProduct("1");

            InventoryItemEntity inventory1 = new InventoryItemEntity();
            inventory1.Id = (++counter).ToString();
            inventory1.ExpiredDate = new DateTime(2018, 3, 1);
            inventory1.Product = prd1;
            //inventory1.PurchaseDate = new DateTime(2017, 11, 1);
            inventory1.PurchasePrice = 95000;
            inventory1.Quantity = 15;
            inventory1.Remarks = "Re-stock";
            //inventory1.TransactionCode = "20171101-P-01";

            InventoryItemEntity inventory2 = new InventoryItemEntity();
            inventory2.Id = (++counter).ToString();
            inventory2.ExpiredDate = new DateTime(2017, 12, 30);
            inventory2.Product = prd1;
            //inventory2.PurchaseDate = new DateTime(2017, 5, 16);
            inventory2.PurchasePrice = 90000;
            inventory2.Quantity = 2;
            inventory2.Remarks = "Keterangan";
            //inventory2.TransactionCode = "20170516-P-04";

            list1.Add(inventory1);
            list1.Add(inventory2);
            mapInventory.Add(prd1.Id, list1);


            List<InventoryItemEntity> list2 = new List<InventoryItemEntity>();
            ProductEntity prd2 = productService.getProduct("2");

            InventoryItemEntity inventory3 = new InventoryItemEntity();
            inventory3.Id = (++counter).ToString();
            inventory3.ExpiredDate = new DateTime(2018, 12, 1);
            inventory3.Product = prd2;
            //inventory3.PurchaseDate = new DateTime(2017, 9, 1);
            inventory3.PurchasePrice = 215000;
            inventory3.Quantity = 20;
            inventory3.Remarks = "Promo";
            //inventory3.TransactionCode = "20170901-P-01";

            InventoryItemEntity inventory4 = new InventoryItemEntity();
            inventory4.Id = (++counter).ToString();
            inventory4.ExpiredDate = new DateTime(2018, 4, 30);
            inventory4.Product = prd2;
            //inventory4.PurchaseDate = new DateTime(2017, 1, 31);
            inventory4.PurchasePrice = 200000;
            inventory4.Quantity = 2;
            inventory4.Remarks = "Baru";
            //inventory4.TransactionCode = "20170131-P-02";

            list2.Add(inventory3);
            list2.Add(inventory4);
            mapInventory.Add(prd2.Id, list2);

        }

        public List<InventoryItemEntity> getProductInventories(ProductEntity product, bool currentQuantity)
        {
            if(inventories == null)
            {
                return mapInventory[product.Id];
            }
            else { return inventories; }
        }

        public void saveProductInventory(InventoryItemEntity inventory)
        {
            if (inventory != null)
            {
                if (String.IsNullOrEmpty(inventory.Id))
                {
                    inventory.Id = (++counter).ToString();
                }

                if (!mapInventory.ContainsKey(inventory.Product.Id))
                {
                    List<InventoryItemEntity> list = new List<InventoryItemEntity>();
                    list.Add(inventory);
                    mapInventory.Add(inventory.Product.Id, list);
                }
                else
                {
                    List<InventoryItemEntity> existingList = mapInventory[inventory.Product.Id];
                    foreach(InventoryItemEntity i in existingList)
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
