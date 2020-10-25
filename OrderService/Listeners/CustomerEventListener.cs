using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OrderService.Connections;
using OrderService.Events;
using OrderService.Handlers;

namespace OrderService.Listeners
{
    public class CustomerEventListener : IHostedService, IEventListener
    {
        private readonly ICustomerEventSubscriber _subscriber;
        private readonly ICustomerEventHandler _eventHandler;

        public CustomerEventListener(ICustomerEventSubscriber subscriber, ICustomerEventHandler eventHandler)
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
            if (headers.ContainsKey("type"))
            {
                if (GetHeaderKeyAsString(headers["type"]).Equals("CreditApprovedEvent"))
                {
                    await _eventHandler.Handle(JsonConvert.DeserializeObject<CreditApprovedEvent>(message));
                }
                else if (GetHeaderKeyAsString(headers["type"]).Equals("CreditRejectedEvent"))
                {
                    await _eventHandler.Handle(JsonConvert.DeserializeObject<CreditRejectedEvent>(message));
                }
                else if (GetHeaderKeyAsString(headers["type"]).Equals("CustomerCreatedEvent"))
                {
                    await _eventHandler.Handle(JsonConvert.DeserializeObject<CustomerCreatedEvent>(message));
                }
            }
            else
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
