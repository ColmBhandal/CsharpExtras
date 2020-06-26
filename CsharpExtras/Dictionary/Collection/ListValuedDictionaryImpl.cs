using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CsharpExtras.Dictionary.Collection
{
    class ListValuedDictionaryImpl<TKey, TVal> : CollectionValuedDictionaryBase<TKey, TVal, IList<TVal>>,
        IListValuedDictionary<TKey, TVal>
    {
        public void InsertAtIndex(TKey key, TVal val, NonnegativeInteger index)
        {
            if (key == null)
            {
                throw new ArgumentNullException("Cannot insert with a null key");
            }
            if (!CollectionValuedDictionary.ContainsKey(key))
            {
                CollectionValuedDictionary.Add(key, NewCollection());
            }
            IList<TVal>? list = CollectionValuedDictionary[key];
            if(list != null)
            {
                int listCount = list.Count;
                if (index > listCount)
                {
                    throw new ArgumentException(string.Format(
                        "Insertion index {0} cannot exceed list count {1}", index, listCount));
                }
                list.Insert(index, val);
            }
            else
            {
                throw new InvalidOperationException(string.Format(
                    "List at index {0} is null", key.ToString()));
            }
        }

        protected override bool CollectionEquals(IList<TVal> thisList, IList<TVal> otherList) =>
            thisList.SequenceEqual(otherList);

        protected override IList<TVal> NewCollection() => new List<TVal>();
    }
}