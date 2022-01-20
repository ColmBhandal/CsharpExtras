using CsharpExtras.Compare;
using CsharpExtras.Extensions.Helper.Dictionary;
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
        public abstract IEnumerable<IList<TKey>> KeyTuples { get; }

        public IEnumerable<(IList<TKey>, TVal)> KeyValuePairs =>
            KeyTuples.Select(k => (k, GetValueFromTuple(k)));

        public IEnumerable<TVal> Values => KeyTuples.Select(GetValueFromTuple);

        public abstract NonnegativeInteger Count { get; }

        public abstract TVal this[params TKey[] keys] { get; }

        public abstract IEnumerable<IList<TKey>> KeyTuplePrefixes(NonnegativeInteger arity);

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

        public NonnegativeInteger Remove(params TKey[] prefix)
        {
            return Remove(prefix as IEnumerable<TKey>);
        }

        public abstract NonnegativeInteger Remove(IEnumerable<TKey> prefix);

        public bool Update(TVal value, params TKey[] keys)
        {
            return Update(value, keys as IEnumerable<TKey>);
        }

        public abstract bool Update(TVal value, IEnumerable<TKey> keyTuple);

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

        public IComparisonResult Compare(ICurryDictionary<TKey, TVal> other, Func<TVal, TVal, bool> isEqualValues)
        {
            int otherArity = other.Arity;
            int otherCount = other.Count;
            if (Arity != otherArity || Count != otherCount)
            {
                return new CurryDictionaryComparisonImpl<TKey, TVal>(Arity, otherArity, Count, otherCount, null);
            }
            return IsSubset(other, isEqualValues);
        }

        /// <summary>
        /// This method is delegated to sub-classes to check is this dictionary a subset of the other one
        /// </summary>
        protected abstract IComparisonResult IsSubset
            (ICurryDictionary<TKey, TVal> other, Func<TVal, TVal, bool> isEqualValues);

        public void DoForAllCurriedDictionaries(Action<ICurryDictionary<TKey, TVal>> action, NonnegativeInteger arity)
        {
            Action<IList<TKey>, ICurryDictionary<TKey, TVal>> pairAction = (k, d) => action(d);
            DoForAllPairs(pairAction, arity);
        }

        public void DoForAllPairs(Action<IList<TKey>, ICurryDictionary<TKey, TVal>> action, NonnegativeInteger arity)
        {
            IEnumerable<IList<TKey>> prefixes = KeyTuplePrefixes(arity);
            foreach (IList<TKey> prefix in prefixes)
            {
                ICurryDictionary<TKey, TVal> curriedChild = GetCurriedDictionary(prefix);
                action(prefix, curriedChild);
            }
        }

        public void UpdateKeys(Func<TKey, TKey> keyInjection, NonnegativeInteger arity)
        {
            if(arity > Arity)
            {
                throw new ArgumentException
                    ($"Cannot get key tuple prefixes. Given arity is exceeds Arity of this object: {arity} > {Arity}");
            }
            DoForAllCurriedDictionaries(d => d.UpdateFirstKeyInTuples(keyInjection), arity);
        }

        public abstract void UpdateFirstKeyInTuples(Func<TKey, TKey> keyInjection);
    }
}
