using CakeGUI.classes.util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class InventoryItemEntity: IDataErrorInfo
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        //public String TransactionCode { get; set; }
        [JsonProperty("product")]
        public ProductEntity Product { get; set; }
        //public DateTime PurchaseDate { get; set; }
        [JsonProperty("expiredDate")]
        [JsonConverter(typeof (ISODateConverter))]
        public DateTime ExpiredDate { get; set; }
        [JsonProperty("quantity")]
        public Int32 Quantity { get; set; }
        [JsonProperty("purchasePrice")]
        public Decimal PurchasePrice { get; set; }
        [JsonProperty("remarks")]
        public String Remarks { get; set; }
        [JsonProperty("inventory")]
        public InventoryEntity Inventory { get; set; }

        #region IDataErrorInfo Validations

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (columnName == "Product")
                {
                    if (Product == null || string.IsNullOrEmpty(Product.Id))
                        return "Product harus diisi";
                }

                if (columnName == "ExpiredDate")
                {
                    if (ExpiredDate == null)
                        return "Tanggal kadaluarsa harus diisi";
                }

                if (columnName == "Quantity")
                {
                    try
                    {
                        if (Quantity <= 0)
                            return "Jumlah harus diisi";

                    }
                    catch
                    {
                        return "Jumlah tidak valid";
                    }
                }

                if (columnName == "PurchasePrice")
                {
                    try
                    {
                        if (PurchasePrice <= 0)
                            return "Harga beli harus diisi";

                    }
                    catch
                    {
                        return "Harga beli tidak valid";
                    }
                }

                if (columnName == "Remarks")
                {
                    if (string.IsNullOrEmpty(Remarks))
                        return "Keterangan harus diisi";                        
                }

                // If there's no error, null gets returned
                return null;
            }
        }
        #endregion
    }
}
