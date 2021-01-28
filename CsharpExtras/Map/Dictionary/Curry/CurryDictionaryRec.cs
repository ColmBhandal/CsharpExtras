using CsharpExtras.Extensions;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    class CurryDictionaryRec<TKey, TVal> : CurryDictionaryBase<TKey, TVal>, ICurryDictionaryRec<TKey, TVal>
    {
        public override TVal this[params TKey[] keys]
        {
            get => GetValueFromTuple(keys);
            set => throw new NotImplementedException();
        }        

        public ICurryDictionary<TKey, TVal> CurriedChild(TKey key) =>
            _currier[key];

        public override NonnegativeInteger Arity { get; }

        private readonly IDictionary<TKey, ICurryDictionary<TKey, TVal>> _currier;

        public CurryDictionaryRec(int arity) : this((PositiveInteger)arity) { }


        public CurryDictionaryRec(PositiveInteger arity)
        {
            Arity = (NonnegativeInteger)arity;
            _currier = new Dictionary<TKey, ICurryDictionary<TKey, TVal>>();
        }

        public override bool ContainsKeyTuple(IEnumerable<TKey> keys)
        {
            Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, bool> recursor =
                (dict, keys) => dict.ContainsKeyTuple(keys);
            return TailRecurse(recursor, keys);
        }
        public override TVal GetValueFromTuple(IEnumerable<TKey> keys)
        {
            Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, TVal> recursor =
                (dict, keys) => dict.GetValueFromTuple(keys);
            return TailRecurse(recursor, keys);
        }

        public override bool Add(TVal value, IEnumerable<TKey> keys)
        {
            Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, bool> recursor =
                (dict, keys) => dict.Add(value, keys);
            return TailRecurse(recursor, keys);
        }

        private TReturn TailRecurse<TReturn>(Func<ICurryDictionary<TKey, TVal>, IEnumerable<TKey>, TReturn> recursor,
            IEnumerable<TKey> keys)
        {
            AssertArityIsCorrect(keys);
            TKey firstKey = keys.First();
            ICurryDictionary<TKey, TVal> curriedChild = CurriedChild(firstKey);
            IEnumerable<TKey> tail = keys.Skip(1);
            return recursor(curriedChild, tail);
        }
    }
}
