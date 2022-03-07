using CsharpExtras.Compare;
using CsharpExtras.Compare.Array;
using CsharpExtras.Map.Dictionary;
using CsharpExtras.Map.Dictionary.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using static CsharpExtras.Extensions.ArrayOrientationClass;

namespace CsharpExtras.Extensions
{
    public static class ArrayExtension
    {
        public static IComparisonResult Compare<TVal>(this TVal[] arr, TVal[] other, Func<TVal, TVal, bool> isEqualValues)
        {
            //TODO: Implement
            return new ArrayComparisonResultImpl<TVal>();
        }

        //Non-mvp: Test this
        /// <returns>A pair indicating the first element found and its index, or (-1, default) if nothing found.</returns>
        public static (int index, TVal element)? FindFirstOccurrenceOfSet<TVal>(this TVal[] arr, ISet<TVal> set)
        {
            return FindFirstOccurrenceOfSet(arr, set, 0, arr.Length);
        }

        //Non-mvp: Test this
        /// <param name="startIndex">Start searching the array from this index inclusive i.e. don't look at lower indices</param>
        /// <param name="endIndex">Stop searching the array beyond this index, and don't include this index in the search</param>
        /// <returns>A pair indicating the first element found and its index, or null if nothing found.</returns>
        public static (int index, TVal element)? FindFirstOccurrenceOfSet<TVal>(this TVal[] arr, ISet<TVal> set, int startIndex, int endIndex)
        {
            for (int i = startIndex; i < arr.Length && i < endIndex; i++)
            {
                TVal element = arr[i];
                if (set.Contains(element))
                {
                    return (i, element);
                }
            }
            return null;
        }


        /// <summary>
        /// Sub-array starting at a given index and stopping before another index.
        /// </summary>        
        /// <param name="startAt">Index to start at (inclusive). Negative indices will be truncated to zero.</param>
        /// <param name="stopBefore">Index before which to stop. Indices greater than array length will be truncated to the array length.</param>
        /// <returns></returns>
        public static TVal[] SubArray<TVal>(this TVal[] data, int startAt, int stopBefore)
        {
            startAt = Math.Max(startAt, 0);
            stopBefore = Math.Min(stopBefore, data.Length);

            int length = stopBefore - startAt;
            TVal[] result = new TVal[length];
            Array.Copy(data, startAt, result, 0, length);
            return result;
        }

        public static TVal[] SubArray<TVal>(this TVal[] data, int startAt)
        {
            return data.SubArray(startAt, data.Length);
        }

