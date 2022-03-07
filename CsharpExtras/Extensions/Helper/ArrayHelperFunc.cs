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
            return new ArrayComparisonResultImpl<TVal>
                    (thisShape, otherShape, indexBase, null, null);
        }

        private static List<int> GetShape<TVal>(TVal[,] arr)
        {
            return new List<int> { arr.GetLength(0), arr.GetLength(1) };
        }
    }
}
