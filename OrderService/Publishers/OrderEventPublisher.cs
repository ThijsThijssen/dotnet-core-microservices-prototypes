using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OrderService.Connections;
using OrderService.Events;

namespace OrderService.Publishers
{
    public class OrderEventPublisher
    {
        private readonly IConnectionProvider _connection;
        private readonly IPublisher _publisher;

        public OrderEventPublisher(IConnectionProvider connection)
        {
            _connection = connection;
            _publisher = new Publisher(connectionProvider: _connection,
                exchange: "OrderEvents", exchangeType: "topic");
        }

        public void PublishOrderCreatedEvent(OrderCreatedEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            var headers = new Dictionary<string, object>();

            headers.Add("type", "OrderCreatedEvent");

            _publisher.Publish(message: message, routingKey: "order.events", headers);
        }
    }
}
