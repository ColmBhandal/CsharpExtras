using CsharpExtras.ValidatedType.Numeric.Integer;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Curry
{
    public interface ICurryDictionary<TKey, TVal>
    {
        NonnegativeInteger Arity { get; }
        TVal this[params TKey[] keys] { get; set; }

        /// <summary>
        /// Checks if the key tuple is contained in the dictionary.
        /// Throws exception if tuple is of incorrect length.
        /// </summary>
        bool ContainsKeyTuple(params TKey[] keys);
        /// <summary>
        /// Checks if the key tuple is contained in the dictionary.
        /// Throws exception if tuple is of incorrect length.
        /// </summary>
        bool ContainsKeyTuple(IEnumerable<TKey> keys);
        TVal GetValueFromTuple(params TKey[] keys);
        TVal GetValueFromTuple(IEnumerable<TKey> keys);

        /// <summary>
        /// Adds the element at the given key if it's not already there.
        /// </summary>
        /// <returns>True iff the value was added</returns>
        bool Add(TVal value, params TKey[] keys);
        /// <summary>
        /// Adds the element at the given key if it's not already there.
        /// </summary>
        /// <returns>True iff the value was added</returns>
        bool Add(TVal value, IEnumerable<TKey> keys);
    }
}