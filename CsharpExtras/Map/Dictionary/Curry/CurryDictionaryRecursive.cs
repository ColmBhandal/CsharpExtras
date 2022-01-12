using CsharpExtras.Extensions;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    class CurryDictionaryRecursive<TKey, TVal> : CurryDictionaryBase<TKey, TVal>
    {
        public override TVal this[params TKey[] keys]
        {
            get => GetValueFromTuple(keys);
        }        
        public override NonnegativeInteger Arity { get; }

        //NB: We must not expose this to the outside world - otherwise we risk an inconsistent Count 
        private readonly IDictionary<TKey, ICurryDictionary<TKey, TVal>> _currier;

        public CurryDictionaryRecursive(int arity) : this((PositiveInteger)arity) { }

        //TODO: Implement this properly
        public override int Count => 104;

        public CurryDictionaryRecursive(PositiveInteger arity)
        {
            Arity = (NonnegativeInteger)arity;
            _currier = new Dictionary<TKey, ICurryDictionary<TKey, TVal>>();
        }
        public override IEnumerable<IList<TKey>> Keys
        {
            get
            {
                foreach (KeyValuePair<TKey, ICurryDictionary<TKey, TVal>> pair in _currier)
                {
                    TKey key = pair.Key;
                    ICurryDictionary<TKey, TVal> dict = pair.Value;
                    IEnumerable<IList<TKey>> childKeyset = dict.Keys;
                    foreach (IList<TKey> tuple in childKeyset)
                    {
                        tuple.Insert(0, key);
                        yield return tuple;
                    }
                }
            }
        }

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
            if (!_currier.ContainsKey(firstKey))
            {
                return false;
            }
            ICurryDictionary<TKey, TVal> curriedChild = _currier[firstKey];
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
            if (!_currier.ContainsKey(firstKey))
            {
                throw new ArgumentException($"Cannot curry dictionary with given prefix as key {firstKey} is not found");
            }
            ICurryDictionary<TKey, TVal> curriedChild = _currier[firstKey];
            IEnumerable<TKey> tailPrefix = prefix.Skip(1);
            return curriedChild.GetCurriedDictionary(tailPrefix);
        }

        public override bool Add(TVal value, IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            TKey firstKey = keyTuple.First();
            IEnumerable<TKey> tail = keyTuple.Skip(1);
            if (_currier.ContainsKey(firstKey))
            {
                ICurryDictionary<TKey, TVal> curryChild = _currier[firstKey];
                return curryChild.Add(value, tail);
            }
            else if(Arity > 1)
            {
                ICurryDictionary<TKey, TVal> curryChild = new CurryDictionaryRecursive<TKey, TVal>(Arity - 1);
                bool isAddSuccessful = curryChild.Add(value, tail);
                _currier.Add(firstKey, curryChild);
                return isAddSuccessful;
            }
            else
            {
                ICurryDictionary<TKey, TVal> curryChild = new NullaryCurryDictionary<TKey, TVal>(value);
                _currier.Add(firstKey, curryChild);
                return true;
            }
        }

        //Assumes keyTuple is in this dictionary
        private TReturn TailRecurse<TReturn>(Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, TReturn> recursor,
            IEnumerable<TKey> keyTuple)
        {
            AssertArityIsCorrect(keyTuple);
            TKey firstKey = keyTuple.First();
            ICurryDictionary<TKey, TVal> curriedChild = _currier[firstKey];
            IEnumerable<TKey> tail = keyTuple.Skip(1);
            return recursor(curriedChild, tail);
        }
    }
}
