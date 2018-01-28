using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface ProductInventoryOutService
    {
        void saveProductInventory(InventoryOutEntity inventoryOut);
        InventoryOutEntity getById(string id);
    }
}
