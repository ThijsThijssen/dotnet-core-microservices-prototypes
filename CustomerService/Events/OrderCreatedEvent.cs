using System;
namespace CustomerService.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public readonly long OrderId;
        public readonly long CustomerId;
        public readonly long TotalCredit;

        public OrderCreatedEvent(Guid id, DateTimeOffset timeStamp, long orderId, long customerId, long totalCredit)
        {
            Id = id;
            TimeStamp = timeStamp;
            OrderId = orderId;
            CustomerId = customerId;
            TotalCredit = totalCredit;
        }
    }
}
