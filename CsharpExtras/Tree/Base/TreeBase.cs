using CsharpExtras.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsharpExtras.Tree
{
    public abstract class TreeBase<TPayload> : VisitableBase<ITreeBase<TPayload>>, ITreeBase<TPayload>
    {
        public abstract IEnumerable<ITreeBase<TPayload>> Children { get; }

        public abstract TPayload Payload { get; }

        public bool HasChildren()
        {
            return Children.Any();
        }

        public void DoForAllChildren(Action<ITreeBase<TPayload>> action)
        {
            foreach(ITreeBase<TPayload> child in Children)
            {
                action.Invoke(child);
            }
        }
    }
}
