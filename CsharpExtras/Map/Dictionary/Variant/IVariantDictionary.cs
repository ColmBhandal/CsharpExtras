﻿using CsharpExtras.Map.Dictionary.Variant.Semi;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Variant
{
    //Non-mvp: Further refine the types sitting above this - convert some into the Map types
    public interface IVariantDictionary<K, V> : IInKeyInValueSemiDictionary<K, V>, IInKeyOutValueSemiDictionary<K, V>,
        IOutKeySemiDictionaryBase<K>, IFixedKeyOutValueSemiDictionary<K, V>, IDictionary<K, V>
    {
        new int Count { get; }
        new V this[K key] { get;set; }
        new bool IsReadOnly { get; }
        new IEnumerable<K> Keys { get; }
        new ICollection<V> Values { get; }
        new void Add(K key, V value);
        new bool Remove(K key);
        new bool ContainsKey(K key);
        new ISuccessTuple<V> TryGetValue(K key);
        new void Clear();
    }
}
