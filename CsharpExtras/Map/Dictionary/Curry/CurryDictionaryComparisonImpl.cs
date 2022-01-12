using CsharpExtras.Extensions.Helper.Dictionary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    internal class CurryDictionaryComparisonImpl<TKey, TVal> : IDictionaryComparison
    {
        public bool IsEqual => MessageAndIsEqual.isEqual;
        public string Message => MessageAndIsEqual.message;

        /// <param name="thisCount">Number of elements in this dictionary</param>
        /// <param name="otherCount">Number of elements in other dictionary</param>
        /// <param name="firstMismatch">First mistmach, if one exists, including the keys and the underly dictionary comparison</param>
        public CurryDictionaryComparisonImpl(int thisCount, int otherCount, (IList<TKey> keys, IDictionaryComparison comparison) firstMismatch)
        {
            ThisCount = thisCount;
            OtherCount = otherCount;
            FirstMismatch = firstMismatch;
        }

        private int ThisCount { get; }
        private int OtherCount { get; }

        private (IList<TKey> keys, IDictionaryComparison comparison)? FirstMismatch { get; }

        private (string, bool)? _messageAndIsEqual;
        private (string message, bool isEqual) MessageAndIsEqual => _messageAndIsEqual ??= GetMessageAndIsEqual();

        private (string message, bool isEqual) GetMessageAndIsEqual()
        {
            if (ThisCount != OtherCount)
            {
                return ($"Count mismatch. This dictionary has {ThisCount} elements. Other dictionary has {OtherCount} elements",
                    false);
            }
            if (FirstMismatch is (IList<TKey> keys, IDictionaryComparison comparison))
            {
                return ($"Mismatch found at key tuple {keys}. Cause by underlying dictionary mismatch: {comparison.Message}", false);
            }
            return ("Dictionaries are equal", true);
        }
    }
}
