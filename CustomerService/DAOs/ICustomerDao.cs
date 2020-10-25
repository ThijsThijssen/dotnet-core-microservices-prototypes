using System;
using System.Threading.Tasks;
using CustomerService.Models;

namespace CustomerService.DAOs
{
    public interface ICustomerDao
    {
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> GetCustomerById(long customerId);
    }
}
