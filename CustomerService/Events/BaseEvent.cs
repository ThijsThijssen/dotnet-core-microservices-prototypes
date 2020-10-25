﻿using System;
namespace CustomerService.Events
{
    public class BaseEvent : IEvent
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
