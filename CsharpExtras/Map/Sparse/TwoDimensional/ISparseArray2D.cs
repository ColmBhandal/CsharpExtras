using CsharpExtras.Compare;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;

namespace CsharpExtras.Map.Sparse.TwoDimensional
{
    /// <summary>
    /// This is a wrapper/convenience class for a sparse array of two dimensions.
    /// In this case, dimension 0 defines a row coordinate and dimension 1 defines a column coordinate
    /// </summary>    
    public interface ISparseArray2D<TVal>
    {
        /// <summary>
        /// Get or set the value at the given row, column coordinates
        /// </summary>
        TVal this[int row, int column] { get; set; }

        /// <summary>
        /// Gets the underlying sparse array - use this for any methods not directly on the 2D array
        /// </summary>
        ISparseArray<TVal> BackingArray { get; }
        
        /// <summary>
        /// The numeric index of the row axis
        /// </summary>
        NonnegativeInteger RowAxisIndex { get; }

        /// <summary>
        /// The numeric index of the column axis
        /// </summary>
        NonnegativeInteger ColumnAxisIndex { get; }

        /// <summary>
        /// Compares this 2D array to another one using a comparitor - delegates to the BackingArray
        /// </summary>
        IComparisonResult CompareUsedValues(ISparseArray2D<TVal> other, Func<TVal, TVal, bool> comparitor);

        /// <summary>
        /// Gets an area at the given coordinates. If coordinates within the area are invalid, throws an exception.
        /// </summary>
        /// <param name="startRow">Row coordinate of the top-left of the rectangle</param>
        /// <param name="startCol">Column coordinate of the top-left of the rectangle</param>
        /// <param name="endRow">Row coordinate of the bottom-right of the rectangle</param>
        /// <param name="endCol">Column coordinate of the bottom-righ of the rectangle</param>
        /// <returns>A 2D array of values defind by the rectangle at the given coordinates</returns>
        TVal[,] GetArea(int startRow, int startCol, int endRow, int endCol);
        
        /// <summary>
        /// Inserts columns at the given insertion index, shifting by the given vector
        /// </summary>
        /// <param name="insertionIndex">The index at which the new columns will be inserted.
        /// Note: the shifting operation will include this index.</param>
        /// <param name="shiftVector">A vector encapsulating both how many columns to insert and what direction to shift.
        /// Note: if this is zero, then the shift has no effect. If this is non zero, then the amount of columns 
        /// added is equal to the absolute value, and the shift direction is equal to the sign.</param>
        void InsertColumns(int insertionIndex, int shiftVector);

        /// <summary>
        /// Inserts rows at the given insertion index, shifting by the given vector
        /// </summary>
        /// <param name="insertionIndex">The index at which the new rows will be inserted.
        /// Note: the shifting operation will include this index.</param>
        /// <param name="shiftVector">A vector encapsulating both how many rows to insert and what direction to shift.
        /// Note: if this is zero, then the shift has no effect. If this is non zero, then the amount of rows 
        /// added is equal to the absolute value, and the shift direction is equal to the sign.</param>
        void InsertRows(int insertionIndex, int shiftVector);

        /// <summary>
        /// Sets an area to the given values at the given coordinates.
        /// If coordinates within the area are invalid, throws an exception.
        /// </summary>
        /// <param name="area">The are to write to this array</param>
        /// <param name="leftmostRow">The row coordinate of the top-left corner of the rectangle to which to write</param>
        /// <param name="topMostColumn">The column coordinate of the top-left corner of the rectangle to which to write</param>
        void SetArea(TVal[,] area, int leftmostRow, int topMostColumn);
    }
}