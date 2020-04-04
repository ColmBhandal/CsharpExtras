using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public interface IVisitorBase<TVisitable> where TVisitable : IVisitableBase<TVisitable>
    {
        void Visit(TVisitable obj);
    }
}
