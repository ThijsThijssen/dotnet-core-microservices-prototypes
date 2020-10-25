using System;
namespace CustomerService.Events
{
    public class CustomerCreatedEvent : BaseEvent
    {
        public readonly long CustomerId;
        public readonly long AvailableCredit;

        public CustomerCreatedEvent(Guid id, DateTimeOffset timeStamp, long customerId, long availableCredit)
        {
            Id = id;
            TimeStamp = timeStamp;
            CustomerId = customerId;
            AvailableCredit = availableCredit;
        }
    }
}
