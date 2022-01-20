using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.ValidatedType;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse
{
    internal class SparseArrayImpl<TVal> : ISparseArray<TVal>
    {
        public PositiveInteger Dimension { get; }
        public TVal DefaultValue { get; }

        //TODO: Implement
        public NonnegativeInteger UsedValuesCount => (NonnegativeInteger) 72;

        //TODO: Implement
        public TVal this[params int[] coordinates]
        {
            get => DefaultValue;
            set { return; }
        }

        private readonly ICurryDictionary<ValidIndex, TVal> _backingDictionary;
        private readonly ICsharpExtrasApi _api;

        private readonly Func<NonnegativeInteger, int, bool> _validationFunction;

        private ILazyFunctionMap<(int index, int dimension), SparseArrayImpl<TVal>.ValidIndex> _validIndexCache;

        /// <summary>
        /// Constructs a new sparse array
        /// </summary>
        /// <param name="dimension">The dimension i.e. number of coordinates per mapping in the sparse array</param>        
        /// <param name="validationFunction">Given an axis index and an index along that axis, returns whether the given index is valid.
        /// Note: this function only needs to handle axis indices from 0 up to the dimension of this array,
        /// even though that parameter is typed for all positive integers</param>
        /// <param name="defaultValue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SparseArrayImpl(PositiveInteger dimension, ICsharpExtrasApi api,
            Func<NonnegativeInteger, int, bool> validationFunction, TVal defaultValue)
        {
            Dimension = dimension ?? throw new ArgumentNullException(nameof(dimension));
            DefaultValue = defaultValue;
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _validationFunction = validationFunction;
            _backingDictionary = api.NewGenericCurryDictionaryWrapper(
                api.NewCurryDictionary<int, TVal>(Dimension), KeyInTransform, KeyOutTransform, v => v, v => v);
            _validIndexCache = api.NewLazyFunctionMap<(int index, int axisIndex), SparseArrayImpl<TVal>.ValidIndex>
                ((p) => new ValidIndex(p.index, (PositiveInteger)p.axisIndex, this));
        }

        public IComparisonResult CompareUsedValues(ISparseArray<TVal> other, Func<TVal, TVal, bool> valueComparer)
        {
            //TODO: Implement properly (this is just a stub)
            return new CurryDictionaryComparisonImpl<int, int>(1, 1, 1, 1, null);
        }

        private int KeyInTransform(SparseArrayImpl<TVal>.ValidIndex index, int axisIndex) => index;

        private SparseArrayImpl<TVal>.ValidIndex KeyOutTransform(int index, int axisIndex) => _validIndexCache[(index, axisIndex)];

        private class ValidIndex : ImmutableValidated<int>
        {
            private readonly PositiveInteger _axisIndexIndex;
            private readonly SparseArrayImpl<TVal> _array;

            public ValidIndex(int index, PositiveInteger axisIndex, SparseArrayImpl<TVal> array) : base(index)
            {
                _array = array;
                _axisIndexIndex = axisIndex;
            }

            protected override string ValidityConditionTextDescription =>
                "Ensures an index at a given dimension is valid";

            protected override bool IsValid(int t)
            {
                //TODO: Implement using the _validationFunction of the array & also ensure the dimension is less than that of the array.
                return false;
            }
        }
    }
}
