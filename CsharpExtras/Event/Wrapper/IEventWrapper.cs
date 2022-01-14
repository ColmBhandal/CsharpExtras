using System;

namespace CsharpExtras.Event.Wrapper
{
    public interface IEventWrapper<TObj, TEvent>
    {
        void Run(Func<TObj, TEvent> action);
    }
}