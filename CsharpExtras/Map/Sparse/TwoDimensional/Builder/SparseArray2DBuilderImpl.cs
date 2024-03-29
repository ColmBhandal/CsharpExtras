﻿using CsharpExtras.Api;
using CsharpExtras.Map.Sparse.Builder;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse.TwoDimensional.Builder
{
    internal class SparseArray2DBuilderImpl<TVal> : ISparseArray2DBuilder<TVal>
    {
        private readonly ISparseArrayBuilder<TVal> _innerBuilder;
        private readonly ICsharpExtrasApi _api;

        public SparseArray2DBuilderImpl(TVal defaultValue, ICsharpExtrasApi api)
        {
            _innerBuilder = api.NewSparseArrayBuilder((PositiveInteger)2, defaultValue);
            _api = api;
        }

        public ISparseArray2D<TVal> Build()
        {
            ISparseArray<TVal> backingArray = _innerBuilder.Build();
            return new SparseArray2DImpl<TVal>(backingArray, _api);
        }

        public ISparseArray2DBuilder<TVal> WithColumnValidation(Func<int, bool> validator)
        {
            _innerBuilder.WithAxisValidationFunction(validator, (NonnegativeInteger)1);
            return this;
        }

        public ISparseArray2DBuilder<TVal> WithRowValidation(Func<int, bool> validator)
        {
            _innerBuilder.WithAxisValidationFunction(validator, (NonnegativeInteger)0);
            return this;
        }

        public ISparseArray2DBuilder<TVal> WithValue(TVal value, int row, int column)
        {
            _innerBuilder.WithValue(value, row, column);
            return this;
        }
    }
}
