﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Dictionary
{

    class MultiValueMapImpl<TKey, TVal> : IMultiValueMap<TKey, TVal>, IDictionary<TKey, ISet<TVal>>
    {
        private readonly IDictionary<TKey, ISet<TVal>> _setValuedMap = new Dictionary<TKey, ISet<TVal>>();

        public ISet<TVal> this[TKey key] { get => _setValuedMap[key]; set => _setValuedMap[key] = value; }

        public ICollection<TKey> Keys => _setValuedMap.Keys;

        public ICollection<ISet<TVal>> Values => _setValuedMap.Values;

        public int Count => _setValuedMap.Count;

        public bool IsReadOnly => _setValuedMap.IsReadOnly;

        public void Add(TKey key, TVal value)
        {
            if (!_setValuedMap.ContainsKey(key))
            {
                _setValuedMap.Add(key, new HashSet<TVal>());
            }
            _setValuedMap[key].Add(value);
        }
        public IMultiValueMap<TKey, V> TransformValues<V>(Func<TVal, V> transformer)
        {
            IMultiValueMap<TKey, V> transformedMap = new MultiValueMapImpl<TKey, V>();
            foreach(TKey key in Keys)
            {
                ISet<TVal> thisSet = this[key];
                IEnumerable<V> transformedValues = thisSet.Select(transformer);
                foreach(V transformedValue in transformedValues)
                {
                    transformedMap.Add(key, transformedValue);
                }
            }
            return transformedMap;
        }

        public void Add(TKey key, ISet<TVal> value)
        {
            _setValuedMap.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, ISet<TVal>> item)
        {
            _setValuedMap.Add(item);
        }

        //Non-mvp: Test
        public bool AnyValues()
        {
            foreach(ISet<TVal> set in Values)
            {
                if (set.Any())
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _setValuedMap.Clear();
        }

        public bool Contains(KeyValuePair<TKey, ISet<TVal>> item)
        {
            return _setValuedMap.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _setValuedMap.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, ISet<TVal>>[] array, int arrayIndex)
        {
            _setValuedMap.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, ISet<TVal>>> GetEnumerator()
        {
            return _setValuedMap.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _setValuedMap.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, ISet<TVal>> item)
        {
            return _setValuedMap.Remove(item);
        }

        public bool TryGetValue(TKey key, out ISet<TVal> value)
        {
            return _setValuedMap.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _setValuedMap.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (obj is IMultiValueMap<TKey, TVal> otherMap)
            {
                if(otherMap.Count != Count)
                {
                    return false;
                }
                foreach (KeyValuePair<TKey, ISet<TVal>> pair in _setValuedMap)
                {
                    TKey key = pair.Key;
                    if (!otherMap.ContainsKey(key))
                    {
                        return false;
                    }
                    ISet<TVal>? otherSet = otherMap[key];
                    ISet<TVal>? thisSet = pair.Value;
                    bool isOtherNull = otherSet == null;
                    if (thisSet != null)
                    {
                        if (isOtherNull)
                        {
                            return false;
                        }
                        bool setEquals = thisSet.SetEquals(otherSet);
                        if (!setEquals)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!isOtherNull)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = -170788016;
            foreach (KeyValuePair<TKey, ISet<TVal>> pair in _setValuedMap)
            {
                if (pair.Key != null)
                {
                    hashCode ^= pair.Key.GetHashCode();
                }
                foreach (TVal value in pair.Value)
                {
                    if (value != null)
                    {
                        hashCode ^= value.GetHashCode();
                    }
                }
            }
            return hashCode;
        }
    }
}