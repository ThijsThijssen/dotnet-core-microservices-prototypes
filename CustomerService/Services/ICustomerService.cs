using System;
using System.Threading.Tasks;
using CustomerService.Models;

namespace CustomerService.Services
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> GetCustomerById(long customerId);
    }
}
