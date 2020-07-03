using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Collection
{
    public interface ISetValuedDictionary<TKey, TVal>
        : ICollectionValuedDictionary<TKey, TVal, ISet<TVal>>
    {
        ISetValuedDictionary<TKey, TOther> TransformValues<TOther>(Func<TVal, TOther> transformer);
    }
}
