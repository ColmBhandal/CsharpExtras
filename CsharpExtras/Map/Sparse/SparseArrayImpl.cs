using CsharpExtras.Api;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.ValidatedType;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse
{
    internal class SparseArrayImpl<TVal>
    {
        public PositiveInteger Dimension { get; }

        private readonly ICurryDictionary<ValidIndex, TVal> _backingDictionary;
        private readonly ICsharpExtrasApi _api;
        
        private readonly Func<int, int, bool> _validationFunction;

        private ILazyFunctionMap<(int index, int dimension), SparseArrayImpl<TVal>.ValidIndex> _validIndexCache;

        public SparseArrayImpl(int indexBase, PositiveInteger dimension, ICsharpExtrasApi api,
            Func<int, int, bool> validationFunction)
        {
            Dimension = dimension ?? throw new ArgumentNullException(nameof(dimension));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _validationFunction = validationFunction;
            _backingDictionary = api.NewGenericCurryDictionaryWrapper(
                api.NewCurryDictionary<int, TVal>(Dimension), KeyInTransform, KeyOutTransform, v => v, v => v);
            _validIndexCache = api.NewLazyFunctionMap<(int index, int dimension), SparseArrayImpl<TVal>.ValidIndex>
                ((p) => new ValidIndex(p.index, (PositiveInteger)p.dimension, this));
        }

        private int KeyInTransform(SparseArrayImpl<TVal>.ValidIndex index, int dimension) => index;

        private SparseArrayImpl<TVal>.ValidIndex KeyOutTransform(int index, int dimension) => _validIndexCache[(index, dimension)];

        private class ValidIndex : ImmutableValidated<int>
        {
            private readonly PositiveInteger _dimension;
            private readonly SparseArrayImpl<TVal> _array;

            public ValidIndex(int index, PositiveInteger dimension, SparseArrayImpl<TVal> array) : base(index)
            {
                _array = array;
                _dimension = dimension;
            }

            protected override string ValidityConditionTextDescription =>
                "Ensures an index at a given dimension is valid";

            protected override bool IsValid(int t)
            {
                //TODO: Implement using the _validationFunction of the array & also ensure the dimension is leq that of the array.
                return false;
            }
        }
    }
}
