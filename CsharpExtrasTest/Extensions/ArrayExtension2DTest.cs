using CsharpExtras.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CsharpExtrasTest.Extensions
{
    [TestFixture]
    public class ArrayExtension2DTest
    {
        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForFoldingArrayToSingleRow")]
        public void Given_MultiDimensionalArray_When_FoldingToSingleRow_Then_CorrectValueReturned(
            string[,] data, Func<string, string, string> foldFunction, string[] expectedResult)
        {
            //Act
            string[] result = data.FoldToSingleRow(foldFunction);

            //Assert
            Assert.AreEqual(expectedResult, result, "Folded row should equal expected value");
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForFoldingArrayToSingleColumn")]
        public void Given_MultiDimensionalArray_When_FoldingToSingleColumn_Then_CorrectValueReturned(
            string[,] data, Func<string, string, string> foldFunction, string[] expectedResult)
        {
            //Act
            string[] result = data.FoldToSingleColumn(foldFunction);

            //Assert
            Assert.AreEqual(expectedResult, result, "Folded column should equal expected value");
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForCheckAnyMatch")]
        public void Given_MultidimensionalArray_When_CheckingThatAnyMatch_Then_CorrectValueReturned(
            string[,] data, Func<string, bool> checkerFunction, bool expectedResult)
        {
            //Act
            bool result = data.Any(checkerFunction);

            //Assert
            Assert.AreEqual(expectedResult, result, "Check for any should return the correct result");
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForCheckAllMatch")]
        public void Given_MultidimensionalArray_When_CheckingThatAllMatch_Then_CorrectValueReturned(
            string[,] data, Func<string, bool> checkerFunction, bool expectedResult)
        {
            //Act
            bool result = data.All(checkerFunction);

            //Assert
            Assert.AreEqual(expectedResult, result, "Check that all match should return the correct result");
        }

        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForCountMatches")]
        public void Given_MultidimensionalArray_When_CountingMatches_Then_CorrectValueReturned(
            string[,] data, Func<string, bool> checkerFunction, int expectedResult)
        {
            //
            //Act
            int result = data.Count(checkerFunction);

            //Assert
            Assert.AreEqual(expectedResult, result, "Counting matches should return the correct result");
        }


        [Test]
        [Category("Unit")]
        public void Given2dArrayWhenMappedThenResultantArrayIsAsExpected()
        {
            //Arrange
            string[,] grid = new string[,] { { "H", "el", "lo " }, { "Wor", "ld", "!" } };

            //Act
            int[,] charCount = grid.Map(s => s.Length);

            //Assert
            Assert.AreEqual(1, charCount[0, 0]);
            Assert.AreEqual(2, charCount[0, 1]);
            Assert.AreEqual(3, charCount[0, 2]);
            Assert.AreEqual(3, charCount[1, 0]);
            Assert.AreEqual(2, charCount[1, 1]);
            Assert.AreEqual(1, charCount[1, 2]);
        }

        [Test]
        [Category("Unit")]
        public void TestGiven2DArrayInputWhenTransposedThenArrayDimensionsAreFlipped()
        {
            //Act
            IList<string[,]> testData = GenerateArrayTranspositionTestData();

            //Assert
            foreach (string[,] testCase in testData)
            {
                AssertTransposedArrayFlipsDimensions(testCase);
            }
        }
        private IList<string[,]> GenerateArrayTranspositionTestData()
        {
            IList<string[,]> testData = new List<string[,]>
            {
                new string[,] { { "1", "2" }, { "3", "4" } },
                new string[,] { { "1" }, { "2" }, { "3" } },
                new string[,] { { "1", "2", "3" } },
                new string[,] { { "1", "2", "3" }, { "4", "5", "6" } }
            };
            return testData;
        }

        private void AssertTransposedArrayFlipsDimensions(string[,] input)
        {
            //Act
            string[,] transposed = input.Transpose();

            //Assert
            Assert.AreEqual(transposed.GetLength(0), input.GetLength(1));
            Assert.AreEqual(transposed.GetLength(1), input.GetLength(0));
        }

        [Test]
        [Category("Unit")]
        public void TestGiven2DArrayInputWhenTransposedThenArrayDataIsTransposed()
        {
            //Act
            IList<string[,]> testData = GenerateArrayTranspositionTestData();

            //Assert
            foreach (var testCase in testData)
            {
                AssertTransposedArrayFlipsData(testCase);
            }
        }

        private void AssertTransposedArrayFlipsData(string[,] input)
        {
            string[,] transposed = input.Transpose();

            for (int i = 0; i < transposed.GetLength(0); i++)
            {
                for (int j = 0; j < transposed.GetLength(1); j++)
                {
                    Assert.AreEqual(transposed[i, j], input[j, i]);
                }
            }
        }

        [Test, Category("Unit")]
        public void Given2DArraysOfStringsWhenZipArrayWithConcatThen2DArrayReturned()
        {
            //Arrange
            string[,] first = new string[,] { { "Hel", "wo" }, { "1", "2" } };
            string[,] second = new string[,] { { "lo", "rld" }, { "3", "4" } };

            //Act
            string[,] zipped = first.ZipArray((s1, s2) => s1 + s2, second);

            //Assert
            Assert.AreEqual(zipped, new string[,] { { "Hello", "world" }, { "13", "24" } });
        }

        [Test, Category("Unit")]
        public void Given2DArraysOfStringsOfDifferentSizesWhenZipArrayWithConcatThen2DArrayReturnedWithArrayIntersection()
        {
            //Arrange
            string[,] first = new string[,] { { "Hel", "wo", "a" }, { "1", "2", "b" } };
            string[,] second = new string[,] { { "lo", "rld" }, { "3", "4" }, { "5", "6" } };

            //Act
            string[,] zipped = first.ZipArray((s1, s2) => s1 + s2, second);

            //Assert
            Assert.AreEqual(zipped, new string[,] { { "Hello", "world" }, { "13", "24" } });
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForWrite1DArrayTo2DRow")]
        public void Given_2DArrayOfBytes_When_Write1DArrayToRow_Then_ResultIsAsExpected
            (byte[,] data, byte[] dataToWrite, int row, int offset, byte[,] expected)
        {
            //Act
            data.WriteToRow(dataToWrite, row, offset);

            //Assert
            Assert.AreEqual(expected, data);
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForWrite1DArrayTo2DColumn")]
        public void Given_2DArrayOfBytes_When_Write1DArrayToColumn_Then_ResultIsAsExpected
           (byte[,] data, byte[] dataToWrite, int column, int offset, byte[,] expected)
        {
            //Act
            data.WriteToColumn(dataToWrite, column, offset);

            //Assert
            Assert.AreEqual(expected, data);
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForWriteToAreaTest")]
        public void Given_2DArrayOfBytes_When_WriteToArea_Then_ResultIsAsExpected
           (byte[,] data, byte[,] dataToWrite, int rowOffset, int columnOffset, byte[,] expected)
        {
            //Act
            data.WriteToArea(dataToWrite, rowOffset, columnOffset);

            //Assert
            Assert.AreEqual(expected, data,
                string.Format("Failure for (rowOffset, columnOffset) = ({0}, {1})", rowOffset, columnOffset));
        }

        [Test, Category("Unit")]
        [TestCase(-1), TestCase(2), TestCase(462)]
        public void Given_2DArrayOfInts_When_InvalidRowPassedToWrite_Then_ExceptionThrown(int row)
        {
            //Act
            int[,] data = new int[,] { { 1, 2, 3, 4 }, { 11, 12, 13, 14 } };

            //Assert
            Assert.Catch(() => data.WriteToRow(new int[] { }, row, 0),
                string.Format("Expected exception for row {0}", row));
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderForArrayToJaggedArray")]
        public void Given_MultiDimensionalArray_When_ConvertedToJaggedArray_Then_CorrectValueReturned(int[,] array, int[][] expectedArray)
        {
            //Act
            int[][] actualArray = ArrayExtension2D.ConvertToJaggedArray(array);

            //Assert
            Assert.AreEqual(expectedArray, actualArray);
        }

        [Test, Category("Unit")]
        [TestCase(-1), TestCase(4), TestCase(34)]
        public void Given_2DArrayOfInts_When_InvalidColumnPassedToWrite_Then_ExceptionThcolumnn(int column)
        {
            //Act
            int[,] data = new int[,] { { 1, 2, 3, 4 }, { 11, 12, 13, 14 } };

            //Assert
            Assert.Catch(() => data.WriteToColumn(new int[] { }, column, 0),
                string.Format("Expected exception for column {0}", column));
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderFor2DSubArrayTest")]
        public void Given_2DArrayOfBytes_When_SubArray_Then_ExpectedSubArrayReturned
            (byte[,] data, int startAtRow, int startAtColumn, int stopBeforeRow, int stopBeforeColumn, byte[,] expected)
        {
            //Act
            byte[,] sub = data.SubArray(startAtRow, startAtColumn, stopBeforeRow, stopBeforeColumn);

            //Assert
            Assert.AreEqual(expected, sub, string.Format(
                "Failure for sub array with coordinates ({0}, {1}) --> ({2}, {3}))",
                startAtRow, startAtColumn, stopBeforeRow, stopBeforeColumn));
        }

        [Test, TestCaseSource("ProviderForFirstIndexTupleOfTest")]
        public void Given_2DArrayOfBytes_When_GetIndexTupleOfMatcher_Then_ExpectedTupleReturned(
            Func<byte, bool> matcher, (int majorIndex, int minorIndex)? expectedTuple)
        {
            //Assemble
            byte[,] sourceArray = new byte[,]
            {
                {1, 2, 3 },
                {4, 5, 6},
                {7, 8, 9},
            };

            //Act
            (int majorIndex, int minorIndex)? actualTuple = sourceArray.FirstIndexTupleOf(matcher);

            //Assert
            Assert.AreEqual(expectedTuple, actualTuple);
        }

        #region Providers
        private static IEnumerable<object[]> ProviderForFirstIndexTupleOfTest()
        {
            //The test has a square array of number 1 - 9. The following test data assumes that.
            Func<byte, bool> oddNumberMatcher = b => b % 2 == 1;
            Func<byte, bool> evenNumberMatcher = b => b % 2 == 0;
            Func<byte, bool> matchNine = b => b == 9;
            Func<byte, bool> tryMatchNmberOutsideArray = b => b == 99;
            Func<byte, bool> neverMatch = b => false;
            Func<byte, bool> alwaysMatch = b => true;
            return new List<object[]>
            {
                new object[]{oddNumberMatcher, (0, 0)},
                new object[]{evenNumberMatcher, (0, 1)},
                new object[]{ matchNine, (2, 2)},
                new object[]{ tryMatchNmberOutsideArray, null},
                new object[]{ neverMatch, null},
                new object[]{ alwaysMatch, (0, 0)},
            };
        }

        private static IEnumerable<object[]> ProviderForWriteToAreaTest()
        {
            return new List<object[]> {
                new object[] { new byte[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } },
                    new byte[,] { { 101, 111, 121 }, { 102, 112, 122 }, { 103, 113, 123 }},
                    0, 0,
                    new byte[,] { { 101, 111, 121, 31 }, { 102, 112, 122, 32 }, { 103, 113, 123, 33 }, { 4, 14, 24, 34 } } },
                new object[] { new byte[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } },
                    new byte[,] { { 101, 111 }, { 102, 112 }, { 103, 113}},
                    1, 1,
                    new byte[,] { { 1, 11, 21, 31 }, { 2, 101, 111, 32 }, { 3, 102, 112, 33 }, { 4, 103, 113, 34 } } },
                new object[] { new byte[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } },
                    new byte[,] { { 101, 111, 121 }, { 102, 112, 122 }, { 103, 113, 123 }},
                    -2, -1,
                    new byte[,] { { 113, 123, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } } },
                new object[] { new byte[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } },
                    new byte[,] { { 101, 111, 121 }, { 102, 112, 122 }, { 103, 113, 123 }},
                    2, 2,
                    new byte[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 101, 111 }, { 4, 14, 102, 112 } },
                }
            };
        }

        private static IEnumerable<object[]> ProviderFor2DSubArrayTest()
        {
            return new List<object[]> {
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, -6, -3, 7, 5,
                    new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, 0, 0, 4, 2,
                    new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, 1, 1, 3, 2,
                    new byte[,] { { 12 }, { 13 } }},
                new object[] { new byte[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } },
                    1, 1, 3, 3,
                   new byte[,] { { 12, 22 }, { 13, 23 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, 1, 1, 3, 1,
                    new byte[2,0]},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, 1, 0, 1, 2,
                    new byte[0,2]},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, 2, 1, 2, 1,
                    new byte[0,0]},
            };
        }

        private static IEnumerable<object[]> ProviderForWrite1DArrayTo2DColumn()
        {
            return new List<object[]> {
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new byte[] {21, 22, 23}, 1, 1,
                    new byte[,] { { 1, 11 }, { 2, 21 }, { 3, 22 }, { 4, 23 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new byte[] {21, 22, 23}, 1, 0,
                    new byte[,] { { 1, 21 }, { 2, 22 }, { 3, 23 }, { 4, 14 }} },
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new byte[] { 1, 2, 3, 4 }, 0, 0,
                    new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new byte[] { 1, 2, 3, 4 }, 0, 2,
                    new byte[,] { { 1, 11 }, { 2, 12 }, { 1, 13 }, { 2, 14 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new byte[] { 21, 22, 23, 24 }, 0, 4,
                    new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }},
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new byte[] { 21, 22, 23, 24 }, 0, -1,
                    new byte[,] { { 22, 11 }, { 23, 12 }, { 24, 13 }, { 4, 14 } }},
            };
        }

        private static IEnumerable<object[]> ProviderForWrite1DArrayTo2DRow()
        {
            return new List<object[]> {
                new object[] { new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new byte[] {21, 22, 23}, 1, 1,
                    new byte[,] { { 1, 2, 3, 4 }, {11, 21, 22, 23 } } },
                new object[] { new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new byte[] {21, 22, 23}, 1, 0,
                    new byte[,] { { 1, 2, 3, 4 }, { 21, 22, 23, 14} }},
                new object[] { new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new byte[] { 1, 2, 3, 4 }, 0, 0,
                    new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }},
                new object[] { new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new byte[] { 1, 2, 3, 4 }, 0, 2,
                    new byte[,] { { 1, 2, 1, 2 }, {11, 12, 13, 14} }},
                new object[] { new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new byte[] { 21, 22, 23, 24 }, 0, 4,
                    new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }},
                new object[] { new byte[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new byte[] { 21, 22, 23, 24 }, 0, -1,
                    new byte[,] { { 22, 23, 24, 4 }, {11, 12, 13, 14} }},
            };
        }

        private static IEnumerable<object[]> ProviderForCheckAnyMatch()
        {
            return new List<object[]> {
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str == "a"), true },
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str == "A"), false },
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str .Length > 1), false }
            };
        }

        private static IEnumerable<object[]> ProviderForCheckAllMatch()
        {
            return new List<object[]> {
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str == "a"), false },
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str == "A"), false },
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str .Length == 1), true },
                new object[] { new string[,] { { "a", "b" }, { "12", "2" } }, (Func<string, bool>)(str => str .Length == 1), false }
            };
        }

        private static IEnumerable<object[]> ProviderForCountMatches()
        {
            return new List<object[]> {
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str == "a"), 1 },
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str == "C"), 0 },
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, bool>)(str => str .Length == 1), 4 },
                new object[] { new string[,] { { "a", "b" }, { "12", "2" } }, (Func<string, bool>)(str => str .Length == 1), 3 }
            };
        }

        private static IEnumerable<object[]> ProviderForFoldingArrayToSingleRow()
        {
            return new List<object[]> {
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, string, string>)((a, b) => a + b), new string[] { "a1", "b2" }, },
                new object[] { new string[,] { { "a", "b", "c" }, { "1", "2", "3" } }, (Func<string, string, string>)((a, b) => a + b), new string[] { "a1", "b2", "c3" }, },
                new object[] { new string[,] { { "a" }, { "1"} }, (Func<string, string, string>)((a, b) => a + b), new string[] { "a1" }, },
            };
        }

        private static IEnumerable<object[]> ProviderForFoldingArrayToSingleColumn()
        {
            return new List<object[]> {
                new object[] { new string[,] { { "a", "b" }, { "1", "2" } }, (Func<string, string, string>)((a, b) => a + b), new string[] { "ab", "12" }, },
                new object[] { new string[,] { { "a", "b", "c" }, { "1", "2", "3" } }, (Func<string, string, string>)((a, b) => a + b), new string[] { "abc", "123" }, },
                new object[] { new string[,] { { "a" }, { "1"} }, (Func<string, string, string>)((a, b) => a + b), new string[] { "a", "1" }, },
            };
        }

        private static IEnumerable<object[]> ProviderForArrayToJaggedArray()
        {
            return new List<object[]> {
                new object[] { new int[,] { { 1,2},{3,4 } } ,new int[][] { new int[] { 1, 2 }, new int[] { 3, 4 } }},
                new object[] { new int[,] {  }, new int[][] {  } },
                new object[] { new int[,] { { 1},{ 2}  }, new int[][] { new int[] { 1 }, new int[] { 2 } } }
            };
        }
        #endregion
    }
}
