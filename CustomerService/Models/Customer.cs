using System;
namespace CustomerService.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public long AvailableCredit { get; set; }
    }
}
