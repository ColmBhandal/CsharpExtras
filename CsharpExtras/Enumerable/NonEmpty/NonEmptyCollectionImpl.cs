using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Enumerable.NonEmpty
{
    public class NonEmptyCollectionImpl<T> : INonEmptyCollection<T>
    {
        private readonly ICollection<T> _collection;

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        public NonEmptyCollectionImpl(T value)
        {
            if (value == null)
            {
                throw new ArgumentException("Must provide at least one element for non-empty collection");
            }
            _collection = new List<T>
            {
                value
            };
        }

        public NonEmptyCollectionImpl(ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
            {
                throw new ArgumentException("Must provide at least one element for non-empty collection");
            }
            _collection = collection;
        }

        public void Add(T item)
        {
            _collection.Add(item);
        }

        public void Clear()
        {
            //non-mvp: Rethink design, should this fail silently instead? Or transform into a regular collection?
            throw new NotSupportedException("Cannot clear an NonEmptyCollection");
        }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public bool Remove(T item)
        {
            if (_collection.Count <= 1)
            {
                throw new NotSupportedException("Cannot remove last element in an NonEmptyCollection");
            }
            return _collection.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}
