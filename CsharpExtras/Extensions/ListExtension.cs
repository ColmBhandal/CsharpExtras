using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.CustomExtensions
{
    public static class ListExtension
    {
        public static IList<TMapped> Map<TValue, TMapped>(this IList<TValue> set, Func<TValue, TMapped> mapper)
        {
            IList<TMapped> listToReturn = new List<TMapped>();
            foreach (TValue value in set)
            {
                TMapped mapped = mapper(value);
                listToReturn.Add(mapped);
            }
            return listToReturn;
        }
    }
}
