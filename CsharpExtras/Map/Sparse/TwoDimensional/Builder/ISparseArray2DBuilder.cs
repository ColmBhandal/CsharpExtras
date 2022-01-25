using System;

namespace CsharpExtras.Map.Sparse.TwoDimensional.Builder
{
    public interface ISparseArray2DBuilder<TVal>
    {        
        /// <returns>A new 2D sparse array, built from the data entered during the builder configuration</returns>
        ISparseArray2D<TVal> Build();

        /// <summary>
        /// Add the given validator function to the row dimension
        /// </summary>
        /// <param name="validator">The validator function to add</param>
        ISparseArray2DBuilder<TVal> WithRowValidation(Func<int, bool> validator);

        /// <summary>
        /// Add the given validator function to the column dimension
        /// </summary>
        /// <param name="validator">The validator function to add</param>
        ISparseArray2DBuilder<TVal> WithColumnValidation(Func<int, bool> validator);

        /// <summary>
        /// Buffers a value for write to the final sparse 2D array
        /// </summary>
        ISparseArray2DBuilder<TVal> WithValue(TVal value, int row, int column);

        //Non-MVP: Add withArea method which buffers a 2D array for write
    }
}