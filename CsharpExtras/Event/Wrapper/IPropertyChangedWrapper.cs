using System;

namespace CsharpExtras.Event.Wrapper
{
    public interface IPropertyChangedWrapper<TObj, TEvent>
    {
        event Action<TEvent>? OnPropertyChanged;

        TObj Run(Action<TObj> act);
        TReturn Get<TReturn>(Func<TObj, TReturn> f);
    }
}