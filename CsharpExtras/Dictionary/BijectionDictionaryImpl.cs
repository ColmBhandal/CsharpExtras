using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Dictionary
{
    public class BijectionDictionaryImpl<TKey, TValue> : IBijectionDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _delegate = new Dictionary<TKey, TValue>();
        private readonly IDictionary<TValue, TKey> _reverseDelegate = new Dictionary<TValue, TKey>();

        private IBijectionDictionary<TValue, TKey>? _reverse;
        public IBijectionDictionary<TValue, TKey> Reverse => _reverse ?? (_reverse = new BijectionDictionaryImpl<TValue, TKey>(_reverseDelegate, this));

        public BijectionDictionaryImpl()
        {
        }

        public BijectionDictionaryImpl(IDictionary<TKey, TValue> dictionary)
        {
            _delegate = dictionary;
            foreach (TKey key in dictionary.Keys)
            {
                TValue value = dictionary[key];
                if (_reverseDelegate.ContainsKey(value))
                {
                    throw new ArgumentException("Cannot build a two-way dictionary from a dictionary with duplicate values");
                }
                _reverseDelegate.Add(value, key);
            }
        }

        private BijectionDictionaryImpl(IDictionary<TKey, TValue> dictionary, BijectionDictionaryImpl<TValue, TKey> reverseDictionary)
        {
            _delegate = dictionary;
            _reverse = reverseDictionary;
            _reverseDelegate = reverseDictionary._delegate;
        }


        public void Add(TKey key, TValue value)
        {
            if (_delegate.ContainsKey(key) || _reverseDelegate.ContainsKey(value))
            {
                throw new ArgumentException(string.Format("An item with the same key ({0}) has already been added.", key?.ToString() ?? ""));
            }

            _delegate.Add(key, value);
            _reverseDelegate.Add(value, key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _delegate.Clear();
            _reverseDelegate.Clear();
        }

        public bool Remove(TKey key)
        {
            if (!_delegate.ContainsKey(key) || !_reverseDelegate.ContainsKey(_delegate[key]))
            {
                return false;
            }
            return _reverseDelegate.Remove(_delegate[key]) && _delegate.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _delegate.Remove(item) && _reverseDelegate.Remove(item.Value);
        }

        #region Delegating to IDictionary
        public TValue this[TKey key] { get => _delegate[key]; set => _delegate[key] = value; }

        public ICollection<TKey> Keys => _delegate.Keys;

        public ICollection<TValue> Values => _delegate.Values;

        public int Count => _delegate.Count;

        public bool IsReadOnly => _delegate.IsReadOnly;

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _delegate.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _delegate.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _delegate.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _delegate.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _delegate.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _delegate.GetEnumerator();
        }
        #endregion
    }
}
