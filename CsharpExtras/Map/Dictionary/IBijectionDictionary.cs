using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Map.Dictionary
{
    public interface IBijectionDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        IBijectionDictionary<TValue, TKey> Reverse { get; }
    }
}
