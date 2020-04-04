using Tree.Visitor;
using Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
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
