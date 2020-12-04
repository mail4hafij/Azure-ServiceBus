using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventSubscriber
{
    class Program
    {
        private const string ServiceBusConnectionString = "topic_read_key_connection_string";
        private const string TopicName = "topic_name";
        private const string SubscriptionName = "subcription_name";
        private static ISubscriptionClient _subscriptionClient;
        
        static async Task Main()
        {
            _subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            // ExceptionHandler
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler);
            // ProcessMessageHandler
            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            await _subscriptionClient.CloseAsync();
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Event received: {Encoding.UTF8.GetString(message.Body)}");
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine(exceptionReceivedEventArgs.Exception);
            return Task.CompletedTask;
        }
    }
}
