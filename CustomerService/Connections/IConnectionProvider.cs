using System;
using RabbitMQ.Client;

namespace CustomerService.Connections
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}
