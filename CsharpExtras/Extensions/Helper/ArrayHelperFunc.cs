using CsharpExtras.Compare;
using CsharpExtras.Compare.Array;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Extensions.Helper
{
    internal static class ArrayHelperFunc
    {
        internal static IComparisonResult Compare<TVal>(this TVal[] arr, TVal[] other,
            Func<TVal, TVal, bool> isEqualValues, int indexBase)
        {
            int thisLength = arr.Length;
            int otherLength = other.Length;
            IEnumerable<int> thisShape = thisLength.AsSingleton();
            IEnumerable<int> otherShape = otherLength.AsSingleton();
            if (thisLength != otherLength)
            {
                return new ArrayComparisonResultImpl<TVal>
                    (thisShape, otherShape, indexBase, null, null);
            }
            for(int arrayIndex = 0; arrayIndex < thisLength; arrayIndex++)
            {
                TVal thisValue = arr[arrayIndex];
                TVal otherValue = other[arrayIndex];
                if (!isEqualValues(thisValue, otherValue))
                {
                    return new ArrayComparisonResultImpl<TVal>
                    (thisShape, otherShape, indexBase, (arrayIndex.AsSingleton(), thisValue), otherValue?.ToString());
                }
            }
            return new ArrayComparisonResultImpl<TVal>
                    (thisShape, otherShape, indexBase, null, null);
        }

        internal static IComparisonResult Compare<TVal>(this TVal[,] arr, TVal[,] other,
            Func<TVal, TVal, bool> isEqualValues, int indexBase)
        {
            IList<int> thisShape = GetShape(arr);
            IList<int> otherShape = GetShape(other);
            if (!Enumerable.SequenceEqual(thisShape, otherShape))
            {
                return new ArrayComparisonResultImpl<TVal>
                    (thisShape, otherShape, indexBase, null, null);
            }
            for (int rowIndex = 0; rowIndex < thisShape[0]; rowIndex++)
            {
                for(int colIndex=0; colIndex < thisShape[1]; colIndex++)
                {
                    TVal thisValue = arr[rowIndex, colIndex];
                    TVal otherValue = other[rowIndex, colIndex];
                    if (!isEqualValues(thisValue, otherValue))
                    {
                        return new ArrayComparisonResultImpl<TVal>
                        (thisShape, otherShape, indexBase, (new List<int>
                            { rowIndex, colIndex}, thisValue), otherValue?.ToString());
                    }
                }
            }
            return new ArrayComparisonResultImpl<TVal>
                    (thisShape, otherShape, indexBase, null, null);
        }

        private static List<int> GetShape<TVal>(TVal[,] arr)
        {
            return new List<int> { arr.GetLength(0), arr.GetLength(1) };
        }
    }
}
