using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Tree.Visitor.Counter
{
    public abstract class NodeCounterVisitorBase<TPayload> : TreeVisitorBase<TPayload>, INodeCounterVisitor<TPayload>
    {
        public int Count { get; protected set; } = 0;
    }
}
