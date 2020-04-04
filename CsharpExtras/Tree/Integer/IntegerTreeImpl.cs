using CustomExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Integer
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
