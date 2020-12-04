using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventEmitter
{
    class Program
    {
        private const string ServiceBusConnectionString = "topic_send_key_connection_string";
        private const string TopicName = "topic_name";
        
        static async Task Main()
        {
            ITopicClient topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            var data = new SomeData()
            {
                Id = Guid.NewGuid().ToString(),
                Data = "Hello world!"
            };
            var payload = JsonConvert.SerializeObject(data);

            var msg = new Message(Encoding.UTF8.GetBytes(payload));
            msg.UserProperties.Add("Type", "eventData");
            msg.MessageId = Guid.NewGuid().ToString();

            await topicClient.SendAsync(msg);
            await topicClient.CloseAsync();
            Console.WriteLine("Event emitted successfully.");
        }

        public class SomeData
        {
            public string Id { get; set; }
            public string Data { get; set; }
        }

    }
}
