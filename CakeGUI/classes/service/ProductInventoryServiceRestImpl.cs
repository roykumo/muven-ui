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
    public class ProductInventoryServiceRestImpl : ProductInventoryService
    {
        private static ProductService productService = ProductServiceRestImpl.Instance;

        private static ProductInventoryServiceRestImpl instance;

        private ProductInventoryServiceRestImpl() { }

        public static ProductInventoryServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductInventoryServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");
        
        public void saveProductInventory(InventoryEntity inventory)
        {
            var request = new RestRequest("inventory/add", Method.POST);
            
            request.JsonSerializer = util.JsonSerializer.Default;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(inventory);

            IRestResponse<TCommonResponse<InventoryEntity>> response = client.Execute<TCommonResponse<InventoryEntity>>(request);

            InventoryEntity savedProduct = response.Data.Data;
            
        }

    }
}
