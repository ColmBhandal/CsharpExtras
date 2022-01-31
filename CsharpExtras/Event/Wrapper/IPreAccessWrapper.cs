using System;

namespace CsharpExtras.Event.Wrapper
{
    public interface IPreAccessWrapper<TObj>
    {
        TReturn Get<TReturn>(Func<TObj, TReturn> f);
        void Run(Action<TObj> act);
    }
}