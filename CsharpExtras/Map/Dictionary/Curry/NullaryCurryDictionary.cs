using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    class NullaryCurryDictionary<TKey, TVal> : CurryDictionaryBase<TKey, TVal>
    {
        private readonly TVal _singletonImmutableValue;

        public NullaryCurryDictionary(TVal singletonValue)
        {
            _singletonImmutableValue = singletonValue;
        }

        public override TVal this[params TKey[] keys]
        {
            get => GetSingletonValue(keys);
            set => throw new NotImplementedException();
        }

        public override NonnegativeInteger Arity => (NonnegativeInteger) 0;

        private TVal GetSingletonValue(TKey[] keys)
        {
            int keyLength = keys.Length;
            if (keyLength != 0)
            {
                throw new ArgumentException($"Nullary dictionary can only accept 0 keys. " +
                    $"Instead, found {keyLength} keys.");
            }
            return _singletonImmutableValue;
        }

        public override bool ContainsKeyTuple(IEnumerable<TKey> keys)
        {
            AssertArityIsCorrect(keys);
            return true;
        }

        public override TVal GetValueFromTuple(IEnumerable<TKey> keys)
        {
            AssertArityIsCorrect(keys);
            return _singletonImmutableValue;
        }

        public override bool Add(TVal value, IEnumerable<TKey> keys)
        {
            AssertArityIsCorrect(keys);
            return false;
        }
    }
}
