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

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("error http : " + response.StatusCode + " - " + response.ErrorMessage);
            }
            else
            {
                if (response.Data == null)
                {
                    throw new Exception("response data null");
                }
                else
                {
                    if (response.Data.ResponseStatus == null)
                    {
                        throw new Exception("response status null");
                    }
                    else
                    {
                        if (response.Data.ResponseStatus.ResponseCode != "00")
                        {
                            throw new Exception("error api : " + response.Data.ResponseStatus.ResponseCode + " - " + response.Data.ResponseStatus.ResponseDesc);
                        }
                    }
                }
            }

            InventoryEntity savedProduct = response.Data.Data;
            
        }

        public string getTrxCode(string type, string date)
        {
            var request = new RestRequest("inventory/maxTrxNumber", Method.GET);
            
            request.AddParameter("type", type);

            IRestResponse<TCommonResponse<int>> product = client.Execute<TCommonResponse<int>>(request);

            if (product.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("error http : " + product.StatusCode + " - " + product.ErrorMessage);
            }
            else
            {
                if (product.Data == null)
                {
                    throw new Exception("response data null");
                }
                else
                {
                    if (product.Data.ResponseStatus == null)
                    {
                        throw new Exception("response status null");
                    }
                    else
                    {
                        if (product.Data.ResponseStatus.ResponseCode != "00")
                        {
                            throw new Exception("error api : " + product.Data.ResponseStatus.ResponseCode + " - " + product.Data.ResponseStatus.ResponseDesc);
                        }
                    }
                }
            }

            return DateTime.Now.ToString("yyyyMMdd") + "_" + type + product.Data.Data.ToString().PadLeft(2, '0');
        }

        public InventoryEntity getById(string id)
        {
            var request = new RestRequest("inventory/{id}", Method.GET);
            request.AddUrlSegment("id", id);

            IRestResponse<TCommonResponse<InventoryEntity>> inventory = client.Execute<TCommonResponse<InventoryEntity>>(request);

            if (inventory.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("error http : " + inventory.StatusCode + " - " + inventory.ErrorMessage);
            }
            else
            {
                if (inventory.Data == null)
                {
                    throw new Exception("response data null");
                }
                else
                {
                    if (inventory.Data.ResponseStatus == null)
                    {
                        throw new Exception("response status null");
                    }
                    else
                    {
                        if (inventory.Data.ResponseStatus.ResponseCode != "00")
                        {
                            throw new Exception("error api : " + inventory.Data.ResponseStatus.ResponseCode + " - " + inventory.Data.ResponseStatus.ResponseDesc);
                        }
                    }
                }
            }

            return inventory.Data.Data;
        }
    }
}
