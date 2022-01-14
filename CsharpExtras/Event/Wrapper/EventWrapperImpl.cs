using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Event.Wrapper
{
    internal class EventWrapperImpl<TObj, TEvent> : IEventWrapper<TObj, TEvent>
    {
        private readonly TObj _object;
        private readonly Action<TEvent> _eventHandler;

        public EventWrapperImpl(TObj obj, Action<TEvent> eventHandler)
        {
            _object = obj;
            _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
        }

        public void Run(Func<TObj, TEvent> action)
        {
            //TODO
        }
    }
}
