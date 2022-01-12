using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Extensions;
using System;
using CsharpExtras.Extensions.Helper.Dictionary;

namespace CsharpExtrasTest.Extensions
{
    [TestFixture,Category("Unit")]
    public class DictionaryTest
    {
        private const string Zero = "Zero";
        private const string One = "One";
        private const string Two = "Two";
        private const string Infty = "Infinity and Beyond";

        [Test]
        public void GIVEN_DictionariesWithUnequalPairs_WHEN_DictEquals_THEN_ResultIsNotEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>()
            {
                {2, "Some other value"},
                { 1, "Hello World"},
                {3, "x" }
            };
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>()
            {
                { 1, "Hello World"},
                {3, "x" },
                {4, "The fourth dimension was discovered by Einstein" }
            };

            //Act
            IDictionaryComparison comparison = dictionary1.DictEquals(dictionary2);

            //Assert
            Assert.IsFalse(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_ThisDictionaryPairsAreStrictSupersetOfOther_WHEN_DictEquals_THEN_ResultIsNotEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>()
            {
                {2, "Some other value"},
                { 1, "Hello World"},
                {3, "x" }
            };
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>()
            {
                { 1, "Hello World"},
                {3, "x" }
            };

            //Act
            IDictionaryComparison comparison = dictionary1.DictEquals(dictionary2);

            //Assert
            Assert.IsFalse(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_ThisDictionaryPairsAreStrictSubsetOfOther_WHEN_DictEquals_THEN_ResultIsNotEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>()
            {
                { 1, "Hello World"},
                {3, "x" }
            };
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>()
            {
                {2, "Some other value"},
                { 1, "Hello World"},
                {3, "x" }
            };

            //Act
            IDictionaryComparison comparison = dictionary1.DictEquals(dictionary2);

            //Assert
            Assert.IsFalse(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_DictionariesWithSamePairsAddedInDifferentOrder_WHEN_DictEquals_THEN_ResultIsEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>()
            {
                { 1, "Hello World"},
                {2, "Some other value" },
                {3, "x" }
            };
            Dictionary<int, string> dictionary2 = new Dictionary<int, string>()
            {
                {2, "Some other value"},
                { 1, "Hello World"},
                {3, "x" }
            };

            //Act
            IDictionaryComparison comparison = dictionary1.DictEquals(dictionary2);

            //Assert
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_EmptyDictionary_WHEN_DictEqualsItself_THEN_ResultIsEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
            };

            //Act
            IDictionaryComparison comparison = dictionary.DictEquals(dictionary);

            //Assert
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_FilledDictionary_WHEN_DictEqualsItself_THEN_ResultIsEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
                { 1, "Hello World"},
                {2, "Some other value" }
            };

            //Act
            IDictionaryComparison comparison = dictionary.DictEquals(dictionary);

            //Assert
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GivenStringIntDictionaryAndStringBoolDictionaryWhenZippedWithNegationOperatorThenResultantDictionaryIsAsExpected()
        {
            //Arrange
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            IDictionary<string, bool> mockBoolDict = MockStrBoolDictionary();
            
            //Act
            //Zip with simple function that negates values when the boolean is false
            IDictionary<string, int> zippedDict = mockDict.ZipValues(mockBoolDict, (i, b) => b ? i : -i);
            
            //Assert
            Assert.AreEqual(zippedDict.Keys, new string[] {"1", "2", "3"}, "Result dictionary keys are incorrect");
            Assert.AreEqual(0, zippedDict["1"]);
            Assert.AreEqual(-2, zippedDict["2"]);
            Assert.AreEqual(3, zippedDict["3"]);
        }

        [Test]
        public void GivenStringIntDictionaryWhenValuesMappedToStringThenValuesMatchExpectedMapValues()
        {
            //Arrange
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            
            //Act
            IDictionary<string, string> mappedDictionary = mockDict.MapValues(MockIntToStringMap);
            ICollection<int> sourceValues = mockDict.Values;
            IEnumerable<string> mappedValues = mappedDictionary.Values;
            IEnumerable<string> mappedFromSource = sourceValues.Select(MockIntToStringMap);
            
            //Assert
            Assert.AreEqual(mappedValues, mappedFromSource);
        }

        [Test]
        public void GivenStringIntDictionaryWhenValuesMappedToStringThenKeysRemainUnchanged()
        {
            //Arrange
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            
            //Act
            IDictionary<string, string> mappedDictionary = mockDict.MapValues(MockIntToStringMap);
            
            //Assert
            Assert.AreEqual(mockDict.Keys, mappedDictionary.Keys);
        }

        [Test]
        public void GivenDictionaryWhenAddWithKeyDerivedFromValueCalledThenCorrectItemAddedToDictionary()
        {
            //Arrange
            IDictionary<string, int> dictionary = new Dictionary<string,int>();
            string KeyFromValue(int value)
            {
                return value + "";
            }

            //Act
            dictionary.AddWithKeyDerivedFromValue(5, KeyFromValue);
            //Assert

            Assert.IsTrue(dictionary.ContainsKey("5"));
            Assert.IsTrue(dictionary.Values.Contains(5));
            Assert.AreEqual(dictionary["5"], 5);
            Assert.AreEqual(dictionary.Count,1);
        }

        [Test]
        public void GIVEN_Dictionary_WHEN_MapValuesWithKeyValueMapper_THEN_ExpectedDictionaryReturned()
        {
            //Assemble
            IDictionary<int, string> mockDict = new Dictionary<int, string>()
                { [1] = "1", [2] = "2", [3] = "3" };
            Func<int, string, (int, string)> mapper = (i, s) => (i, s);

            //Act
            IDictionary<int, (int, string)> actualDict = mockDict.MapValues(mapper);

            //Assert
            IDictionary<int, (int, string)> expectedDict = new Dictionary<int, (int, string)>()
            { [1] = (1, "1"), [2] = (2, "2"), [3] = (3, "3") };
            Assert.AreEqual(expectedDict, actualDict);
        }

        [Test]
        public void GivenDictionaryWhenAddWithValueDerivedFromKeyCalledThenCorrectItemAddedToDictionary()
        {
            //Arrange
            IDictionary<string, int> dictionary = new Dictionary<string, int>();
            int ValueFromKey(string key)
            {
                return Int32.Parse(key);
            }

            //Act
            dictionary.AddWithValueDerivedFromKey("5", ValueFromKey);
            //Assert

            Assert.IsTrue(dictionary.ContainsKey("5"));
            Assert.IsTrue(dictionary.Values.Contains(5));
            Assert.AreEqual(dictionary["5"],5);
            Assert.AreEqual(dictionary.Count, 1);
        }

        private IDictionary<string, int> MockStrIntDictionary()
        {
            Dictionary<string, int> mockDict = new Dictionary<string, int>();
            mockDict.Add("0", 0);
            mockDict.Add("1", 0);
            mockDict.Add("2", 2);
            mockDict.Add("3", 3);
            return mockDict;
        }

        private IDictionary<string, bool> MockStrBoolDictionary()
        {
            Dictionary<string, bool> mockDict = new Dictionary<string, bool>();
            mockDict.Add("1", true);
            mockDict.Add("2", false);
            mockDict.Add("3", true);
            return mockDict;
        }

        private string MockIntToStringMap(int i)
        {
            switch (i)
            {
                case 0: return Zero;
                case 1: return One;
                case 2: return Two;
                default: return Infty;
            }
        }
    }
}
