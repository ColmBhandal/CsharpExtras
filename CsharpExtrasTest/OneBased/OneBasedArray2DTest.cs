using CsharpExtras.Api;
using CsharpExtras.Enumerable.OneBased;
using NUnit.Framework;
using System;

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

        private IOneBasedArray2D<string> GenTestData()
        {
            string[,] testData = new string[,] {
                { One + 1, Two + 1, Three + 1, Four + 1 },
                { One + 2, Two + 2, Three + 2, Four + 2 } };
            return _api.NewOneBasedArray2D(testData);
        }
    }
}
