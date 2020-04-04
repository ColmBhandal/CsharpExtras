using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Tree.Visitor.Counter
{
    public class NodeCounterVisitorImpl<TPayload> : NodeCounterVisitorBase<TPayload>, INodeCounterVisitor<TPayload>
    {
        protected override void DefaultOperation(ITreeBase<TPayload> tree)
        {
            Count++;
        }
    }
}
