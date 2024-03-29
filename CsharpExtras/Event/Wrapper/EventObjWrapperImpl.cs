﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Event.Wrapper
{
    internal class EventObjWrapperImpl<TObj, TEvent> : IEventObjWrapper<TObj, TEvent>
    {
        protected readonly TObj _object;
        private readonly Action<TEvent> _eventHandler;

        public EventObjWrapperImpl(TObj obj, Action<TEvent> eventHandler)
        {
            _object = obj;
            _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
        }

        public void Run(Func<TObj, TEvent> action)
        {
            TEvent ev = action(_object);
            _eventHandler(ev);
        }

        public TResult Get<TResult>(Func<TObj, (TEvent e, TResult result)> f)
        {
            (TEvent e, TResult result) = f(_object);
            _eventHandler(e);
            return result;
        }
    }
}
