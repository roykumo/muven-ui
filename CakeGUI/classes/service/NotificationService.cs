﻿using System;
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
    }
}