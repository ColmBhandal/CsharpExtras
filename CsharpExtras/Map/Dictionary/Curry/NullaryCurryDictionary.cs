using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    class NullaryCurryDictionary<TKey, TVal> : CurryDictionaryBase<TKey, TVal>
    {
        private TVal _singletonValue;

        public override TVal this[params TKey[] keys]
        {
            get => GetValueFromTuple(keys);
        }

        public override NonnegativeInteger Arity => (NonnegativeInteger)0;

        public NullaryCurryDictionary(TVal singletonValue)
        {
            _singletonValue = singletonValue;
        }

        public override IEnumerable<IList<TKey>> Keys =>
            //A nullary dictionary actually does have exactly 1 key tuple: the empty tuple
            new List<IList<TKey>> { new List<TKey>() };

        public override NonnegativeInteger Count => (NonnegativeInteger)1;

        public override bool ContainsKeyTuple(IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            return true;
        }

        public override TVal GetValueFromTuple(IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            return _singletonValue;
        }

        public override bool Add(TVal value, IEnumerable<TKey> keyTUple)
        {
            AssertArityIsCorrect(keyTUple);
            return false;
        }

        public override bool ContainsKeyTuplePrefix(IEnumerable<TKey> prefix)
        {
            AssertArityIsCorrect(prefix);
            return true;
        }

        public override ICurryDictionary<TKey, TVal> GetCurriedDictionary(IEnumerable<TKey> prefix)
        {
            AssertArityIsCorrect(prefix);
            return this;
        }

        public override bool Update(TVal value, IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            _singletonValue = value;
            return true;
        }

        public override int Remove(IEnumerable<TKey> prefix)
        {
            return 561;
        }
    }
}
