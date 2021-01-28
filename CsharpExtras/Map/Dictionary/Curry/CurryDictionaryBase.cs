using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    abstract class CurryDictionaryBase<TKey, TVal> : ICurryDictionary<TKey, TVal>
    {
        public abstract NonnegativeInteger Arity { get; }

        public abstract TVal this[params TKey[] keys] { get; set; }

        public bool ContainsKeyTuple(params TKey[] keys)
        {
            return ContainsKeyTuple(keys as IEnumerable<TKey>);
        }

        public abstract bool ContainsKeyTuple(IEnumerable<TKey> keys);

        public TVal GetValueFromTuple(params TKey[] keys)
        {
            return GetValueFromTuple(keys as IEnumerable<TKey>);
        }

        public abstract TVal GetValueFromTuple(IEnumerable<TKey> keys);
        public bool Add(TVal value, params TKey[] keys)
        {
            return Add(value, keys as IEnumerable<TKey>);
        }
        public abstract bool Add(TVal value, IEnumerable<TKey> keys);

        protected void AssertArityIsCorrect(IEnumerable<TKey> keys)
        {
            int keyLength = keys.Count();
            if (keyLength != Arity)
            {
                int keyCount = keys.Count();
                if (keyCount != Arity)
                {
                    throw new ArgumentException(
                        $"Tuple of length {keyCount} is of incorrect arity. " +
                        $"This dictionary expects a tuple of arity = {Arity}.");
                }
            }
        }
    }
}
