using System;
using OrderService.Events;

namespace OrderService.Handlers
{
    public interface ICustomerEventHandler : IEventHandler<CreditApprovedEvent>,
        IEventHandler<CreditRejectedEvent>, IEventHandler<CustomerCreatedEvent>
    {
        
    }
}
