using CsharpExtras.Map.Dictionary;
using System;
using System.Collections.Generic;
using static CsharpExtras.Extensions.ArrayOrientationClass;
using static CsharpExtras.Extensions.ArrayExtension;
using CsharpExtras.Map.Dictionary.Collection;

namespace CsharpExtras.Enumerable.OneBased
{
    public interface IOneBasedArray<TVal> : IEnumerable<TVal>
    {
        TVal this[int oneBasedIndex] { get; set; }
        TVal[] ZeroBasedEquivalent { get; }
        int Length { get; }

        int FirstIndexOf(Func<TVal, bool> matchFunction);
        int FirstIndexOf(TVal val);
        void PairAndExecute<TOther>(IOneBasedArray<TOther> other, Action<TVal, TOther> pairProcessor);
        void Resize(int newSize);

        IOneBasedArray2D<TVal> To2DArray(ArrayOrientation orientation);

        IDictionary<TVal, IList<int>> FindDuplicateIndices();
        IOneBasedArray<TResult> ZipArray<TOther, TResult>(Func<TVal, TOther, TResult> zipper, IOneBasedArray<TOther> other);
        IOneBasedArray<TResult> Map<TResult>(Func<TVal, TResult> mapper);
        /// <summary>
        /// Maps elements of this array by applying a function on the existing elements plus their one-based indices.
        /// </summary>
        /// <param name="mapper">A function which takes a value & a one-based index and returns a result</param>
        /// <returns>A new array, the same size as this one, filled with the result of applying the mapper function to the original values & one-based indices.</returns>
        IOneBasedArray<TResult> Map<TResult>(Func<int, TVal, TResult> mapper);

        IDictionary<TVal, IList<int>> InverseMap();

        /// <summary>
        /// Uses the values of this array as the keys of a new dictionary, and the values of the other array as the values of that dictionary.
        /// If there are any duplicate values in this array, then an exception is thrown.
        /// </summary>
        IDictionary<TVal, TOther> ZipToDictionary<TOther>(IOneBasedArray<TOther> other);
        ISetValuedDictionary<TVal, TOther> ZipToSetDictionary<TOther>(IOneBasedArray<TOther> other);

        IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TResult>(Func<TVal, TOther1, TOther2, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2);
        IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TOther3, TResult>(Func<TVal, TOther1, TOther2, TOther3, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2, IOneBasedArray<TOther3> other3);
        IOneBasedArray<TVal> ZipMulti<TOther>(Func<TVal, TOther, TVal> zipper, IOneBasedArray<TOther> other, params IOneBasedArray<TOther>[] extras);
        (int index, TVal element)? FindFirstOccurrenceOfSet(ISet<TVal> set);

        /// <param name="startIndex">Start searching the array from this index inclusive i.e. don't look at lower indices</param>
        /// <param name="endIndex">Stop searching the array beyond this index, and exclude this index in the search</param>
        /// <returns>A pair indicating the first element found and its index, or null if nothing found.</returns>
        (int index, TVal element)? FindFirstOccurrenceOfSet(ISet<TVal> set, int startIndex, int endIndex);

        /// <summary>
        /// Use the provided function to fold an array of values into a single value.
        /// The function is called with the first two elements of the array, then with the output of the previous call and the next cell.
        /// The entire array is processed in this way and the final value is returned.
        /// </summary>
        TVal FoldToSingleValue(Func<TVal, TVal, TVal> foldFunction);
    }
}
