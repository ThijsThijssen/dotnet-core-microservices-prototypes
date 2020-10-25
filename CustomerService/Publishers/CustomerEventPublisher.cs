using System;
using System.Collections.Generic;
using CustomerService.Connections;
using CustomerService.Events;
using Newtonsoft.Json;

namespace CustomerService.Publishers
{
    public class CustomerEventPublisher
    {
        private readonly IConnectionProvider _connection;
        private readonly IPublisher _publisher;

        public CustomerEventPublisher(IConnectionProvider connection)
        {
            _connection = connection;
            _publisher = new Publisher(connectionProvider: _connection,
                exchange: "CustomerEvents", exchangeType: "topic");
        }

        public void PublishCustomerCreatedEvent(CustomerCreatedEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            var headers = new Dictionary<string, object>();

            headers.Add("type", "CustomerCreatedEvent");

            _publisher.Publish(message: message, routingKey: "customer.events", headers);
        }

        public void PublishCreditApprovedEvent(CreditApprovedEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            var headers = new Dictionary<string, object>();

            headers.Add("type", "CreditApprovedEvent");

            _publisher.Publish(message: message, routingKey: "customer.events", headers);
        }

        public void PublishCreditRejectedEvent(CreditRejectedEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            var headers = new Dictionary<string, object>();

            headers.Add("type", "CreditRejectedEvent");

            _publisher.Publish(message: message, routingKey: "customer.events", headers);
        }
    }
}
