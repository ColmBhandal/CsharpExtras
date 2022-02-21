using CsharpExtras.Extensions;
using NUnit.Framework;
using System.Collections.Generic;

namespace CsharpExtrasTest.Extensions
{
    [TestFixture]
    public class IEnumerableTest
    {
        private const string Zero = "Zero";
        private const string One = "One";
        private const string Two = "Two";
        private const string Three = "Three";
        
        [Test, Category("Unit")]
        public void GIVEN_Enumerables_WHEN_UnionMax_THEN_ExpectedResult()
        {
            //Arrange
            IList<int> enumerable1 = new List<int> { 1, 2, 3 };
            int[] enumerable2 = new int[] { 0, -1, -4 };

            //Act
            int max = enumerable1.UnionMax(enumerable2);

            //Assert
            Assert.AreEqual(3, max);
        }

        [Test, Category("Unit")]
        [TestCaseSource("ValidIndexingData")]
        public void GivenEnumerableWhenIndexedThenResultantDictionaryIsAsExpected(int startIndex, int step, int[] expectedKeysInOrder)
        {
            //Arrange
            string[] mockData = new string[] { Zero, One, Two };
            IDictionary<int, string> indexedDict = mockData.Index(startIndex, step);
                
            //Act
            ICollection<int> actualKeys = indexedDict.Keys;
                
            //Assert
            Assert.AreEqual(expectedKeysInOrder, actualKeys, "Keys in resultant dictionary differ from those expected.");
            for(int originalIndex = 0; originalIndex < mockData.Length; originalIndex++)
            {
                int keyInIndexedDict = expectedKeysInOrder[originalIndex];
                string expectedData = mockData[originalIndex];
                string actualData = indexedDict[keyInIndexedDict];
                Assert.AreEqual(expectedData, actualData, string.Format(
                    "Indexed dictionary data does not match original enumerable data. Index in original enumerable: {0}. Index in indexed dictionary: {1}",
                    originalIndex, keyInIndexedDict));
            }
        }

        [Test, Category("Unit")]
        public void GIVEN_Enumerable_WHEN_IndexZeroBased_THEN_DictionaryWithOneBasedIndicesReturned()
        {
            //Assemble
            string[] mockData = new string[] { Zero, One, Two };

            //Act
            IDictionary<int, string> dict = mockData.IndexZeroBased();

            //Assert
            Assert.AreEqual(new Dictionary<int, string> {[0] = Zero, [1] = One, [2] = Two}, dict);
        }

        [Test, Category("Unit")]
        public void GIVEN_Enumerable_WHEN_IndexOneBased_THEN_DictionaryWithOneBasedIndicesReturned()
        {
            //Assemble
            string[] mockData = new string[] { One, Two, Three };

            //Act
            IDictionary<int, string> dict = mockData.IndexOneBased();

            //Assert
            Assert.AreEqual(new Dictionary<int, string> { [1] = One, [2] = Two, [3] = Three }, dict);
        }

        [Test, Category("Unit")]
        public void GIVEN_Enumerable_WHEN_OneBasedFirstIndexOfMatchingElement_THEN_OneBasedIndexReturned()
        {
            //Assemble
            string[] mockData = new string[] { One, Two, Three };

            //Act
            int firstIndex = mockData.FirstIndexOfOneBased(Two);

            //Assert
            Assert.AreEqual(2, firstIndex);
        }

        [Test, Category("Unit")]
        public void GIVEN_Enumerable_WHEN_OneBasedFirstIndexOfNonMatchingElement_THEN_MinusOneReturned()
        {
            //Assemble
            string[] mockData = new string[] { One, Two, Three };

            //Act
            int firstIndex = mockData.FirstIndexOfOneBased("Non-existent-data");

            //Assert
            Assert.AreEqual(-1, firstIndex);
        }

        [Test, Category("Unit")]
        public void GIVEN_Enumerable_WHEN_ZeroBasedFirstIndexOfMatchingElement_THEN_ZeroBasedIndexReturned()
        {
            //Assemble
            string[] mockData = new string[] { Zero, One, Two };

            //Act
            int firstIndex = mockData.FirstIndexOfZeroBased(One);

            //Assert
            Assert.AreEqual(1, firstIndex);
        }

        [Test, Category("Unit")]
        public void GIVEN_Enumerable_WHEN_ZeroBasedFirstIndexOfNonMatchingElement_THEN_MinusZeroReturned()
        {
            //Assemble
            string[] mockData = new string[] { Zero, One, Two };

            //Act
            int firstIndex = mockData.FirstIndexOfZeroBased("Non-existent-data");

            //Assert
            Assert.AreEqual(-1, firstIndex);
        }


        #region Providers
        public static IEnumerable<object[]> ValidIndexingData()
        {
            return new List<object[]>()
            {
                new object[] { 0, 2, new int[] { 0, 2, 4} },
                new object[] { 0, -3, new int[] { 0, -3, -6} },
                new object[] { 6, 5, new int[] { 6, 11, 16} },
                new object[] { 6, -7, new int[] { 6, -1, -8} },
            };
        }
    #endregion
}
}
