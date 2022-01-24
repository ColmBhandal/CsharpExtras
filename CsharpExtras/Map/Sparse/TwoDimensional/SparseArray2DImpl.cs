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
        public ISparseArray<TVal> BackingArray { get; }

        public SparseArray2DImpl(ICsharpExtrasApi api, ISparseArray<TVal> backingArray)
        {
            ValidateBackingArray(backingArray);
            _api = api ?? throw new ArgumentNullException(nameof(api));
            BackingArray = backingArray ?? throw new ArgumentNullException(nameof(backingArray));
        }

        private void ValidateBackingArray(ISparseArray<TVal> backingArray)
        {
            PositiveInteger dimension = backingArray.Dimension;
            if(dimension != 2)
            {
                throw new ArgumentException($"Dimension of backing array must be exactly 2. Found: {(int) dimension}");
            }
        }

        public TVal this[int row, int column]
        {
            get => BackingArray[row, column];
            set { BackingArray[row, column] = value; }
        }

        public IComparisonResult CompareUsedValues(ISparseArray2D<TVal> other, Func<TVal, TVal, bool> comparitor)
        {
            return BackingArray.CompareUsedValues(other.BackingArray, comparitor);
        }

        public void SetArea(TVal[,] area, int leftmostRow, int topMostColumn)
        {
            ValidateArea(area, leftmostRow, topMostColumn);
            for (int i = 0; i < area.GetLength(0); i++)
            {
                for (int j = 0; j < area.GetLength(1); j++)
                {
                    int rowAbs = leftmostRow + i;
                    int colAbs = topMostColumn + j;
                    BackingArray[rowAbs, colAbs] = area[i, j];
                }
            }
        }

        private void ValidateArea(TVal[,] area, int leftmostRow, int topMostColumn)
        {
            for (int i = 0; i < area.GetLength(0); i++)
            {
                int rowAbs = leftmostRow + i;
                if (!BackingArray.IsValid(rowAbs, (NonnegativeInteger)0))
                {
                    throw new IndexOutOfRangeException($"Invalid row index: {i}");
                }
            }
            for (int j = 0; j < area.GetLength(1); j++)
            {
                int colAbs = topMostColumn + j;
                if (!BackingArray.IsValid(colAbs, (NonnegativeInteger)1))
                {
                    throw new IndexOutOfRangeException($"Invalid column index: {j}");
                }
            }
        }

        public TVal[,] GetArea(int startRow, int startCol, int endRow, int endCol)
        {
            if(startRow > endRow || startCol > endCol)
            {
                throw new ArgumentException($"Top left corner ({startRow}, {startCol})" +
                    $" must be leq bottom right corner ({endRow}, {endCol}) of area");
            }
            int numRows = endRow - startRow + 1;
            int numCols = endCol - startCol + 1;
            TVal[,] area = new TVal[numRows, numCols];
            for (int rowRel = 0; rowRel < numRows; rowRel++)
            {
                for (int colRel = 0; colRel < numCols; colRel++)
                {
                    int rowAbs = startRow + rowRel;
                    int colAbs = startCol + colRel;
                    area[rowRel, colRel] = BackingArray[rowAbs, colAbs];
                }
            }
            return area;
        }
    }
}
