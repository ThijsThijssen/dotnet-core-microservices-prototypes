using System;
using System.Threading.Tasks;
using OrderService.DAOs;
using OrderService.Events;
using OrderService.Models;
using OrderService.Publishers;

namespace OrderService.Services
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly IOrderDao _orderDao;
        private readonly OrderEventPublisher _publisher;

        public OrderServiceImpl(IOrderDao orderDao, OrderEventPublisher publisher)
        {
            _orderDao = orderDao;
            _publisher = publisher;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await _orderDao.CreateOrder(order);

            // create OrderCreatedEvent
            var @event = new OrderCreatedEvent
            (
                id: Guid.NewGuid(),
                timeStamp: DateTime.Now,
                orderId: order.OrderId,
                customerId: order.CustomerId,
                totalCredit: order.TotalCredit
            );

            // publish event
            _publisher.PublishOrderCreatedEvent(@event);

            return order;
        }

        public async Task<Order> GetOrderById(long orderId)
        {
            return await _orderDao.GetOrderById(orderId);
        }

        public async Task<Order> UpdateOrderById(long orderId, Order order)
        {
            return await _orderDao.UpdateOrderById(orderId, order);
        }
    }
}
