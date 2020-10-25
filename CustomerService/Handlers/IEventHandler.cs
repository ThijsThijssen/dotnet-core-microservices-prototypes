using System;
using System.Threading.Tasks;

namespace CustomerService.Handlers
{
    public interface IEventHandler<T>
    {
        Task Handle(T @event);
    }
}
