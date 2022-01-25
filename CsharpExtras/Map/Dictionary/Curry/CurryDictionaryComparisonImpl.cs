using CsharpExtras.Compare;
using CsharpExtras.Extensions.Helper.Dictionary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Curry
{
    internal class CurryDictionaryComparisonImpl<TKey, TVal> : IComparisonResult
    {
        public bool IsEqual => MessageAndIsEqual.isEqual;
        public string Message => MessageAndIsEqual.message;

        /// <param name="thisCount">Number of elements in this dictionary</param>
        /// <param name="otherCount">Number of elements in other dictionary</param>
        /// <param name="firstMismatch">First mistmach, if one exists, including the keys and the underly dictionary comparison</param>
        /// <param name="otherValMistmatch">Other value for first mismatch, as a string, or null if other value not found</param>
        public CurryDictionaryComparisonImpl(int thisArity, int otherArity, int thisCount, int otherCount,
            (IList<TKey> keyTuple, TVal val)? firstMismatch, string? otherValMistmatch)
        {
            ThisCount = thisCount;
            OtherCount = otherCount;
            ThisArity = thisArity;
            OtherArity = otherArity;
            FirstMismatch = firstMismatch;
            OtherValMistmachOrNull = otherValMistmatch;
        }

        private int ThisArity { get; }
        private int OtherArity { get; }
        private int ThisCount { get; }
        private int OtherCount { get; }

        private (IList<TKey> keyTuple, TVal val)? FirstMismatch { get; }

        private string? OtherValMistmachOrNull { get; }

        private (string, bool)? _messageAndIsEqual;
        private (string message, bool isEqual) MessageAndIsEqual => _messageAndIsEqual ??= GetMessageAndIsEqual();

        private (string message, bool isEqual) GetMessageAndIsEqual()
        {
            if (ThisArity != OtherArity)
            {
                return ($"Arity mismatch. This dictionary has arity = {ThisArity}. Other dictionary has arity = {OtherArity}", false);
            }
            if (ThisCount != OtherCount)
            {
                return ($"Count mismatch. This dictionary has {ThisCount} elements. Other dictionary has {OtherCount} elements", false);
            }
            if (FirstMismatch is (IList<TKey> keyTuple, TVal val))
            {
                string otherVal = OtherValMistmachOrNull ?? "NOT FOUND";
                return ($"Mismatch found at key tuple {string.Join(",", keyTuple)}. This value: {val}. Other value: {otherVal}", false);
            }
            return ("Dictionaries are equal", true);
        }
    }
}
