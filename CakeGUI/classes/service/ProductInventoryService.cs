using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface ProductInventoryService
    {
        List<InventoryEntity> getProductInventories(ProductEntity product);
        void saveProductInventory(InventoryEntity inventory);
    }
}
