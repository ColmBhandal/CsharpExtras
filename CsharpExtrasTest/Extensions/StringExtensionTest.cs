using NUnit.Framework;
using System.Collections.Generic;

namespace CustomExtensions
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test, Category("Unit"), Category("Quick")]
        [TestCaseSource("ValidRegexCaptureGroupTuples")]
        public void GivenStringAndRegexWhenGetFirstGroupInFirstMatchThenExpectedContentIsReturned
            (string input, string regex, string expected)
        {
            string actual = input.GetFirstRegexCaptureGroupInFirstMatchOrBlank(regex);
            Assert.AreEqual(expected, actual);
        }

        private static IEnumerable<object[]> ValidRegexCaptureGroupTuples()
        {
            return new List<object[]>()
            {
                new object[] { "Hello World", "Wo(r)ld", "r"},
                new object[] { "Hello World", "(H)", "H"},
                new object[] { "Hello World", "(.+)W", "Hello "},
            };
        }

        [Test, Category("Unit"), Category("Quick")]
        [TestCaseSource("ValidInsertSpacesPairs")]
        public void GivenStringWhenInsertSpacesThenResultHasSpacesInsertedAsExpected
            (string input, string expected)
        {
            string actual = input.InsertSpaceBetweenEachLetternAndFollowingUppercaseLetter();
            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> ValidInsertSpacesPairs()
        {
            return new List<object[]>()
            {
                new object[] { "nouppercharsorspaces", "nouppercharsorspaces" },
                new object[] { "UpperCharsNoSpaces", "Upper Chars No Spaces" },
                new object[] { "  spaces but  no upper chars  ", "  spaces but  no upper chars  " },
                new object[] { "Upper Chars And Spaces", "Upper Chars And Spaces" }
            };
        }


        [Test, Category("Unit"), Category("Quick")]
        public void TestGivenStringWithVariedNewlineCharsWhenGetLinesThenSetContainsSegmentsBetweenTheLines()
        {
            string segment1 = "asdfads";
            string segment2 = "3742936";
            string segment3 = "adefldhask";
            string segment4 = "'#xcv;#a";
            string joinedString = segment1 + "\r\n" + segment2 + "\r" + segment3 + "\n" + segment4 + "\r\n";
            ISet<string> lines = joinedString.Lines();
            Assert.AreEqual(new HashSet<string>() { segment1, segment2, segment3, segment4 }, lines);
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        [TestCase("abcbd", "b", 3)]
        [TestCase("aa -- bb -- cc -- ", "--", 4)]
        [TestCase("xxyyzz", "-", 1)]
        public void TestGivenInputStringWhenSplitWithSplitterStringThenCorrectNumberOfPiecesCreated(
            string input,
            string splitter,
            int expectedCount)
        {
            string[] pieces = input.Split(splitter);
            Assert.AreEqual(expectedCount, pieces.Length);
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void ContainsAnyTrueWithNoMatchesInMany()
        {
            Assert.IsFalse("AxBfd-afd_ sf".ContainsAny("adgfkjadhsfas", "afgadlsfgaksfjads", "Bfdxxxxs"));
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void ContainsAnyTrueWithOneMatchInMany()
        {
            Assert.IsTrue("AxBfd-afd_ sf".ContainsAny("adgfkjadhsfas", "afgadlsfgaksfjads", "Bfd"));
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void RemoveRegexMatchesRemovesMoreThanOneMatch()
        {
            Assert.AreEqual("AxBfd-afd_ sf".RemoveRegexMatches("[ _-]"), "AxBfdafdsf");
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void GivenStringWithSpacesWhenRemoveWhitespaceThenStringWithoutWhitespaceReturned()
        {
            Assert.AreEqual(" A  b C    d   E   f  G".RemoveWhitespace(), "AbCdEfG");
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void GivenStringWithSlashRSlashNWhenRemoveNewlinesThenBothNewlineCharactersRemoved()
        {
            Assert.AreEqual("FirstLine\r\nSecondLine".RemoveNewlines(), "FirstLineSecondLine");
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void GivenStringWithSlashNWhenRemoveNewlinesThenSlashNIsRemoved()
        {
            Assert.AreEqual("FirstLine\nSecondLine".RemoveNewlines(), "FirstLineSecondLine");
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void GivenStringWithSlashRWhenRemoveNewlinesThenSlashRIsRemoved()
        {
            Assert.AreEqual("FirstLine\rSecondLine".RemoveNewlines(), "FirstLineSecondLine");
        }

        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void SplitEmptyStrIsSingletonEmptyStr()
        {
            Assert.AreEqual("".Split("dfasfadsf").Length, 1);
        }

        //non-mvp: more split test methods


        [Test]
        [Category("Unit")]
        [Category("Quick")]
        public void EqualsIgnoreDigitsTrueWhenBothSidesHaveDifferentDigits()
        {
            Assert.IsTrue("s000df11asd1hf23las".EqualsIgnoreDigits("sdf33asdhf057la12389s"));
        }
    }
}
