using Tree.Visitor;
using Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
    public interface ITreeBase<TPayload> : IVisitableBase<ITreeBase<TPayload>>
    {
        TPayload Payload { get; }
        IEnumerable<ITreeBase<TPayload>> Children{ get; }

        void DoForAllChildren(Action<ITreeBase<TPayload>> action);
        bool HasChildren();
    }
}
