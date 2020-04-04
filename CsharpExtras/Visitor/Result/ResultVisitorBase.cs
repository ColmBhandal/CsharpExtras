using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Visitor.Result
{
    public class ResultVisitorBase<TVisitable, TResult> : IResultVisitorBase<TVisitable, TResult> where TVisitable : IVisitableBase<TVisitable>
    {
        private readonly IResultStorageVisitorBase<TVisitable, TResult> _storageVisitor;

        public ResultVisitorBase(IResultStorageVisitorBase<TVisitable, TResult> storageVisitor)
        {
            _storageVisitor = storageVisitor ?? throw new ArgumentNullException(nameof(storageVisitor));
        }

        public TResult GetResult(TVisitable visitable)
        {
            _storageVisitor.Visit(visitable);
            return _storageVisitor.Result;
        }
    }
}
