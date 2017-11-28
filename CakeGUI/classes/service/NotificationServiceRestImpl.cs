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

            return products.Data.Data;
        }
    }
}
