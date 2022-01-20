using CsharpExtras.Api;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse.Builder
{
    internal class SparseArrayBuilderImpl<TVal> : ISparseArrayBuilder<TVal>
    {
        public TVal DefaultValue { get; }
        public PositiveInteger Dimension { get; }
        private readonly ICsharpExtrasApi _api;

        public SparseArrayBuilderImpl(TVal defaultValue, PositiveInteger dimension, ICsharpExtrasApi api)
        {
            DefaultValue = defaultValue;
            Dimension = dimension ?? throw new ArgumentNullException(nameof(dimension));
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public ISparseArray<TVal> Build()
        {
            return new SparseArrayImpl<TVal>(Dimension, _api, (i, j) => true, DefaultValue);
        }

        public ISparseArrayBuilder<TVal> WithValidationFunction(Func<int, bool> indexValidationFunction, NonnegativeInteger axisIndex)
        {
            return this;
        }
        
        public ISparseArrayBuilder<TVal> WithValue(TVal value, params int[] coordinates)
        {
            return this;
        }
    }
}
