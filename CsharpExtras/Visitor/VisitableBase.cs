using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Visitor
{
    public abstract class VisitableBase<TVisitable> : IVisitableBase<TVisitable> where TVisitable : IVisitableBase<TVisitable>
    {
        public virtual void Accept(IVisitorBase<TVisitable> visitor)
        {
            /*This disgusting casting to dynamics is necessary in order to force the C# compiler not to resolve the overload at compile time
            Without this, we'd have to override Accept for every child of TreeBase, which would be a horrible violation of DRY
            Empirical testing revealed that both casts to dynamics are necessary*/
            dynamic visitorDynamic = visitor as dynamic;
            visitorDynamic.Visit(this as dynamic);
        }
    }
}
