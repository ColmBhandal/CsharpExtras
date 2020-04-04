using Tree.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Integer
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
