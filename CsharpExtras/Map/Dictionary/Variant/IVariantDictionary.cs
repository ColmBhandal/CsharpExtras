using CsharpExtras.Map.Dictionary.Variant.Semi;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Variant
{
    //Non-mvp: Further refine the types sitting above this - convert some into the Map types
    public interface IVariantDictionary<K, V> : IInKeyInValueSemiDictionary<K, V>, IInKeyOutValueSemiDictionary<K, V>,
        IOutKeySemiDictionaryBase<K>, IFixedKeyOutValueSemiDictionary<K, V>, IDictionary<K, V>
    { }
}