        public static string[] RemoveBlankEntries(this string[] data)
        {
            return data.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        /// <summary>
        /// Creates a 2D array from this 1D array, with the given array orientation.
        /// </summary>
        /// <param name="orientation">If ROW, then the 2D array created will have a single row, 
        /// whose values will match that of the source array. Else the 2D array will have a single column, matching the source array.</param>
        /// <returns>A new 2D array whose values match that of the original 1D array and which is oriented according to the given orientation.</returns>
        public static TVal[,] To2DArray<TVal>(this TVal[] array, ArrayOrientation orientation)
        {
            TVal[,] outputArray;
            if (orientation == ArrayOrientation.COLUMN)
            {
                outputArray = new TVal[array.Length, 1];
            }
            else
            {
                outputArray = new TVal[1, array.Length];
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (orientation == ArrayOrientation.COLUMN)
                {
                    outputArray[i, 0] = array[i];
                }
                else
                {
                    outputArray[0, i] = array[i];
                }
            }
            return outputArray;
        }

        //TODO: Test
        public static TVal[] DeepCopy<TVal>(this TVal[] array)
        {
            int length = array.Length;
            TVal[] copy = new TVal[length];
            array.CopyTo(copy, 0);
            return copy;
        }

        public static IDictionary<TVal, IList<int>> Inverse<TVal>(this TVal[] array)
        {
            IDictionary<TVal, IList<int>> inverseMap = new Dictionary<TVal, IList<int>>();

            for (int index = 0; index < array.Length; index++)
            {
                TVal value = array[index];
                if (inverseMap.ContainsKey(value))
                {
                    inverseMap[value].Add(index);
                }
                else
                {
                    IList<int> lst = new List<int> { index };
                    inverseMap.Add(value, lst);
                }
            }
            return inverseMap;
        }

        public static IDictionary<TVal, IList<int>> FindDuplicateIndices<TVal>(this TVal[] array)
        {
            return array.Inverse().FilterValues(lst => lst.Count > 1);
        }

        //Non-mvp: Test
        public static IDictionary<TVal, TOther> ZipToDictionary<TVal, TOther>(this TVal[] array, TOther[] other)
        {

            (TVal s, TOther u)[] zippedValues = array.ZipArray<TVal, TOther, (TVal, TOther)>((s, u) => (s, u), other);
            IDictionary<TVal, TOther> dict = new Dictionary<TVal, TOther>();
            foreach ((TVal leftValue, TOther rightValue) in zippedValues)
            {
                if (dict.ContainsKey(leftValue))
                {
                    throw new InvalidOperationException(string.Format(
                        "Zip failed: The key {0} seems to be duplicated in the left array.", leftValue));
                }
                dict.Add(leftValue, rightValue);
            }
            return dict;
        }

        public static ISetValuedDictionary<TVal, TOther> ZipToSetDictionary<TVal, TOther>(this TVal[] array, TOther[] other)
        {
            (TVal s, TOther u)[] zippedValues = array.ZipArray<TVal, TOther, (TVal, TOther)>((s, u) => (s, u), other);
            ISetValuedDictionary<TVal, TOther> dict = new SetValuedDictionaryImpl<TVal, TOther>();
            foreach ((TVal leftValue, TOther rightValue) in zippedValues)
            {
                dict.Add(leftValue, rightValue);
            }
            return dict;
        }

        public static TResult[] ZipArray<TVal, TOther, TResult>(this TVal[] array, Func<TVal, TOther, TResult> zipper, TOther[] other)
        {
            int zipLength = Math.Min(array.Length, other.Length);
            TResult[] resultArrayZeroBased = new TResult[zipLength];
            for (int i = 0; i < zipLength; i++)
            {
                resultArrayZeroBased[i] = zipper(array[i], other[i]);
            }
            return resultArrayZeroBased;
        }

        public static TResult[] ZipArray<TVal, TOther1, TOther2, TResult>(this TVal[] array, Func<TVal, TOther1, TOther2, TResult> zipper, TOther1[] other1, TOther2[] other2)
        {
            int zipLength = Math.Min(Math.Min(array.Length, other1.Length), other2.Length);
            TResult[] resultArrayZeroBased = new TResult[zipLength];
            for (int i = 0; i < zipLength; i++)
            {
                resultArrayZeroBased[i] = zipper(array[i], other1[i], other2[i]);
            }
            return resultArrayZeroBased;
        }

        public static TResult[] ZipArray<TVal, TOther1, TOther2, TOther3, TResult>
            (this TVal[] array, Func<TVal, TOther1, TOther2, TOther3, TResult> zipper, TOther1[] other1, TOther2[] other2, TOther3[] other3)
        {
            int zipLength = Math.Min(Math.Min(Math.Min(array.Length, other1.Length), other2.Length), other3.Length);
            TResult[] resultArrayZeroBased = new TResult[zipLength];
            for (int i = 0; i < zipLength; i++)
            {
                resultArrayZeroBased[i] = zipper(array[i], other1[i], other2[i], other3[i]);
            }
            return resultArrayZeroBased;
        }

        public static TResult[] ZipMulti<TOther, TResult>(this TResult[] array, Func<TResult, TOther, TResult> zipper, TOther[] other, params TOther[][] extras)
        {
            TResult[] currentZip = array.ZipArray(zipper, other);
            foreach(TOther[] extra in extras)
            {
                currentZip = currentZip.ZipArray(zipper, extra);
            }
            return currentZip;
        }

        /// <summary>
        /// Zips one or more arrays into a single array using an enumerable-based fold operation.
        /// </summary>
        /// <typeparam name="TVal">The type of values in this array</typeparam>
        /// <typeparam name="TOther">The type of values in the enumerable of other arrays</typeparam>
        /// <typeparam name="TResult">The type of values in the resultant array</typeparam>
        /// <param name="array">This array</param>
        /// <param name="zipper">A function which, given an element from this array and an enumerable of elements from 
        /// the other arrays, returns a value in the resultant array.</param>
        /// <param name="others">An enumerable of other arrays.</param>
        /// <returns>A new array, the shape of which is the intersection of the shapes of this array and all the others, 
        /// and the values of which are the result of applying the zipper across all values at the corresponding indices 
        /// in all the arrays.</returns>
        public static TResult[] ZipEnum<TVal, TOther, TResult>(this TVal[] array, Func<TVal, IEnumerable<TOther>, TResult> zipper, IEnumerable<TOther[]> others)
        {
            int minLength = others.Select(o => o.Length).Union(array.Length.AsSingleton()).Min();            
            TResult[] result = new TResult[minLength];
            for(int i=0; i<minLength; i++)
            {
                TResult val = zipper(array[i], others.Select(o => o[i]));
                result[i] = val;
            }
            return result;
        }

        /// <summary>
        /// Creates a new array, populated by mapping the values of the old array using a mapper function.
        /// </summary>
        /// <param name="mapper">A function which maps an element in the original array to an element in the new array</param>
        /// <returns>A new array, whose values are the result of applying the mapper function to the associated element in the source array.</returns>
        public static TResult[] Map<TVal, TResult>(this TVal[] array, Func<TVal, TResult> mapper)
        {
            TResult func(TVal x, int i) => mapper(x);
            return array.Map(func);
        }

        /// <summary>
        /// Creates a new array, populated by mapping the values/indices of the old array using a mapper function.
        /// </summary>
        /// <param name="mapper">A function which maps an element and its index in the original array to an element in the new array.</param>
        /// <returns>A new array, whose values are the result of applying the mapper function to the associated element in the source array and its index.</returns>
        public static TResult[] Map<TVal, TResult>(this TVal[] array, Func<TVal, int, TResult> mapper)
        {
            int length = array.Length;
            TResult[] resultArray = new TResult[length];
            for (int i = 0; i < length; i++)
            {
                resultArray[i] = mapper(array[i], i);
            }
            return resultArray;
        }

        public static TVal FoldToSingleValue<TVal>(this TVal[] array, Func<TVal, TVal, TVal> foldFunction)
        {
            if (array.Length == 0)
            {
                throw new ArgumentOutOfRangeException("Cannot fold an empty array");
            }
            TVal result = array[0];
            for (int index = 1; index < array.Length; index++)
            {
                result = foldFunction(result, array[index]);
            }
            return result;
        }

        public static void AssertIndexIsInBounds<TVal>(this TVal[] array, int index)
        {
            int length = array.Length;
            if (index < 0 || index >= length)
            {
                throw new ArgumentOutOfRangeException(string.Format("Index {0} is outside of the bounds of the array. " +
                    "Should be in the range [{1}, {2}]", 0, length));
            }
        }

        /// <summary>
        /// Converts this array to a reverse list-based dictionary
        /// </summary>
        public static IListValuedDictionary<TVal, int> ConvertToReverseListValuedDictionary<TVal>(this TVal[] array, int indexOffset)
        {
            IListValuedDictionary<TVal, int> dict =
                array.ConvertToReverseCollectionValuedDictionary<TVal, IList<int>, IListValuedDictionary<TVal, int>>
                (() => new ListValuedDictionaryImpl<TVal, int>(), indexOffset);
            return dict;
        }


        /// <summary>
        /// Converts this array to a reverse set-based dictionary
        /// </summary>
        public static ISetValuedDictionary<TVal, int> ConvertToReverseSetValuedDictionary<TVal>(this TVal[] array, int indexOffset)
        {
            ISetValuedDictionary<TVal, int> dict =
                array.ConvertToReverseCollectionValuedDictionary<TVal, ISet<int>, ISetValuedDictionary<TVal, int>>
                (() => new SetValuedDictionaryImpl<TVal, int>(), indexOffset);
            return dict;
        }

        /// <summary>
        /// Converts this array to a reverse colleciton-based dictionary
        /// Each element is mapped to its pre-image under the normal index->value mapping of the array
        /// </summary>
        public static TDict ConvertToReverseCollectionValuedDictionary<TVal, TColl, TDict>
            (this TVal[] array, Func<TDict> newCollection, int indexOffset)
            where TColl : ICollection<int>
            where TDict : ICollectionValuedDictionary<TVal, int, TColl>
        {
            TDict dictionary = array.ConvertToDictAbstract(
                newCollection,
                (dict, i, val) => dict.Add(val, i), indexOffset);
            return dictionary;
        }

        /// <summary>
        /// Converts this array to a reverse dictionary, or throws an error if the array is not bijective
        /// </summary>
        public static IDictionary<TVal, int> ConvertToReverseDictionary<TVal>(this TVal[] array, int indexOffset)
        {
            IDictionary<TVal, int> dictionary
                = array.ConvertToDictAbstract(
                () => new Dictionary<TVal, int>(),
                (dict, i, val) => dict.Add(val, i), indexOffset);
            return dictionary;
        }

        /// <summary>
        /// Converts this array to a bijection dictionary, or throws an error if the array is not bijective
        /// </summary>
        public static IBijectionDictionary<int, TVal> ConvertToBijectionDictionary<TVal>(this TVal[] array, int indexOffset)
        {
            IBijectionDictionary<int, TVal> dictionary
                = array.ConvertToDictAbstract(
                () => new BijectionDictionaryImpl<int, TVal>(),
                (dict, i, val) => dict.Add(i, val), indexOffset);
            return dictionary;
        }

        /// <summary>
        /// Converts this array to a dictionary keyed on the indices of the array
        /// </summary>
        public static IDictionary<int, TVal> ConvertToDictionary<TVal>(this TVal[] array, int indexOffset)
        {
            IDictionary<int, TVal> dictionary
                = array.ConvertToDictAbstract(
                () => new Dictionary<int, TVal>(),
                (dict, i, val) => dict.Add(i, val), indexOffset);
            return dictionary;
        }
        
        /// <param name="indexOffset">Add this to array indices before storing them in the dictionary.</param>
        private static TDict ConvertToDictAbstract<TVal, TDict>(this TVal[] array,
            Func<TDict> newDict, Action<TDict, int, TVal> addToDict, int indexOffset)
        {
            TDict dictionary = newDict();
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    int dictIndex = indexOffset + i;
                    addToDict(dictionary, dictIndex, array[i]);
                }
                catch(Exception e)
                {
                    throw new InvalidOperationException(string.Format(
                        "Error trying to add array element at index {0} to dictionary", i), e);
                }
            }
            return dictionary;
        }
    }
}
