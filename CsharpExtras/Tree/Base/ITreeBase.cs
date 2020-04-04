using CsharpExtras.Visitor;
using System;
using System.Collections.Generic;

namespace CsharpExtras.Tree
{
    public interface ITreeBase<TPayload> : IVisitableBase<ITreeBase<TPayload>>
    {
        TPayload Payload { get; }
        IEnumerable<ITreeBase<TPayload>> Children{ get; }

        void DoForAllChildren(Action<ITreeBase<TPayload>> action);
        bool HasChildren();
    }
}
