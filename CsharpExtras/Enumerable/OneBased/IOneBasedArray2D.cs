﻿using CsharpExtras.Compare;
using System;
using System.Collections.Generic;

namespace CsharpExtras._Enumerable.OneBased
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
        /// Maps elements in this array to a new array, using a mapper function that takes one-based row/column indices as arguments
        /// </summary>
        /// <param name="mapper">Takes as arguments a row index, column index and value in the source array and returns the values for the corresponding row/column in the new array</param>
        /// <returns>A new array containing mapped values</returns>
        IOneBasedArray2D<TResult> Map<TResult>(Func<TVal, int, int, TResult> mapper);

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
        IOneBasedArray<TVal> FlattenRowMajor();
        IOneBasedArray<TVal> FlattenColumnMajor();
        int LastUsedRow(Predicate<TVal> isUsed);
        int LastUsedColumn(Predicate<TVal> isUsed);
        (int majorIndex, int minorIndex)? FirstIndexTupleOf(Func<TVal, bool> matcher);

        /// <summary>
        /// Zips one or more arrays into a single array using an enumerable-based fold operation.
        /// </summary>
        /// <typeparam name="TOther">The type of values in the enumerable of other arrays</typeparam>
        /// <typeparam name="TResult">The type of values in the resultant array</typeparam>
        /// <param name="zipper">A function which, given an element from this array and an enumerable of elements from 
        /// the other arrays, returns a value in the resultant array.</param>
        /// <param name="others">An enumerable of other arrays.</param>
        /// <returns>A new array, the shape of which is the intersection of the shapes of this array and all the others, 
        /// and the values of which are the result of applying the zipper across all values at the corresponding indices 
        /// in all the arrays.</returns>
        IOneBasedArray2D<TResult> ZipEnum<TOther, TResult>(Func<TVal, IEnumerable<TOther>, TResult> zipper, IEnumerable<IOneBasedArray2D<TOther>> others);

        /// <summary>
        /// Compares this array to the other array value-by-value
        /// </summary>
        /// <param name="other">The other array against which to compare</param>
        /// <param name="isEqualValues">A function which checks if values are equal</param>
        /// <returns>The result of the comparison, which can be used to determine if the arrays are equal according to the value-equality function passed.
        /// If the arrays are of a different shape, then the comparison will always be unequal</returns>
        IComparisonResult Compare(IOneBasedArray2D<TVal> other, Func<TVal, TVal, bool> isEqualValues);
    }
}
