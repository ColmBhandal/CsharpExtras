using CsharpExtras.Map.Dictionary;
using System;
using System.Collections.Generic;
using static CsharpExtras.Extensions.ArrayOrientationClass;
using static CsharpExtras.Extensions.ArrayExtension;
using CsharpExtras.Map.Dictionary.Collection;

namespace CsharpExtras._Enumerable.OneBased
{
    /// <summary>
    /// One-dimensional array with one-based indexing i.e. indexes start at one
    /// </summary>
    /// <typeparam name="TVal">Value type of the array</typeparam>
    public interface IOneBasedArray<TVal> : IEnumerable<TVal>
    {
        /// <summary>
        /// Property indexer - gets the element at the given index
        /// </summary>
        /// <param name="oneBasedIndex">One-based index - the first element in the array has index 1, the second index 2 etc.</param>
        /// <returns>The value in the array at the given index</returns>
        TVal this[int oneBasedIndex] { get; set; }

        /// <summary>
        /// A standard zero-based array which is the equivalent of this one-based array, but with indices starting at zero
        /// </summary>
        TVal[] ZeroBasedEquivalent { get; }

        /// <summary>
        /// Number of elements in the array
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Yields the index in the array of the first value that matches the given lamda
        /// </summary>
        /// <param name="matchFunction">A function which is used to test values in the array</param>
        /// <returns>The index in the array of the first value that matches the given matchFunction, or -1 if none exists</returns>
        int FirstIndexOf(Func<TVal, bool> matchFunction);

        /// <summary>
        /// Gets the first index of the value in the array
        /// </summary>
        /// <param name="val">The value to search for</param>
        /// <returns>The first index of the value in the array, as per the equals operator, or -1 if the value isn't found</returns>
        int FirstIndexOf(TVal val);

        /// <summary>
        /// Pairs this array with another array and executes the given processor on each pair. Note: if the arrays are of different sizes,
        /// then then pairings will only happen up to the length of the smaller one.
        /// </summary>
        /// <typeparam name="TOther"></typeparam>
        /// <param name="other"></param>
        /// <param name="pairProcessor"></param>
        void PairAndExecute<TOther>(IOneBasedArray<TOther> other, Action<TVal, TOther> pairProcessor);

        /// <summary>
        /// Resizes this array to the new size
        /// </summary>
        /// <param name="newSize">The new size to which this array should be resized</param>
        void Resize(int newSize);

        /// <summary>
        /// Creates a 2D array from this 1D array, with the given array orientation.
        /// </summary>
        /// <param name="orientation">If ROW, then the 2D array created will have a single row, 
        /// whose values will match that of the source array. Else the 2D array will have a single column, matching the source array.</param>
        /// <returns>A new 2D array whose values match that of the original 1D array and which is oriented according to the given orientation.</returns>
        IOneBasedArray2D<TVal> To2DArray(ArrayOrientation orientation);

        /// <summary>
        /// Gets all the values which occur more than once in this array i.e. all the duplicates, and their indexes
        /// </summary>
        /// <returns>A dictionary mapping each value in the array which is duplicated to the list of indexes at which it occurs. 
        /// Each entry in this dictionary will have a list of at least two elements, because every entry represents a duplicate.</returns>
        IDictionary<TVal, IList<int>> FindDuplicateIndices();

        /// <summary>
        /// Zips this arary with another array using a zip funciton
        /// </summary>
        /// <typeparam name="TOther">The type of values in the other array</typeparam>
        /// <typeparam name="TResult">The type of values in the resulting array</typeparam>
        /// <param name="zipper">A function which, given a value from this array and one from another array, produces a value in the resulting array</param>
        /// <param name="other">The other array with which to zip this array</param>
        /// <returns>A new array, the entries in which are produced from applying the zipper function across all pairs of elements in this array and the other array. 
        /// If the arrays are of different lengths, then the length of the resultant array will match the shorter length of the two.</returns>
        IOneBasedArray<TResult> ZipArray<TOther, TResult>(Func<TVal, TOther, TResult> zipper, IOneBasedArray<TOther> other);
        
        /// <summary>
        /// Maps each value in this array using a mapper function
        /// </summary>
        /// <typeparam name="TResult">The type of elements in the resultant array</typeparam>
        /// <param name="mapper">A funciton which maps elements in this array to those in the resultant array</param>
        /// <returns>A new array, the entries of which are the result of applying the given mapper at each element in this array</returns>
        IOneBasedArray<TResult> Map<TResult>(Func<TVal, TResult> mapper);

        /// <summary>
        /// Maps elements of this array by applying a function on the existing elements plus their one-based indices.
        /// </summary>
        /// <param name="mapper">A function which takes a value and a one-based index and returns a result</param>
        /// <returns>A new array, the same size as this one, filled with the result of applying the mapper function to the original values and one-based indices.</returns>
        IOneBasedArray<TResult> Map<TResult>(Func<TVal, int, TResult> mapper);

        /// <summary>
        /// Gets a dictionary mapping values to their indices
        /// </summary>
        /// <returns>A dictionary mapping each value in this array to the indices at which that value occurs</returns>
        IDictionary<TVal, IList<int>> InverseMap();

        /// <summary>
        /// Uses the values of this array as the keys of a new dictionary, and the values of the other array as the values of that dictionary.
        /// If there are any duplicate values in this array, then an exception is thrown.
        /// </summary>
        /// <typeparam name="TOther">The type of values in the other array</typeparam>
        /// <param name="other">The other array</param>
        /// <returns>A dictionary whose keys are the values of this array and whose values are the values at the corresponding index in the other array</returns>
        IDictionary<TVal, TOther> ZipToDictionary<TOther>(IOneBasedArray<TOther> other);

