using CsharpExtras.Compare;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;

namespace CsharpExtras.Map.Sparse
{
    /// <summary>
    /// A sparse array is one in which we expect only a sparse number of entries i.e. we expect it to be mostly empty space
    /// A sparse array does not need an initial size. It can grow to any size.
    /// A sparse array can handle arrays of arbitrary dimensions. The dimension is specified on construction.
    /// A sparse array is less efficient than a regular array, certainly at storing contiguous values, as it uses nested dictionaries under the hood
    /// Each axis i.e. dimension index of a sparse array can be supplied with a validation condition on indices within that dimension e.g. lower/upper bounds.
    /// Invalid index access will cause an exception.
    /// For example, a 2D sparse array could allow x-axis values (dimension index 0) only between 0 and 20, but any y-axis (dimension index 1) values
    /// Because it is sparse, a sparse array is given a default value which will be returned for any entries in the array that were not explicitly assigned
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    public interface ISparseArray<TVal>
    {
        /// <summary>
        /// Gets/sets the underlying value at the given sequence of coordinates
        /// </summary>
        /// <param name="coordinates">A sequence of coordinates which must match the dimension of this array</param>
        /// <returns>The last value that was written to those coordinates, or else the default if no value was written</returns>
        TVal this[params int[] coordinates] { get; set; }

        /// <summary>
        /// Gets the value stored at the given coordinates
        /// </summary>
        /// <param name="coordinates">The coordinates that map to the given value</param>
        /// <returns>The value at the given coordinates</returns>
        TVal GetValueFromCoordinates(IEnumerable<int> coordinates);

        /// <summary>
        /// The default value will be returned for any entry in the array whose value was not explicitly assigned
        /// </summary>
        TVal DefaultValue { get; }

        /// <summary>
        /// The dimension is the number of coordinates per entry
        /// </summary>
        PositiveInteger Dimension { get; }
        
        /// <summary>
        /// The count of used values in this array. This will be increased whenever a value is written to some previously unpopulated index of the array.
        /// If the default value is written to an index, then it does not contribute to the used values count.
        /// </summary>
        NonnegativeInteger UsedValuesCount { get; }

        /// <summary>
        /// Compares the used values in this sparse array to those in another one
        /// Note: only used values are compared, not defaults.
        /// </summary>
        /// <param name="other">The other array against which to compare</param>
        /// <param name="valueComparer">A function which is used to determine if two values are equal. It's up to the caller to use a sensible function here.</param>
        /// <returns>A comparison result which will indicate whether the used values in this array are equal to the other array according to the comparer</returns>                
        IComparisonResult CompareUsedValues(ISparseArray<TVal> other, Func<TVal, TVal, bool> valueComparer);
        
        /// <summary>
        /// Is the given index along the given axis valid
        /// </summary>
        /// <param name="index">The index to check</param>
        /// <param name="axisIndex">The index of the axis along which to check the given index. Note, axes are indexed from 0 up to <see cref="Dimension"/> minus 1</param>
        /// <returns>True iff the given index is valid</returns>
        bool IsValid(int index, NonnegativeInteger axisIndex);

        /// <summary>
        /// Shifts all elements in the array along a given axis by a given vector
        /// </summary>
        /// <param name="axisIndex">The index of the axis along which to check the given index. Note, axes are indexed from 0 up to <see cref="Dimension"/> minus 1</param>
        /// <param name="firstShiftIndex">Start shifting elements from this index onwards</param>
        /// <param name="shiftVector">The vector by which to shift. The sign of this value determines which direction elements are shifted.
        /// The sign also determines whether it's elements less than or greater than the first shift index that are shifted.
        /// The indices of all elements to be shifted are incremented along the shift axis by the shift vector.</param>
        void Shift(NonnegativeInteger axisIndex, int firstShiftIndex, int shiftVector);

        /// <summary>
        /// Zips this array with another one to produce a new array.
        /// The dimension of the other array must exactly match this one, otherwise an exception is thrown.
        /// </summary>
        /// <typeparam name="TOther">The type of elements in the other array</typeparam>
        /// <typeparam name="TResult">The type of elements in the resultant array</typeparam>
        /// <param name="zipper">A function which, given an element from this array and the other array,
        /// returns an element of the resultant array.</param>
        /// <param name="other">The other array with which to zip this one</param>
        /// <param name="defaultVal">The default value for the new sparse array</param>
        /// <param name="validationFunction">Validates indices in the resultant array. If any zipped indices are invalid,
        /// the zip operation will throw an exception.</param>
        /// <returns>A new sparse array, the used indices of which will be the union of used indices for this array and the other array
        /// and the values of which will be the result of applying the zipper function at the corresponding indices in this array and the other array</returns>
        /// <returns></returns>
        ISparseArray<TResult> Zip<TOther, TResult>(Func<TVal, TOther, TResult> zipper,
            ISparseArray<TOther> other, TResult defaultVal,
            Func<NonnegativeInteger, int, bool> validationFunction);

        /// <summary>
        /// Enumerable of all used entries in this array. A used entry is a list of coordinates along with the value at those coordinates.
        /// </summary>
        IEnumerable<(IList<int>, TVal)> UsedEntries { get; }

        /// <summary>
        /// Checks if the given valid coordinates are used.
        /// If invalid coordinates are supplied then throws an exception.
        /// </summary>
        /// <param name="coordinates">The coordinates at which to check if there is a used value.</param>
        /// <returns>True iff the given coordinates are used.
        /// Coordinates are considered used if a non-default value resides there.</returns>
        bool IsUsed(params int[] coordinates);

        /// <summary>
        /// Checks if the given valid coordinates are used.
        /// If invalid coordinates are supplied then throws an exception.
        /// </summary>
        /// <param name="coordinates">The coordinates at which to check if there is a used value.</param>
        /// <returns>True iff the given coordinates are used.
        /// Coordinates are considered used if a non-default value resides there.</returns>
        bool IsUsed(IEnumerable<int> coordinates);
    }
}