using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Map.Sparse.Compare;
using CsharpExtras.Map.Sparse.TwoDimensional.Builder;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse.TwoDimensional
{
    internal class SparseArray2DImpl<TVal> : ISparseArray2D<TVal>
    {
        public ISparseArray<TVal> BackingArray { get; }

        private NonnegativeInteger? _rowAxisIndex;
        public NonnegativeInteger RowAxisIndex => _rowAxisIndex ??= (NonnegativeInteger)0;
        private NonnegativeInteger? _columnAxisIndex;
        private readonly ICsharpExtrasApi _api;

        public NonnegativeInteger ColumnAxisIndex => _columnAxisIndex ??= (NonnegativeInteger)1;

        public SparseArray2DImpl(ISparseArray<TVal> backingArray, ICsharpExtrasApi api)
        {
            ValidateBackingArray(backingArray);
            BackingArray = backingArray ?? throw new ArgumentNullException(nameof(backingArray));
            _api = api;
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

        public void InsertRows(int insertionIndex, int shiftVector) =>
            BackingArray.Shift((NonnegativeInteger)0, insertionIndex, shiftVector);

        public void InsertColumns(int insertionIndex, int shiftVector) =>
            BackingArray.Shift((NonnegativeInteger)1, insertionIndex, shiftVector);

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

        private void ValidateArea(TVal[,] area, int leftmostRow, int topMostColumn)
        {
            for (int i = 0; i < area.GetLength(0); i++)
            {
                int rowAbs = leftmostRow + i;
                if (!BackingArray.IsValid(rowAbs, RowAxisIndex))
                {
                    throw new IndexOutOfRangeException($"Invalid row index: {i}");
                }
            }
            for (int j = 0; j < area.GetLength(1); j++)
            {
                int colAbs = topMostColumn + j;
                if (!BackingArray.IsValid(colAbs, ColumnAxisIndex))
                {
                    throw new IndexOutOfRangeException($"Invalid column index: {j}");
                }
            }
        }

        private void ValidateBackingArray(ISparseArray<TVal> backingArray)
        {
            PositiveInteger dimension = backingArray.Dimension;
            if (dimension != 2)
            {
                throw new ArgumentException($"Dimension of backing array must be exactly 2. Found: {(int)dimension}");
            }
        }

        public ISparseArray2D<TResult> Zip<TOther, TResult>(Func<TVal, TOther, TResult> zipper,
            ISparseArray2D<TOther> other, TResult defaultVal, Func<NonnegativeInteger, int, bool> validationFunction)
        {
            ISparseArray<TResult> backingZipped = BackingArray.Zip(zipper, other.BackingArray, defaultVal, validationFunction);           
            return new SparseArray2DImpl<TResult>(backingZipped, _api);
        }
    }
}
