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
        bool IsValid(int index, NonnegativeInteger axisIndex);
    }
}