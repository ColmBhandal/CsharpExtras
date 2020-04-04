using CustomExtensions;
using Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBased
{
    public interface IOneBasedArray<TVal> : IEnumerable<TVal>
    {
        TVal this[int oneBasedIndex] { get; set; }
        TVal[] ZeroBasedEquivalent { get; }
        int Length { get; }

        int OneBasedFirstIndexOf(Func<TVal, bool> matchFunction);
        void PairAndExecute<TOther>(IOneBasedArray<TOther> other, Action<TVal, TOther> pairProcessor);
        void Resize(int newSize);
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
    }

    class OneBasedArrayImpl<TVal> : IOneBasedArray<TVal>
    {
        private TVal[] _backingArray;

        public OneBasedArrayImpl(TVal[] backingArray)
        {
            _backingArray = backingArray;
        }

        public TVal this[int oneBasedIndex]
        {
            get
            {
                ValidateIndex(oneBasedIndex);
                return _backingArray[oneBasedIndex - 1];
            }
            set
            {
                ValidateIndex(oneBasedIndex);
                _backingArray[oneBasedIndex - 1] = value;
            }
        }

        public int Length => _backingArray.Length;

        public TVal[] ZeroBasedEquivalent => _backingArray;

        public IEnumerator<TVal> GetEnumerator()
        {
            for(int i = 0; i < Length; i++)
            {
                yield return ZeroBasedEquivalent[i];
            }
        }

        public (int index, TVal element) FindFirstOccurrenceOfSet(ISet<TVal> set, int startIndex, int endIndex)
        {
            (int zeroBasedIndex, TVal element) = ZeroBasedEquivalent.FindFirstOccurrenceOfSet<TVal>(set, startIndex, endIndex);
            int oneBasedIndex = zeroBasedIndex + 1;
            return (oneBasedIndex, element);
        }

        public (int index, TVal element) FindFirstOccurrenceOfSet(ISet<TVal> set)
        {
            (int zeroBasedIndex, TVal element) = ZeroBasedEquivalent.FindFirstOccurrenceOfSet<TVal>(set);
            int oneBasedIndex = zeroBasedIndex + 1;
            return (oneBasedIndex, element);
        }

        public int OneBasedFirstIndexOf(Func<TVal, bool> matchFunction)
        {
            return ZeroBasedEquivalent.FirstIndexOf(matchFunction) + 1;
        }

        public void Resize(int newSize)
        {
            Array.Resize<TVal>(ref _backingArray, newSize);
        }

        public IDictionary<TVal, TOther> ZipToDictionary<TOther>(IOneBasedArray<TOther> other)
        {
            return ZeroBasedEquivalent.ZipToDictionary(other.ZeroBasedEquivalent);
        }

        public IMultiValueMap<TVal, TOther> ZipToMultiValueMap<TOther>(IOneBasedArray<TOther> other)
        {
            return ZeroBasedEquivalent.ZipToMultiValueMap(other.ZeroBasedEquivalent);
        }

        public IOneBasedArray<TResult> ZipArray<TOther, TResult>(Func<TVal, TOther, TResult> zipper, IOneBasedArray<TOther> other)
        {
            TResult[] zippedZeroBased = ZeroBasedEquivalent.ZipArray(zipper, other.ZeroBasedEquivalent);
            return new OneBasedArrayImpl<TResult>(zippedZeroBased);
        }

        public IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TResult>(Func<TVal, TOther1, TOther2, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2)
        {
            TResult[] zippedZeroBased = ZeroBasedEquivalent.ZipArray(zipper, other1.ZeroBasedEquivalent, other2.ZeroBasedEquivalent);
            return new OneBasedArrayImpl<TResult>(zippedZeroBased);
        }

        public IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TOther3, TResult>(Func<TVal, TOther1, TOther2, TOther3, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2, IOneBasedArray<TOther3> other3)
        {
            TResult[] zippedZeroBased = ZeroBasedEquivalent.ZipArray(zipper, other1.ZeroBasedEquivalent, other2.ZeroBasedEquivalent, other3.ZeroBasedEquivalent);
            return new OneBasedArrayImpl<TResult>(zippedZeroBased);
        }

        public IOneBasedArray<TVal> ZipMulti<TOther>(Func<TVal, TOther, TVal> zipper, IOneBasedArray<TOther> other, params IOneBasedArray<TOther>[] extras)
        {
            TOther[][] extrasZeroBased = extras.Map(arr => arr.ZeroBasedEquivalent);
            TVal[] zippedZeroBased = ZeroBasedEquivalent.ZipMulti(zipper, other.ZeroBasedEquivalent, extrasZeroBased);
            return new OneBasedArrayImpl<TVal>(zippedZeroBased);
        }

        public void PairAndExecute<TOther>(IOneBasedArray<TOther> other, Action<TVal, TOther> pairProcessor)
        {
            int zipLength = Math.Min(Length, other.Length);
            for (int i = 1; i <= zipLength; i++)
            {
                pairProcessor(this[i], other[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ValidateIndex(int oneBasedIndex)
        {
            int len = _backingArray.Length;
            if (oneBasedIndex < 1 || oneBasedIndex > len)
            {
                throw new IndexOutOfRangeException(string.Format(
                    "One-based index {0} out of bounds. Should be in the range [{1}, {2}].",
                    oneBasedIndex, 1, len));
            }
        }

        public IOneBasedArray<TResult> Map<TResult>(Func<TVal, TResult> mapper)
        {
            TResult[] zeroBasedMapped = ZeroBasedEquivalent.Map(mapper);
            return new OneBasedArrayImpl<TResult>(zeroBasedMapped);
        }

        public IDictionary<TVal, IList<int>> FindDuplicateIndices()
        {
            IDictionary<TVal, IList<int>> zeroBasedDuplicates = ZeroBasedEquivalent.FindDuplicateIndices();
            return zeroBasedDuplicates.MapValues(list => list.Map(i => i + 1));
        }

        public IDictionary<TVal, IList<int>> InverseMap()
        {
            IDictionary<TVal, IList<int>> reversedZeroBased = ZeroBasedEquivalent.Inverse();
            return reversedZeroBased.MapValues(list => list.Map(i => i + 1));
        }
    }
}
