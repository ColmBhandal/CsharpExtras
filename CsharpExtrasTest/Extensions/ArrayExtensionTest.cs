using CsharpExtras.Extensions;
using CsharpExtras.Dictionary;
using CsharpExtras.RandomDataGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Dictionary.Collection;

namespace CustomExtensions
{
    [TestFixture]
    public class ArrayExtensionTest
    {
        [Test, Category("Unit")]
        public void GivenTwoArraysWithLongerValArrayWhenZippedToMultiValueMapThenAllExpectedMappingsAreInMap()
        {
            int[] keys = new int[] { 1, 2, 1 };
            string[] vals = new string[] { "One", "Two", "Three", "dasfash", "One" };
            ISetDictionary<int, string> zipped = keys.ZipToSetDictionary(vals);
            Assert.AreEqual(2, zipped.Count, "Expected the resultant array count to be equal to the minimum of the key & value array counts");
            Assert.AreEqual(2, zipped[1].Count);
            Assert.AreEqual(1, zipped[2].Count);
        }

        [Test, Category("Unit")]
        public void GivenTwoArraysWithLongerKeyArrayWhenZippedToMultiValueMapThenAllExpectedMappingsAreInMap()
        {
            int[] keys = new int[] { 1, 2, 1, 3, 4, 5, 1, 2 };
            string[] vals = new string[] { "One", "Two", "Three" };
            ISetDictionary<int, string> zipped = keys.ZipToSetDictionary(vals);
            Assert.AreEqual(2, zipped.Count, "Expected the resultant array count to be equal to the minimum of the key & value array counts");
            Assert.AreEqual(2, zipped[1].Count);
            Assert.AreEqual(1, zipped[2].Count);
            Assert.False(zipped.ContainsKey(3));
        }

        [Test, Category("Unit")]
        public void GivenArrayOfUniqueValuesAndOtherArrayOfDifferentLengthWhenZippedToDictionaryThenAllExpectedMappingsAreInDictionary()
        {
            int[] keys = new int[] { 1, 2 };
            string[] vals = new string[] { "One", "Two", "Three", "dasfash"};
            IDictionary<int, string> zipped = keys.ZipToDictionary(vals);
            Assert.AreEqual(2, zipped.Count, "Expected the resultant array count to be equal to the minimum of the key & value array counts");
            Assert.AreEqual("One", zipped[1]);
            Assert.AreEqual("Two", zipped[2]);
        }
        
        [Test, Category("Unit")]
        public void GivenArraysOfStringsOfDifferingLengthsWhenZipArrayWithConcatThenArrayOfConcatsOfShortestLengthReturned()
        {
            string[] first = new string[] { "Hel", "wo", "adfs", "35230j" };
            string[] second = new string[] { "lo", "rld!"};
            string[] zipped = first.ZipArray((s1, s2) => s1 + s2, second);
            Assert.AreEqual(zipped, new string[] { "Hello", "world!" });
        }

        [Test, Category("Unit")]
        public void GivenArraysRepresentingRowsOfLettersWhenZipMultiWithConcatThenSingleArrayOfColumnsIsReturned()
        {
            string[] row1 = new string[] { "H", "w" };
            string[] row2 = new string[] { "e", "o" };
            string[] row3 = new string[] { "l", "r" };
            string[] row4 = new string[] { "l", "l" };
            string[] row5 = new string[] { "o", "d" };
            string[] row6 = new string[] { "", "!" };
            string[] zipped = row1.ZipMulti((s1, s2) => s1 + s2, row2, row3, row4, row5, row6);
            Assert.AreEqual(zipped, new string[] { "Hello", "world!" });
        }
        
        [Test, Category("Unit")]
        public void TestGivenArrayWithMultipleElementsWhenMapAppliedWithKnownFunctionThenArrayWithMappedelementsReturned()
        {
            string[] arr = new string[] { "a", "aa", "aas", "sdfs", " )0Pr" };
            int[] mapped = arr.Map(s => s.Length);
            Assert.AreEqual(new int[] {1, 2, 3, 4, 5}, mapped);
        }

