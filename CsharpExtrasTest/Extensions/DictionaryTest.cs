using CustomExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Category("Quick")]
        public void GivenStringIntDictionaryAndStringBoolDictionaryWhenZippedWithNegationOperatorThenResultantDictionaryIsAsExpected()
        {
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            IDictionary<string, bool> mockBoolDict = MockStrBoolDictionary();
            //Zip with simple function that negates values when the boolean is false
            IDictionary<string, int> zippedDict = mockDict.ZipValues(mockBoolDict, (i, b) => b ? i : -i);
            Assert.AreEqual(zippedDict.Keys, new string[] {"1", "2", "3"}, "Result dictionary keys are incorrect");
            Assert.AreEqual(0, zippedDict["1"]);
            Assert.AreEqual(-2, zippedDict["2"]);
            Assert.AreEqual(3, zippedDict["3"]);
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void GivenStringIntDictionaryWhenValuesMappedToStringThenValuesMatchExpectedMapValues()
        {
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            IDictionary<string, string> mappedDictionary = mockDict.MapValues(MockIntToStringMap);
            ICollection<int> sourceValues = mockDict.Values;
            IEnumerable<string> mappedValues = mappedDictionary.Values;
            IEnumerable<string> mappedFromSource = sourceValues.Select(MockIntToStringMap);
            Assert.AreEqual(mappedValues, mappedFromSource);
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void GivenStringIntDictionaryWhenValuesMappedToStringThenKeysRemainUnchanged()
        {
            IDictionary<string, int> mockDict = MockStrIntDictionary();
            IDictionary<string, string> mappedDictionary = mockDict.MapValues(MockIntToStringMap);
            Assert.AreEqual(mockDict.Keys, mappedDictionary.Keys);
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
