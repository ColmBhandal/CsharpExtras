using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Visitor
{
    public abstract class TreeVisitorBase<TPayload> : ITreeVisitor<TPayload>
    {
        protected virtual void DefaultOperation(ITreeBase<TPayload> tree) { }

        public virtual void Visit(ITreeBase<TPayload> tree)
        {
            DoOpAndVisitAllChildren(tree, DefaultOperation);
        }

        protected void DoOpAndVisitAllChildren(ITreeBase<TPayload> tree, Action<ITreeBase<TPayload>> operation)
        {
            operation(tree);
            VisitAllChildren(tree);
        }

        protected void VisitAllChildren(ITreeBase<TPayload> tree)
        {
            IEnumerable<ITreeBase<TPayload>> children = tree.Children;
            foreach (ITreeBase<TPayload> child in children)
            {
                child.Accept(this);
            }
        }
    }
}
