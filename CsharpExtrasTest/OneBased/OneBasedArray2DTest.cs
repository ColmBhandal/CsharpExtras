using CsharpExtras.Api;
using CsharpExtras.Enumerable.OneBased;
using NUnit.Framework;
using System;
using static CsharpExtras.Extensions.ArrayOrientationClass;
using static CsharpExtras.Extensions.ArrayExtension;
using System.Collections.Generic;

namespace OneBased
{
    [TestFixture]
    [Category("Unit")]
    public class OneBasedArray2DTest
    {
        private const string One = "One";
        private const string Two = "Two";
        private const string Three = "Three";
        private const string Four = "Four";

        private readonly ICsharpExtrasApi _api = new CsharpExtrasApi();
        
        [Test]        
        public void OneBasedGetterYieldsCorrectResults()
        {
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            Assert.AreEqual(arrOneBased[1,1], One+1);
            Assert.AreEqual(arrOneBased[1,2], Two+1);
            Assert.AreEqual(arrOneBased[2,3], Three+2);
            Assert.AreEqual(arrOneBased[2,4], Four+2);
        }

        [Test]
        public void ZeroIndexOutOfBoundsDim0()
        {
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[0, 1]; });
        }

        [Test]
        public void ZeroIndexOutOfBoundsDim1()
        {
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[1, 0]; });
        }

        [Test]
        public void UpperIndexOutOfBoundsDim0()
        {
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[5, 2]; });
        }

        [Test]
        public void UpperIndexOutOfBoundsDim1()
        {
            IOneBasedArray2D<string> arrOneBased = GenTestData();
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[2, 5]; });
        }

        [Test]
        public void Given_2DArray_When_SlicedIntoRows_Then_AllRowsHaveCorrectLength()
        {
            IOneBasedArray2D<string> array = new OneBasedArray2DImpl<string>(new string[,] { { "1", "2" }, { "a", "b" }, { "5", "6" } });
            for (int row = 1; row <= array.GetLength(0); row++)
            {
                IOneBasedArray<string> rowSlice = array.SliceRow(row);
                Assert.AreEqual(2, rowSlice.Length, "Sliced row should have correct length");
            }
        }

        [Test]
        public void Given_2DArray_When_SlicedIntoColumns_Then_AllColumnsHaveCorrectLength()
        {
            IOneBasedArray2D<string> array = new OneBasedArray2DImpl<string>(new string[,] { { "1", "2" }, { "a", "b" }, { "5", "6" } });
            for (int col = 1; col <= array.GetLength(1); col++)
            {
                IOneBasedArray<string> colSlice = array.SliceColumn(col);
                Assert.AreEqual(3, colSlice.Length, "Sliced column should have correct length");
            }
        }

        [Test]
        public void Given_1dArray_When_ConvertedTo2dArrayAcrossColumns_Then_New2dArrayHasCorrectNumberOfRowsAndColumns()
        {
            IOneBasedArray<string> oneDimArray = new OneBasedArrayImpl<string>(new string[] { "a", "b", "c", "d" });
            IOneBasedArray2D<string> twoDimArray = oneDimArray.To2DArray(ArrayOrientation.COLUMN);

            Assert.AreEqual(1, twoDimArray.GetLength(0), "New 2D array should have 1 row");
            Assert.AreEqual(4, twoDimArray.GetLength(1), "New 2D array should have 4 columns");
        }

        [Test]
        public void Given_1dArray_When_ConvertedTo2dArrayAcrossRows_Then_New2dArrayHasCorrectNumberOfRowsAndColumns()
        {
            IOneBasedArray<string> oneDimArray = new OneBasedArrayImpl<string>(new string[] { "a", "b", "c", "d" });
            IOneBasedArray2D<string> twoDimArray = oneDimArray.To2DArray(ArrayOrientation.ROW);

            Assert.AreEqual(4, twoDimArray.GetLength(0), "New 2D array should have 4 rows");
            Assert.AreEqual(1, twoDimArray.GetLength(1), "New 2D array should have 1 column");
        }

        [Test, TestCaseSource("ProviderForWrite1DArrayTo2DRow")]        
        public void Given_2DArrayOfInts_When_Write1DArrayToRow_Then_ResultIsAsExpected
            (int[,] data, int[] dataToWrite, int row, int offset, int[] expected)
        {
            IOneBasedArray2D<int> oneBasedData = new OneBasedArray2DImpl<int>(data);
            IOneBasedArray<int> oneBasedDataToWrite = new OneBasedArrayImpl<int>(dataToWrite);
            IOneBasedArray<int> oneBasedExpected = new OneBasedArrayImpl<int>(expected);
            oneBasedData.WriteToRow(oneBasedDataToWrite, row, offset);
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
            OneBasedArray2DImpl<int> oneBasedData = new OneBasedArray2DImpl<int>(data);
            OneBasedArrayImpl<int> oneBasedDataToWrite = new OneBasedArrayImpl<int>(dataToWrite);
            IOneBasedArray<int> oneBasedExpected = new OneBasedArrayImpl<int>(expected);
            oneBasedData.WriteToColumn(oneBasedDataToWrite, column, offset);
            for (int i = 1; i <= oneBasedExpected.Length; i++)
            {
                Assert.AreEqual(oneBasedExpected[i], oneBasedData[i, column],
                    string.Format("Mismatch at row {0} column {1} for offset {2}", i, column, offset));
            }
        }

        #region Test Data
        private static IEnumerable<object[]> ProviderForWrite1DArrayTo2DColumn()
        {
            return new List<object[]> {
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] {21, 22, 23}, 2, 1,
                    new int[]{11, 21, 22, 23}},
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] {21, 22, 23}, 2, 0,
                    new int[]{21, 22, 23, 14}},
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] { 1, 2, 3, 4 }, 1, 0,
                    new int[]{1, 2, 3, 4}},
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] { 1, 2, 3, 4 }, 1, 2,
                    new int[]{1, 2, 1, 2}},
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] { 21, 22, 23, 24 }, 1, 4,
                    new int[]{1, 2, 3, 4}},
                new object[] { new int[,] { { 1, 11 }, { 2, 12 }, { 3, 13 }, { 4, 14 } }, new int[] { 21, 22, 23, 24 }, 1, -1,
                    new int[]{22, 23, 24, 4}},
            };
        }

        private static IEnumerable<object[]> ProviderForWrite1DArrayTo2DRow()
        {
            return new List<object[]> {
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] {21, 22, 23}, 2, 1,
                    new int[]{11, 21, 22, 23}},
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] {21, 22, 23}, 2, 0,
                    new int[]{21, 22, 23, 14}},
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] { 1, 2, 3, 4 }, 1, 0,
                    new int[]{1, 2, 3, 4}},
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] { 1, 2, 3, 4 }, 1, 2,
                    new int[]{1, 2, 1, 2}},
                new object[] { new int[,] { { 1, 2, 3, 4 }, {11, 12, 13, 14} }, new int[] { 21, 22, 23, 24 }, 1, 4,
                    new int[]{1, 2, 3, 4}},
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
