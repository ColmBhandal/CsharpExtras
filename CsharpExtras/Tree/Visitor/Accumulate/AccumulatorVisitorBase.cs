using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Visitor.Void.Integer
{
    public abstract class AccumulatorVisitorBase<TPayload, TResult> : TreeVisitorBase<TPayload>, IAccumulatorVisitor<TResult>        
    {
        public abstract TResult Result { get; protected set;}

        protected abstract TResult Accumulate(TPayload a, TResult b);

        protected override void DefaultOperation(ITreeBase<TPayload> tree)
        {
            TPayload payload = tree.Payload;
            Result = Accumulate(payload, Result);
        }
    }
}
