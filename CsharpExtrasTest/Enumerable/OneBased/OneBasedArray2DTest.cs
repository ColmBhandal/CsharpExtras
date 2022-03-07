using CsharpExtras.Api;
using CsharpExtras._Enumerable.OneBased;
using NUnit.Framework;
using System;
using static CsharpExtras.Extensions.ArrayOrientationClass;
using static CsharpExtras.Extensions.ArrayExtension;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Compare;

namespace CsharpExtrasTest._Enumerable.OneBased
{
    [TestFixture, Category("Unit")]
    public class OneBasedArray2DTest
    {
        private const string One = "One";
        private const string Two = "Two";
        private const string Three = "Three";
        private const string Four = "Four";

        private readonly ICsharpExtrasApi _api = new CsharpExtrasApi();

        [Test, TestCaseSource(nameof(ProviderForUnequalCompare))]
        public void GIVEN_UnEqualArrays_WHEN_Compare_THEN_IsEqualFalse
            ((double[,] array1, double[,] array2, string label) testData)
        {
            //Arrange
            IOneBasedArray2D<double> array1 = _api.NewOneBasedArray2D(testData.array1);
            IOneBasedArray2D<double> array2 = _api.NewOneBasedArray2D(testData.array2);

            //Act
            IComparisonResult result = array1.Compare(array2,
                (d1, d2) => d1 == d2);

            //Assert
            Assert.IsFalse(result.IsEqual);
        }

        private static IEnumerable<(double[,] array1, double[,] array2, string label)>
            ProviderForUnequalCompare()
        {
            return new List<(double[,] array1, double[,] array2, string label)>
            {
                (
                    new double[,]
                    {
                        {1, 2, 3},
                        {1.1, 0.5, 6.01},
                    },
                    new double[,]
                    {
                        {1, 2, 3},
                        {1.1, 0.501, 6.01},
                    },
                    "Same shape, different value"
                ),

                (
                    new double[,]
                    {
                        {1, 2, 3},
                        {1.1, 0.5, 6.01},
                    },
                    new double[,]
                    {
                        {1, 2, 3},
                    },
                    "Different shape, other less rows"
                ),

                (
                    new double[,]
                    {
                        {1, 2, 3},
                        {1.1, 0.5, 6.01},
                    },
                    new double[,]
                    {
                        {1, 2},
                        {1.1, 0.5},
                    },
                    "Different shape, other less columns"
                )
            };
        }

        [Test]
        public void GIVEN_DistinctEqualPopulatedArrays_WHEN_Compare_THEN_IsEqualTrue()
        {
            //Arrange
            IOneBasedArray2D<string> array1 = _api.NewOneBasedArray2D(new string[,]{
                { "Hello", "World", "42" },
                { "Goodbye", "Universe", "43" }
            });
            IOneBasedArray2D<string> array2 = _api.NewOneBasedArray2D(new string[,]{
                { "Hello", "World", "42" },
                { "Goodbye", "Universe", "43" }
            });
            //Act
            IComparisonResult result = array1.Compare(array2, string.Equals);

            //Assert
            Assert.IsTrue(result.IsEqual);
        }

        [Test]
        public void GIVEN_DistinctEmptyArrays_WHEN_CompareWithFalseValueComparer_THEN_IsEqualTrue()
        {
            //Arrange
            IOneBasedArray2D<string> array1 = _api.NewOneBasedArray2D<string>(0, 0);
            IOneBasedArray2D<string> array2 = _api.NewOneBasedArray2D<string>(0, 0);

            //Act
            IComparisonResult result = array1.Compare(array2, (s1, s2) => false);

            //Assert
            Assert.IsTrue(result.IsEqual, "Distinct empty arrays should be equal, even if the comparison funciton always returns false");
        }

