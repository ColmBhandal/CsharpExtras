using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Extensions
{
    public static class IEnumerableExtension
    {
        public static IDictionary<int, U> IndexOneBased<U>(this IEnumerable<U> values) => Index(values, 1);
        public static IDictionary<int, U> IndexZeroBased<U>(this IEnumerable<U> values) => Index(values, 0);

        public static IDictionary<int, U> Index<U>(this IEnumerable<U> values, int baseIndex)
            => Index(values, baseIndex, 1);


        /// <summary>
        /// Returns a single value as a singleton enumerable. See: https://stackoverflow.com/q/1577822/5134722
        /// </summary>
        /// <typeparam name="T">The type of the value, and the type of values in the resulting enumerable</typeparam>
        /// <param name="value">The value from which to create the singleton enumerable</param>
        /// <returns>A new enumerable which has exactly one element that is the given value</returns>
        public static IEnumerable<T> AsSingleton<T>(this T value)
        {
            yield return value;
        }

        /// <summary>
        /// Indexes this enumerable into a dictionary whose keys are the indices of elements in the enumerable.
        /// </summary>
        /// <typeparam name="U">The type of elements in the enumerable</typeparam>
        /// <param name="values"></param>
        /// <param name="baseIndex">The index to assign to the first index of the enumerable</param>
        /// <param name="step">The difference between successive indices in the enumerable</param>
        /// <returns>A dictionary whose keys are the indices of elements in the enumerable,
        /// starting at the given base index and incresing by the given step</returns>
        public static IDictionary<int, U> Index<U>(this IEnumerable<U> values, int baseIndex, int step)
        {
            IDictionary<int, U> dict = new Dictionary<int, U>();
            int currIndex = baseIndex;
            foreach (U u in values)
            {
                dict.Add(currIndex, u);
                currIndex += step;
            }
            return dict;
        }
        
        public static IEnumerable<T> CastSafe<U, T>(this IEnumerable<U> values) where U : T
        {
            foreach(U u in values)
            {
                yield return u;
            }
        }

        public static int FirstIndexOfOneBased<T>(this IEnumerable<T> values, T value) =>
            values.FirstIndexOf(value, 1);
        public static int FirstIndexOfZeroBased<T>(this IEnumerable<T> values, T value) =>
            values.FirstIndexOf(value, 0);

        public static int FirstIndexOf<T>(this IEnumerable<T> values, T value, int baseIndex)
        {            
            return values.FirstIndexOf(v => v != null && v.Equals(value), baseIndex);
        }

        /// <summary>
        /// Find the first row that matches the provided function. If none found, return -1
        /// </summary>
        /// <param name="equalityProvider">The function will stop at the first match of this</param>
        /// <param name="baseIndex">The index to assign to the first element in the enumerable</param>
        /// <returns></returns>
        public static int FirstIndexOf<T>(this IEnumerable<T> values, Func<T, bool> equalityProvider, int baseIndex)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                if (equalityProvider.Invoke(values.ElementAt(i)))
                {
                    return baseIndex + i;
                }
            }
            return -1;
        }

        //non-mvp: sliceRow should be consistent with Slice column (can we write a common function to reduce repetition?)
        public static IEnumerable<T> SliceRowToEnum<T>(this T[,] array, int row)
        {
            for (int i = array.GetLowerBound(1); i <= array.GetUpperBound(1); i++)
            {
                yield return array[row, i];
            }
        }
        
        public static IEnumerable<T> SliceColumnToEnum<T>(this T[,] array, int column)
        {
            for (int i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
            {
                yield return array[i, column];
            }
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (T value in values)
            {
                action.Invoke(value);
            }
            return values;
        }

        /// <summary>
        /// Safely find the maximum value of an IEnumerable of type int.
        /// If the collection is empty, the default value is returned.
        /// </summary>
        public static int Max(this IEnumerable<int> values, int defaultValue)
        {
            if (values.Any())
            {
                return values.Max();
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Find the max value between two IEnumerables of type int.
        /// An ArgumentException is thrown if both collections are empty.
        /// </summary>        
        [Obsolete("Use Union() followed by Max() instead")]
        public static int UnionMax(this IEnumerable<int> first, IEnumerable<int> second) =>
            UnionBound(first, second, e => e.Max());

        private static T UnionBound<T>(this IEnumerable<int> first, IEnumerable<int> second,
            Func<IEnumerable<int>, T> getBound) => getBound(first.Union(second));
    }
}
