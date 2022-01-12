using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Extensions.Helper.Dictionary
{
    internal class DictionaryComparisonImpl<TKey, TVal> : IDictionaryComparison
    {
        public bool IsEqual => MessageAndIsEqual.isEqual;
        public string Message => MessageAndIsEqual.message;

        /// <param name="thisCount">Number of elements in this dictionary</param>
        /// <param name="otherCount">Number of elements in other dictionary</param>
        /// <param name="firstMismatch">First mistmach, if one exists, or null if there are no mismatches</param>
        public DictionaryComparisonImpl(int thisCount, int otherCount, (TKey, TVal)? firstMismatch)
        {
            ThisCount = thisCount;
            OtherCount = otherCount;
            FirstMismatch = firstMismatch;
        }

        private int ThisCount { get; }
        private int OtherCount { get; }

        private (TKey, TVal)? FirstMismatch { get; }

        private (string, bool)? _messageAndIsEqual;
        private (string message, bool isEqual) MessageAndIsEqual => _messageAndIsEqual ??= GetMessageAndIsEqual();

        private (string message, bool isEqual) GetMessageAndIsEqual()
        {
            if (ThisCount != OtherCount)
            {
                return ($"Count mismatch. This dictionary has {ThisCount} elements. Other dictionary has {OtherCount} elements",
                    false);
            }
            if (FirstMismatch is (TKey k, TVal v))
            {
                return ($"Mismatch found at key {k} and value {v}", false);
            }
            return ("Dictionaries are equal", true);
        }
    }
}
