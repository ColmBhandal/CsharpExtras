using System;
using System.Collections.Generic;

namespace CsharpExtras.Enumerable.OneBased
{
    /// <summary>
    /// This is a two dimensional (non-jagged) array whose indices are one-based.
    /// As an enumerable, the elements of this array are sequenced in row major order.
    /// </summary>
    /// <typeparam name="TVal">The type of elements in the array.</typeparam>
    public interface IOneBasedArray2D<TVal> : IEnumerable<TVal>
    {
        TVal this[int oneBasedIndex0, int oneBasedIndex1] { get; set; }
        TVal[,] ZeroBasedEquivalent { get; }

        int GetLength(int dimZeroBased);
        IOneBasedArray2D<TResult> Map<TResult>(Func<TVal, TResult> mapper);

        /// <summary>
        /// Zip two 2D arrays into a single 2D array using a custom zipper function.
        /// If the two input arrays are of different sizes, the size of the output array is the intersection of the two input arrays.
        /// </summary>
        IOneBasedArray2D<TResult> ZipArray<TOther, TResult>(Func<TVal, TOther, TResult> zipper, IOneBasedArray2D<TOther> other);

        bool Any(Func<TVal, bool> checkerFunction);

        bool All(Func<TVal, bool> checkerFunction);

        int Count();

        int Count(Func<TVal, bool> checkerFunction);

        /// <summary>
        /// Slice the 2D array into a single row.
        /// </summary>
        IOneBasedArray<TVal> SliceRow(int row);

        /// <summary>
        /// Slice the 2D array into a single column.
        /// </summary>
        IOneBasedArray<TVal> SliceColumn(int column);

        /// <summary>
        /// Use the provided function to fold the 2D array into a single column.
        /// For each row, the function will fold the row data into a single value and put that value in the new array.
        /// Returns an array with the folded values of all rows.
        /// </summary>
        IOneBasedArray<TVal> FoldToSingleColumn(Func<TVal, TVal, TVal> foldFunction);

        /// <summary>
        /// Use the provided function to fold the 2D array into a single row.
        /// For each column, the function will fold the column data into a single value and put that value in the new array.
        /// Returns an array with the folded values of all columns.
        /// </summary>
        IOneBasedArray<TVal> FoldToSingleRow(Func<TVal, TVal, TVal> foldFunction);
        void WriteToRow(IOneBasedArray<TVal> values, int row, int offset);
        void WriteToColumn(IOneBasedArray<TVal> values, int column, int offset);
        void WriteToArea(IOneBasedArray2D<TVal> values, int rowOffset, int columnOffset);
        IOneBasedArray2D<TVal> SubArray(int startAtRow, int startAtColumn, int stopBeforeRow, int stopBeforeColumn);
    }
}
