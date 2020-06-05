using CsharpExtras.Dictionary;
using System;
using System.Collections.Generic;
using static CsharpExtras.CustomExtensions.ArrayExtension;

namespace CsharpExtras.Enumerable.OneBased
{
    public interface IOneBasedArray<TVal> : IEnumerable<TVal>
    {
        TVal this[int oneBasedIndex] { get; set; }
        TVal[] ZeroBasedEquivalent { get; }
        int Length { get; }

        int OneBasedFirstIndexOf(Func<TVal, bool> matchFunction);
        void PairAndExecute<TOther>(IOneBasedArray<TOther> other, Action<TVal, TOther> pairProcessor);
        void Resize(int newSize);

        IOneBasedArray2D<TVal> To2DArray(ArrayOrientation orientation);

        IDictionary<TVal, IList<int>> FindDuplicateIndices();
        IOneBasedArray<TResult> ZipArray<TOther, TResult>(Func<TVal, TOther, TResult> zipper, IOneBasedArray<TOther> other);
        IOneBasedArray<TResult> Map<TResult>(Func<TVal, TResult> mapper);

        IDictionary<TVal, IList<int>> InverseMap();

        /// <summary>
        /// Uses the values of this array as the keys of a new dictionary, and the values of the other array as the values of that dictionary.
        /// If there are any duplicate values in this array, then an exception is thrown.
        /// </summary>
        IDictionary<TVal, TOther> ZipToDictionary<TOther>(IOneBasedArray<TOther> other);
        
        IMultiValueMap<TVal, TOther> ZipToMultiValueMap<TOther>(IOneBasedArray<TOther> other);

        IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TResult>(Func<TVal, TOther1, TOther2, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2);
        IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TOther3, TResult>(Func<TVal, TOther1, TOther2, TOther3, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2, IOneBasedArray<TOther3> other3);
        IOneBasedArray<TVal> ZipMulti<TOther>(Func<TVal, TOther, TVal> zipper, IOneBasedArray<TOther> other, params IOneBasedArray<TOther>[] extras);
        (int index, TVal element) FindFirstOccurrenceOfSet(ISet<TVal> set);
        (int index, TVal element) FindFirstOccurrenceOfSet(ISet<TVal> set, int startIndex, int endIndex);

        TVal CollapseToSingleValue(Func<TVal, TVal, TVal> collapseFunction);
    }
}
