using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Dictionary.Collection
{
    public interface IListValuedDictionary<TKey, TVal> : ICollectionValuedDictionary<TKey, TVal, IList<TVal>>
    {
    }
}
