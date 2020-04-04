using CsharpExtras.Tree.Base;
using static CsharpExtras.Log.StaticLogManager;

namespace CsharpExtras.Tree.Visitor.Counter
{
    /// <summary>
    /// Count only the leaf nodes of the tree in question, not the intermediate nodes
    /// </summary>
    public class LeaftCounterVisitorImpl<TPayload> : NodeCounterVisitorBase<TPayload>, INodeCounterVisitor<TPayload>
    {
        public virtual void Visit(ILeafBase<TPayload> leaf)
        {
            if (leaf == null)
            {
                Logger.Error("Skipping over null leaf during leaf counting operation");
            }
            Count++;
        }
    }
}
