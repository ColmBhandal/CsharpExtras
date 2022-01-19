using CsharpExtras.Extensions;
using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry.Wrapper
{
    internal class GenericCurryDictionaryWrapperImpl<TKeyInner, TKeyOuter, TValInner, TValOuter>
        : ICurryDictionary<TKeyOuter, TValOuter>
    {
        ICurryDictionary<TKeyInner, TValInner> _backingDictionary;
        Func<TKeyOuter, TKeyInner> _keyInTransform;
        Func<TKeyInner, TKeyOuter> _keyOutTransform;
        Func<TValOuter, TValInner> _valInTransform;
        Func<TValInner, TValOuter> _valOutTransform;

        public GenericCurryDictionaryWrapperImpl(ICurryDictionary<TKeyInner, TValInner> backingDictionary,
            Func<TKeyOuter, TKeyInner> keyInTransform,
            Func<TKeyInner, TKeyOuter> keyOutTransform,
            Func<TValOuter, TValInner> valInTransform,
            Func<TValInner, TValOuter> valOutTransform)
        {
            _backingDictionary = backingDictionary ?? throw new ArgumentNullException(nameof(backingDictionary));
            _keyOutTransform = keyOutTransform ?? throw new ArgumentNullException(nameof(keyOutTransform));
            _keyInTransform = keyInTransform ?? throw new ArgumentNullException(nameof(keyInTransform));
            _valOutTransform = valOutTransform ?? throw new ArgumentNullException(nameof(valOutTransform));
            _valInTransform = valInTransform ?? throw new ArgumentNullException(nameof(valInTransform));
        }

        public TValOuter this[params TKeyOuter[] keyTuple] => _valOutTransform(_backingDictionary[keyTuple.Map(_keyInTransform)]);

        public NonnegativeInteger Arity => _backingDictionary.Arity;

        public IEnumerable<IList<TKeyOuter>> KeyTuples => _backingDictionary.KeyTuples.Select(l => l.Map(_keyOutTransform));

        public IEnumerable<(IList<TKeyOuter>, TValOuter)> KeyValuePairs => _backingDictionary.KeyValuePairs.Select
            (p => (p.Item1.Map(_keyOutTransform), _valOutTransform(p.Item2)));

        public IEnumerable<TValOuter> Values => _backingDictionary.Values.Select(_valOutTransform);

        public NonnegativeInteger Count => _backingDictionary.Count;

        public bool Add(TValOuter value, params TKeyOuter[] keyTuple)
        {
            return _backingDictionary.Add(_valInTransform(value), keyTuple.Map(_keyInTransform));
        }

        public bool Add(TValOuter value, IEnumerable<TKeyOuter> keyTuple)
        {
            return _backingDictionary.Add(_valInTransform(value), keyTuple.Select(_keyInTransform));
        }

        public IDictionaryComparison Compare(ICurryDictionary<TKeyOuter, TValOuter> other,
            Func<TValOuter, TValOuter, bool> isEqualValues)
        {
            ICurryDictionary<TKeyInner, TValInner> otherWrapped = ReverseWrap(other);
            return _backingDictionary.Compare(otherWrapped, (v1, v2) => isEqualValues(_valOutTransform(v1), _valOutTransform(v2)));
        }

        public bool ContainsKeyTuple(params TKeyOuter[] keyTuple)
        {
            return _backingDictionary.ContainsKeyTuple(keyTuple.Map(_keyInTransform));
        }

        public bool ContainsKeyTuple(IEnumerable<TKeyOuter> keyTuple)
        {
            return _backingDictionary.ContainsKeyTuple(keyTuple.Select(_keyInTransform));
        }

        public bool ContainsKeyTuplePrefix(params TKeyOuter[] prefix)
        {
            return _backingDictionary.ContainsKeyTuplePrefix(prefix.Map(_keyInTransform));
        }

        public bool ContainsKeyTuplePrefix(IEnumerable<TKeyOuter> prefix)
        {
            return _backingDictionary.ContainsKeyTuplePrefix(prefix.Select(_keyInTransform));
        }

        public void DoForAllCurriedDictionaries(Action<ICurryDictionary<TKeyOuter, TValOuter>> action, NonnegativeInteger arity)
        {
            Action<ICurryDictionary<TKeyInner, TValInner>> wrappedAction = d => action(Wrap(d));
            _backingDictionary.DoForAllCurriedDictionaries(wrappedAction, arity);
        }

        public void DoForAllPairs(Action<IList<TKeyOuter>, ICurryDictionary<TKeyOuter, TValOuter>> action, NonnegativeInteger arity)
        {
            Action<IList<TKeyInner>, ICurryDictionary<TKeyInner, TValInner>> wrappedAction = 
                (k, d) => action(k.Map(_keyOutTransform), Wrap(d));
            _backingDictionary.DoForAllPairs(wrappedAction, arity);
        }

        public ICurryDictionary<TKeyOuter, TValOuter> GetCurriedDictionary(params TKeyOuter[] prefix)
        {
            return Wrap(_backingDictionary.GetCurriedDictionary(prefix.Map(_keyInTransform)));
        }

        public ICurryDictionary<TKeyOuter, TValOuter> GetCurriedDictionary(IEnumerable<TKeyOuter> prefix)
        {
            return Wrap(_backingDictionary.GetCurriedDictionary(prefix.Select(_keyInTransform)));
        }

        public TValOuter GetValueFromTuple(params TKeyOuter[] keyTuple)
        {
            return _valOutTransform(_backingDictionary.GetValueFromTuple(keyTuple.Map(_keyInTransform)));
        }

        public TValOuter GetValueFromTuple(IEnumerable<TKeyOuter> keyTuple)
        {
            return _valOutTransform(_backingDictionary.GetValueFromTuple(keyTuple.Select(_keyInTransform)));
        }

        public IEnumerable<IList<TKeyOuter>> KeyTuplePrefixes(NonnegativeInteger arity)
        {
            return _backingDictionary.KeyTuplePrefixes(arity).Select(l => l.Map(_keyOutTransform));
        }

        public NonnegativeInteger Remove(IEnumerable<TKeyOuter> prefix)
        {
            return _backingDictionary.Remove(prefix.Select(_keyInTransform));
        }

        public NonnegativeInteger Remove(params TKeyOuter[] prefix)
        {
            return _backingDictionary.Remove(prefix.Map(_keyInTransform));
        }

        public bool Update(TValOuter value, params TKeyOuter[] keyTuple)
        {
            return _backingDictionary.Update(_valInTransform(value), keyTuple.Map(_keyInTransform));
        }

        public bool Update(TValOuter value, IEnumerable<TKeyOuter> keyTuple)
        {
            return _backingDictionary.Update(_valInTransform(value), keyTuple.Select(_keyInTransform));
        }

        public void UpdateFirstKeyInTuples(Func<TKeyOuter, TKeyOuter> keyInjection)
        {
            _backingDictionary.UpdateFirstKeyInTuples(ReverseWrapKeyEndoFunction(keyInjection));
        }

        public void UpdateKeys(Func<TKeyOuter, TKeyOuter> keyInjection, NonnegativeInteger prefixArity)
        {
            _backingDictionary.UpdateKeys(ReverseWrapKeyEndoFunction(keyInjection), prefixArity);
        }

        private GenericCurryDictionaryWrapperImpl<TKeyInner, TKeyOuter, TValInner, TValOuter>
            Wrap(ICurryDictionary<TKeyInner, TValInner> dict)
        {
            return new GenericCurryDictionaryWrapperImpl<TKeyInner, TKeyOuter, TValInner, TValOuter>
                (dict, _keyInTransform, _keyOutTransform, _valInTransform, _valOutTransform);
        }

        private GenericCurryDictionaryWrapperImpl<TKeyOuter, TKeyInner, TValOuter, TValInner>
            ReverseWrap(ICurryDictionary<TKeyOuter, TValOuter> other)
        {
            return new GenericCurryDictionaryWrapperImpl<TKeyOuter, TKeyInner, TValOuter, TValInner>
                (other, _keyOutTransform, _keyInTransform, _valOutTransform, _valInTransform);
        }

        private Func<TKeyInner, TKeyInner> ReverseWrapKeyEndoFunction(Func<TKeyOuter, TKeyOuter> keyInjection)
        {
            return k => _keyInTransform(keyInjection(_keyOutTransform(k)));
        }
    }
}
