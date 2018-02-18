using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;

namespace CakeGUI.classes.service
{
    interface TransactionService
    {
        List<TransactionEntity> getTransactionList(Int32 year, Int32 month, Int32 day);
    }
}
