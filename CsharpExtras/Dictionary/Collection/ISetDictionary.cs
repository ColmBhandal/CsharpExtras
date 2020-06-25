using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Dictionary.Collection
{
    public interface ISetDictionary<TKey, TVal>
        : ICollectionDictionary<TKey, TVal, ISet<TVal>>
    {
        ISetDictionary<TKey, TOther> TransformValues<TOther>(Func<TVal, TOther> transformer);
    }
}
