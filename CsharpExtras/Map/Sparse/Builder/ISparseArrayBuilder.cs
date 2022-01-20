using CsharpExtras.ValidatedType.Numeric.Integer;
using System;

namespace CsharpExtras.Map.Sparse.Builder
{
    /// <summary>
    /// Builder for building a sparse new array object
    /// </summary>
    /// <typeparam name="TVal">The value type in the sparse array object to be built</typeparam>
    public interface ISparseArrayBuilder<TVal>
    {
        /// <summary>
        /// The default value to be set in the sparse array object
        /// </summary>
        TVal DefaultValue { get; }

        /// <summary>
        /// The dimension of the sparse array object
        /// </summary>
        PositiveInteger Dimension { get; }

        /// <summary>
        /// Builds a new sparse array
        /// </summary>
        /// <returns>The newly built sparse array</returns>
        ISparseArray<TVal> Build();

        /// <summary>
        /// Adds a new validation function for the given axis index. If no validation function is added, the default will
        /// be x => true. If successive validation functions are added at the same index, then last one wins.
        /// Note: it is highly recommended that the validation funciton used is both pure and immutable.
        /// In particular, the function should be immutable i.e. the logic should not change over time.
        /// The problem with an impure/mutable function is that sparse arrays cache valid indices under the hood,
        /// the assumption being that once valid, an index is valid forever within the context of a sparse array
        /// </summary>
        /// <param name="indexValidationFunction">A function which validates indices along the axis specified by the axis index</param>
        /// <param name="axisIndex">The index of the axis with which to associate the validation function</param>
        ISparseArrayBuilder<TVal> WithValidationFunction(Func<int, bool> indexValidationFunction, NonnegativeInteger axisIndex);

        /// <summary>
        /// Caches a value to be set in the corresponding sparse array when it is built
        /// </summary>
        /// <param name="value">The value to write to the sparse array</param>
        /// <param name="coordinates">The coordinates at which to write the value</param>
        ISparseArrayBuilder<TVal> WithValue(TVal value, params int[] coordinates);
    }
}