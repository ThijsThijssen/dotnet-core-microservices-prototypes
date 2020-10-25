using System;
using System.Threading.Tasks;
using OrderService.Events;
using OrderService.Models;
using OrderService.Publishers;
using OrderService.Services;

namespace OrderService.Handlers
{
    public class CustomerEventHandler : ICustomerEventHandler
    {
        private readonly IOrderService _orderService;
        private readonly OrderEventPublisher _publisher;

        public CustomerEventHandler(IOrderService orderService, OrderEventPublisher publisher)
        {
            _orderService = orderService;
            _publisher = publisher;
        }

        public async Task Handle(CreditApprovedEvent @event)
        {
            Console.WriteLine("CreditApprovedEvent");

            // get order by id
            var order = await _orderService.GetOrderById(@event.OrderId);

            // update status to approved
            order.Status = "APPROVED";

            // save order
            await _orderService.UpdateOrderById(@event.OrderId, order);
        }

        public async Task Handle(CreditRejectedEvent @event)
        {
            Console.WriteLine("CreditRejectedEvent");

            // get order by id
            var order = await _orderService.GetOrderById(@event.OrderId);

            // update status to approved
            order.Status = "REJECTED";

            // save order
            await _orderService.UpdateOrderById(@event.OrderId, order);
        }

        public async Task Handle(CustomerCreatedEvent @event)
        {
            // check if customer exists otherwise add it to database
            Console.WriteLine("CustomerCreatedEvent");
            await Task.Delay(100);
        }
    }
}
