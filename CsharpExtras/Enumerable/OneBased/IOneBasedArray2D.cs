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

        IOneBasedArray<TVal> SliceRow(int row);

        IOneBasedArray<TVal> SliceColumn(int column);

        IOneBasedArray<TVal> CollapseToSingleColumn(Func<TVal, TVal, TVal> collapseFunction);

        IOneBasedArray<TVal> CollapseToSingleRow(Func<TVal, TVal, TVal> collapseFunction);
    }
}
