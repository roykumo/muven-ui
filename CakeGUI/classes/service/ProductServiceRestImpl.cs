using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
using CakeGUI.classes.entity.rest;
using RestSharp;
using CakeGUI.classes.util;
using Newtonsoft.Json;

namespace CakeGUI.classes.service
{
    public class ProductServiceRestImpl : ProductService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ProductServiceRestImpl instance;
        static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private ProductServiceRestImpl() { }

        public static ProductServiceRestImpl Instance
        {
            get
            {
                try
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceRestImpl();
                    }
                }catch(Exception ex)
                {
                    log.Error(ex.Message);
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");
        public ProductEntity getProduct(String id)
        {
            var request = new RestRequest("product/{id}", Method.GET);
            request.AddUrlSegment("id", id);

            IRestResponse<TCommonResponse<ProductEntity>> product = client.Execute<TCommonResponse<ProductEntity>>(request);

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

            return product.Data.Data;
        }

        public List<ProductEntity> getProducts()
        {
            var request = new RestRequest("product/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            IRestResponse<TCommonResponsePaging<ProductEntity>> products = client.Execute<TCommonResponsePaging<ProductEntity>>(request);
            
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

        public void saveProduct(ProductEntity product)
        {
            var request = new RestRequest("product/add", Method.POST);
            if (!string.IsNullOrEmpty(product.Id)) {
                request = new RestRequest("product/update", Method.POST);
            }

            request.JsonSerializer = util.JsonSerializer.Default;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(product);

            IRestResponse<TCommonResponse<ProductEntity>> response = client.Execute<TCommonResponse<ProductEntity>>(request);

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

            ProductEntity savedProduct = response.Data.Data;
            
        }

        public bool deleteProduct(ProductEntity product)
        {
            var request = new RestRequest("product/delete/{id}", Method.GET);
            request.AddUrlSegment("id", product.Id);

            IRestResponse<TCommonResponse<ProductEntity>> response = client.Execute<TCommonResponse<ProductEntity>>(request);

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

            if (response.Data.ResponseStatus.ResponseCode == "00")
                return true;
            else
                return false;
        }

        public ProductEntity getProductByBarcode(String barcode)
        {
            var request = new RestRequest("product", Method.GET);
            request.AddParameter("barcode", barcode);

            IRestResponse<TCommonResponse<ProductEntity>> product = client.Execute<TCommonResponse<ProductEntity>>(request);

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

            return product.Data.Data;
        }

        public List<ProductEntity> getProducts(ProductTypeEntity type)
        {
            var request = new RestRequest("product/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            List<KeyValue> listFilter = new List<KeyValue>();
            KeyValue keyValue = new KeyValue();
            keyValue.Key = "productType";
            keyValue.Value = type.Id;
            listFilter.Add(keyValue);

            string strListFilter = JsonConvert.SerializeObject(listFilter);

            request.AddQueryParameter("field", strListFilter);

            IRestResponse<TCommonResponsePaging<ProductEntity>> products = client.Execute<TCommonResponsePaging<ProductEntity>>(request);

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

        public List<ProductEntity> getProducts(List<KeyValue> listFilter)
        {
            var request = new RestRequest("product/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            if (listFilter != null && listFilter.Count>0)
            {
                string strListFilter = JsonConvert.SerializeObject(listFilter);
                request.AddQueryParameter("field", strListFilter);
            }

            IRestResponse<TCommonResponsePaging<ProductEntity>> products = client.Execute<TCommonResponsePaging<ProductEntity>>(request);

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

        public List<ProductStockEntity> getStocks(ProductTypeEntity type, string barcode)
        {
            var request = new RestRequest("product/stock", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);
            if (!string.IsNullOrEmpty(barcode))
            {
                request.AddQueryParameter("code", barcode);
            }

            IRestResponse<TCommonResponse<List<ProductStockEntity>>> products = client.Execute<TCommonResponse<List<ProductStockEntity>>>(request);

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

            return products.Data.Data;
        }

        public List<ProductStockEntity> getStocks(ProductTypeEntity type, ProductCategoryEntity category, string barcode)
        {
            var request = new RestRequest("product/stock", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);

            if(category != null && !string.IsNullOrEmpty(category.Id)) 
                request.AddQueryParameter("category", category.Id);

            if (!string.IsNullOrEmpty(barcode))
            {
                request.AddQueryParameter("code", barcode);
            }

            IRestResponse<TCommonResponse<List<ProductStockEntity>>> products = client.Execute<TCommonResponse<List<ProductStockEntity>>>(request);

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

            return products.Data.Data;
        }

        public List<ProductStockEntity> getStocks(ProductTypeEntity type, ProductCategoryEntity category, string barcode, string group)
        {
            var request = new RestRequest("product/stock", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);

            if (category != null && !string.IsNullOrEmpty(category.Id))
                request.AddQueryParameter("category", category.Id);

            if (!string.IsNullOrEmpty(barcode))
            {
                request.AddQueryParameter("code", barcode);
            }

            if (!string.IsNullOrEmpty(group))
            {
                request.AddQueryParameter("group", group);
            }

            IRestResponse<TCommonResponse<List<ProductStockEntity>>> products = client.Execute<TCommonResponse<List<ProductStockEntity>>>(request);

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

            return products.Data.Data;
        }
    }
}
