using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface ProductInventoryItemService
    {
        List<InventoryItemEntity> getProductInventories(ProductEntity product, bool currentQuantity);
        void saveProductInventory(InventoryItemEntity inventory);
    }
}
