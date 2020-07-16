using CsharpExtras.Map.Dictionary.Base;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public interface IInKeyOutValueSemiDictionary<in K, out V> : IDictionaryBase,
        IInKeySemiDictionaryBase<K>, IOutValueSemiDictionaryBase<V>
    {
        V this[K key] { get; }
        ISuccessTuple<V> TryGetValue(K key);
    }

}
