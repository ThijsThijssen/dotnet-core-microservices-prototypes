using System;
using System.Threading.Tasks;
using CustomerService.DAOs;
using CustomerService.Events;
using CustomerService.Models;
using CustomerService.Publishers;

namespace CustomerService.Services
{
    public class CustomerServiceImpl : ICustomerService
    {
        private readonly ICustomerDao _customerDao;
        private readonly CustomerEventPublisher _publisher;

        public CustomerServiceImpl(ICustomerDao customerDao, CustomerEventPublisher publisher)
        {
            _customerDao = customerDao;
            _publisher = publisher;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            await _customerDao.CreateCustomer(customer);

            // create OrderCreatedEvent
            var @event = new CustomerCreatedEvent
            (
                id: Guid.NewGuid(),
                timeStamp: DateTime.Now,
                customerId: customer.CustomerId,
                availableCredit: customer.AvailableCredit
            );

            // publish event
            _publisher.PublishCustomerCreatedEvent(@event);

            return customer;
        }

        public async Task<Customer> GetCustomerById(long customerId)
        {
            return await _customerDao.GetCustomerById(customerId);
        }
    }
}
