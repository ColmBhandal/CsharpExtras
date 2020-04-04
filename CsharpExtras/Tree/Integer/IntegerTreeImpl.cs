namespace CsharpExtras.Tree.Integer
{
    class IntegerTreeImpl : ChildSetTreeBase<int, IChildSetIntegerTree, IIntegerLeaf>, IChildSetIntegerTree, IIntegerTree
    {        
        public IntegerTreeImpl(int payload) : base(payload)
        {
        }

        protected override IChildSetIntegerTree GetMe()
        {
            return this;
        }

        protected override IIntegerLeaf LeafFromPayload(int payload)
        {
            return new IntegerLeafImpl(payload);
        }
    }
}
