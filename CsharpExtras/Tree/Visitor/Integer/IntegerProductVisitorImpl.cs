using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Visitor.Integer
{
    public class IntegerProductVisitorImpl : IntegerAccumulatorVisitorBase, IIntegerAccumulationVisitor
    {
        public override int Result { get; protected set; } = 1;

        protected override int Accumulate(int a, int b)
        {
            return a * b;
        }
    }
}
