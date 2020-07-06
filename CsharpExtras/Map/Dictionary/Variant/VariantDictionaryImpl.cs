using CsharpExtras.Map.Dictionary.Variant.Semi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Variant
{
    class VariantDictionaryImpl<K, V> : IVariantDictionary<K, V>
    {
        private readonly IDictionary<K, V> _backingDictionary;

        public VariantDictionaryImpl(IDictionary<K, V> backingDictionary)
        {
            _backingDictionary = backingDictionary ?? throw new ArgumentNullException(nameof(backingDictionary));
        }

        public V this[K key] { set => _backingDictionary[key] = value; }

        V IInKeyOutValueSemiDictionary<K, V>.this[K key] => _backingDictionary[key];

        V IDictionary<K, V>.this[K key] { get => _backingDictionary[key]; set => _backingDictionary[key] = value; }

        public int Count => _backingDictionary.Count;

        public bool IsReadOnly => _backingDictionary.IsReadOnly;

        public IEnumerable<K> Keys => _backingDictionary.Keys;

        public ICollection<V> Values => _backingDictionary.Values;

        ICollection<K> IDictionary<K, V>.Keys => _backingDictionary.Keys;

        IEnumerable<V> IOutValueSemiDictionaryBase<V>.Values => Values;

        public void Add(K key, V value)
        {
            _backingDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            _backingDictionary.Add(item);
        }

        public void Clear()
        {
            _backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return _backingDictionary.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return _backingDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            _backingDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _backingDictionary.GetEnumerator();
        }

        public bool Remove(K key)
        {
            return _backingDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return _backingDictionary.Remove(item);
        }

        public ISuccessTuple<V> TryGetValue(K key)
        {
            bool wasSuccessful = _backingDictionary.TryGetValue(key, out V v);
            return new SuccessTupleImpl<V>(wasSuccessful, v);
        }

        public bool TryGetValue(K key, out V value)
        {
            return _backingDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_backingDictionary).GetEnumerator();
        }
    }
}
