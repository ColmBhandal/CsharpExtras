using CsharpExtras.Compare;
using System;

namespace CsharpExtras.Map.Sparse.TwoDimensional
{
    public interface ISparseArray2D<TVal>
    {
        TVal this[int row, int column] { get; set; }

        IComparisonResult CompareUsedValues(ISparseArray2D<TVal> other, Func<TVal, TVal, bool> comparitor);
        TVal[,] GetArea(int startRow, int startCol, int endRow, int endCol);
        void SetArea(TVal[,] area, int rowIndex, int colIndex);
    }
}