using CsharpExtras.Map.Dictionary;
using Dictionary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    [TestFixture]
    class BijectionDictionaryTest
    {
        [Test]
        [Category("Unit")]
        public void TestGivenUniqueKeyValuePairsWhenAddedToTwoWayDictionaryThenCanBeAccessed()
        {
            //Arrange
            int maxCount = 10;
            int lastIndex = maxCount - 1;
            
            //Act
            IBijectionDictionary<int, string> dict = GenerateNewTestDictionary(maxCount);

            //Assert
            Assert.AreEqual(maxCount, dict.Count);
            Assert.AreEqual(GetTestStringBasedOnIndex(0), dict[0]);
            Assert.AreEqual(GetTestStringBasedOnIndex(lastIndex), dict[lastIndex]);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenUniqueKeyValuePairsWhenAddedToTwoWayDictionaryThenReverseDictionaryCanBeAccessed()
        {
            //Arrange
            int maxCount = 10;
            int lastIndex = maxCount - 1;
            
            //Act
            IBijectionDictionary<int, string> dict = GenerateNewTestDictionary(maxCount);

            //Assert
            Assert.AreEqual(maxCount, dict.Count);
            Assert.AreEqual(dict.Count, dict.Reverse.Count);

            Assert.AreEqual(0, dict.Reverse[GetTestStringBasedOnIndex(0)]);
            Assert.AreEqual(lastIndex, dict.Reverse[GetTestStringBasedOnIndex(lastIndex)]);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenNonUniqueKeyValuePairsWhenAddedToTwoWayDictionaryThenExceptionsThrownForDuplicates()
        {
            //Arrange
            IBijectionDictionary<int, string> dict = new BijectionDictionaryImpl<int, string>();

            //Act
            dict.Add(0, GetTestStringBasedOnIndex(0));
            int countBefore = dict.Count;

            //Assert
            // Exception thrown for duplicate keys
            Assert.Throws<ArgumentException>(() => dict.Add(0, GetTestStringBasedOnIndex(0)));
            Assert.AreEqual(countBefore, dict.Count, "Item added to dictionary after exception thrown");
            Assert.AreEqual(countBefore, dict.Reverse.Count, "Item added to reversed dictionary after exception thrown");

            // Exception thrown for duplicate values for unique keys
            Assert.Throws<ArgumentException>(() => dict.Add(1, GetTestStringBasedOnIndex(0)));
            Assert.AreEqual(countBefore, dict.Count, "Item added to dictionary after exception thrown");
            Assert.AreEqual(countBefore, dict.Reverse.Count, "Item added to reversed dictionary after exception thrown");

            // Add succeeds with unique key + value
            dict.Add(1, GetTestStringBasedOnIndex(1));
        }

        [Test]
        [Category("Unit")]
        public void TestGivenTwoWayDictionaryWhenRemoveItemsFromOriginalVersionThenItemsAlsoRemovedFromReversedVersion()
        {
            //Arrange
            int maxCount = 10;
            IBijectionDictionary<int, string> dict = GenerateNewTestDictionary(maxCount);

            int indexToRemove = 5;
            
            //Act
            string valueToRemove = GetTestStringBasedOnIndex(indexToRemove);

            //Assert
            Assert.AreEqual(valueToRemove, dict[indexToRemove], "GIVEN: Dictionary is not setup correctly");
            Assert.AreEqual(indexToRemove, dict.Reverse[valueToRemove], "GIVEN: Dictionary is not setup correctly");

            dict.Remove(indexToRemove);

            Assert.IsFalse(dict.ContainsKey(indexToRemove));
            Assert.IsFalse(dict.Reverse.ContainsKey(valueToRemove));
            Assert.AreEqual(dict.Count, dict.Reverse.Count);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenTwoWayDictionaryWhenRemoveItemsFromReversedVersionThenItemsAlsoRemovedFromOriginalVersion()
        {
            //Arrange
            int maxCount = 10;
            IBijectionDictionary<int, string> dict = GenerateNewTestDictionary(maxCount);

            int indexToRemove = 5;
            
            //Act
            string valueToRemove = GetTestStringBasedOnIndex(indexToRemove);

            //Assert
            Assert.AreEqual(valueToRemove, dict[indexToRemove], "GIVEN: Dictionary is not setup correctly");
            Assert.AreEqual(indexToRemove, dict.Reverse[valueToRemove], "GIVEN: Dictionary is not setup correctly");

            dict.Reverse.Remove(valueToRemove);

            Assert.IsFalse(dict.ContainsKey(indexToRemove));
            Assert.IsFalse(dict.Reverse.ContainsKey(valueToRemove));
            Assert.AreEqual(dict.Count, dict.Reverse.Count);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenTwoWayDictionaryWhenGettingReverseOfReverseThenReverseOfReverseIsTheSameObjectAsOriginal()
        {
            //Arrange
            int maxCount = 10;
            
            //Act
            IBijectionDictionary<int, string> dict = GenerateNewTestDictionary(maxCount);

            //Assert
            Assert.AreEqual(maxCount, dict.Count);
            Assert.AreEqual(maxCount, dict.Reverse.Count);
            Assert.AreEqual(maxCount, dict.Reverse.Reverse.Count);

            Assert.AreSame(dict, dict.Reverse.Reverse);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenTwoWayDictionaryWhenAllItemsClearedThenReverseVersionIsAlsoEmpty()
        {
            //Arrange
            int maxCount = 10;
            
            //Act
            IBijectionDictionary<int, string> dict = GenerateNewTestDictionary(maxCount);

            //Assert
            Assert.AreEqual(maxCount, dict.Count, "GIVEN: Dictionary not setup correctly");
            Assert.AreEqual(maxCount, dict.Reverse.Count, "GIVEN: Dictionary not setup correctly");

            dict.Clear();

            Assert.AreEqual(0, dict.Count);
            Assert.AreEqual(0, dict.Reverse.Count);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenExistingDictionaryWhenWrappedInTwoWayDictionaryThenReverseVersionIsPopulated()
        {
            //Arrange
            int maxCount = 10;
            
            //Act
            IDictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < maxCount; i++)
            {
                dict.Add(i, GetTestStringBasedOnIndex(i));
            }

            //Assert
            Assert.AreEqual(maxCount, dict.Count, "GIVEN: Source dictionary has incorrect length");

            IBijectionDictionary<int, string> twoWayDict = new BijectionDictionaryImpl<int, string>(dict);
            Assert.AreEqual(maxCount, twoWayDict.Count, "Two way dictionary not populated correctly");
            Assert.AreEqual(maxCount, twoWayDict.Reverse.Count, "Reverse dictionary not populated correctly");

            AssertOriginalAndReversedVersionsMatchExactly(twoWayDict);
        }

        [Test]
        [Category("Unit")]
        public void TestGivenExistingDictionaryWithDuplicateValuesWhenWrappedInTwoWayDictionaryThenExceptionIsThrown()
        {
            //Arrange
            int maxCount = 2;
            
            //Act
            IDictionary<int, string> dict = new Dictionary<int, string>();
            for (int i = 0; i < maxCount; i++)
            {
                dict.Add(i, "duplicate");
            }

            //Assert
            Assert.AreEqual(maxCount, dict.Count, "GIVEN: Source dictionary has incorrect length");
            Assert.Throws<ArgumentException>(() => new BijectionDictionaryImpl<int, string>(dict),
                "Should throw exception if source dictionary has duplicate values");
        }

        private void AssertOriginalAndReversedVersionsMatchExactly<TKey, TValue>(IBijectionDictionary<TKey, TValue> dictionary)
        {
            foreach (TKey key in dictionary.Keys)
            {
                TValue value = dictionary[key];
                TKey reversedKey = dictionary.Reverse[value];

                Assert.AreEqual(key, reversedKey, "Mismatch between original and reversed dictionaries for key " + key.ToString());
            }
        }

        private IBijectionDictionary<int, string> GenerateNewTestDictionary(int maxCount)
        {
            IBijectionDictionary<int, string> dict = new BijectionDictionaryImpl<int, string>();
            for (int i = 0; i < maxCount; i++)
            {
                dict.Add(i, GetTestStringBasedOnIndex(i));
            }
            return dict;
        }

        private string GetTestStringBasedOnIndex(int index)
        {
            return "test-" + index;
        }
    }
}
