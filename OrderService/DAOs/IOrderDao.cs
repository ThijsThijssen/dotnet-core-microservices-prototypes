using System;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.DAOs
{
    public interface IOrderDao
    {
        Task<Order> CreateOrder(Order order);
        Task<Order> GetOrderById(long orderId);
        Task<Order> UpdateOrderById(long orderId, Order order);
    }
}
