using System;
using RabbitMQ.Client;

namespace OrderService.Connections
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}
