using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Dictionary.Collection
{
    abstract class CollectionValuedDictionaryBase<TKey, TVal, TColl>
        : ICollectionValuedDictionary<TKey, TVal, TColl>, IDictionary<TKey, TColl>
        where TColl : class, ICollection<TVal>
    {

        private IDictionary<TKey, TColl>? _collectionValuedDictionary;
        protected IDictionary<TKey, TColl> CollectionValuedDictionary => _collectionValuedDictionary ??
            (_collectionValuedDictionary = new Dictionary<TKey, TColl>());

        public ICollection<TKey> Keys => CollectionValuedDictionary.Keys;

        public ICollection<TColl> Values => CollectionValuedDictionary.Values;

        public int Count => CollectionValuedDictionary.Count;

        public bool IsReadOnly => CollectionValuedDictionary.IsReadOnly;

        public TColl this[TKey key] { get => CollectionValuedDictionary[key]; set => CollectionValuedDictionary[key] = value; }

        protected abstract TColl NewCollection();

        protected abstract bool CollectionEquals(TColl thisColl, TColl otherColl);

        public void Add(TKey key, TVal value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("Cannot add with a null key");
            }
            if (!CollectionValuedDictionary.ContainsKey(key))
            {
                CollectionValuedDictionary.Add(key, NewCollection());
            }
            CollectionValuedDictionary[key].Add(value);
        }

        public TDict TransformValues<TOther, TOtherColl, TDict>(Func<TVal, TOther> transformer,
            Func<TDict> emptyDictionaryGenerator)
            where TDict : ICollectionValuedDictionary<TKey, TOther, TOtherColl>
            where TOtherColl : ICollection<TOther>
        {
            TDict transformedMap = emptyDictionaryGenerator();
            foreach (TKey key in Keys)
            {
                TColl thisColl = this[key];
                IEnumerable<TOther>? transformedValues = thisColl.Select(transformer);
                foreach (TOther transformedValue in transformedValues)
                {
                    transformedMap.Add(key, transformedValue);
                }
            }
            return transformedMap;
        }

        //Non-mvp: Test
        public bool AnyValues()
        {
            foreach (TColl collection in Values)
            {
                if (collection != null && collection.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool DictEquals(ICollectionValuedDictionary<TKey, TVal, TColl> otherDict)
        {
            if (otherDict.Count != Count)
            {
                return false;
            }
            foreach (KeyValuePair<TKey, TColl> pair in CollectionValuedDictionary)
            {
                TKey key = pair.Key;
                if (!otherDict.ContainsKey(key))
                {
                    return false;
                }
                TColl? otherColl = otherDict[key];
                TColl? thisColl = pair.Value;
                if (thisColl != null)
                {
                    if (otherColl == null)
                    {
                        return false;
                    }
                    bool setEquals = CollectionEquals(thisColl, otherColl);
                    if (!setEquals)
                    {
                        return false;
                    }
                }
                else
                {
                    if (otherColl != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Add(TKey key, TColl value)
        {
            CollectionValuedDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return CollectionValuedDictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return CollectionValuedDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TColl value)
        {
            return CollectionValuedDictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TColl> item)
        {
            CollectionValuedDictionary.Add(item);
        }

        public void Clear()
        {
            CollectionValuedDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TColl> item)
        {
            return CollectionValuedDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TColl>[] array, int arrayIndex)
        {
            CollectionValuedDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TColl> item)
        {
            return CollectionValuedDictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TColl>> GetEnumerator()
        {
            return CollectionValuedDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)CollectionValuedDictionary).GetEnumerator();
        }
    }
}
