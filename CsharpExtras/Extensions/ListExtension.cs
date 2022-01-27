using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Extensions
{
    /// <summary>
    /// Extension functions for lists
    /// </summary>
    public static class ListExtension
    {

        /// <see cref="Map{TValue, TMapped}(IList{TValue}, Func{TValue, int, TMapped})">
        /// The difference in this case is that the mapper function ignores indices and only operates on values</see>
        public static IList<TMapped> Map<TValue, TMapped>(this IList<TValue> list, Func<TValue, TMapped> mapper)
        {
            Func<TValue, int, TMapped> mapWithIndex = (v, i) => mapper(v);
            return list.Map(mapWithIndex);
        }

        /// <summary>
        /// Applies the given mapper function to the list
        /// </summary>
        /// <typeparam name="TValue">Value of items in the input list</typeparam>
        /// <typeparam name="TMapped">Value of items in the returned list</typeparam>
        /// <param name="list">The input list</param>
        /// <param name="mapper">A function which maps a value and an index in the input list to a value in the returned list</param>
        /// <returns>A new list, the result of which is the application of the mapper function across the original</returns>
        public static IList<TMapped> Map<TValue, TMapped>(this IList<TValue> list, Func<TValue, int, TMapped> mapper)
        {
            int count = list.Count;
            IList<TMapped> listToReturn = new List<TMapped>(count);
            for (int i = 0; i < count; i++)
            {
                TValue value = list[i];
                TMapped mapped = mapper(value, i);
                listToReturn.Add(mapped);
            }
            return listToReturn;
        }
    }
}
