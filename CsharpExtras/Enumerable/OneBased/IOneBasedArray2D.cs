using System;

namespace CsharpExtras.Enumerable.OneBased
{
    public interface IOneBasedArray2D<TVal>
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
        /// Use the provided function to collapse the 2D array into a single column.
        /// For each row, the function will collapse the row data into a single value and put that value in the new array.
        /// Returns an array with the collapsed values of all rows.
        /// </summary>
        IOneBasedArray<TVal> CollapseToSingleColumn(Func<TVal, TVal, TVal> collapseFunction);

        /// <summary>
        /// Use the provided function to collapse the 2D array into a single row.
        /// For each column, the function will collapse the column data into a single value and put that value in the new array.
        /// Returns an array with the collapsed values of all columns.
        /// </summary>
        IOneBasedArray<TVal> CollapseToSingleRow(Func<TVal, TVal, TVal> collapseFunction);
    }
}
