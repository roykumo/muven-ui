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
    class NotificationServiceRestImpl : NotificationService
    {
        private static NotificationServiceRestImpl instance;
        private NotificationServiceRestImpl() { }

        public static NotificationServiceRestImpl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotificationServiceRestImpl();
                }
                return instance;
            }
        }

        private RestClient client = new RestClient("http://localhost:8908");

        public List<SaleNotificationEntity> getSaleNotification(ProductTypeEntity type)
        {
            var request = new RestRequest("notification/sale", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);

            IRestResponse<TCommonResponse<List<SaleNotificationEntity>>> products = client.Execute<TCommonResponse<List<SaleNotificationEntity>>>(request);

            if(products.StatusCode!= System.Net.HttpStatusCode.OK)
            {
                throw new Exception("error http : " + products.StatusCode + " - "+ products.ErrorMessage);
            }
            else
            {
                if (products.Data==null)
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

        public List<StatusNotificationEntity> getStatusNotification(ProductTypeEntity type, string barcode)
        {
            var request = new RestRequest("notification/status", Method.GET);
            client.AddHandler("application/json", util.JsonSerializer.Default);
            request.JsonSerializer = util.JsonSerializer.Default;

            request.AddQueryParameter("type", type.Id);
            if (!string.IsNullOrEmpty(barcode))
            {
                request.AddQueryParameter("barcode", barcode);
            }

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
