using CsharpExtras.Compare;
using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Curry
{
    public interface ICurryDictionary<TKey, TVal>
    {
        /// <summary>
        /// The arity of the dictionary is the number of elements in each full key tuple that maps to a value
        /// </summary>
        NonnegativeInteger Arity { get; }
        
        /// <summary>
        /// Gets the value stored at the given key tuple
        /// </summary>
        /// <param name="keyTuple">A key tuple whose length must match the arity of this dictionary.
        /// The tuple must correspond to an existing element of the dictionary.</param>
        /// <returns>The value that's stored at the specified key. Throws a key not found exceptoin otherwise.</returns>
        TVal this[params TKey[] keyTuple] { get; }

        /// <summary>
        /// Enumerable of all key tuples in this dictionary i.e. all key tuples that map to some value
        /// </summary>
        IEnumerable<IList<TKey>> KeyTuples { get; }

        /// <summary>
        /// Enumerable of all key-tuple / value pairs in this dictionary
        /// </summary>
        IEnumerable<(IList<TKey>, TVal)> KeyValuePairs { get; }

        /// <summary>
        /// Enumerable of all values in this dictionary
        /// </summary>
        IEnumerable<TVal> Values { get; }

        /// <summary>
        /// Gets all the key tuple prefixes of the given arity
        /// </summary>
        /// <param name="arity">The arity of each key prefix i.e. the number of keys in each prefix-tuple</param>
        /// <returns>An enumerable which iterates through the key prefixes of the given arity</returns>
        IEnumerable<IList<TKey>> KeyTuplePrefixes(NonnegativeInteger arity);

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

        /// <summary>
        /// Gets the value stored at the given key tuple
        /// </summary>
        /// <param name="keyTuple">The key tuple that maps to the given value</param>
        /// <returns>The value at the given key tuple</returns>
        TVal GetValueFromTuple(params TKey[] keyTuple);
        
        /// <summary>
        /// Gets the value stored at the given key tuple
        /// </summary>
        /// <param name="keyTuple">The key tuple that maps to the given value</param>
        /// <returns>The value at the given key tuple</returns>
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

        /// <summary>
        /// Removes all key-tuple to value mappings matching the given key-tuple prefix
        /// </summary>
        /// <param name="prefix">Any key tuples to value mappings whose key tuple starts with this prefix will be removed</param>
        /// <returns></returns>
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
        IComparisonResult Compare(ICurryDictionary<TKey, TVal> other, Func<TVal, TVal, bool> isEqualValues);

        /// <summary>
        /// Performs the given action on all curried dictionaries at the given arity
        /// Warning: atomicity is not guaranteed by this method. If the action fails before it completes
        /// for all dictionaries, but it has succeeded for some dictionaries, then the success cases will not be rolled back
        /// </summary>
        /// <param name="action">Action to perform on each curried dictionary</param>
        /// <param name="arity">The arity of the key tuples used to generate all the curried dictionaries.
        /// Note: if the arity is zero, then the action is performed on this dictionary itself</param>
        void DoForAllCurriedDictionaries(Action<ICurryDictionary<TKey, TVal>> action, NonnegativeInteger arity);

        /// <summary>
        /// Performs the given action on all pairs of key-prefixes and curried dictionaries at the given arity
        /// </summary>
        /// <param name="action">The action to perform. It takes a key prefix and a curried dictionary as arguments.
        /// Warning: atomicity is not guaranteed by this method. If the action fails before it completes
        /// for all dictionaries, but it has succeeded for some dictionaries, then the success cases will not be rolled back</param>
        /// <param name="arity">The arity of the key tuples uset to generate all the pairs</param>
        void DoForAllPairs(Action<IList<TKey>, ICurryDictionary<TKey, TVal>> action, NonnegativeInteger arity);

        /// <summary>
        /// Updates the keys for all curried dictionaries at the given arity.
        /// </summary>
        /// <param name="keyInjection">A function which maps keys to keys. This must be injective on the keyset of every dictionary at the given arity.</param>
        /// <param name="prefixArity">The arity of the key tuples uset to generate all the curried dictionaries upon which to apply the map</param>
        void UpdateKeys(Func<TKey, TKey> keyInjection, NonnegativeInteger prefixArity);
        
        /// <summary>
        /// Updates the first key in every key tuple of this curry dictionary
        /// </summary>
        /// <param name="keyInjection">A function mapping keys to keys, which must be injective on the set of keys upon which it operates.
        /// Example: If (2, 5) and (3, 4) are both key tuples in a dictionary, then a key injection function which maps all keys to the value 1 will fail.
        /// This is the case even though the key-tuples (1, 5) and (1, 4) are unique. This function does not look at full key tuples, it only
        /// considers the first keys. So the set of keys upon which it operates is {2, 3}.
        /// And since 2 and 3 will map to 1 in this case, then the function is non-injective and the update will fail.</param>
        void UpdateFirstKeyInTuples(Func<TKey, TKey> keyInjection);
    }
}