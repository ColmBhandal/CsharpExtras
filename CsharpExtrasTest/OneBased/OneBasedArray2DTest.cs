using CsharpExtras.Api;
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

        private IOneBasedArray2D<string> GenTestData()
        {
            string[,] testData = new string[,] {
                { One + 1, Two + 1, Three + 1, Four + 1 },
                { One + 2, Two + 2, Three + 2, Four + 2 } };
            return _api.NewOneBasedArray2D(testData);
        }
    }
}
