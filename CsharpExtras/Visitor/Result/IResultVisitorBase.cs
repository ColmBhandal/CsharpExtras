﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Visitor.Result
{
    public interface IResultVisitorBase<TVisitable, TResult>
    {
        TResult GetResult(TVisitable visitable);
    }
}
