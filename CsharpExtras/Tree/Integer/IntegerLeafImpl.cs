using CsharpExtras.Tree.Base;

namespace CsharpExtras.Tree.Integer
{
    class IntegerLeafImpl : LeafBase<int>, IIntegerLeaf, IIntegerTree
    {
        public IntegerLeafImpl(int payload) : base(payload)
        {
        }
    }
    public interface IIntegerLeaf : ILeafBase<int>
    {
    }
}
