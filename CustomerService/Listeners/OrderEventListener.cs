using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomerService.Connections;
using CustomerService.Events;
using CustomerService.Handlers;
using CustomerService.Publishers;
using CustomerService.Services;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CustomerService.Listeners
{
    public class OrderEventListener : IHostedService, IEventListener
    {
        private readonly IOrderEventSubscriber _subscriber;
        private readonly IOrderEventHandler _eventHandler;

        public OrderEventListener(IOrderEventSubscriber subscriber, IOrderEventHandler eventHandler)
        {
            _subscriber = subscriber;
            _eventHandler = eventHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(ProcessMessage);
            return Task.CompletedTask;
        }

        public async Task<bool> ProcessMessage(string message, IDictionary<string, object> headers)
        {
            Console.WriteLine(message);

            if (headers.ContainsKey("type"))
            {
                if (GetHeaderKeyAsString(headers["type"]).Equals("OrderCreatedEvent"))
                {
                    await _eventHandler.Handle(JsonConvert.DeserializeObject<OrderCreatedEvent>(message));
                }
            } else
            {
                return false;
            }

            return true;
        }

        private string GetHeaderKeyAsString(object headerKey)
        {
            return System.Text.Encoding.UTF8.GetString((byte[])headerKey);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
