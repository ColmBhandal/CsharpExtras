using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Dictionary.Collection
{
    class SetDictionaryImpl<TKey, TVal>
        : CollectionDictionaryBase<TKey, TVal, ISet<TVal>>, ISetDictionary<TKey, TVal>
    {
        protected override bool CollectionEquals(ISet<TVal> thisSet, ISet<TVal> otherSet)
        {
            return thisSet.SetEquals(otherSet);
        }

        protected override ISet<TVal> NewCollection()
        {
            return new HashSet<TVal>();
        }

        public ISetDictionary<TKey, TOther> TransformValues<TOther>
            (Func<TVal, TOther> transformer)
        {
            return TransformValues<TOther, ISet<TOther>, ISetDictionary<TKey, TOther>>(transformer,
                () => new SetDictionaryImpl<TKey, TOther>());
        }
    }
}
