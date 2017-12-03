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
    public class ProductInventoryOutServiceRestImpl : ProductInventoryOutService
    {
        private static ProductInventoryOutServiceRestImpl instance;

        private ProductInventoryOutServiceRestImpl() { }

        public static ProductInventoryOutServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductInventoryOutServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");
        
        public void saveProductInventory(InventoryOutEntity inventoryOut)
        {
            var request = new RestRequest("inventory/out/add", Method.POST);
            
            request.JsonSerializer = util.JsonSerializer.Default;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(inventoryOut);

            IRestResponse<TCommonResponse<InventoryOutEntity>> response = client.Execute<TCommonResponse<InventoryOutEntity>>(request);

            InventoryOutEntity savedProduct = response.Data.Data;            
        }

    }
}
