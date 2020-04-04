using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Tree.Visitor.Integer
{
    public class IntegerSummationVisitorImpl : IntegerAccumulatorVisitorBase, IIntegerAccumulationVisitor
    {
        public override int Result { get; protected set; } = 0;

        protected override int Accumulate(int a, int b)
        {
            return a + b;
        }
    }
}
