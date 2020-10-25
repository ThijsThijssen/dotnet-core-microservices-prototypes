using System;
namespace OrderService.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public long CustomerId { get; set; }
        public long TotalCredit { get; set; }
        public string Status { get; set; }
    }
}
