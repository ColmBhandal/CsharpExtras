using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace CsharpExtras.Extensions
{
    public static class ArrayExtension2D
    {

        /// <param name="p">A predicate on an entire column</param>
        /// <returns>The last column where the predicate is true, or -1 if none found</returns>
        public static int LastColumnWhere<TVal>(this TVal[,] array, Predicate<TVal[]> p)
        {
            int lastColumnIndex = array.GetLength(1) - 1;
            for (int columnIndex = lastColumnIndex; columnIndex >= 0; columnIndex--)
            {
                TVal[] columnSlice = array.SliceColumn(columnIndex);
                if (p(columnSlice))
                {
                    return columnIndex;
                }
            }
            return -1;
        }

        /// <param name="p">A predicate on an entire row</param>
        /// <returns>The last row where the predicate is true, or -1 if none found</returns>        
        public static int LastRowWhere<TVal>(this TVal[,] array, Predicate<TVal[]> p)
        {
            int lastRowIndex = array.GetLength(0) - 1;
            for(int rowIndex = lastRowIndex; rowIndex >=0; rowIndex--)
            {
                TVal[] rowSlice = array.SliceRow(rowIndex);
                if (p(rowSlice))
                {
                    return rowIndex;
                }
            }
            return -1;
        }

        public static int LastUsedRow(this string[,] array)
        {
            return array.LastUsedRow(s => !string.IsNullOrWhiteSpace(s));
        }
        public static int LastUsedColumn(this string[,] array)
        {
            return array.LastUsedColumn(s => !string.IsNullOrWhiteSpace(s));
        }
        
        /// <param name="isUsed">A usage predicate- true false for any given element in the array</param>
         /// <returns>The last column at which there exists an element matching the usage predicate, or -1 if none found</returns>
        public static int LastUsedColumn<TVal>(this TVal[,] array, Predicate<TVal> isUsed)
        {
            return array.LastColumnWhere(slice => Array.Exists(slice, isUsed));
        }
        
        /// <param name="isUsed">A usage predicate- true false for any given element in the array</param>
        /// <returns>The last row at which there exists an element matching the usage predicate, or -1 if none found</returns>
        public static int LastUsedRow<TVal>(this TVal[,] array, Predicate<TVal> isUsed)
        {
            return array.LastRowWhere(slice => Array.Exists(slice, isUsed));
        }

        public static TVal[] FoldToSingleColumn<TVal>(this TVal[,] array, Func<TVal, TVal, TVal> foldFunction)
        {
            if (array.GetLength(0) == 0 || array.GetLength(1) == 0)
            {
                throw new ArgumentOutOfRangeException("Cannot fold an empty array");
            }

            TVal[] output = new TVal[array.GetLength(0)];
            for (int row = 0; row < array.GetLength(0); row++)
            {
                TVal[] rowData = array.SliceRow(row);
                output[row] = rowData.FoldToSingleValue(foldFunction);
            }
            return output;
        }

        public static TVal[] FoldToSingleRow<TVal>(this TVal[,] array, Func<TVal, TVal, TVal> foldFunction)
        {
            if (array.GetLength(0) == 0 || array.GetLength(1) == 0)
            {
                throw new ArgumentOutOfRangeException("Cannot fold an empty array");
            }

            TVal[] output = new TVal[array.GetLength(1)];
            for (int column = 0; column < array.GetLength(1); column++)
            {
                TVal[] columnData = array.SliceColumn(column);
                output[column] = columnData.FoldToSingleValue(foldFunction);
            }
            return output;
        }

        public static bool Any<TVal>(this TVal[,] array, Func<TVal, bool> checkerFunction)
        {
            (int, int)? firstIndexTuple = array.FirstIndexTupleOf(checkerFunction);
            return (firstIndexTuple != null);
        }

        /// <summary>
        /// Searches the array in row major order until a matching value is found
        /// </summary>
        /// <returns>A tuple containing the row/column index of the match, or null if none is found</returns>
        public static (int majorIndex, int minorIndex)? FirstIndexTupleOf<TVal>
            (this TVal[,] array, Func<TVal, bool> matcherFunction)
        {

            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int column = 0; column < array.GetLength(1); column++)
                {
                    if (matcherFunction(array[row, column]))
                    {
                        return (row, column);
                    }
                }
            }
            return null;
        }

        public static bool All<TVal>(this TVal[,] array, Func<TVal, bool> checkerFunction)
        {
            Func<TVal, bool> inverseChecker = (value) => !checkerFunction(value);
            return !array.Any(inverseChecker);
        }

        public static int Count<TVal>(this TVal[,] array)
        {
            return array.GetLength(0) * array.GetLength(1);
        }

        public static int Count<TVal>(this TVal[,] array, Func<TVal, bool> checkerFunction)
        {
            int count = 0;
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int column = 0; column < array.GetLength(1); column++)
                {
                    if (checkerFunction(array[row, column]))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Zip two 2D arrays into a single 2D array using a custom zipper function.
        /// If the two input arrays are of different sizes, the size of the output array is the intersection of the two input arrays.
        /// </summary>
        public static TResult[,] ZipArray<TVal, TOther, TResult>(this TVal[,] array, Func<TVal, TOther, TResult> zipper, TOther[,] other)
        {
            int zipLength0 = Math.Min(array.GetLength(0), other.GetLength(0));
            int zipLength1 = Math.Min(array.GetLength(1), other.GetLength(1));
            TResult[,] resultArrayZeroBased = new TResult[zipLength0, zipLength1];

            for (int dim0 = 0; dim0 < zipLength0; dim0++)
            {
                for (int dim1 = 0; dim1 < zipLength1; dim1++)
                {
                    resultArrayZeroBased[dim0, dim1] = zipper(array[dim0, dim1], other[dim0, dim1]);
                }
            }
            return resultArrayZeroBased;
        }

        public static TResult[,] Map<TVal, TResult>(this TVal[,] array, Func<TVal, TResult> mapper)
        {
            TResult func(TVal x, int i, int j) => mapper(x);
            return array.Map(func);
        }

        /// <summary>
        /// Creates a new array, populated by mapping the values/indices of the old array using a mapper function.
        /// </summary>
        /// <param name="mapper">A function which maps an element and its row/column indices in the original array to an element in the new array</param>
        /// <returns>A new array, whose values are the result of applying the mapper function to the associated element in the source array and its row/column indices.</returns>
        public static TResult[,] Map<TVal, TResult>(this TVal[,] array, Func<TVal, int, int, TResult> mapper)
        {
            int length0 = array.GetLength(0);
            int length1 = array.GetLength(1);
            TResult[,] resultArray = new TResult[length0, length1];
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    resultArray[i, j] = mapper(array[i, j], i, j);
                }
            }
            return resultArray;
        }

        public static T[,] Transpose<T>(this T[,] array)
        {
            T[,] transposed = new T[array.GetLength(1), array.GetLength(0)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    transposed[j, i] = array[i, j];
                }
            }
            return transposed;
        }

        public static T[] SliceRow<T>(this T[,] array, int row)
        {
            IEnumerable<T> enumSlice = array.SliceRowToEnum(row);
            return enumSlice.ToArray();
        }

        public static T[] SliceColumn<T>(this T[,] array, int column)
        {
            IEnumerable<T> enumSlice = array.SliceColumnToEnum(column);
            return enumSlice.ToArray();
        }

        public static void AssertRowIndexIsInBounds<TVal>(this TVal[,] array, int row)
        {
            int length = array.GetLength(0);
            if (row < 0 || row >= length)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Row index {0} is out of bounds for 2D array. Expected range: [{1}, {2}]",
                    row, 0, length));
            }
        }
        public static void AssertColumnIndexIsInBounds<TVal>(this TVal[,] array, int column)
        {
            int length = array.GetLength(1);
            if (column < 0 || column >= length)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Column index {0} is out of bounds for 2D array. Expected range: [{1}, {2}]",
                    column, 0, length));
            }
        }

        /// <summary>
        /// Writes a 1D array to the specified row in a 2D array.
        /// The 1D array is shifted relative to the row in the 2D array by either a positive or negative shift.
        /// After the shift, the 1D array's values are written to any 2D array values that line up with it.
        /// Note: not all values in the 1D array are guaranteed to be written
        /// and not all values in the 2D row are guaranteed to be written.
        /// Only overlapping values will be written.
        /// </summary>        
        /// <param name="rowValues">The 1D array to write.</param>
        /// <param name="row">The row in the 2D array to which to write the array.</param>
        /// <param name="columnOffset">A positive or negative value specifying the amount by which to shift the 1D array.</param>
        public static void WriteToRow<TVal>(this TVal[,] array, TVal[] rowValues, int row, int columnOffset)
        {
            array.AssertRowIndexIsInBounds(row);
            int stopBeforeColumn = Math.Min(rowValues.Length, array.GetLength(1) - columnOffset);
            int startColumn = Math.Max(0, -columnOffset);
            for(int column = startColumn; column < stopBeforeColumn; column++)
            {
                array[row, column + columnOffset] = rowValues[column];
            }
        }

        /// <summary>
        /// Writes a 1D array to the specified column in a 2D array.
        /// The 1D array is shifted relative to the column in the 2D array by either a positive or negative shift.
        /// After the shift, the 1D array's values are written to any 2D array values that line up with it.
        /// Note: not all values in the 1D array are guaranteed to be written
        /// and not all values in the 2D column are guaranteed to be written.
        /// Only overlapping values will be written.
        /// </summary>        
        /// <param name="columnValues">The 1D array to write.</param>
        /// <param name="column">The column in the 2D array to which to write the array.</param>
        /// <param name="rowOffset">A positive or negative value specifying the amount by which to shift the 1D array.</param>h
        public static void WriteToColumn<TVal>(this TVal[,] array, TVal[] columnValues, int column, int rowOffset)
        {
            array.AssertColumnIndexIsInBounds(column);
            int stopBeforeRow = Math.Min(columnValues.Length, array.GetLength(0) - rowOffset);
            int startRow = Math.Max(0, -rowOffset);
            for (int row = startRow; row < stopBeforeRow; row++)
            {
                array[row + rowOffset, column] = columnValues[row];
            }
        }

        /// <summary>
        /// Writes one 2D array to another. The arrays do not need to be the same size.
        /// The array to write is first aligned with the top left corner of the target array.
        /// Then the array to write is shifted by a row and column offset, which could be negative.
        /// After the shift, the 2 arrays will be overlapping by some rectangular area, possibly empty.
        /// The values written to the target are exactly those which overlap after the shift.
        /// </summary>
        /// <param name="targetArray">Write to this array.</param>
        /// <param name="arrayToWrite">Write values from this array.</param>
        /// <param name="rowOffset">The amount by which to shift the row value of the values to write.</param>
        /// <param name="columnOffset">The amout by which to shift the column value of the values to write.</param>
        public static void WriteToArea<TVal>(this TVal[,] targetArray, TVal[,] arrayToWrite, int rowOffset, int columnOffset)
        {
            int stopBeforeRow = Math.Min(arrayToWrite.GetLength(0), targetArray.GetLength(0) - rowOffset);
            int stopBeforeColumn = Math.Min(arrayToWrite.GetLength(1), targetArray.GetLength(1) - columnOffset);
            int startRow = Math.Max(0, -rowOffset);
            int startColumn = Math.Max(0, -columnOffset);
            for (int row = startRow; row < stopBeforeRow; row++)
            {
                for (int column = startColumn; column < stopBeforeColumn; column++)
                {
                    targetArray[row + rowOffset, column + columnOffset] = arrayToWrite[row, column];
                }
            }
        }

        /// <summary>
        /// Rectangular sub array of this 2D array defined by the given coordinates.
        /// </summary>
        /// <param name="startAtRow">Row index to start at. Negative indices will be truncated to zero.</param>
        /// <param name="startAtColumn">Column index to start at. Negative indices will be truncated to zero.</param>
        /// <param name="stopBeforeRow">Row index before which to stop. Indices greater than number of rows will be truncated to that number.</param>
        /// <param name="stopBeforeColumn">Column index before which to stop. Indices greater than number of columns will be truncated to that number.</param>
        /// <returns></returns>
        public static TVal[,] SubArray<TVal>(this TVal[,] array, int startAtRow, int startAtColumn, int stopBeforeRow, int stopBeforeColumn)
        {
            startAtRow = Math.Max(startAtRow, 0);
            stopBeforeRow = Math.Min(stopBeforeRow, array.GetLength(0));
            startAtColumn = Math.Max(startAtColumn, 0);
            stopBeforeColumn = Math.Min(stopBeforeColumn, array.GetLength(1));

            int rowLength = stopBeforeRow - startAtRow;
            int columnLength = stopBeforeColumn - startAtColumn;
            TVal[,] result = new TVal[rowLength, columnLength];
            for(int row = startAtRow; row < stopBeforeRow; row++)
            {
                for(int col = startAtColumn; col < stopBeforeColumn; col++)
                {
                    result[row - startAtRow, col - startAtColumn] = array[row, col];
                }
            }
            return result;
        }

        /// <summary>
        /// Flattens this 2D array into a 1D array using row major order.
        /// </summary>
        public static TVal[] FlattenRowMajor<TVal>(this TVal[,] array)
        {
            TVal[] flattenedArray = new TVal[array.Length];
            int numRows = array.GetLength(0);
            int numCols = array.GetLength(1);
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    int flatIndex = row * numCols + col;
                    flattenedArray[flatIndex] = array[row, col];
                }
            }
            return flattenedArray;
        }

        /// <summary>
        /// Flattens this 2D array into a 1D array using column major order.
        /// </summary>
        public static TVal[] FlattenColumnMajor<TVal>(this TVal[,] array)
        {
            TVal[] flattenedArray = new TVal[array.Length];
            int numRows = array.GetLength(0);
            int numCols = array.GetLength(1);
            for (int col = 0; col < numCols; col++) 
            {
                for (int row = 0; row < numRows; row++)
                {
                    int flatIndex = col * numRows + row;
                    flattenedArray[flatIndex] = array[row, col];
                }
            }
            return flattenedArray;
        }
        /// <summary>
        /// Returns a jagged array from the multi-dimensional array
        /// Jagged array's indices always start at zero. 
        /// If the lower bounds of the multidimensional array are non-zero, a number of blank entries will be inserted into the jagged array, 
        /// such that the indices of elements in the jagged array match those in the original array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dimensionalArray"></param>
        /// <returns></returns>
        public static T[][] ConvertToJaggedArray<T>(this T[,] dimensionalArray)
        {
            int rowsFirstIndex = dimensionalArray.GetLowerBound(0);
            int rowsLastIndex = dimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = dimensionalArray.GetLowerBound(1);
            int columnsLastIndex = dimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = dimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
    }
}
