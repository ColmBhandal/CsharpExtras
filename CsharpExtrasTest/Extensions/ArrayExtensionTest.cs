using CsharpExtras.Extensions;
using CsharpExtras.Map.Dictionary;
using CsharpExtras.RandomDataGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Map.Dictionary.Collection;

namespace CustomExtensions
{
    [TestFixture, Category("Unit")]
    public class ArrayExtensionTest
    {
        [Test, TestCase(0), TestCase(1), TestCase(7)]
        public void Given_BijectiveArrayOfBytes_When_ConvertToReverseListDictionary_ThenDictionaryPairsAreAsExpected(int offset)
        {
            //Assemble
            byte[] array = new byte[] { 1, 2, 1, 2, 1};

            //Act
            IListValuedDictionary<byte, int> dict = array.ConvertToReverseListValuedDictionary(offset);

            //Assert
            IEnumerable<(byte Key, IList<int> Value)> actualPairs = dict.AsEnumerable().Select(p => (p.Key, p.Value));            
            IList<int> list1 = new List<int>(); list1.Add(offset); list1.Add(offset + 2); list1.Add(offset + 4);
            List<int> list2 = new List<int>(); list2.Add(offset+1); list2.Add(offset + 3);
            IEnumerable<(byte Key, IList<int> Value)> expectedPairs = new (byte Key, IList<int> Value)[]
            {(1, list1), (2, list2)};
            Assert.AreEqual(expectedPairs, actualPairs);
        }

        [Test, TestCase(0), TestCase(1), TestCase(7)]
        public void Given_BijectiveArrayOfBytes_When_ConvertToBijectionDictionary_ThenBothDirectionsInResultAreAsExpected(int offset)
        {
            //Assemble
            byte[] array = new byte[] { 1, 2, 3, 5, 8 };

            //Act
            IBijectionDictionary<int, byte> biDict = array.ConvertToBijectionDictionary(offset);

            //Assert

            //Forwards
            IEnumerable<(int Key, byte Value)> actualPairsFwd = biDict.AsEnumerable().Select(p => (p.Key, p.Value));
            IEnumerable<(int index, byte val)> expectedPairsFwd = new (int index, byte val)[]
            {(offset, 1), (offset+1, 2), (offset+2, 3), (offset+3, 5), (offset+4, 8)};
            Assert.AreEqual(expectedPairsFwd, actualPairsFwd);

            //Backwards
            IBijectionDictionary<byte, int> reverse = biDict.Reverse;
            IEnumerable<(byte Key, int Value)> actualPairsRev = reverse.AsEnumerable().Select(p => (p.Key, p.Value));
            IEnumerable<(byte Key, int Value)> expectedPairsRev = new (byte Key, int Value)[]
            {(1, offset), (2, offset+1), (3, offset+2), (5, offset+3), (8, offset+4)};
            Assert.AreEqual(expectedPairsRev, actualPairsRev);
        }

        [Test, TestCase(0), TestCase(1), TestCase(7)]
        public void Given_BijectiveArrayOfBytes_When_ConvertToReverseDictionary_ThenDictionaryPairsAreAsExpected(int offset)
        {
            //Assemble
            byte[] array = new byte[] { 1, 2, 3, 5, 8 };

            //Act
            IDictionary<byte, int> dict = array.ConvertToReverseDictionary(offset);

            //Assert
            IEnumerable<(byte Key, int Value)> actualPairs = dict.AsEnumerable().Select(p => (p.Key, p.Value));
            IEnumerable<(byte Key, int Value)> expectedPairs = new (byte Key, int Value)[]
            {(1, offset), (2, offset+1), (3, offset+2), (5, offset+3), (8, offset+4)};
            Assert.AreEqual(expectedPairs, actualPairs);
        }

        [Test, TestCase(0), TestCase(1), TestCase(7)]
        public void Given_ArrayOfBytes_When_ConvertToDictionary_ThenDictionaryPairsAreAsExpected(int offset)
        {
            //Assemble
            byte[] array = new byte[] { 1, 2, 3, 5, 8};

            //Act
            IDictionary<int, byte> dict = array.ConvertToDictionary(offset);

            //Assert
            IEnumerable<(int Key, byte Value)> actualPairs = dict.AsEnumerable().Select(p => (p.Key, p.Value));
            IEnumerable<(int index, byte val)> expectedPairs = new (int index, byte val)[]
            {(offset, 1), (offset+1, 2), (offset+2, 3), (offset+3, 5), (offset+4, 8)};
            Assert.AreEqual(expectedPairs, actualPairs);
        }
        
        [Test]
        public void GivenTwoArraysWithLongerValArrayWhenZippedToMultiValueMapThenAllExpectedMappingsAreInMap()
        {
            //Arrange
            int[] keys = new int[] { 1, 2, 1 };
            string[] vals = new string[] { "One", "Two", "Three", "dasfash", "One" };
            
            //Act
            ISetValuedDictionary<int, string> zipped = keys.ZipToSetDictionary(vals);
            
            //Assert
            Assert.AreEqual(2, zipped.Count, "Expected the resultant array count to be equal to the minimum of the key & value array counts");
            Assert.AreEqual(2, zipped[1].Count);
            Assert.AreEqual(1, zipped[2].Count);
        }

