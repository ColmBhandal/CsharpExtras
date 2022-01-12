using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    class NullaryCurryDictionary<TKey, TVal> : CurryDictionaryBase<TKey, TVal>
    {
        private readonly TVal _singletonImmutableValue;

        public override TVal this[params TKey[] keys]
        {
            get => GetSingletonValue(keys);
        }

        public override NonnegativeInteger Arity => (NonnegativeInteger)0;

        public NullaryCurryDictionary(TVal singletonValue)
        {
            _singletonImmutableValue = singletonValue;
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
            return _singletonImmutableValue;
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

        private TVal GetSingletonValue(TKey[] keyTuple)
        {
            int keyLength = keyTuple.Length;
            if (keyLength != 0)
            {
                throw new ArgumentException($"Nullary dictionary can only accept 0 keys. " +
                    $"Instead, found {keyLength} keys.");
            }
            return _singletonImmutableValue;
        }
    }
}