        [Test]
        public void GIVEN_ArrayAndEmptyOthers_WHEN_ZipEnumIgnoringOthers_THEN_ResultIsAsExpected()
        {
            //Arrange
            IOneBasedArray2D<string> array = _api.NewOneBasedArray2D(
                new string[,] { { "Zero", "One", "Two", "Three" } });
            IList<IOneBasedArray2D<int>> emptyOthers = new List<IOneBasedArray2D<int>>();
            static string func(string s, IEnumerable<int> e) => s;

            //Act
            IOneBasedArray2D<string> result = array.ZipEnum(func, emptyOthers);

            //Assert
            string[,] expected = new string[,] { { "Zero", "One", "Two", "Three" } };
            Assert.AreEqual(expected, result.ZeroBasedEquivalent);
        }

        [Test]
        public void GIVEN_ExceptionInZipAndEmptyArray_WHEN_ZipEnum_THEN_ResultIsEmpty()
        {
            //Arrange
            IOneBasedArray2D<string> array = _api.NewOneBasedArray2D<string>(0, 0);
            IList<IOneBasedArray2D<int>> others = new List<IOneBasedArray2D<int>>
            {
                _api.NewOneBasedArray2D(new int[,] { { 1, 2, 3 } })
            };
            static (string, int) func(string s, IEnumerable<int> e) =>
                throw new InvalidOperationException("Intentionally throwing exception for test");

            //Act
            IOneBasedArray2D<(string, int)> result = array.ZipEnum(func, others);

            //Assert
            (string, int)[,] expected = new (string, int)[0, 0];
            Assert.AreEqual(expected, result.ZeroBasedEquivalent);
        }

        [Test]
        public void GIVEN_ExceptionInZipAndEmptyOthers_WHEN_ZipEnum_THEN_Exception()
        {
            //Arrange
            IOneBasedArray2D<string> array = _api.NewOneBasedArray2D(new string[,] { { "Zero", "One", "Two" } });
            IList<IOneBasedArray2D<int>> others = new List<IOneBasedArray2D<int>>
            {
            };
            static (string, int) func(string s, IEnumerable<int> e) =>
                throw new InvalidOperationException("Intentionally throwing exception for test");

            //Act / Assert
            Assert.Throws<InvalidOperationException>(() => array.ZipEnum(func, others));
        }
        [Test]
        public void GIVEN_ExceptionInZipAndNonEmptyOthers_WHEN_ZipEnum_THEN_Exception()
        {
            //Arrange
            IOneBasedArray2D<string> array = _api.NewOneBasedArray2D(new string[,] { { "Zero", "One", "Two" } });
            IList<IOneBasedArray2D<int>> others = new List<IOneBasedArray2D<int>>
            {
                _api.NewOneBasedArray2D(new int[,] { { 1, 2, 3 } })
            };
            static (string, int) func(string s, IEnumerable<int> e) =>
                throw new InvalidOperationException("Intentionally throwing exception for test");

            //Act / Assert
            Assert.Throws<InvalidOperationException>(() => array.ZipEnum(func, others));
        }

        [Test]
        public void GIVEN_Arrays_WHEN_ZipEnum_THEN_ResultIsAsExpected()
        {
            //Arrange
            IOneBasedArray2D<string> array = _api.NewOneBasedArray2D(new string[,] {
                { "00", "01", "02", "03" },
                { "10", "11", "12", "13" },
                { "20", "21", "22", "23" },
                { "30", "31", "32", "33" },
            });
            IList<IOneBasedArray2D<int>> others = new List<IOneBasedArray2D<int>>
            {
                //Inentionally make some of the others different shapes to this one
                _api.NewOneBasedArray2D(new int[,] {
                    { 1, 2, 3, 4, 5 },
                    { 5, 6, 7, 8, 9 },
                    { 9, 10, 11, 12, 13 },
                    { 13, 14, 15, 16, 17 },
                }),
               _api.NewOneBasedArray2D(new int[,] {
                    { 1, 2, 3, 4,},
                    { 5, 6, 7, 8},
                    { 9, 10, 11, 12},
                    { 13, 14, 15, 16},
                    { 17, 18, 19, 20},
                }),
                _api.NewOneBasedArray2D(new int[,] {
                    { 1, 2, 3},
                    { 5, 6, 7},
                    { 9, 10, 11},
                }),
            };
            static (string, int) func(string s, IEnumerable<int> e) => (s, e.Aggregate((i, j) => i + j));

            //Act
            IOneBasedArray2D<(string, int)> result = array.ZipEnum(func, others);

            //Assert
            (string, int)[,] expected = new (string, int)[,] {
                { ("00", 3), ("01", 6), ("02", 9) },
                { ("10", 15), ("11", 18), ("12", 21) },
                { ("20", 27), ("21", 30), ("22", 33) }
            };
            Assert.AreEqual(expected, result.ZeroBasedEquivalent);
        }

