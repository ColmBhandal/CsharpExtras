using CsharpExtras.ValidatedType.Numeric.Integer;
using System;

namespace CsharpExtras.Map.Sparse.Builder
{
    internal interface ISparseArrayBuilder<TVal>
    {
        TVal DefaultValue { get; }
        PositiveInteger Dimension { get; }

        ISparseArray<TVal> Builder();
        ISparseArrayBuilder<TVal> WithValidationFunction(Func<int, bool> indexValidationFunction, int axisIndex);
        ISparseArrayBuilder<TVal> WithValue(TVal value, params int[] coordinates);
    }
}