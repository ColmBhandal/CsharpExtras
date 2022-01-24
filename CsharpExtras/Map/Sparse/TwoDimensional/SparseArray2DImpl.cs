using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Map.Sparse.Compare;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse.TwoDimensional
{
    internal class SparseArray2DImpl<TVal> : ISparseArray2D<TVal>
    {
        private readonly ICsharpExtrasApi _api;
        private readonly ISparseArray<TVal> _backingArray;

        public SparseArray2DImpl(ICsharpExtrasApi api, ISparseArray<TVal> backingArray)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _backingArray = backingArray ?? throw new ArgumentNullException(nameof(backingArray));
        }

        public TVal this[int row, int column]
        {
            get => _backingArray.DefaultValue;
            set { var x = value; }
        }

        public IComparisonResult CompareUsedValues(ISparseArray2D<TVal> other, Func<TVal, TVal, bool> comparitor)
        {
            return new SparseArray2DComparisonImpl<TVal>(1, 2, null);
        }

        public void SetArea(TVal[,] area, int rowIndex, int colIndex)
        {

        }

        public TVal[,] GetArea(int startRow, int startCol, int endRow, int endCol)
        {
            //TODO: Implement
            return new TVal[1,1];
        }
    }
}
