using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Compare.Array
{
    internal class ArrayComparisonResultImpl<TVal> : IComparisonResult
    {
        public bool IsEqual => MessageAndIsEqual.isEqual;
        public string Message => MessageAndIsEqual.message;

        private (string, bool)? _messageAndIsEqual;
        private (string message, bool isEqual) MessageAndIsEqual => _messageAndIsEqual ??= GetMessageAndIsEqual();

        private IEnumerable<int> ThisShape { get; }
        private IEnumerable<int> OtherShape { get; }

        private (IEnumerable<int>, TVal)? FirstMismatch { get; }

        private string? OtherValMistmachOrNull { get; }

        public ArrayComparisonResultImpl(IEnumerable<int> thisShape,
            IEnumerable<int> otherShape,
            (IEnumerable<int>, TVal)? firstMismatch,
            string? otherValMistmachOrNull)
        {
            ThisShape = thisShape ?? throw new ArgumentNullException(nameof(thisShape));
            OtherShape = otherShape ?? throw new ArgumentNullException(nameof(otherShape));
            FirstMismatch = firstMismatch;
            OtherValMistmachOrNull = otherValMistmachOrNull;
        }

        private (string message, bool isEqual) GetMessageAndIsEqual()
        {
            if (!Enumerable.SequenceEqual(ThisShape, OtherShape))
            {
                return ($"Shape mismatch. This array has shape {PrintTuple(ThisShape)}. " +
                    $"Other array has shape {PrintTuple(OtherShape)}.",
                    false);
            }
            if (FirstMismatch is (IEnumerable<int> k, TVal v))
            {
                string otherVal = OtherValMistmachOrNull ?? "NOT FOUND";
                return ($"Mismatch found at indices {PrintTuple(k)}. " +
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
