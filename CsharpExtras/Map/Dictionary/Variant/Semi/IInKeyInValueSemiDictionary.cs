using CsharpExtras.Map.Dictionary.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public interface IInKeyInValueSemiDictionary<in K, in V> : IDictionaryBase, IInKeySemiDictionaryBase<K>
    {
        V this[K key] { set; }
        void Add(K key, V value);
    }
}
