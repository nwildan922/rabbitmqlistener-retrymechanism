using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQReiverCoreAPI
{
    public class AppSettings
    {
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public string ApiTransaction { get; set; }
        public string AllowedHosts { get; set; }
        public Logging Logging { get; set; }

        public AppSettings()
        {

        }
    }

    public static class DataConfiguration
    {
        public static AppSettings Configuration
        {
            get
            {
                string json = System.IO.File.ReadAllText("appsettings.json");
                var data = JsonConvert.DeserializeObject<AppSettings>(json);
                return data;
            }
        }
    }

    public class LogLevel 
    {
        public string Default { get; set; }
    }
    public class Logging {
        public LogLevel LogLevel { get; set; }
    }
}
