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
    public class ProductCategoryServiceRestImpl : ProductCategoryService
    {
        private static ProductCategoryServiceRestImpl instance;

        private ProductCategoryServiceRestImpl() { }

        public static ProductCategoryServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductCategoryServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");
        public ProductCategoryEntity getProductCategory(String id)
        {
            var request = new RestRequest("product/{id}", Method.GET);
            request.AddUrlSegment("id", id);

            IRestResponse<TCommonResponse<ProductCategoryEntity>> product = client.Execute<TCommonResponse<ProductCategoryEntity>>(request);

            return product.Data.Data;
        }

        public List<ProductCategoryEntity> getProductCategories()
        {
            var request = new RestRequest("productCategory/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            IRestResponse<TCommonResponsePaging<ProductCategoryEntity>> products = client.Execute<TCommonResponsePaging<ProductCategoryEntity>>(request);

            return products.Data.Paging.Data;
        }

        public List<ProductCategoryEntity> getProductCategoriesByType(ProductTypeEntity type)
        {
            var request = new RestRequest("productCategory/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            List<KeyValue> listFilter = new List<KeyValue>();
            KeyValue keyValue = new KeyValue();
            keyValue.Key = "productType";
            keyValue.Value = type.Id;
            listFilter.Add(keyValue);

            string strListFilter = JsonConvert.SerializeObject(listFilter);

            request.AddQueryParameter("field", strListFilter);

            IRestResponse<TCommonResponsePaging<ProductCategoryEntity>> products = client.Execute<TCommonResponsePaging<ProductCategoryEntity>>(request);

            return products.Data.Paging.Data;
        }

        public List<ProductCategoryEntity> getProductCategories(List<KeyValue> listFilter)
        {
            var request = new RestRequest("productCategory/list/filter", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;
            
            string strListFilter = JsonConvert.SerializeObject(listFilter);

            request.AddQueryParameter("field", strListFilter);

            IRestResponse<TCommonResponsePaging<ProductCategoryEntity>> products = client.Execute<TCommonResponsePaging<ProductCategoryEntity>>(request);

            return products.Data.Paging.Data;
        }

        public void saveProductCategory(ProductCategoryEntity product)
        {
            var request = new RestRequest("productCategory/add", Method.POST);
            if (!string.IsNullOrEmpty(product.Id)) {
                request = new RestRequest("productCategory/update", Method.POST);
            }

            request.JsonSerializer = util.JsonSerializer.Default;
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(product);

            IRestResponse<TCommonResponse<ProductCategoryEntity>> response = client.Execute<TCommonResponse<ProductCategoryEntity>>(request);

            ProductCategoryEntity savedProduct = response.Data.Data;
            
        }

        public bool deleteProductCategory(ProductCategoryEntity product)
        {
            var request = new RestRequest("productCategory/delete/{id}", Method.GET);
            request.AddUrlSegment("id", product.Id);

            IRestResponse<TCommonResponse<ProductCategoryEntity>> response = client.Execute<TCommonResponse<ProductCategoryEntity>>(request);

            if (response.Data.ResponseStatus.ResponseCode == "00")
                return true;
            else
                return false;
        }
        
    }
}
