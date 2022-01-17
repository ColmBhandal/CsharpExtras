using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Event.Wrapper
{
    internal class PropertyChangedWrapperImpl<TObj, TBefore, TAfter, TEvent> 
        : IPropertyChangedWrapper<TObj, TEvent>
    {
        public event Action<TEvent>? OnPropertyChanged;

        private readonly TObj _obj;
        private readonly Func<TObj, TBefore> _beforeGetter;
        private readonly Func<TObj, TAfter> _afterGetter;
        private readonly Func<TBefore, TAfter, TEvent> _eventGenerator;

        public PropertyChangedWrapperImpl(TObj obj,
            Func<TObj, TBefore> beforeGetter,
            Func<TObj, TAfter> afterGetter,
            Func<TBefore, TAfter, TEvent> eventGenerator)
        {
            _obj = obj;
            _beforeGetter = beforeGetter ?? throw new ArgumentNullException(nameof(beforeGetter));
            _afterGetter = afterGetter ?? throw new ArgumentNullException(nameof(afterGetter));
            _eventGenerator = eventGenerator ?? throw new ArgumentNullException(nameof(eventGenerator));
        }

        public TReturn Get<TReturn>(Func<TObj, TReturn> f)
        {
            TBefore beforeValue = _beforeGetter(_obj);
            TReturn returnValue = f(_obj);
            TAfter afterValue = _afterGetter(_obj);
            TEvent eventValue = _eventGenerator(beforeValue, afterValue);
            OnPropertyChanged?.Invoke(eventValue);
            return returnValue;
        }

        public TObj Run(Action<TObj> act)
        {
            Func<TObj, TObj> actAsFunc = o => { act(o); return o; };
            return Get(actAsFunc);
        }
    }
}
