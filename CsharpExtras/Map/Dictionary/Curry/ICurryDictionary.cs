using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
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
        /// Removes all mappings corresponding to the given prefix.
        /// </summary>
        /// <param name="prefix">A key-tuple prefix which should be less than or equal to the arity of this dictionary</param>
        /// <returns>The number of elements removed. Specifically, if there are no matching keys, returns 0.</returns>
        NonnegativeInteger Remove(IEnumerable<TKey> prefix);

        /// <summary>
        /// Updates the value at the given key tuple if it's there, otherwise does nothing
        /// </summary>
        /// <param name="value">The value with which to overwrite the existing value</param>
        /// <param name="keyTuple">The index of the value to update. Must match arity of this dictionary.</param>
        /// <returns>True if an existing value was updated, false if there was no value at the given key tuple.</returns>
        bool Update(TVal value, params TKey[] keyTuple);

        /// <summary>
        /// Updates the value at the given key tuple if it's there, otherwise does nothing
        /// </summary>
        /// <param name="value">The value with which to overwrite the existing value</param>
        /// <param name="keyTuple">The index of the value to update. Must match arity of this dictionary.</param>
        /// <returns>True if an existing value was updated, false if there was no value at the given key tuple.</returns>
        bool Update(TVal value, IEnumerable<TKey> keyTuple);
        NonnegativeInteger Remove(params TKey[] prefix);

        /// <summary>
        /// The total number of values in this dictionary
        /// </summary>
        NonnegativeInteger Count { get; }
        
        /// <summary>
        /// Compares this dictionary to another one. Dictionaries are deemed equal if they are the same size and contain the same set of key/value pairs.
        /// </summary>
        /// <param name="isEqualValues">This function is used to compare values within the dictionary, returning true iff values are equal in some sense</param>
        /// <returns>A dictionary comparison result</returns>
        IDictionaryComparison Compare(ICurryDictionary<TKey, TVal> other, Func<TVal, TVal, bool> isEqualValues);
    }
}