using System;
using CustomerService.Events;

namespace CustomerService.Handlers
{
    public interface IOrderEventHandler : IEventHandler<OrderCreatedEvent>
    {
        
    }
}