        [Test]
        public void GIVEN_ArrayOfStrings_WHEN_MapByConcatenatingIndices_THEN_ResultIsAsExpected()
        {
            //Assemble
            IOneBasedArray2D<string> array = new OneBasedArray2DImpl<string>(new string[,] { { "OneOne", "OneTwo" }, { "TwoOne", "TwoTwo" } });
            Func<string, int, int, string> func = (s, i, j) => $"{s}{i},{j}";

            //Act
            IOneBasedArray2D<string> result = array.Map(func);

            //Assert
            Assert.AreEqual(new string[,] { { "OneOne1,1", "OneTwo1,2" }, { "TwoOne2,1", "TwoTwo2,2" } }, result);
        }

        [Test]
        public void Given_2DStringArrayWithBlanksOnly_When_LastUsedColumnCalled_Then_ResultIsMinusOne()
        {
            //assemble
            string[,] zeroBased = new string[,] {
                { "", "  "},
                { "\n\r\t", null}};
            IOneBasedArray2D<string> oneBased = _api.NewOneBasedArray2D(zeroBased);

            //act
            int actualLastUsedColumn = oneBased.LastUsedColumn();

            //assert
            Assert.AreEqual(-1, actualLastUsedColumn);
        }

        [Test]
        public void Given_2DStringArrayWithNonBlanks_When_LastUsedColumnCalled_Then_ResultIsCorrect()
        {
            //assemble
            string[,] zeroBased = new string[,] {
                { "a1", "", "a3", "a4", ""},
                { "b1", "b2", "b3", "b4", "  "},
                { "", "  ", "c3", "\n\r\t", null} };
            IOneBasedArray2D<string> oneBased = _api.NewOneBasedArray2D(zeroBased);

            //act
            int actualLastUsedColumn = oneBased.LastUsedColumn();

            //assert
            Assert.AreEqual(4, actualLastUsedColumn);
        }

        [Test]
        public void Given_2DStringArrayWithBlanksOnly_When_LastUsedRowCalled_Then_ResultIsMinusOne()
        {
            //assemble
            string[,] zeroBased = new string[,] {
                { "", "  "},
                { "\n\r\t", null}};
            IOneBasedArray2D<string> oneBased = _api.NewOneBasedArray2D(zeroBased);

            //act
            int actualLastUsedRow = oneBased.LastUsedRow();

            //assert
            Assert.AreEqual(-1, actualLastUsedRow);
        }

        [Test]
        public void Given_2DStringArrayWithNonBlanks_When_LastUsedRowCalled_Then_ResultIsCorrect()
        {
            //assemble
            string[,] zeroBased = new string[,] {
                { "a1", "a2", "a3", "a4", "a5"},
                { "b1", "b2", "b3", "b4", "b5"},
                { "", "  ", "\n ", "\n\r\t", null} };
            IOneBasedArray2D<string> oneBased = _api.NewOneBasedArray2D(zeroBased);

            //act
            int actualLastUsedRow = oneBased.LastUsedRow();

            //assert
            Assert.AreEqual(2, actualLastUsedRow);
        }

        [Test]
        public void Given_2DArray_When_IterateAsEnumerable_Then_RowMajorOrderSequenceReturned()
        {
            //assemble
            int[,] zeroBased = new int[,] { { 1, 2 }, { 3, 4 } };
            IOneBasedArray2D<int> oneBased = _api.NewOneBasedArray2D(zeroBased);
            IList<int> actualSequence = new List<int>();
            IList<int> expectedSequence = new List<int>(new int[] {1, 2, 3, 4});

            //act
            foreach (int i in oneBased)
            {
                actualSequence.Add(i);
            }

            //assert
            Assert.AreEqual(expectedSequence, actualSequence);
        }

