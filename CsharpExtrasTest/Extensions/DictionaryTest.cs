using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Extensions;
using System;
using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.Compare;

namespace CsharpExtrasTest.Extensions
{
    [TestFixture,Category("Unit")]
    public class DictionaryTest
    {
        private const string Zero = "Zero";
        private const string One = "One";
        private const string Two = "Two";
        private const string Three = "Three";
        private const string Infty = "Infinity and Beyond";

        [Test]
        public void GIVEN_NonInjectionOnKeyset_WHEN_UpdateKeys_THEN_InjectiveViolationException()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
                {0, "Some value"},
                {1, "Hello World"},
                {2, "x" },
                {3, "x" },
                {4, "x" }
            };

            Func<int, int> nonInjection = i => 7;

            //Act / Assert
            Assert.Throws<InjectiveViolationException>(() => dictionary.UpdateKeys(nonInjection));
        }

        [Test]
        public void GIVEN_InjectionOnKeyset_WHEN_UpdateKeys_THEN_DictionaryIsAsExpected()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
                {0, "Some value"},
                {1, "Hello World"},
                {2, "x" }
            };
            Dictionary<int, string> expectedDictionary = new Dictionary<int, string>()
            {
                {1, "Some value"},
                {2, "Hello World"},
                {3, "x" }
            };

            Func<int, int> injection = i => i + 1;

            //Act
            //Note: the function here is only injective on the keyset, not across its entire domain
            dictionary.UpdateKeys(injection);

            //Assert
            IComparisonResult comparison = dictionary.Compare(expectedDictionary, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_InjectionOnKeyset_WHEN_MapKeys_THEN_DictionaryIsAsExpected()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
                {0, "Some value"},
                {1, "Hello World"},
                {2, "x" }
            };
            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>()
            {
                {Zero, "Some value"},
                {One, "Hello World"},
                {Two, "x" }
            };

            //Act
            //Note: the function here is only injective on the keyset, not across its entire domain
            IDictionary<string, string> mappedDict = dictionary.MapKeys(MockIntToStringMap);

            //Assert
            IComparisonResult comparison = mappedDict.Compare(expectedDictionary, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_NonInjectionOnKeyset_WHEN_MapKeys_THEN_InjectiveViolationException()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
                {0, "Some value"},
                {1, "Hello World"},
                {2, "x" },
                {3, "x" },
                {4, "x" }
            };

            //Act / Assert
            //Note: the function is non-injective here because it only maps a subset of ints to unique values
            Assert.Throws<InjectiveViolationException>(() => dictionary.MapKeys(MockIntToStringMap));
        }

        [Test]
        public void GIVEN_ExceptionThrowingFunctionAndEmptyDictionary_WHEN_MapKeys_THEN_EmptyDictionaryReturned()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            Assert.AreEqual(0, dictionary.Count, "GIVEN: Expected empty dictionary to have count of zero to begin with");

            Func<int, string> exceptionThrower = i => throw new InvalidOperationException();

            //Act
            IDictionary<string, string> mappedDict = dictionary.MapKeys(exceptionThrower);

            //Assert
            Assert.AreEqual(0, mappedDict.Count);
        }

        [Test]
        public void GIVEN_DictionariesWithSameSizeButUnequalPairs_WHEN_Compare_THEN_ResultIsNotEqual()
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
            IComparisonResult comparison = dictionary1.Compare(dictionary2, string.Equals);

            //Assert
            Assert.IsFalse(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_ThisDictionaryPairsAreStrictSupersetOfOther_WHEN_Compare_THEN_ResultIsNotEqual()
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
            IComparisonResult comparison = dictionary1.Compare(dictionary2, string.Equals);

            //Assert
            Assert.IsFalse(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_ThisDictionaryPairsAreStrictSubsetOfOther_WHEN_Compare_THEN_ResultIsNotEqual()
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
            IComparisonResult comparison = dictionary1.Compare(dictionary2, string.Equals);

            //Assert
            Assert.IsFalse(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_DictionariesWithSamePairsAddedInDifferentOrder_WHEN_Compare_THEN_ResultIsEqual()
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
            IComparisonResult comparison = dictionary1.Compare(dictionary2, string.Equals);

            //Assert
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_EmptyDictionary_WHEN_CompareItself_THEN_ResultIsEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
            };

            //Act
            IComparisonResult comparison = dictionary.Compare(dictionary, string.Equals);

            //Assert
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_FilledDictionary_WHEN_CompareItself_THEN_ResultIsEqual()
        {
            //Arrange
            Dictionary<int, string> dictionary = new Dictionary<int, string>()
            {
                { 1, "Hello World"},
                {2, "Some other value" }
            };

            //Act
            IComparisonResult comparison = dictionary.Compare(dictionary, string.Equals);

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
