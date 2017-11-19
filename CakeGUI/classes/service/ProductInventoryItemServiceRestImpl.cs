using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
using RestSharp;
using CakeGUI.classes.entity.rest;
using Newtonsoft.Json;

namespace CakeGUI.classes.service
{
    public class ProductInventoryItemServiceRestImpl : ProductInventoryItemService
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        private static ProductInventoryItemServiceRestImpl instance;

        private ProductInventoryItemServiceRestImpl() { }

        public static ProductInventoryItemServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductInventoryItemServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");

        public List<InventoryItemEntity> getProductInventories(ProductEntity product)
        {
            var request = new RestRequest("inventoryItem/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            List<KeyValue> listFilter = new List<KeyValue>();
            KeyValue keyValue = new KeyValue();
            keyValue.Key = "inventory.product";
            keyValue.Value = product.Id;
            listFilter.Add(keyValue);

            string strListFilter = JsonConvert.SerializeObject(listFilter);

            request.AddQueryParameter("field", strListFilter);

            IRestResponse<TCommonResponsePaging<InventoryItemEntity>> products = client.Execute<TCommonResponsePaging<InventoryItemEntity>>(request);

            return products.Data.Paging.Data;
        }

        public void saveProductInventory(InventoryItemEntity inventory)
        {
            var request = new RestRequest("inventoryItem/add", Method.POST);
            if (!string.IsNullOrEmpty(inventory.Id))
            {
                request = new RestRequest("inventoryItem/update", Method.POST);
            }

            request.JsonSerializer = util.JsonSerializer.Default;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(inventory);

            IRestResponse<TCommonResponse<InventoryItemEntity>> response = client.Execute<TCommonResponse<InventoryItemEntity>>(request);

            InventoryItemEntity savedProduct = response.Data.Data;

        }

    }
}
