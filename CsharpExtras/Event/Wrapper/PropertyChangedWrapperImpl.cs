using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Event.Wrapper
{
    internal class PropertyChangedWrapperImpl<TObj, TBefore, TAfter, TEvent> 
        : IPropertyChangedWrapper<TObj, TEvent>
    {
        private readonly Func<TObj, TBefore> _beforeGetter;
        private readonly Func<TObj, TAfter> _afterGetter;
        private readonly Func<TBefore, TAfter, TEvent> _eventGenerator;

        public PropertyChangedWrapperImpl(TObj obj,
            Func<TObj, TBefore> beforeGetter,
            Func<TObj, TAfter> afterGetter,
            Func<TBefore, TAfter, TEvent> eventGenerator)
        {
            _beforeGetter = beforeGetter ?? throw new ArgumentNullException(nameof(beforeGetter));
            _afterGetter = afterGetter ?? throw new ArgumentNullException(nameof(afterGetter));
            _eventGenerator = eventGenerator ?? throw new ArgumentNullException(nameof(eventGenerator));
        }

        public void Run(Action<TObj> act)
        {
            //TODO: Implement
        }
    }
}
