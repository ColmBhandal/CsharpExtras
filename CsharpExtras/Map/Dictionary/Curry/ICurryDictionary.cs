using CsharpExtras.ValidatedType.Numeric.Integer;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Curry
{
    public interface ICurryDictionary<TKey, TVal>
    {
        NonnegativeInteger Arity { get; }
        TVal this[params TKey[] keyTuple] { get; }
        IEnumerable<IList<TKey>> Keys { get; }
        IEnumerable<(IList<TKey>, TVal)> KeyValuePairs { get; }
        IEnumerable<TVal> Values { get; }

        /// <summary>
        /// Checks if the key tuple is contained in the dictionary.
        /// Throws exception if tuple is of incorrect length.
        /// </summary>
        bool ContainsKeyTuple(params TKey[] keyTuple);
        /// <summary>
        /// Checks if the key tuple is contained in the dictionary.
        /// Throws exception if tuple is of incorrect length.
        /// </summary>
        bool ContainsKeyTuple(IEnumerable<TKey> keyTuple);
        /// <summary>
        /// Checks if there is some key tuple in the keyset of this dictionary 
        /// starting with the given prefix
        /// </summary>
        bool ContainsKeyTuplePrefix(params TKey[] prefix);
        /// <summary>
        /// Checks if there is some key tuple in the keyset of this dictionary 
        /// starting with the given prefix
        /// </summary>
        bool ContainsKeyTuplePrefix(IEnumerable<TKey> prefix);

        TVal GetValueFromTuple(params TKey[] keyTuple);
        TVal GetValueFromTuple(IEnumerable<TKey> keyTuple);


        /// <summary>
        /// "Curries" this dictionary with a prefix of keys, 
        /// yielding a dictionary mapping suffixes of all keys matching the previs to associated values
        /// For example, if this dictionary maps (1, 2, 3) -> "Hello", (1, 2, 4) -> "World" and
        /// (10, 11, 12) -> "Non-match", then passing a prefix of (1, 2) to this function yields the map:
        /// (1) -> "Hello" and (2) -> "World"
        /// </summary>
        ICurryDictionary<TKey, TVal> GetCurriedDictionary(params TKey[] prefix);
        /// <summary>
        /// "Curries" this dictionary with a prefix of keys, 
        /// yielding a dictionary mapping suffixes of all keys matching the previs to associated values
        /// For example, if this dictionary maps (1, 2, 3) -> "Hello", (1, 2, 4) -> "World" and
        /// (10, 11, 12) -> "Non-match", then passing a prefix of (1, 2) to this function yields the map:
        /// (1) -> "Hello" and (2) -> "World"
        /// </summary>
        ICurryDictionary<TKey, TVal> GetCurriedDictionary(IEnumerable<TKey> prefix);


        /// <summary>
        /// Adds the element at the given key if it's not already there.
        /// </summary>
        /// <returns>True iff the value was added</returns>
        bool Add(TVal value, params TKey[] keyTuple);
        /// <summary>
        /// Adds the element at the given key if it's not already there.
        /// </summary>
        /// <returns>True iff the value was added</returns>
        bool Add(TVal value, IEnumerable<TKey> keyTuple);

        /// <summary>
        /// The total number of values in this dictionary
        /// </summary>
        NonnegativeInteger Count { get; }
    }
}