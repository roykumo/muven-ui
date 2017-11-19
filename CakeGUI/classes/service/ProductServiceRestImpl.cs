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
        private static ProductServiceRestImpl instance;
        static ProductTypeService productTypeService = ProductTypeServiceRestImpl.Instance;

        private ProductServiceRestImpl() { }

        public static ProductServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductServiceRestImpl();
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

            return product.Data.Data;
        }

        public List<ProductEntity> getProducts()
        {
            var request = new RestRequest("product/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            IRestResponse<TCommonResponsePaging<ProductEntity>> products = client.Execute<TCommonResponsePaging<ProductEntity>>(request);

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

            ProductEntity savedProduct = response.Data.Data;
            
        }

        public bool deleteProduct(ProductEntity product)
        {
            var request = new RestRequest("product/delete/{id}", Method.GET);
            request.AddUrlSegment("id", product.Id);

            IRestResponse<TCommonResponse<ProductEntity>> response = client.Execute<TCommonResponse<ProductEntity>>(request);

            if (response.Data.ResponseStatus.ResponseCode == "00")
                return true;
            else
                return false;
        }

        public ProductEntity getProductByBarcode(String barcode)
        {
            /*if(mapProduct != null && mapProduct.Count > 0)
            {
                foreach(ProductEntity p in mapProduct.Values)
                {
                    if (p.BarCode.Equals(barcode))
                    {
                        return p;
                    }
                }
                
            }*/
            return null;
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

            return products.Data.Paging.Data;
        }

        public List<ProductStockEntity> getStocks(ProductTypeEntity type)
        {
            var request = new RestRequest("product/stock", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);

            IRestResponse<TCommonResponse<List<ProductStockEntity>>> products = client.Execute<TCommonResponse<List<ProductStockEntity>>>(request);

            return products.Data.Data;
        }
    }
}
