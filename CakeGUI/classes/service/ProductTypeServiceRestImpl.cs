using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeGUI.classes.entity;
using RestSharp;
using CakeGUI.classes.entity.rest;

namespace CakeGUI.classes.service
{
    public class ProductTypeServiceRestImpl : ProductTypeService
    {
        private static ProductTypeServiceRestImpl instance;
        private ProductTypeServiceRestImpl() { }

        public static ProductTypeServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductTypeServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");
        public ProductTypeEntity getProductType(String id)
        {
            //var request = new RestRequest("resource/{id}", Method.POST);
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", "123");

            //// add parameters for all properties on an object
            //request.AddObject(object);

            //// or just whitelisted properties
            //request.AddObject(object, "PersonId", "Name", ...);

            //// easily add HTTP Headers
            //request.AddHeader("header", "value");

            //// add files to upload (works with compatible verbs)
            //request.AddFile("file", path);

            //// execute the request
            //IRestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string

            //// or automatically deserialize result
            //// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            //IRestResponse<Person> response2 = client.Execute<Person>(request);
            //var name = response2.Data.Name;

            //// or download and save file to disk
            //client.DownloadData(request).SaveAs(path);

            //// easy async support
            //await client.ExecuteAsync(request);

            //// async with deserialization
            //var asyncHandle = client.ExecuteAsync<Person>(request, response => {
            //    Console.WriteLine(response.Data.Name);
            //});

            //// abort the request on demand
            //asyncHandle.Abort();

            var request = new RestRequest("productType/{id}", Method.GET);
            request.AddUrlSegment("id", id);

            IRestResponse<TCommonResponse<ProductTypeEntity>> productType = client.Execute<TCommonResponse<ProductTypeEntity>>(request);

            return productType.Data.Data;
        }

        public List<ProductTypeEntity> getProductTypes()
        {
            var request = new RestRequest("productType/list/filter", Method.GET);

            IRestResponse<TCommonResponsePaging<ProductTypeEntity>> productTypes = client.Execute<TCommonResponsePaging<ProductTypeEntity>>(request);

            return productTypes.Data.Paging.Data;
        }
        
    }
}
