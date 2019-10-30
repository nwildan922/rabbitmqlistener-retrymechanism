using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQReiverCoreAPI
{
    //public static class AppConstant
    //{

    //    public static string UrlApi = "http://localhost:54571";
    //}
public class result
    {
        public bool isSuccess { get; set; }
    }
    public class FileJson
    {
        public string JsonString { get; set; }
    }
    public static class RabbitMQSetting
    {
        //public static string hostName = "localhost";
        //public static string username = "admin";
        //public static string password = "indocyber";
        public static string exchangeName = "x.guideline.work";
        public static string queueName = "q.guideline.booking.work";
        public static string routingKey = "q.guideline.booking";
        public static string message = "test";

        //public static string exchangeName = "LOS.E.Direct.Leads";
        //public static string queueName = "LOS.Q.Leads";
        //public static string routingKey = "Leads";
        //public static string username = "admin";
        //public static string password = "indocyber";

        //public static string username = "guest";
        //public static string password = "guest";
    }
}
