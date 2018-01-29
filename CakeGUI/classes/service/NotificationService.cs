using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface NotificationService
    {
        List<SaleNotificationEntity> getSaleNotification(ProductTypeEntity type);
        List<StatusNotificationEntity> getStatusNotification(ProductTypeEntity type, string barcode);
        List<StatusNotificationEntity> getStatusNotification(ProductTypeEntity type, string category, string barcode, string group);
    }
}
