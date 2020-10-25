using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DAOs
{
    public class OrderDaoImpl : IOrderDao
    {
        private readonly OrderContext _context;

        public OrderDaoImpl(OrderContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderById(long orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return null;
            }

            return order;
        }

        public async Task<Order> UpdateOrderById(long orderId, Order order)
        {
            if (orderId != order.OrderId)
            {
                return order;
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!OrderExists(orderId))
                {
                    Console.WriteLine("Order not found!");
                }
                else
                {
                    Console.WriteLine($"Something went wrong with updating an order! {e.StackTrace}");
                }
            }

            return order;
        }

        private bool OrderExists(long orderId)
        {
            return _context.Orders.Any(e => e.OrderId == orderId);
        }
    }
}
