using System;
namespace CustomerService.Events
{
    public class CreditApprovedEvent : BaseEvent
    {
        public readonly long OrderId;

        public CreditApprovedEvent(Guid id, DateTimeOffset timeStamp, long orderId)
        {
            Id = id;
            TimeStamp = timeStamp;
            OrderId = orderId;
        }
    }
}
