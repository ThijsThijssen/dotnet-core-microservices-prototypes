using System;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public interface IEventHandler<T>
    {
        Task Handle(T @event);
    }
}
