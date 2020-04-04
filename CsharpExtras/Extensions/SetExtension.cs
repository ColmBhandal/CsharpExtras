using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.CustomExtensions
{
    public static class SetExtension
    {
        public static ISet<TMapped> Map<TValue, TMapped>(this ISet<TValue> set, Func<TValue, TMapped> mapper)
        {
            var setToReturn = new HashSet<TMapped>();
            foreach (TValue value in set)
            {
                TMapped mapped = mapper(value);
                if (!setToReturn.Contains(mapped))
                {                    
                    setToReturn.Add(mapped);
                }
            }
            return setToReturn;
        }
    }
}
