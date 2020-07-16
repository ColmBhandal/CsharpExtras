using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public interface IFixedKeyOutValueSemiDictionary<K, out V> : IOutValueSemiDictionaryBase<V>
    {
        V this[K key] { get; }
        ISuccessTuple<V> TryGetValue(K key);
        IEnumerable<K> Keys { get; }
    }
}
