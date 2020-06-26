using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Dictionary.Collection
{
    class ListValuedDictionaryImpl<TKey, TVal> : CollectionValuedDictionaryBase<TKey, TVal, IList<TVal>>,
        IListValuedDictionary<TKey, TVal>
    {
        protected override bool CollectionEquals(IList<TVal> thisList, IList<TVal> otherList) =>
            thisList.SequenceEqual(otherList);

        protected override IList<TVal> NewCollection() => new List<TVal>();
    }
}