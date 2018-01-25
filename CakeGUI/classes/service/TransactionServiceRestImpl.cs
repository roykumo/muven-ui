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
    class TransactionServiceRestImpl : TransactionService
    {
        private static TransactionServiceRestImpl instance;
        private TransactionServiceRestImpl() { }

        public static TransactionServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransactionServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");

        public List<TransactionEntity> getTransactionList()
        {
            var request = new RestRequest("transaction/list", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;
            
            IRestResponse<TCommonResponsePaging<TransactionEntity>> products = client.Execute<TCommonResponsePaging<TransactionEntity>>(request);

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

        public List<StatusNotificationEntity> getStatusNotification(ProductTypeEntity type)
        {
            var request = new RestRequest("notification/status", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);

            IRestResponse<TCommonResponse<List<StatusNotificationEntity>>> products = client.Execute<TCommonResponse<List<StatusNotificationEntity>>>(request);

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
