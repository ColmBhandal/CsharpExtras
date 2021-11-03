using CsharpExtras.Api;
using CsharpExtras.Enumerable.OneBased;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Extensions;

namespace OneBased
{
    [TestFixture]
    [Category("Unit")]
    public class OneBasedArrayTest
    {
        private const string One = "One";
        private const string Two = "Two";
        private const string Three = "Three";
        private const string Four = "Four";        
        private readonly ICsharpExtrasApi _api = new CsharpExtrasApi();

        [Test]
        public void Given_OneBasedArray_WhenGetOneBasedFirstIndexOfMatchingElement_Then_OneBasedIndexReturned()
        {
            //Arrange
            byte[] zeroBased = new byte[] { 0, 2, 4, 3, 6, 10 };
            IOneBasedArray<byte> arr = new OneBasedArrayImpl<byte>(zeroBased);

            //Act
            int firstIndex = arr.FirstIndexOf(b => b % 2 == 1);           

            //Assert
            Assert.AreEqual(4, firstIndex);
        }
        
        [Test]
        public void GivenOneBasedArrayWhenMapAppliedThenResultIsArrayOfMappedValues()
        {
            //Arrange
            IOneBasedArray<int> intArr = GenTestIntArray();
            const string Even = "Even";
            const string Odd = "Odd";
            
            //Act
            IOneBasedArray<string> resultArr = intArr.Map(i => i%2 == 0 ? Even : Odd);
            
            //Assert
            Assert.AreEqual(resultArr.ZeroBasedEquivalent, new string[] { Odd, Even, Odd, Even, Odd, Even });
        }

        [Test]
        public void GivenOneBasedArrayWhenPairAndExecuteSumProductOnSelfThenSumOfSquaresResults()
        {
            //Arrange
            IOneBasedArray<int> intArr = GenTestIntArray();            
            int totalSum = 0;
            Action<int, int> sumProdAccumulator = (i, j) => totalSum += i * j;
            
            //Act
            intArr.PairAndExecute<int>(intArr, sumProdAccumulator);
            //Pyramidal number formula used below to calculate sum of squares: https://en.wikipedia.org/wiki/Square_pyramidal_number
            const int N = 6;
            
            //Assert
            Assert.AreEqual(N * N * N / 3 + N * N / 2 + N / 6, totalSum);
        }

        [Test]
        public void GivenTwoOneBasedArraysOfDifferentTypesAndLengthsWhenZippedThenResultIsAsExpected()
        {
            //Act
            IOneBasedArray<string> stringArr = GenTestStringArray();
            IOneBasedArray<int> intArr = GenTestIntArray();
            
            //Assert
            Assert.AreNotEqual(stringArr.Length, intArr.Length,
                "Given: Test arrays should be different legnths to begin with. Equal length arrays does not propely capture this test case.");
            Func<int, string, (int, string)> zipper = (i, s) => (i, s);
            IOneBasedArray<(int, string)> zipped = intArr.ZipArray<string, (int, string)>(zipper, stringArr);
            (int, string)[] zeroBasedZipped = zipped.ZeroBasedEquivalent;
            Assert.AreEqual(zeroBasedZipped, new (int, string)[] { (1, One), (2, Two), (3, Three), (4, Four) });
        }

        [Test]
        public void OneBasedGetterYieldsCorrectResults()
        {
            //Act
            IOneBasedArray<string> arrOneBased = GenTestStringArray();
            
            //Assert
            Assert.AreEqual(arrOneBased[1], One);
            Assert.AreEqual(arrOneBased[2], Two);
            Assert.AreEqual(arrOneBased[3], Three);
            Assert.AreEqual(arrOneBased[4], Four);
        }

        [Test]
        public void ZeroIndexOutOfBounds()
        {
            //Act
            IOneBasedArray<string> arrOneBased = GenTestStringArray();
            
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[0]; });
        }

        [Test]
        public void UpperIndexOutOfBounds()
        {
            //Act
            IOneBasedArray<string> arrOneBased = GenTestStringArray();
            
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { _ = arrOneBased[5]; });
        }

        [Test]
        public void TestGivenOneBasedArrayWithDuplicatesWhenFindDuplicatesThenCorrectIndexesReturned()
        {
            //Arrange
            IOneBasedArray<string> arrOneBased = GenTestStringArrayWithDuplicates();
            
            //Act
            IDictionary<string, IList<int>> duplicates = arrOneBased.FindDuplicateIndices();

            //Assert
            Assert.True(duplicates.ContainsKey(One), "Expected value to be duplicated: " + One);
            Assert.True(duplicates.ContainsKey(Two), "Expected value to be duplicated: " + Two);

            IList<int> oneDupes = duplicates[One];
            Assert.True(oneDupes.Contains(1));
            Assert.True(oneDupes.Contains(4));
            Assert.True(oneDupes.Contains(5));

            IList<int> twoDupes = duplicates[Two];
            Assert.True(twoDupes.Contains(2));
            Assert.True(twoDupes.Contains(7));
        }

        [Test]
        public void TestGivenOneBasedArrayWithDuplicatesWhenInvertingThenCorrectIndexesReturned()
        {
            //Arrange
            IOneBasedArray<string> arrOneBased = GenTestStringArrayWithDuplicates();
            
            //Act
            IDictionary<string, IList<int>> invertedDict = arrOneBased.InverseMap();

            //Assert
            Assert.AreEqual(3, invertedDict[One].Count, "Expected 3 matches for value: " + One);
            Assert.AreEqual(2, invertedDict[Two].Count, "Expected 2 matches for value: " + Two);
            Assert.AreEqual(1, invertedDict[Three].Count, "Expected 1 matches for value: " + Three);
            Assert.AreEqual(1, invertedDict[Four].Count, "Expected 1 matches for value: " + Four);

            IList<int> oneValues = invertedDict[One];
            Assert.True(oneValues.Contains(1));
            Assert.True(oneValues.Contains(4));
            Assert.True(oneValues.Contains(5));

            IList<int> twoValues = invertedDict[Two];
            Assert.True(twoValues.Contains(2));
            Assert.True(twoValues.Contains(7));

            IList<int> threeValues = invertedDict[Three];
            Assert.AreEqual(3, threeValues.First());
        }

        private IOneBasedArray<string> GenTestStringArray()
        {
            string[] testData = new string[] { One, Two, Three, Four };
             return _api.NewOneBasedArray(testData);
        }

        private IOneBasedArray<string> GenTestStringArrayWithDuplicates()
        {
            string[] testData = new string[] { One, Two, Three, One, One, Four, Two};
            return _api.NewOneBasedArray(testData);
        }

        private IOneBasedArray<int> GenTestIntArray()
        {
            int[] testData = new int[] { 1, 2, 3, 4, 5, 6};
            return _api.NewOneBasedArray(testData);
        }
    }
}
