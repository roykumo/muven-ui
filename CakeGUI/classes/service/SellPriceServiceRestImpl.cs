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
    class SellPriceServiceRestImpl: SellPriceService
    {
        private static ProductService productService = ProductServiceImpl.Instance;
        
        private static SellPriceServiceRestImpl instance;
        private SellPriceServiceRestImpl() { }

        public static SellPriceServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SellPriceServiceRestImpl();
                }
                return instance;
            }
        }
        
        private RestClient client = new RestClient("http://localhost:8908");

        public SellPrice getSellPrice(string id)
        {
            throw new NotImplementedException();
        }

        public List<SellPrice> getSellPrices(ProductEntity product)
        {
            var request = new RestRequest("sellPrice/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            List<KeyValue> listFilter = new List<KeyValue>();
            KeyValue keyValue = new KeyValue();
            keyValue.Key = "sellPrice.product";
            keyValue.Value = product.Id;
            listFilter.Add(keyValue);

            string strListFilter = JsonConvert.SerializeObject(listFilter);

            request.AddQueryParameter("field", strListFilter);

            IRestResponse<TCommonResponsePaging<SellPrice>> products = client.Execute<TCommonResponsePaging<SellPrice>>(request);

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

        public void saveSellPrice(SellPrice sellPrice)
        {
            var request = new RestRequest("sellPrice/add", Method.POST);
            if (!string.IsNullOrEmpty(sellPrice.Id))
            {
                request = new RestRequest("sellPrice/update", Method.POST);
            }

            request.JsonSerializer = util.JsonSerializer.Default;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(sellPrice);

            IRestResponse<TCommonResponse<SellPrice>> response = client.Execute<TCommonResponse<SellPrice>>(request);

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

            SellPrice savedSellPrice = response.Data.Data;
        }

        public SellPrice getCurrentSellPrice(ProductEntity product)
        {
            List<SellPrice> list = getSellPrices(product);
            if(list==null || list.Count == 0)
            {
                return null;
            }
            else
            {
                return list.OrderByDescending(m => m.Date).FirstOrDefault();
            }
        }
    }
}
