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

        public List<InventoryItemEntity> getProductInventories(ProductEntity product, bool currentQuantity)
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
            request.AddQueryParameter("sort", "expiredDate");

            IRestResponse<TCommonResponsePaging<InventoryItemEntity>> products = client.Execute<TCommonResponsePaging<InventoryItemEntity>>(request);

            if (products.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("error http : " + products.StatusCode + " - " + products.ErrorMessage);
            }
            else
            {
                if (products.Data == null)
                {
                    throw new Exception("response data null");
                }
                else
                {
                    if (products.Data.ResponseStatus == null)
                    {
                        throw new Exception("response status null");
                    }
                    else
                    {
                        if (products.Data.ResponseStatus.ResponseCode != "00")
                        {
                            throw new Exception("error api : " + products.Data.ResponseStatus.ResponseCode + " - " + products.Data.ResponseStatus.ResponseDesc);
                        }
                    }
                }
            }

            List<InventoryItemEntity> listInventory = products.Data.Paging.Data;

            if(listInventory != null && listInventory.Count > 0 && currentQuantity)
            {
                List<InventoryItemOutEntity> listOut = getInventoryItemOut(product);
                if(listOut!=null && listOut.Count > 0)
                {
                    int count = listOut.Sum(o=>o.Quantity);
                    foreach (InventoryItemEntity item in listInventory)
                    {
                        if (count > 0)
                        {
                            if(item.Quantity < count)
                            {
                                item.Quantity = 0; //kurang item dengan kadaluarsa terdekat dengan itemOut
                                count -= item.Quantity;
                            }
                            else
                            {
                                item.Quantity -= count; 
                                count = 0;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                listInventory.RemoveAll(i => i.Quantity == 0);
            }

            return listInventory;
        }

        private List<InventoryItemOutEntity> getInventoryItemOut(ProductEntity product)
        {
            var request = new RestRequest("inventoryItem/out/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            List<KeyValue> listFilter = new List<KeyValue>();
            KeyValue keyValue = new KeyValue();
            keyValue.Key = "inventory.product";
            keyValue.Value = product.Id;
            listFilter.Add(keyValue);

            string strListFilter = JsonConvert.SerializeObject(listFilter);

            request.AddQueryParameter("field", strListFilter);

            IRestResponse<TCommonResponsePaging<InventoryItemOutEntity>> products = client.Execute<TCommonResponsePaging<InventoryItemOutEntity>>(request);

            if (products.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("error http : " + products.StatusCode + " - " + products.ErrorMessage);
            }
            else
            {
                if (products.Data == null)
                {
                    throw new Exception("response data null");
                }
                else
                {
                    if (products.Data.ResponseStatus == null)
                    {
                        throw new Exception("response status null");
                    }
                    else
                    {
                        if (products.Data.ResponseStatus.ResponseCode != "00")
                        {
                            throw new Exception("error api : " + products.Data.ResponseStatus.ResponseCode + " - " + products.Data.ResponseStatus.ResponseDesc);
                        }
                    }
                }
            }

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

            InventoryItemEntity savedProduct = response.Data.Data;

        }

    }
}
