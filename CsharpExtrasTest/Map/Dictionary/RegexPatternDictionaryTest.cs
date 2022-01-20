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
    public class RegexPatternDictionaryTest
    {
        [Test]
        [Category("Unit")]
        public void TestGivenRegexPatternToStringDictionaryWhenCheckingForMatchesThenCorrectResultReturned()
        {
            //Act
            IRegexPatternDictionary<string> regexDict = BuildTestRegexPatternDict();

            //Assert
            Assert.True(regexDict.HasMatch("hello"), "Expected 'hello' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("a day ago"), "Expected 'hi' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("world"), "Expected 'world' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("bob"), "Expected 'john' to have a match in the dictionary");
            Assert.False(regexDict.HasMatch("doe"), "Expected 'doe' to not have a match in the dictionary");
        }

        [Test]
        [Category("Unit")]
        public void TestGivenRegexPatternToStringDictionaryWhenFindingAllMatchesThenCorrectResultReturned()
        {
            //Act
            IRegexPatternDictionary<string> regexDict = BuildTestRegexPatternDict();

            //Assert
            string allMatches = string.Join(" ", regexDict.FindAllValuesThatMatch("a hello world day"));
            Assert.AreEqual("this is good", allMatches,
                "Expected all matches for 'a hello world' to form the sentence 'this is good'. No other results should be returned.");
        }

        private IRegexPatternDictionary<string> BuildTestRegexPatternDict()
        {
            IRegexPatternDictionary<string> regexDict = new RegexPatternDictionaryImpl<string>();
            regexDict.Add("hello", "this");
            regexDict.Add("wo+rld", "is");
            regexDict.Add("a.*y", "good");
            regexDict.Add("b.*", "bad");
            return regexDict;
        }

        [Test]
        [Category("Unit")]
        public void TestGivenRegexPatternToStringDictionaryWithEscapedPatternWhenCheckingForMatchesThenCorrectResultReturned()
        {
            //Arrange
            IRegexPatternDictionary<string> regexDict = new RegexPatternDictionaryImpl<string>();

            //Act
            regexDict.AddEscapedFullMatchPattern("hello", "this");
            regexDict.AddEscapedFullMatchPattern("wo+rld", "is");
            regexDict.AddEscapedFullMatchPattern("a.*y", "good");
            regexDict.AddEscapedFullMatchPattern("b.*", "bad");

            //Assert
            Assert.True(regexDict.HasMatch("hello"), "Expected 'hello' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("a.*y"), "Expected 'a.*y' to have a match in the dictionary");
            Assert.False(regexDict.HasMatch("hi"), "Expected 'hi' to not have a match in the dictionary");
            Assert.False(regexDict.HasMatch("week"), "Expected 'week' to not have a match in the dictionary");
            Assert.False(regexDict.HasMatch("john"), "Expected 'john' to not have a match in the dictionary");
            Assert.False(regexDict.HasMatch("doe"), "Expected 'doe' to not have a match in the dictionary");
        }
        
        [Test]
        [Category("Unit")]
        public void TestGivenRegexPatternToStringDictionaryWithEscapedPatternWhenGetValueByPatternThenCorrectValueReturned()
        {
            //Arrange
            IRegexPatternDictionary<string> regexDict = new RegexPatternDictionaryImpl<string>();

            string pattern = "hel+o";
            string value = "world";
            
            //Act
            regexDict.AddEscapedFullMatchPattern(pattern, value);

            //Assert
            Assert.True(regexDict.ContainsKey(pattern), "Pattern should exist as dictionary key");
            Assert.AreEqual(value, regexDict[pattern], "Value tied to pattern should be correct");
        }

        [Test]
        [Category("Unit")]
        public void TestGivenRegexPatternToStringDictionaryWithFullMatchPatternWhenCheckingForMatchesThenCorrectResultReturned()
        {
            //Arrange
            IRegexPatternDictionary<string> regexDict = new RegexPatternDictionaryImpl<string>();

            //Act
            regexDict.AddFullMatchPattern("hello", "this");
            regexDict.AddFullMatchPattern("wo+rld", "is");
            regexDict.AddFullMatchPattern("a.*y", "good");
            regexDict.AddFullMatchPattern("b.*", "bad");

            //Assert
            Assert.True(regexDict.HasMatch("hello"), "Expected 'hello' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("wooorld"), "Expected 'wooorld' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("a day"), "Expected 'a day' to have a match in the dictionary");
            Assert.True(regexDict.HasMatch("before"), "Expected 'before' to have a match in the dictionary");
            Assert.False(regexDict.HasMatch("a day ago"), "Expected 'a day ago' to not have a match in the dictionary");
            Assert.False(regexDict.HasMatch("day before"), "Expected 'day before' to not have a match in the dictionary");
        }

    }
}
