using System;
using RabbitMQ.Client;

namespace OrderService.Connections
{
    public class ConnectionProviderImpl : IConnectionProvider
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private bool _disposed;

        public ConnectionProviderImpl(string hostName, string username, string password)
        {
            _factory = new ConnectionFactory
            {
                //Uri = new Uri(connectionString),
                HostName = hostName,
                UserName = username,
                Password = password,
            };

            _connection = _factory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _connection?.Close();
            }

            _disposed = true;
        }
    }
}
