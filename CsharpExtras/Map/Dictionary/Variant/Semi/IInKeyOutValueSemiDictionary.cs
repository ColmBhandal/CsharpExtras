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

    public interface ISuccessTuple<out V>
    {
        bool WasSuccessful { get; }
        V Value { get; }
    }

    public class SuccessTupleImpl<V> : ISuccessTuple<V>
    {
        public bool WasSuccessful { get; }
        public V Value {get;}

        public SuccessTupleImpl(bool wasSuccessful, V value)
        {
            WasSuccessful = wasSuccessful;
            Value = value;
        }
    }

}
