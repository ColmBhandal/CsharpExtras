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
        [TestCaseSource("ProviderForFindFirstOccurrenceOfSet_WithArrayAndSet")]
        public void Given_Array_When_FindFirstOccurrenceOfSet_Then_CorrectValueReturned(int[] arr,ISet<int> set,int expectedIndex, int expectedElement)
        {
            //Act
            var actualResult = ArrayExtension.FindFirstOccurrenceOfSet(arr, set, 0, arr.Length-1);

            //Assert
            if(actualResult is (int index, int element))
            {
                Assert.AreEqual(expectedIndex, index);
                Assert.AreEqual(expectedElement, element);
            }
            else if(actualResult is null)
            {
                Assert.Fail("Null tuple returned");
            }
            else
            {
                Assert.Fail("There's no way this can get reached so the universe has probably imploded.");
            }
        }

        [Test]
        [TestCaseSource("ProviderForFindFirstOccurrenceOfSetNoMatches_WithArrayAndSet")]
        public void Given_Array_When_FindFirstOccurrenceOfSetWithNoMatches_Then_NullReturned(int[] arr, ISet<int> set)
        {
            //Act
            var actualResult = ArrayExtension.FindFirstOccurrenceOfSet(arr, set, 0, arr.Length - 1);

            //Assert
            Assert.IsNull(actualResult, "Expected null tuple returned when there are no matches for set in array");
        }

        [Test]
        [TestCaseSource("ProviderForTo2DArray_With_1DArray_Expected2DArray_Orientation")]
        public void Given_Array_When_To2DArray_Then_CorrectValueReturned(int[] array1D, int[,] expectedArray, ArrayOrientationClass.ArrayOrientation arrayOrientation)
        {
            //Act
            int[,] actualArray = ArrayExtension.To2DArray(array1D, arrayOrientation);

            //Assert
            Assert.AreEqual(expectedArray, actualArray);
        }

        [Test]
        [TestCaseSource("ProviderForDeepCopy_With_Array")]
        public void Given_Array_When_DeepCopy_Then_CorrectValueReturned(int[] expectedArray)
        {
            //Act
            int[] actualArray = ArrayExtension.DeepCopy(expectedArray);

            //Assert
            Assert.AreEqual(expectedArray, actualArray);

            //condition to avoid the IndexOutOfRangeException for the empty array test case
            if (actualArray.Length > 0)
            {
                actualArray[0] += 20;
                actualArray[1] += 10;
                actualArray[2] += 15;
                Assert.AreNotEqual(expectedArray, actualArray,"When the resultant array is modified, the source array shouldn't be modified");
            }
            
        }

        [Test]
        [TestCaseSource("ProviderForInverse_With_Array_ResultingDictionary")]
        public void Given_Array_When_Inverse_Then_CorrectValueReturned(int[] array, IDictionary<int, IList<int>> expected)
        {
            //Act
            IDictionary<int, IList<int>> actual = ArrayExtension.Inverse(array);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_ThreeArraysOfStrings_When_ZipArray_Then_CorrectValueReturned()
        {
            //Arrange
            string[] first = new string[] { "H", "L" };
            string[] second = new string[] { "E", "O" };
            string[] third = new string[] { "L", "!" };
            string[] expected = new string[] { "HEL","LO!" };
            //Act
            string[] actual = ArrayExtension.ZipArray(first,(s1, s2,s3) => s1 + s2+ s3, second,third);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_ThreeArraysOfStringsWithDifferentLengths_When_ZipArray_Then_CorrectValueReturnedWithSmallestArraySize()
        {
            //Arrange
            string[] first = new string[] { "H", "L" };
            string[] second = new string[] { "E", "O","1","A" };
            string[] third = new string[] { "L", "!","A","B","C","D" };
            string[] expected = new string[] { "HEL", "LO!" };
            //Act
            string[] actual = ArrayExtension.ZipArray(first, (s1, s2, s3) => s1 + s2 + s3, second, third);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_FourArraysOfStrings_When_ZipArray_Then_CorrectValueReturned()
        {
            //Arrange
            string[] first = new string[] { "H", "O" };
            string[] second = new string[] { "E", "!" };
            string[] third = new string[] { "L", "H" };
            string[] fourth = new string[] { "L", "I" };
            string[] expected = new string[] { "HELL", "O!HI" };

            //Act
            string[] actual = ArrayExtension.ZipArray(first, (s1, s2, s3, s4) => s1 + s2 + s3+s4, second, third,fourth);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Given_FourArraysOfStringsWithDifferentLenghts_When_ZipArray_Then_CorrectValueReturnedWithSmallestArraySize()
        {
            //Arrange
            string[] first = new string[] { "H", "O" };
            string[] second = new string[] { "E", "!","B","c" };
            string[] third = new string[] { "L", "H","d","fg" };
            string[] fourth = new string[] { "L", "I","asdf" };
            string[] expected = new string[] { "HELL", "O!HI" };

            //Act
            string[] actual = ArrayExtension.ZipArray(first, (s1, s2, s3, s4) => s1 + s2 + s3 + s4, second, third, fourth);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        private static IEnumerable<object[]> ProviderForFoldingArrayToSingleValue()
        {
            return new List<object[]> {
                new object[] { new string[] { "a", "b" }, (Func<string, string, string>)((a, b) => a + b), "ab" },
                new object[] { new string[] { "a", "b", "c" }, (Func<string, string, string>)((a, b) => a + b), "abc" },
                new object[] { new string[] { "a" }, (Func<string, string, string>)((a, b) => a + b), "a" },
            };
        }

        private static IEnumerable<object[]> ProviderForFindFirstOccurrenceOfSet_WithArrayAndSet()
        {
            return new List<object[]> {
                new object[] {new int[] { 1,2,3,4}, new HashSet<int>(new int[] { 2,3,4}) ,1,2}
            };
        }
        private static IEnumerable<object[]> ProviderForFindFirstOccurrenceOfSetNoMatches_WithArrayAndSet()
        {
            return new List<object[]> {
                new object[] {new int[] { 1,2,3,4}, new HashSet<int>(new int[] { 7, 42, 128})}
            };
        }

        private static IEnumerable<object[]> ProviderForTo2DArray_With_1DArray_Expected2DArray_Orientation()
        {
            return new List<object[]>
            {
                new object[] {new int[] {1,2,3,4,5}, new int[1,5] { { 1,2,3,4,5} },ArrayOrientationClass.ArrayOrientation.COLUMN },
                new object[] { new int[] {1,2,3 }, new int[3,1] { { 1},{ 2},{ 3} } ,ArrayOrientationClass.ArrayOrientation.ROW}
            };
        }
        private static IEnumerable<object[]> ProviderForDeepCopy_With_Array()
        {
            return new List<object[]>
            {
                new object[] {new int[] {1,2,3}},
                new object[] { new int[] { }}
            };
        }

        private static IEnumerable<object[]> ProviderForInverse_With_Array_ResultingDictionary()
        {
            return new List<object[]>
            {
                new object[] { new int[] { 1, 2, 3, 4 }, new Dictionary<int, IList<int>>
            {
                { 1, new List<int> { 0 } },
                { 2, new List<int> { 1 } },
                { 3, new List<int> { 2 } },
                { 4, new List<int> { 3 } }
            } },
                new object[] {new int[] {  }, new Dictionary<int, IList<int>>() }
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
