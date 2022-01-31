using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Event.Wrapper
{
    internal class PreAccessWrapperImpl<TObj> : IPreAccessWrapper<TObj>
    {
        private readonly TObj _object;
        private readonly Action<TObj> _preAccessAction;

        public PreAccessWrapperImpl(TObj obj, Action<TObj> preAccessAction)
        {
            _object = obj;
            _preAccessAction = preAccessAction ?? throw new ArgumentNullException(nameof(preAccessAction));
        }

        public void Run(Action<TObj> act)
        {

        }

        public TReturn Get<TReturn>(Func<TObj, TReturn> f)
        {
            //TODO: Implement properly
            return f(_object);
        }
    }
}
