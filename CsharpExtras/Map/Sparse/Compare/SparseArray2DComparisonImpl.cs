using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse.Compare
{
    internal class SparseArray2DComparisonImpl<TVal> : SparseArrayComparisonImpl<TVal>
    {
        public SparseArray2DComparisonImpl(int thisUsedValuesCount, int otherUsedValuesCount,
            (IList<int> keyTuple, TVal val)? firstMismatch, string? otherValMistmatch)
            : base(2, 2, thisUsedValuesCount, otherUsedValuesCount, firstMismatch, otherValMistmatch)
        {
        }
    }
}
