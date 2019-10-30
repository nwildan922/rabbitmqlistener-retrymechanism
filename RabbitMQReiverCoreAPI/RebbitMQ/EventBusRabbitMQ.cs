using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQReiverCoreAPI.Models;
using RabbitMQReiverCoreAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReiverCoreAPI.RebbitMQ
{
    public class EventBusRabbitMQ : IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(IRabbitMQPersistentConnection persistentConnection, string queueName = null)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _queueName = queueName;
        }

        public IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            // channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            //var dict = new Dictionary<string, object>();
            //dict.Add("x-dead-letter-exchange", "x.guideline.wait");
            //dict.Add("x-dead-letter-routing-key", "q.guideline.booking");
            //channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: dict);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                //Console.WriteLine($"Filename : {message} berhasil di save");
                var temp = BFIHelper.GetMessage(message);
                var status = temp.isSuccess;
                if (status)
                {
                    channel.BasicAck(ea.DeliveryTag, false);

                }
                else
                {

                    if (ea.BasicProperties.Headers.Count > 0)
                    {
                        IDictionary<string, object> customDict = new Dictionary<string, object>();
                        customDict = ea.BasicProperties.Headers;
                        var data = customDict.Where(x => x.Key.Equals("x-death")).FirstOrDefault();
                        
                        var count = data.Value as List<System.Object>;
                        long realCount = 0;
                        foreach (var item in count)
                        {
                            var tempdata = item as Dictionary<string,object>;
                            var tempdata2 = tempdata.Where(x => x.Key.Equals("count")).FirstOrDefault();
                            realCount = (long)tempdata2.Value;
                        }
                        
                        if (realCount >= 2)
                        {
                            this.PublishMessage("x.guideline.dead", "q.guideline.booking", message);
                            channel.BasicAck(ea.DeliveryTag, false);
                            BFIHelper.GetDead(message);
                        }
                        else
                        {
                            channel.BasicReject(ea.DeliveryTag, false);
                        }
                    }
                    else
                    {
                        channel.BasicReject(ea.DeliveryTag, false);
                    }
                    

                }
            };


            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };
            return channel;
        }
        public void PublishMessage(string exchangeName, string routingKey, string message)
        {
            var channel = _persistentConnection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var encodedMessage = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: properties, body: encodedMessage);

        }
        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }
        }
    }
}
