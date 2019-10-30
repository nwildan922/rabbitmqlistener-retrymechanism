using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQReiverCoreAPI.RebbitMQ
{
    public static class BFIHelper
    {
        private static RestClient _client = new RestClient();
        private static string url = DataConfiguration.Configuration.ApiTransaction;
        private static Uri BaseURL = new Uri(url);

        public static result GetMessage(string message)
        {
            //var jsonString = JsonConvert.SerializeObject(new FileJson { JsonString = fileName });
            _client.BaseUrl = new Uri(String.Format(BaseURL + "api/Retry/InsertWork/" + message));
            var requests = new RestRequest(Method.POST);

            //requests.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            try
            {
                var data = _client.Execute(requests);
                var rr = new result { isSuccess = bool.Parse(data.Content) };
                return rr;
            }
            catch (Exception)
            {

                return new result { isSuccess = false };
            }
            

        }

        public static result GetDead(string message)
        {
            //var jsonString = JsonConvert.SerializeObject(new FileJson { JsonString = fileName });
            _client.BaseUrl = new Uri(String.Format(BaseURL + "api/Retry/InsertDead/" + message));
            var requests = new RestRequest(Method.POST);

            //requests.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            try
            {
                var data = _client.Execute(requests);
                var rr = new result { isSuccess = bool.Parse(data.Content) };
                return rr;
            }
            catch (Exception)
            {

                return new result { isSuccess = false };
            }


        }
    }
}
