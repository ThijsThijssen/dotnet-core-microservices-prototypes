﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Listeners
{
    public interface ISubscriber : IDisposable
    {
        void Subscribe(Func<string, IDictionary<string, object>, Task<bool>> callback);
    }
}