        [Test]
        public void GivenTwoArraysWithLongerKeyArrayWhenZippedToMultiValueMapThenAllExpectedMappingsAreInMap()
        {
            //Arrange
            int[] keys = new int[] { 1, 2, 1, 3, 4, 5, 1, 2 };
            string[] vals = new string[] { "One", "Two", "Three" };
            
            //Act
            ISetValuedDictionary<int, string> zipped = keys.ZipToSetDictionary(vals);
            
            //Assert
            Assert.AreEqual(2, zipped.Count, "Expected the resultant array count to be equal to the minimum of the key & value array counts");
            Assert.AreEqual(2, zipped[1].Count);
            Assert.AreEqual(1, zipped[2].Count);
            Assert.False(zipped.ContainsKey(3));
        }

        [Test]
        public void GivenArrayOfUniqueValuesAndOtherArrayOfDifferentLengthWhenZippedToDictionaryThenAllExpectedMappingsAreInDictionary()
        {
            //Arrange
            int[] keys = new int[] { 1, 2 };
            string[] vals = new string[] { "One", "Two", "Three", "dasfash"};
            
            //Act
            IDictionary<int, string> zipped = keys.ZipToDictionary(vals);
            
            //Assert
            Assert.AreEqual(2, zipped.Count, "Expected the resultant array count to be equal to the minimum of the key & value array counts");
            Assert.AreEqual("One", zipped[1]);
            Assert.AreEqual("Two", zipped[2]);
        }
        
        [Test]
        public void GivenArraysOfStringsOfDifferingLengthsWhenZipArrayWithConcatThenArrayOfConcatsOfShortestLengthReturned()
        {
            //Arrange
            string[] first = new string[] { "Hel", "wo", "adfs", "35230j" };
            string[] second = new string[] { "lo", "rld!"};
            
            //Act
            string[] zipped = first.ZipArray((s1, s2) => s1 + s2, second);
            
            //Assert
            Assert.AreEqual(zipped, new string[] { "Hello", "world!" });
        }

        [Test]
        public void GivenArraysRepresentingRowsOfLettersWhenZipMultiWithConcatThenSingleArrayOfColumnsIsReturned()
        {
            //Arrange
            string[] row1 = new string[] { "H", "w" };
            string[] row2 = new string[] { "e", "o" };
            string[] row3 = new string[] { "l", "r" };
            string[] row4 = new string[] { "l", "l" };
            string[] row5 = new string[] { "o", "d" };
            string[] row6 = new string[] { "", "!" };
            
            //Act
            string[] zipped = row1.ZipMulti((s1, s2) => s1 + s2, row2, row3, row4, row5, row6);
            
            //Assert
            Assert.AreEqual(zipped, new string[] { "Hello", "world!" });
        }
        
        [Test]
        public void TestGivenArrayWithMultipleElementsWhenMapAppliedWithKnownFunctionThenArrayWithMappedelementsReturned()
        {
            //Arrange
            string[] arr = new string[] { "a", "aa", "aas", "sdfs", " )0Pr" };
            
            //Act
            int[] mapped = arr.Map(s => s.Length);
            
            //Assert
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
            //Arrange
            int arraySize = 20_000;
            int stringLength = 5;
            int repetitions = 100;

            //Act
            string[] arr = PopulateRandomStringArray(arraySize, stringLength);

            //Assert
            for (int i = 0; i < repetitions; i++)
            {
                IDictionary<string, IList<int>> duplicates = FindDuplicates(arr);
                Assert.NotNull(duplicates, "Find duplicates should not return null");
            }
        }

        [Test]
        [TestCaseSource("ProviderForFoldingArrayToSingleValue")]
        public void Given_Array_When_FoldingToSingleValue_Then_CorrectValueReturned(
            string[] data, Func<string, string, string> foldFunction, string expectedResult)
        {
            //Act
            string result = data.FoldToSingleValue(foldFunction);
            
            //Assert
            Assert.AreEqual(expectedResult, result, "Folded value should equal expected value");
        }

        [Test]
        public void Given_MultiDimensionalArray_When_ConvertedToJaggedArray_Then_CorrectValueReturned()
        {
            //Arrange
            int[][] expectedArray = new int[][] { new int[] { 1,2}, new int[] { 3,4} };
            int[,] multiArray = new int[2, 2] { { 1,2},{3,4 } };

            //Act
            int[][] actualArray = ArrayExtension.ConvertToJaggedArray(multiArray);

            //Assert
            for(int i=0; i < actualArray.Length; i++)
            {
                for(int j=0; j <= i; j++)
                {
                    Assert.AreEqual(actualArray[i][j], expectedArray[i][j]);
                }
            }
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
            //Act
            IRandomStringGenerator stringGen = new RandomStringGeneratorImpl();
            string[] arr = new string[length];

            //Assert
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
