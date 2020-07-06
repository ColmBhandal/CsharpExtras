using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Collection
{
    class SetValuedDictionaryImpl<TKey, TVal>
        : CollectionValuedDictionaryBase<TKey, TVal, ISet<TVal>>, ISetValuedDictionary<TKey, TVal>
    {
        protected override bool CollectionEquals(ISet<TVal> thisSet, ISet<TVal> otherSet)
        {
            return thisSet.SetEquals(otherSet);
        }

        protected override ISet<TVal> NewCollection()
        {
            return new HashSet<TVal>();
        }

        public ISetValuedDictionary<TKey, TOther> TransformValues<TOther>
            (Func<TVal, TOther> transformer)
        {
            return TransformValues<TOther, ISet<TOther>, ISetValuedDictionary<TKey, TOther>>(transformer,
                () => new SetValuedDictionaryImpl<TKey, TOther>());
        }
    }
}