        private static IEnumerable<string[]> ArrayProviderForSubArrayTesting()
        {
            return new List<string[]> {
                new string[] { "a1", "a2", "a3", "a4", "a5" },
                new string[] {  },
                new string[] { "a1" },
                new string[] { "a1", "a2" },
            };
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ArrayProviderForSubArrayTesting")]
        public void TestGivenStringArrayWhenGetSubArrayThenCorrectSizeReturned(string[] arr)
        {
            Assert.NotNull(arr, "GIVEN: String array should not be null");

            string[] sub = arr.SubArray(0);
            Assert.AreEqual(arr.Length, sub.Length,
                "Expecting sub array to have the same length as source array");

            if (arr.Length > 0)
            {
                sub = arr.SubArray(1);
                Assert.AreEqual(arr.Length - 1, sub.Length,
                    "Expecting sub array to be shorter than the source array by 1");

                sub = arr.SubArray(0, arr.Length - 1);
                Assert.AreEqual(arr.Length - 1, sub.Length,
                    "Expecting sub array to be shorter than the source array by 1");
            }

            if (arr.Length > 1)
            {
                sub = arr.SubArray(2);
                Assert.AreEqual(arr.Length - 2, sub.Length,
                    "Expecting sub array to be shorter than the source array by 2");

                sub = arr.SubArray(1, arr.Length - 1);
                Assert.AreEqual(arr.Length - 2, sub.Length,
                    "Expecting sub array to be shorter than the source array by 2");
            }
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ArrayProviderForSubArrayTesting")]
        public void TestGivenStringArrayWhenGetSubArrayThenCorrectDataReturned(string[] arr)
        {
            Assert.NotNull(arr, "GIVEN: String array should not be null");

            string[] sub = arr.SubArray(0);
            Assert.AreEqual(arr, sub,
                "Expecting sub array to equal source array");

            if (arr.Length > 1)
            {
                sub = arr.SubArray(1);
                Assert.AreEqual(arr[1], sub[0],
                    "Expecting sub array to start at the right value");
            }
            if (arr.Length > 2)
            {
                sub = arr.SubArray(1, arr.Length - 1);
                Assert.AreEqual(arr[1], sub[0],
                    "Expecting sub array to start at the right value");
                Assert.AreEqual(arr[^2], sub[^1],
                    "Expecting sub array to end at the right value");
            }
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ArrayProviderForSubArrayTesting")]
        public void TestGivenStringArrayWhenGetSubArrayWithInvalidValuesThenFullArrayReturned(string[] arr)
        {
            Assert.NotNull(arr, "GIVEN: String array should not be null");

            string[] sub = arr.SubArray(-1);
            Assert.AreEqual(arr.Length, sub.Length,
                "Expecting sub array to have the same length as source array");

            sub = arr.SubArray(0, arr.Length + 1);
            Assert.AreEqual(arr.Length, sub.Length,
                "Expecting sub array to have the same length as source array");

            sub = arr.SubArray(-1, arr.Length + 1);
            Assert.AreEqual(arr.Length, sub.Length,
                "Expecting sub array to have the same length as source array");
        }

        //non-mvp: Add test for getting subarray from out of bounds on only one side. 
        // For example: -1 to the middle of the source array

        private static IEnumerable<(string[] Result, string[] ExpectedBlanks)> TupleArrayProviderForRemoveBlanksTesting()
        {
            return new List<(string[] input, string[] expected)>
            {
                (input: new string[] {"a1", "", "a3", "a4", "  "}, expected: new string[] {"a1", "a3", "a4"}),
                (input: new string[] { }, expected: new string[] {}),
                (input: new string[] {"a1"}, expected: new string[] {"a1"}),
                (input: new string[] {""}, expected: new string[] {}),
                (input: new string[] {"  ", "a2"}, expected: new string[] {"a2"}),
            };
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("TupleArrayProviderForRemoveBlanksTesting")]
        public void TestGivenStringArrayWhenRemoveBlankEntriesCalledThenAllBlankEntriesAreRemoved((string[] input, string[] expected) tuple)
        {
            Assert.NotNull(tuple.input, "GIVEN: String array should not be null");

            string[] cleaned = tuple.input.RemoveBlankEntries();
            
            Assert.AreEqual(tuple.expected,cleaned, "Array should be equal to expected array");
            Assert.AreEqual(0, cleaned.Count(s => string.IsNullOrWhiteSpace(s)),
                "No blank values should exist in the cleaned array");
        }

        [Test]
        [Category("Unit")]
        public void TestGivenArrayWithDuplicatesWhenFindDuplicatesThenCorrectRowsIdentified()
        {
            string[] array = new string[] {
                "0",
                "a",
                "b",
                "a",
                "c",
                "a",
                "d"
            };

            var duplicates = array.FindDuplicateIndices();

            Assert.NotNull(duplicates);
            Assert.AreEqual(1, duplicates.Count);
            Assert.AreEqual(3, duplicates["a"].Count);
            Assert.AreEqual(new int[] { 1, 3, 5 }, duplicates["a"].ToArray());
        }

        [Test]
        [Category("Manual")]
        [Category("Performance")]
        // Initial test run found duplicates in 20.000 items in an average of 1ms (this is a manual test for now)
        // TODO: Automate test to automatically check against an expected average time
        public void TestGivenArrayWithLargeNumberOfElementsWhenCheckForDuplicatesThenCheckIsFast()
        {
            int arraySize = 20_000;
            int stringLength = 5;
            int repetitions = 100;

            string[] arr = PopulateRandomStringArray(arraySize, stringLength);

            for (int i = 0; i < repetitions; i++)
            {
                IDictionary<string, IList<int>> duplicates = FindDuplicates(arr);
                Assert.NotNull(duplicates, "Find duplicates should not return null");
            }
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForFoldingArrayToSingleValue")]
        public void Given_Array_When_FoldingToSingleValue_Then_CorrectValueReturned(
            string[] data, Func<string, string, string> foldFunction, string expectedResult)
        {
            string result = data.FoldToSingleValue(foldFunction);
            Assert.AreEqual(expectedResult, result, "Folded value should equal expected value");
        }

        private static IEnumerable<object[]> ProviderForFoldingArrayToSingleValue()
        {
            return new List<object[]> {
                new object[] { new string[] { "a", "b" }, (Func<string, string, string>)((a, b) => a + b), "ab" },
                new object[] { new string[] { "a", "b", "c" }, (Func<string, string, string>)((a, b) => a + b), "abc" },
                new object[] { new string[] { "a" }, (Func<string, string, string>)((a, b) => a + b), "a" },
            };
        }

        private string[] PopulateRandomStringArray(int length, int stringLength)
        {
            IRandomStringGenerator stringGen = new RandomStringGeneratorImpl();
            string[] arr = new string[length];

            for (int i = 0; i < length; i++)
            {
                arr[i] = stringGen.RandomAlphaNumericMixedCaseString(stringLength);
            }
            return arr;
        }

        private IDictionary<string, IList<int>> FindDuplicates(string[] array)
        {
            return array.FindDuplicateIndices();
        }
    }
}
