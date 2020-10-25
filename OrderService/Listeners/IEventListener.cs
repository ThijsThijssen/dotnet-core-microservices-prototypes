using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Listeners
{
    public interface IEventListener
    {
        Task<bool> ProcessMessage(string message, IDictionary<string, object> headers);
    }
}
