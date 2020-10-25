using System;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.DAOs
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
