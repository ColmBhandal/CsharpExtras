using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Event.Wrapper
{
    public interface IMockBefore { }
    public interface IMockAfter { }

    public interface IMockGenerator
    {
        IMockEvent Generate(IMockBefore before, IMockAfter after);
    }

    public interface IMockObj 
    {
        IMockBefore GetBefore { get; }
        IMockAfter GetAfter { get; }
    }

    public interface IMockEvent { }

    public interface IMockEventHandler
    {
        void Handle(IMockEvent e);
    }

    public interface IMockActionExecutor
    {
        void Execute(IMockObj obj);
    }

    public interface IMockEventExecutor
    {
        IMockEvent Execute(IMockObj obj);
    }

    public interface IMockGetter<TResult>
    {
        (IMockEvent e, TResult result) ExecuteGet(IMockObj obj);
    }
}
