using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using CsharpExtras.Extensions;
using System;

namespace CustomExtensions
{
    [TestFixture]
    public class DictionaryTest
    {
        private const string Zero = "Zero";
        private const string One = "One";
        private const string Two = "Two";
        private const string Infty = "Infinity and Beyond";

        [Test]
        [Category("Unit")]
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
        [Category("Unit")]
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
        [Category("Unit")]
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
        [Category("Unit")]
        public void GivenDictionaryWhenAddWithKeyDerivedFromValueCalledThenCorrectItemAddedToDictionary()
        {
            //Arrange
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            string KeyFromValue(int value)
            {
                return value + "";
            }

            //Act
            mockDict.AddWithKeyDerivedFromValue(5, KeyFromValue);
            //Assert

            Assert.IsTrue(mockDict.ContainsKey("5"));
            Assert.IsTrue(mockDict.Values.Contains(5));
            Assert.IsTrue(mockDict.Contains(new KeyValuePair<string, int>("5", 5)));
        }

        [Test]
        [Category("Unit")]
        public void GivenDictionaryWhenAddWithValueDerivedFromKeyCalledThenCorrectItemAddedToDictionary()
        {
            //Arrange
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            int ValueFromKey(string key)
            {
                return Int32.Parse(key);
            }

            //Act
            mockDict.AddWithValueDerivedFromKey("5", ValueFromKey);
            //Assert

            Assert.IsTrue(mockDict.ContainsKey("5"));
            Assert.IsTrue(mockDict.Values.Contains(5));
            Assert.IsTrue(mockDict.Contains(new KeyValuePair<string, int>("5", 5)));
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
