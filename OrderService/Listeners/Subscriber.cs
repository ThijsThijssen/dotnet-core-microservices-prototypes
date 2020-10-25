using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OrderService.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.Listeners
{
    public class Subscriber : ICustomerEventSubscriber
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly string _exchange;
        private readonly string _queue;
        private readonly IModel _channel;
        private bool _disposed;

        public Subscriber(
            IConnectionProvider connectionProvider,
            string exchange,
            string routingKey,
            string exchangeType,
            int timeToLive = 30000,
            ushort prefetchSize = 10)
        {
            _connectionProvider = connectionProvider;
            _exchange = exchange;
            _channel = _connectionProvider.GetConnection().CreateModel();
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };
            _channel.ExchangeDeclare(_exchange, exchangeType, arguments: ttl);
            _queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queue, _exchange, routingKey);
            _channel.BasicQos(0, prefetchSize, false);
        }

        public void Subscribe(Func<string, IDictionary<string, object>, Task<bool>> callback)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool success = await callback.Invoke(message, e.BasicProperties.Headers);
                if (success)
                {
                    _channel.BasicAck(e.DeliveryTag, true);
                }
            };

            _channel.BasicConsume(_queue, false, consumer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _channel?.Close();

            _disposed = true;
        }
    }
}
