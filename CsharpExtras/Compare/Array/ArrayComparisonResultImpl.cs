using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Compare.Array
{
    internal class ArrayComparisonResultImpl<TVal> : IComparisonResult
    {
        private readonly int _indexBase;
        public bool IsEqual => MessageAndIsEqual.isEqual;
        public string Message => MessageAndIsEqual.message;

        private (string, bool)? _messageAndIsEqual;
        private (string message, bool isEqual) MessageAndIsEqual => _messageAndIsEqual ??= GetMessageAndIsEqual();

        private IEnumerable<int> ThisShape { get; }
        private IEnumerable<int> OtherShape { get; }

        private (IEnumerable<int>, TVal)? FirstMismatchZeroBased { get; }

        private string? OtherValMistmachOrNull { get; }

        public ArrayComparisonResultImpl(IEnumerable<int> thisShape,
            IEnumerable<int> otherShape,
            int indexBase,
            (IEnumerable<int>, TVal)? firstMismatch,
            string? otherValMistmachOrNull)
        {
            ThisShape = thisShape ?? throw new ArgumentNullException(nameof(thisShape));
            OtherShape = otherShape ?? throw new ArgumentNullException(nameof(otherShape));
            FirstMismatchZeroBased = firstMismatch;
            OtherValMistmachOrNull = otherValMistmachOrNull;
            _indexBase = indexBase;
        }

        private (string message, bool isEqual) GetMessageAndIsEqual()
        {
            if (!Enumerable.SequenceEqual(ThisShape, OtherShape))
            {
                return ($"Shape mismatch. This array has shape {PrintTuple(ThisShape)}. " +
                    $"Other array has shape {PrintTuple(OtherShape)}.",
                    false);
            }
            if (FirstMismatchZeroBased is (IEnumerable<int> k, TVal v))
            {
                string otherVal = OtherValMistmachOrNull ?? "NOT FOUND";
                return ($"Mismatch found at indices {PrintTuple(k.Select(i => i+_indexBase))}. " +
                    $"This value: {v}. Other value: {otherVal}", false);
            }
            return ("Arrays are equal", true);
        }

        private string PrintTuple(IEnumerable<int> tuple)
        {
            return string.Join(",", tuple.Select(i => i.ToString()));
        }
    }
}
