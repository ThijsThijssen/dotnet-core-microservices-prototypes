using System;
using System.Threading.Tasks;
using CustomerService.Connections;
using CustomerService.DAOs;
using CustomerService.Events;
using CustomerService.Publishers;
using CustomerService.Services;

namespace CustomerService.Handlers
{
    public class OrderEventHandler : IOrderEventHandler
    {
        private readonly ICustomerService _customerService;
        private readonly CustomerEventPublisher _publisher;

        public OrderEventHandler(ICustomerService customerService, CustomerEventPublisher publisher)
        {
            _customerService = customerService;
            _publisher = publisher;
        }

        public async Task Handle(OrderCreatedEvent @event)
        {
            // get customer by id (service)
            var customer = await _customerService.GetCustomerById(@event.CustomerId);

            if (customer != null)
            {
                // check if total credit <= available credit (method)
                if (customer.AvailableCredit >= @event.TotalCredit)
                {
                    // TODO remove totalCredit from AvailableCredit.

                    var creditApprovedEvent = new CreditApprovedEvent
                    (
                        id: Guid.NewGuid(),
                        timeStamp: DateTime.Now,
                        orderId: @event.OrderId
                    );

                    // send approved event
                    _publisher.PublishCreditApprovedEvent(creditApprovedEvent);
                }
                else
                {
                    var creditRejectedEvent = new CreditRejectedEvent
                    (
                        id: Guid.NewGuid(),
                        timeStamp: DateTime.Now,
                        orderId: @event.OrderId
                    );

                    // send rejected event
                    _publisher.PublishCreditRejectedEvent(creditRejectedEvent);
                }
            }
            else
            {
                Console.WriteLine($"Customer with {@event.CustomerId} does not exist!");
            }
        }
    }
}
