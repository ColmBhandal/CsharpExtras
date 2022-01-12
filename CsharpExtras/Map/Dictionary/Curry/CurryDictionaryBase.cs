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
        public abstract IEnumerable<IList<TKey>> Keys { get; }

        public IEnumerable<(IList<TKey>, TVal)> KeyValuePairs =>
            Keys.Select(k => (k, GetValueFromTuple(k)));

        public IEnumerable<TVal> Values => Keys.Select(GetValueFromTuple);

        public abstract int Count { get; }

        public abstract TVal this[params TKey[] keys] { get;}

        public bool ContainsKeyTuple(params TKey[] keys)
        {
            return ContainsKeyTuple(keys as IEnumerable<TKey>);
        }

        public abstract bool ContainsKeyTuple(IEnumerable<TKey> keys);
        public bool ContainsKeyTuplePrefix(params TKey[] prefix)
        {
            return ContainsKeyTuplePrefix(prefix as IEnumerable<TKey>);
        }
        public abstract bool ContainsKeyTuplePrefix(IEnumerable<TKey> prefix);

        public TVal GetValueFromTuple(params TKey[] keys)
        {
            return GetValueFromTuple(keys as IEnumerable<TKey>);
        }

        public abstract TVal GetValueFromTuple(IEnumerable<TKey> keys);
        public ICurryDictionary<TKey, TVal> GetCurriedDictionary(params TKey[] prefix)
        {
            return GetCurriedDictionary(prefix as IEnumerable<TKey>);
        }
        public abstract ICurryDictionary<TKey, TVal> GetCurriedDictionary(IEnumerable<TKey> prefix);
        public bool Add(TVal value, params TKey[] keys)
        {
            return Add(value, keys as IEnumerable<TKey>);
        }
        public abstract bool Add(TVal value, IEnumerable<TKey> keys);

        protected void AssertArityIsCorrect(IEnumerable<TKey> keyTuple)
        {
            int keyLength = keyTuple.Count();
            if (keyLength != Arity)
            {
                int keyCount = keyTuple.Count();
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
