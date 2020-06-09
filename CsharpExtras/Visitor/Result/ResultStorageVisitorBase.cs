using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CsharpExtras.Log.StaticLogManager;

namespace CsharpExtras.Visitor.Result
{
    public abstract class ResultStorageVisitorBase<TVisitable, TResult> : IVisitorBase<TVisitable>,
        IResultStorageVisitorBase<TVisitable, TResult> where TVisitable : IVisitableBase<TVisitable>
        where TResult : class
    {
        protected TResult? _result;
        public TResult Result => GetResultOrDefault();

        private bool _visiting  = false;

        private TResult GetResultOrDefault()
        {
            if (_result == null)
            {
                Logger.Warn("No factory defined for visitor. Has Visit been called? Inactive factory will be returned.");
                return (_result = DefaultResult());
            }
            return _result;
        }

        public void Visit(TVisitable obj)
        {
            //We need this variable to bottom out and avoid recursion if there is no overload defined in the sub class
            if (_visiting)
            {
                DefaultVisit(obj);
                Logger.Debug("Using the default visit function because no overloads found");
            }
            else
            {
                _visiting = true;
                obj.Accept(this);
                _visiting = false;
            }
        }

        protected virtual void DefaultVisit(TVisitable obj)
        {
            _result = DefaultResult();
        }

        protected abstract TResult DefaultResult();
    }
}
