using System;
namespace OrderService.Events
{
    public class CreditRejectedEvent : BaseEvent
    {
        public readonly long OrderId;

        public CreditRejectedEvent(Guid id, DateTimeOffset timeStamp, long orderId)
        {
            Id = id;
            TimeStamp = timeStamp;
            OrderId = orderId;
        }
    }
}
