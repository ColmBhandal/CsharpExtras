using System;

namespace CsharpExtras.Event.Wrapper
{
    public interface IPropertyChangedWrapper<TObj, TEvent>
    {
        void Run(Action<TObj> act);
    }
}