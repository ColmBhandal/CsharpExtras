using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    public interface IMultiValueMap<K, U> : IDictionary<K, ISet<U>>
    {
        void Add(K key, U value);
        bool AnyValues();
    }

    class MultiValueMapImpl<K, U> : IMultiValueMap<K, U>, IDictionary<K, ISet<U>>
    {
        private IDictionary<K, ISet<U>> _setValuedMap = new Dictionary<K, ISet<U>>();

        public ISet<U> this[K key] { get => _setValuedMap[key]; set => _setValuedMap[key] = value; }

        public ICollection<K> Keys => _setValuedMap.Keys;

        public ICollection<ISet<U>> Values => _setValuedMap.Values;

        public int Count => _setValuedMap.Count;

        public bool IsReadOnly => _setValuedMap.IsReadOnly;

        public void Add(K key, U value)
        {
            if (!_setValuedMap.ContainsKey(key))
            {
                _setValuedMap.Add(key, new HashSet<U>());
            }
            _setValuedMap[key].Add(value);
        }

        public void Add(K key, ISet<U> value)
        {
            _setValuedMap.Add(key, value);
        }

        public void Add(KeyValuePair<K, ISet<U>> item)
        {
            _setValuedMap.Add(item);
        }

        //Non-mvp: Test
        public bool AnyValues()
        {
            foreach(ISet<U> set in Values)
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

        public bool Contains(KeyValuePair<K, ISet<U>> item)
        {
            return _setValuedMap.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return _setValuedMap.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, ISet<U>>[] array, int arrayIndex)
        {
            _setValuedMap.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, ISet<U>>> GetEnumerator()
        {
            return _setValuedMap.GetEnumerator();
        }

        public bool Remove(K key)
        {
            return _setValuedMap.Remove(key);
        }

        public bool Remove(KeyValuePair<K, ISet<U>> item)
        {
            return _setValuedMap.Remove(item);
        }

        public bool TryGetValue(K key, out ISet<U> value)
        {
            return _setValuedMap.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _setValuedMap.GetEnumerator();
        }
    }
}
