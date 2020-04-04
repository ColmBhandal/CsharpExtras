using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Tree
{
    abstract class PayloadContainerTreeBase<TPayload> : TreeBase<TPayload>
    {
        public PayloadContainerTreeBase(TPayload payload)
        {
            Payload = payload;
        }

        public override TPayload Payload { get; }
    }
}
