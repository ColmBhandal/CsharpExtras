using CsharpExtras.ValidatedType.Numeric.Integer;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Curry
{
    public interface ICurryDictionaryRec<TKey, TVal> : ICurryDictionary<TKey, TVal>
    {
        ICurryDictionary<TKey, TVal> CurriedChild(TKey key);
    }
}