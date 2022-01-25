using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Event.Notify;
using CsharpExtras.Event.Wrapper;
using CsharpExtras.Extensions;
using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    class CurryDictionaryRecursive<TKey, TVal> : CurryDictionaryBase<TKey, TVal>
    {
        private readonly ICsharpExtrasApi _api;

        public event Action<int> AfterCountUpdate
        {
            add => _countNotifier.AfterUpdate += value;
            remove => _countNotifier.AfterUpdate -= value;
        }

        public override TVal this[params TKey[] keys]
        {
            get => GetValueFromTuple(keys);
        }        
        public override NonnegativeInteger Arity { get; }

        /// <summary>
        /// This object encapsulates the direct nested children of this object along enforcing updates to the count when those children change
        /// </summary>
        private readonly IEventObjWrapper<IDictionary<TKey, ICurryDictionary<TKey, TVal>>, int> _currier;

        /// <summary>
        /// This object encapsulates the count and ensures that any updates to the count result in events being triggered
        /// </summary>
        private readonly IUpdateNotifier<NonnegativeInteger, int> _countNotifier;

        public override NonnegativeInteger Count => _countNotifier.Value;

        public CurryDictionaryRecursive(int arity, ICsharpExtrasApi api) : this((PositiveInteger)arity, api) { }

        public CurryDictionaryRecursive(PositiveInteger arity, ICsharpExtrasApi api) : base(api)
        {
            Arity = (NonnegativeInteger)arity;
            _api = api;
            _countNotifier = NewCountNotifier();
            _currier = _api.NewEventObjWrapper<IDictionary<TKey, ICurryDictionary<TKey, TVal>>, int>
                (new Dictionary<TKey, ICurryDictionary<TKey, TVal>>(), _countNotifier.Update);
        }

        public override IEnumerable<IList<TKey>> KeyTuples => KeyTuplePrefixes(Arity);

        public override IEnumerable<IList<TKey>> KeyTuplePrefixes(NonnegativeInteger arity)
            => _currier.Get(c => (0, GetKeyTuplePrefixes(c, arity)));

        public override bool ContainsKeyTuple(IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            return ContainsKeyTuplePrefix(keyTuple);
        }

        public override bool ContainsKeyTuplePrefix(IEnumerable<TKey> prefix)
        {
            if (!prefix.Any())
            {
                return true;
            }
            TKey firstKey = prefix.First();
            if (!CurrierContainsKey(firstKey))
            {
                return false;
            }
            ICurryDictionary<TKey, TVal> curriedChild = GetCurriedChild(firstKey);
            IEnumerable<TKey> tailPrefix = prefix.Skip(1);
            return curriedChild.ContainsKeyTuplePrefix(tailPrefix);
        }

        public override TVal GetValueFromTuple(IEnumerable<TKey> keyTuple)
        {
            Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, TVal> recursor =
                (dict, keys) => dict.GetValueFromTuple(keys);
            return TailRecurse(recursor, keyTuple);
        }

        public override ICurryDictionary<TKey, TVal> GetCurriedDictionary(IEnumerable<TKey> prefix)
        {
            if (!prefix.Any())
            {
                return this;
            }
            TKey firstKey = prefix.First();
            if (!CurrierContainsKey(firstKey))
            {
                throw new ArgumentException($"Cannot curry dictionary with given prefix as key {firstKey} is not found");
            }
            ICurryDictionary<TKey, TVal> curriedChild = GetCurriedChild(firstKey);
            IEnumerable<TKey> tailPrefix = prefix.Skip(1);
            return curriedChild.GetCurriedDictionary(tailPrefix);
        }

        public override bool Update(TVal value, IEnumerable<TKey> keyTuple)
        {
            if (!ContainsKeyTuple(keyTuple))
            {
                return false;
            }
            return TailRecurse((d, k) => d.Update(value, k), keyTuple);
        }

        public override bool Add(TVal value, IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            TKey firstKey = keyTuple.First();
            IEnumerable<TKey> tail = keyTuple.Skip(1);
            if (CurrierContainsKey(firstKey))
            {
                ICurryDictionary<TKey, TVal> curryChild = GetCurriedChild(firstKey);
                return curryChild.Add(value, tail);
            }
            else if (Arity > 1)
            {
                CurryDictionaryRecursive<TKey, TVal> curryChild = new CurryDictionaryRecursive<TKey, TVal>(Arity - 1, _api);
                curryChild.AfterCountUpdate += _countNotifier.Update;
                AddDirectChild(firstKey, curryChild);
                bool isAddSuccessful = curryChild.Add(value, tail);
                return isAddSuccessful;
            }
            else
            {
                ICurryDictionary<TKey, TVal> curryChild = new NullaryCurryDictionary<TKey, TVal>(value, _api);
                AddDirectChild(firstKey, curryChild);
                return true;
            }
        }

        public override NonnegativeInteger Remove(IEnumerable<TKey> prefix)
        {
            if (!prefix.Any())
            {
                return (NonnegativeInteger)0;
            }
            TKey firstKey = prefix.First();
            if (!CurrierContainsKey(firstKey))
            {
                return (NonnegativeInteger)0;
            }
            ICurryDictionary<TKey, TVal> curryChild = GetCurriedChild(firstKey);
            IEnumerable<TKey> tail = prefix.Skip(1);
            if (!tail.Any())
            {
                NonnegativeInteger removeCount = curryChild.Count;
                RemoveDirectChild(firstKey);
                return removeCount;
            }
            else
            {
                NonnegativeInteger removeCount = curryChild.Remove(tail);
                if(curryChild.Count == 0)
                {
                    RemoveDirectChild(firstKey);
                }
                return removeCount;
            }
        }

        protected override IComparisonResult IsSubset(ICurryDictionary<TKey, TVal> other, Func<TVal, TVal, bool> isEqualValues)
        {
            NonnegativeInteger otherArity = other.Arity;
            NonnegativeInteger otherCount = other.Count;
            foreach ((IList<TKey> keyTuple, TVal val) pair in KeyValuePairs)
            {
                if (!other.ContainsKeyTuple(pair.keyTuple))
                {
                    return new CurryDictionaryComparisonImpl<TKey, TVal>(Arity, otherArity, Count, otherCount, pair, null);
                }
                TVal otherValue = other.GetValueFromTuple(pair.keyTuple);
                if (!isEqualValues(pair.val, otherValue))
                {
                    return new CurryDictionaryComparisonImpl<TKey, TVal>(Arity, otherArity, Count, otherCount, pair, otherValue?.ToString());
                }
            }
            return new CurryDictionaryComparisonImpl<TKey, TVal>(Arity, otherArity, Count, otherCount, null, null);
        }

        private void AddDirectChild(TKey key, ICurryDictionary<TKey, TVal> curryChild)
        {
            int count = curryChild.Count;
            _currier.Run(c =>
            {
                c.Add(key, curryChild);
                return count;
            });
        }

        /// <summary>
        /// Updates the count by the given amount
        /// </summary>
        /// <param name="delta">The amount, which can be negative, by which to update the count.</param>
        /// <param name="initialCount">The value of the count before the update happens</param>
        /// <returns>The resultant count, after summing the delta to the initial count</returns>
        /// <exception cref="ArgumentException">Thrown if the updated count goes negative</exception>
        private NonnegativeInteger UpdateCount(NonnegativeInteger initialCount, int delta)
        {
            int newCount = initialCount + delta;
            if(newCount < 0)
            {
                throw new ArgumentException($"Cannot update count of {Count} by delta {delta} as it would result in a negative count");
            }
            return (NonnegativeInteger) (Count+delta);            
        }

        //Assumes keyTuple is in this dictionary
        private TReturn TailRecurse<TReturn>(Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, TReturn> recursor,
            IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            TKey firstKey = keyTuple.First();
            ICurryDictionary<TKey, TVal> curriedChild = GetCurriedChild(firstKey);
            IEnumerable<TKey> tail = keyTuple.Skip(1);
            return recursor(curriedChild, tail);
        }

        private void RemoveDirectChild(TKey key)
        {
            if (!CurrierContainsKey(key))
            {
                return;
            }
            ICurryDictionary<TKey, TVal> curryChild = GetCurriedChild(key);
            int count = curryChild.Count;
            _currier.Run(c =>
            {
                if (c.Remove(key))
                {
                    if(curryChild is CurryDictionaryRecursive<TKey, TVal> recDict)
                    {
                        recDict.AfterCountUpdate -= _countNotifier.Update;
                    }
                    return -count;
                }
                else
                {
                    return 0;
                }
            });
        }

        private IUpdateNotifier<NonnegativeInteger, int> NewCountNotifier()
        {
            IUpdateNotifier<NonnegativeInteger, int> countNotifier =
                _api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateCount);
            return countNotifier;
        }
        
        /// <summary>
        /// Gets all key prefixes of the given arity from the currier object
        /// </summary>
        /// <param name="currier">The dictionary object from which to get the key prefixes</param>
        /// <param name="arity">The arity of the key prefixes. This must be less than or equal to the arity of the currier.</param>
        /// <returns>An enumerable of key prefixes in the given currier matching the given arity</returns>
        private IEnumerable<IList<TKey>> GetKeyTuplePrefixes(IDictionary<TKey, ICurryDictionary<TKey, TVal>> currier,
            NonnegativeInteger arity)
        {          
            if(arity > Arity)
            {
                throw new ArgumentException
                    ($"Cannot get key tuple prefixes. Given arity is exceeds Arity of this object: {arity} > {Arity}");
            }
            return GetKeyTuplePrefixesUnsafe(currier, arity);
        }

        //Does not do arity check
        private IEnumerable<IList<TKey>> GetKeyTuplePrefixesUnsafe(IDictionary<TKey, ICurryDictionary<TKey, TVal>> currier,
            NonnegativeInteger arity)
        {
            if (arity == (NonnegativeInteger)0)
            {
                yield return new List<TKey> { };
            }
            else
            {
                NonnegativeInteger subArity = (NonnegativeInteger)(arity - 1);
                foreach (KeyValuePair<TKey, ICurryDictionary<TKey, TVal>> pair in currier)
                {
                    TKey key = pair.Key;
                    ICurryDictionary<TKey, TVal> dict = pair.Value;
                    if (dict.Count == 0)
                    {
                        throw new InvalidOperationException("Unexpectedly found a nested dictionary with no elements");
                    }
                    IEnumerable<IList<TKey>> childKeyset = dict.KeyTuplePrefixes(subArity);
                    foreach (IList<TKey> tuple in childKeyset)
                    {
                        tuple.Insert(0, key);
                        yield return tuple;
                    }
                }
            }
        }

        private ICurryDictionary<TKey, TVal> GetCurriedChild(TKey firstKey)
        {
            return _currier.Get(c => (0, c[firstKey]));
        }

        private bool CurrierContainsKey(TKey firstKey)
        {
            return _currier.Get(c => (0, c.ContainsKey(firstKey)));
        }

        public override void UpdateFirstKeyInTuples(Func<TKey, TKey> keyInjection)
        {
            _currier.Run(d => UpdateKeys(d, keyInjection));
        }

        /// <returns>A count difference of zero is returned, as we assume that the key update function on dictionaries does not affect count</returns>
        int UpdateKeys(IDictionary<TKey, ICurryDictionary<TKey, TVal>> currier, Func<TKey, TKey> keyInjection)
        {
            currier.UpdateKeys(keyInjection);
            return 0;
        }
    }
}