        [Test]        
        public void OneBasedGetterYieldsCorrectResults()
        {
            //Act
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            
            //Assert
            Assert.AreEqual(arrOneBased[1,1], One+1);
            Assert.AreEqual(arrOneBased[1,2], Two+1);
            Assert.AreEqual(arrOneBased[2,3], Three+2);
            Assert.AreEqual(arrOneBased[2,4], Four+2);
        }

        [Test]
        public void ZeroIndexOutOfBoundsDim0()
        {
            //Act
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[0, 1]; });
        }

        [Test]
        public void ZeroIndexOutOfBoundsDim1()
        {
            //Act
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[1, 0]; });
        }

        [Test]
        public void UpperIndexOutOfBoundsDim0()
        {
            //Act
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[5, 2]; });
        }

        [Test]
        public void UpperIndexOutOfBoundsDim1()
        {
            //Act
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[2, 5]; });
        }

        [Test]
        public void Given_2DArray_When_SlicedIntoRows_Then_AllRowsHaveCorrectLength()
        {
            //Act
            IOneBasedArray2D<string> array = new OneBasedArray2DImpl<string>(new string[,] { { "1", "2" }, { "a", "b" }, { "5", "6" } });
            
            //Assert
            for (int row = 1; row <= array.GetLength(0); row++)
            {
                IOneBasedArray<string> rowSlice = array.SliceRow(row);
                Assert.AreEqual(2, rowSlice.Length, "Sliced row should have correct length");
            }
        }

        [Test]
        public void Given_2DArray_When_SlicedIntoColumns_Then_AllColumnsHaveCorrectLength()
        {
            //Act
            IOneBasedArray2D<string> array = new OneBasedArray2DImpl<string>(new string[,] { { "1", "2" }, { "a", "b" }, { "5", "6" } });
            
            //Assert
            for (int col = 1; col <= array.GetLength(1); col++)
            {
                IOneBasedArray<string> colSlice = array.SliceColumn(col);
                Assert.AreEqual(3, colSlice.Length, "Sliced column should have correct length");
            }
        }

        [Test]
        public void Given_1dArray_When_ConvertedTo2dArrayAcrossColumns_Then_New2dArrayHasCorrectNumberOfRowsAndColumns()
        {
            //Act
            IOneBasedArray<string> oneDimArray = new OneBasedArrayImpl<string>(new string[] { "a", "b", "c", "d" });
            IOneBasedArray2D<string> twoDimArray = oneDimArray.To2DArray(ArrayOrientation.ROW);

            //Assert
            Assert.AreEqual(1, twoDimArray.GetLength(0), "New 2D array should have 1 row");
            Assert.AreEqual(4, twoDimArray.GetLength(1), "New 2D array should have 4 columns");
        }

        [Test]
        public void Given_1dArray_When_ConvertedTo2dArrayAcrossRows_Then_New2dArrayHasCorrectNumberOfRowsAndColumns()
        {
            //Act
            IOneBasedArray<string> oneDimArray = new OneBasedArrayImpl<string>(new string[] { "a", "b", "c", "d" });
            IOneBasedArray2D<string> twoDimArray = oneDimArray.To2DArray(ArrayOrientation.COLUMN);

            //Assert
            Assert.AreEqual(4, twoDimArray.GetLength(0), "New 2D array should have 4 rows");
            Assert.AreEqual(1, twoDimArray.GetLength(1), "New 2D array should have 1 column");
        }

        [Test, TestCaseSource("ProviderForWrite1DArrayTo2DRow")]        
        public void Given_2DArrayOfInts_When_Write1DArrayToRow_Then_ResultIsAsExpected
            (int[,] data, int[] dataToWrite, int row, int offset, int[] expected)
        {
            //Arrange
            IOneBasedArray2D<int> oneBasedData = new OneBasedArray2DImpl<int>(data);
            IOneBasedArray<int> oneBasedDataToWrite = new OneBasedArrayImpl<int>(dataToWrite);
            IOneBasedArray<int> oneBasedExpected = new OneBasedArrayImpl<int>(expected);
            
            //Act
            oneBasedData.WriteToRow(oneBasedDataToWrite, row, offset);
            
            //Assert
            for (int i = 1; i <= oneBasedExpected.Length; i++)
            {
                Assert.AreEqual(oneBasedExpected[i], oneBasedData[row, i],
                    string.Format("Mismatch at row {0} column {1} for offset {2}", row, i, offset));
            }
        }

        [Test, TestCaseSource("ProviderForWrite1DArrayTo2DColumn")]
        public void Given_2DArrayOfInts_When_Write1DArrayToColumn_Then_ResultIsAsExpected
            (int[,] data, int[] dataToWrite, int column, int offset, int[] expected)
        {
            //Arrange
            OneBasedArray2DImpl<int> oneBasedData = new OneBasedArray2DImpl<int>(data);
            OneBasedArrayImpl<int> oneBasedDataToWrite = new OneBasedArrayImpl<int>(dataToWrite);
            IOneBasedArray<int> oneBasedExpected = new OneBasedArrayImpl<int>(expected);
            
            //Act
            oneBasedData.WriteToColumn(oneBasedDataToWrite, column, offset);
            
            //Assert
            for (int i = 1; i <= oneBasedExpected.Length; i++)
            {
                Assert.AreEqual(oneBasedExpected[i], oneBasedData[i, column],
                    string.Format("Mismatch at row {0} column {1} for offset {2}", i, column, offset));
            }
        }

        [Test, TestCaseSource("ProviderForWriteToArea")]
        public void Given_2DArrayOfInts_When_WriteToArea_Then_ResultIsAsExpected
            (int[,] data, int[,] dataToWrite, int rowOffset, int columnOffset, int[,] expected)
        {
            //Arrange
            IOneBasedArray2D<int> oneBasedData = new OneBasedArray2DImpl<int>(data);
            IOneBasedArray2D<int> oneBasedDataToWrite = new OneBasedArray2DImpl<int>(dataToWrite);            
            
            //Act
            oneBasedData.WriteToArea(oneBasedDataToWrite, rowOffset, columnOffset);
            
            //Assert
            Assert.AreEqual(data, expected,
                string.Format("Failure for (rowOffset, columnOffset) = ({0}, {1})", rowOffset, columnOffset));
        }

        [Test, Category("Unit")]
        [TestCaseSource("ProviderFor2DSubArrayTest")]
        public void Given_2DArrayOfBytes_When_SubArray_Then_ExpectedSubArrayReturned
        (byte[,] data, int startAtRow, int startAtColumn, int stopBeforeRow, int stopBeforeColumn, byte[,] expected)
        {
            //Arrange
            IOneBasedArray2D<byte> oneBasedData = new OneBasedArray2DImpl<byte>(data);
            IOneBasedArray2D<byte> oneBasedExpected = new OneBasedArray2DImpl<byte>(expected);
            
            //Act
            IOneBasedArray2D<byte> sub = oneBasedData.SubArray(startAtRow, startAtColumn, stopBeforeRow, stopBeforeColumn);
            
            //Assert
            Assert.AreEqual(oneBasedExpected.ZeroBasedEquivalent, sub.ZeroBasedEquivalent, string.Format(
                "Failure for sub array with coordinates ({0}, {1}) --> ({2}, {3}))",
                startAtRow, startAtColumn, stopBeforeRow, stopBeforeColumn));
        }

        [Test, TestCaseSource("ProviderForFlattenRowMajorTest")]
        public void Given_2DArrayOfByes_When_FlattenRowMajor_Then_ExpectedOneDimArrayReturned(
            byte[,] zeroBasedInput, byte[] expected)
        {
            //Assemble
            OneBasedArray2DImpl<byte> oneBased = new OneBasedArray2DImpl<byte>(zeroBasedInput);

            //Act
            IOneBasedArray<byte> flattened = oneBased.FlattenRowMajor();

            //Assert
            Assert.AreEqual(expected, flattened.ZeroBasedEquivalent);
        }

        [Test, TestCaseSource("ProviderForFlattenColumnMajorTest")]
        public void Given_2DArrayOfBytes_When_FlattenColumnRoMajor_Then_ExpectedOneDimArrayReturned(
            byte[,] zeroBasedInput, byte[] expected)
        {
            //Assemble
            OneBasedArray2DImpl<byte> oneBased = new OneBasedArray2DImpl<byte>(zeroBasedInput);

            //Act
            IOneBasedArray<byte> flattened = oneBased.FlattenColumnMajor();

            //Assert
            Assert.AreEqual(expected, flattened.ZeroBasedEquivalent);
        }

        [Test, TestCaseSource("ProviderForFirstIndexTupleOfTest")]
        public void Given_2DArrayOfBytes_When_GetIndexTupleOfMatcher_Then_ExpectedTupleReturned(
            Func<byte, bool> matcher, (int majorIndex, int minorIndex)? expectedTuple)
        {
            //Assemble
            IOneBasedArray2D<byte> sourceArray = new OneBasedArray2DImpl<byte>(new byte[,]
            {
                {1, 2, 3 },
                {4, 5, 6},
                {7, 8, 9},
            });

            //Act
            (int majorIndex, int minorIndex)? actualTuple = sourceArray.FirstIndexTupleOf(matcher);

            //Assert
            Assert.AreEqual(expectedTuple, actualTuple);
        }

        #region Test Data
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
                new object[]{oddNumberMatcher, (1, 1)},
                new object[]{evenNumberMatcher, (1, 2)},
                new object[]{ matchNine, (3, 3)},
                new object[]{ tryMatchNmberOutsideArray, null},
                new object[]{ neverMatch, null},
                new object[]{ alwaysMatch, (1, 1)},
            };
        }

        private static IEnumerable<object[]> ProviderForFlattenColumnMajorTest()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new byte[,]
                    {
                        {00, 01, 02},
                        {10, 11, 12},
                        {20, 21, 22}
                    },
                    new byte[] { 00, 10, 20, 01, 11, 21, 02, 12, 22 }
                },
            };
        }

        private static IEnumerable<object[]> ProviderForFlattenRowMajorTest()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new byte[,]
                    {
                        {00, 01, 02},
                        {10, 11, 12},
                        {20, 21, 22}
                    },
                    new byte[] { 00, 01, 02, 10, 11, 12, 20, 21, 22 }
                },
            };
        }

        private static IEnumerable<object[]> ProviderFor2DSubArrayTest()
        {
            return new List<object[]> {
                new object[] { new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, 1, 1, 4, 4,
                    new byte[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }}},
            };
        }

        private static IEnumerable<object[]> ProviderForWriteToArea()
        {
            return new List<object[]> {
                new object[] { new int[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 23, 33 }, { 4, 14, 24, 34 } },
                    new int[,] { { 101, 111, 121 }, { 102, 112, 122 }, { 103, 113, 123 }},
                    2, 2,
                    new int[,] { { 1, 11, 21, 31 }, { 2, 12, 22, 32 }, { 3, 13, 101, 111 }, { 4, 14, 102, 112 } },
                }
            };
        }
        private static IEnumerable<object[]> ProviderForWrite1DArrayTo2DColumn()
        {
            return new List<object[]> {
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] {21, 22, 23}, 2, 1,
                    new int[]{11, 21, 22, 23}},
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] { 21, 22, 23, 24 }, 1, -1,
                    new int[]{22, 23, 24, 4}},
            };
        }

        private static IEnumerable<object[]> ProviderForWrite1DArrayTo2DRow()
        {
            return new List<object[]> {
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] {21, 22, 23}, 2, 1,
                    new int[]{11, 21, 22, 23}},
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] { 21, 22, 23, 24 }, 1, -1,
                    new int[]{22, 23, 24, 4}},
            };
        }
        private IOneBasedArray2D<string> GenTestData()
        {
            string[,] testData = new string[,] {
                { One + 1, Two + 1, Three + 1, Four + 1 },
                { One + 2, Two + 2, Three + 2, Four + 2 } };
            return _api.NewOneBasedArray2D(testData);
        }
        #endregion
    }
}
