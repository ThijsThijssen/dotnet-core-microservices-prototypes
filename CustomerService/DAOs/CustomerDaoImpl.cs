using System;
using System.Threading.Tasks;
using CustomerService.Models;

namespace CustomerService.DAOs
{
    public class CustomerDaoImpl : ICustomerDao
    {
        private readonly CustomerContext _context;

        public CustomerDaoImpl(CustomerContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> GetCustomerById(long customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return null;
            }

            return customer;
        }
    }
}