        /// <summary>
        /// Similar to <see cref="ZipToDictionary"/> but in the case of duplicate values in this dictionary, adds all other values into a set
        /// </summary>        
        /// <typeparam name="TOther">The type of values in the other array</typeparam>
        /// <param name="other">The other array</param>
        /// <returns>A dictionary whose keys are the unique values in this dictionary and whose values
        /// are the sets of values in the other dictionary corresponding to every occurence of each value in this dictionary</returns>
        ISetValuedDictionary<TVal, TOther> ZipToSetDictionary<TOther>(IOneBasedArray<TOther> other);

        /// <summary>
        /// Zips this array with two other arrays using a three-argument zipper function
        /// </summary>
        /// <typeparam name="TOther1">The type of elements in the first other array</typeparam>
        /// <typeparam name="TOther2">The type of elements in the second other array</typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting array</typeparam>
        /// <param name="zipper">A function which maps triples of elements in all 3 arrays to a single resulting element</param>
        /// <param name="other1">The first other array</param>
        /// <param name="other2">The second other array</param>
        /// <returns>A new array whose length is the minimum of this array and the two other arrays and whose entries are the result of applying the given mapper at each index</returns>
        IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TResult>(Func<TVal, TOther1, TOther2, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2);

        /// <summary>
        /// Zips this array with two other arrays using a four-argument zipper function
        /// </summary>
        /// <typeparam name="TOther1">The type of elements in the first other array</typeparam>
        /// <typeparam name="TOther2">The type of elements in the second other array</typeparam>
        /// <typeparam name="TOther3"></typeparam>
        /// <typeparam name="TResult">The type of elements in the resulting array</typeparam>
        /// <param name="zipper">A function which maps triples of elements in all 4 arrays to a single resulting element</param>
        /// <param name="other1">The first other array</param>
        /// <param name="other2">The second other array</param>
        /// <param name="other3">The third other array</param>
        /// <returns>A new array whose length is the minimum of this array and the three other arrays and whose entries are the result of applying the given mapper at each index</returns>
        IOneBasedArray<TResult> ZipArray<TOther1, TOther2, TOther3, TResult>(Func<TVal, TOther1, TOther2, TOther3, TResult> zipper, IOneBasedArray<TOther1> other1, IOneBasedArray<TOther2> other2, IOneBasedArray<TOther3> other3);

        /// <summary>
        /// Repeatedly zips this array with at least one other array and then an optional sequence of other arrays
        /// </summary>
        /// <typeparam name="TOther">The type of elements in the other array</typeparam>
        /// <param name="zipper">A function which maps en element in this array and one in another array back into an element of this array's type</param>
        /// <param name="other">The required other array with which to zip</param>
        /// <param name="extras">A number of extra arrays with which to zip</param>
        /// <returns>A new array resulting from zipping this array with the other array to produce a new array, and then repeatedly applies the zipper array using each of the extra arrays in turn until they are exhausted.</returns>
        IOneBasedArray<TVal> ZipMulti<TOther>(Func<TVal, TOther, TVal> zipper, IOneBasedArray<TOther> other, params IOneBasedArray<TOther>[] extras);

        /// <summary>
        /// Finds the index/element pair of the element in this array that's contained in the given set
        /// </summary>
        /// <param name="set">A set of values against which to check for set membership</param>
        /// <returns>The index/element pair of the element in this array that's contained in the given set, or null if none found</returns>
        (int index, TVal element)? FindFirstOccurrenceOfSet(ISet<TVal> set);

        /// <summary>
        /// Finds the index/element pair of the element in this array that's contained in the given set
        /// </summary>
        /// <param name="set">A set of values against which to check for set membership</param>
        /// <param name="startIndex">Start searching the array from this index inclusive i.e. don't look at lower indices</param>
        /// <param name="endIndex">Stop searching the array beyond this index, and exclude this index in the search</param>
        /// <returns>The index/element pair of the element in this array that's contained in the given set, or null if none found</returns>
        (int index, TVal element)? FindFirstOccurrenceOfSet(ISet<TVal> set, int startIndex, int endIndex);

        /// <summary>
        /// Use the provided function to fold an array of values into a single value.
        /// The function is called with the first two elements of the array, then with the output of the previous call and the next cell.
        /// The entire array is processed in this way and the final value is returned.
        /// </summary>
        TVal FoldToSingleValue(Func<TVal, TVal, TVal> foldFunction);


        /// <summary>
        /// Zips one or more arrays into a single array using an enumerable-based fold operation.
        /// </summary>
        /// <typeparam name="TOther">The type of values in the enumerable of other arrays</typeparam>
        /// <typeparam name="TResult">The type of values in the resultant array</typeparam>
        /// <param name="zipper">A function which, given an element from this array and an enumerable of elements from 
        /// the other arrays, returns a value in the resultant array.</param>
        /// <param name="others">An enumerable of other arrays.</param>
        /// <returns>A new array, the shape of which is the intersection of the shapes of this array and all the others, 
        /// and the values of which are the result of applying the zipper across all values at the corresponding indices 
        /// in all the arrays.</returns>
        IOneBasedArray<TResult> ZipEnum<TOther, TResult>(Func<TVal, IEnumerable<TOther>, TResult> zipper, IEnumerable<IOneBasedArray<TOther>> others);
    }
}
