using System;
using NUnit.Framework;
using System.Collections.Generic;
using CsharpExtras.Extensions;

namespace CsharpExtrasTest.Extensions
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test, Category("Unit")]
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

        [Test, Category("Unit")]
        [TestCaseSource("ValidInsertSpacesPairs")]
        public void GivenStringWhenInsertSpacesThenResultHasSpacesInsertedAsExpected
            (string input, string expected)
        {
            //Act
            string actual = input.InsertSpaceBetweenEachLetternAndFollowingUppercaseLetter();
            
            //Assert
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


        [Test, Category("Unit")]
        public void TestGivenStringWithVariedNewlineCharsWhenGetLinesThenSetContainsSegmentsBetweenTheLines()
        {
            //Arrange
            string segment1 = "asdfads";
            string segment2 = "3742936";
            string segment3 = "adefldhask";
            string segment4 = "'#xcv;#a";
            string joinedString = segment1 + "\r\n" + segment2 + "\r" + segment3 + "\n" + segment4 + "\r\n";
            
            //Act
            ISet<string> lines = joinedString.Lines();
            
            //Assert
            Assert.AreEqual(new HashSet<string>() { segment1, segment2, segment3, segment4 }, lines);
        }

        [Test]
        [Category("Unit")]
        [TestCase("abcbd", "b", 3)]
        [TestCase("aa -- bb -- cc -- ", "--", 4)]
        [TestCase("xxyyzz", "-", 1)]
        public void TestGivenInputStringWhenSplitWithSplitterStringThenCorrectNumberOfPiecesCreated(
            string input,
            string splitter,
            int expectedCount)
        {
            //Act
            string[] pieces = input.Split(splitter);
            
            //Assert
            Assert.AreEqual(expectedCount, pieces.Length);
        }

        [Test]
        [Category("Unit")]
        public void ContainsAnyTrueWithNoMatchesInMany()
        {
            //Arrange
            string value = "AxBfd-afd_ sf";
            
            //Act
            bool result = value.ContainsAny("adgfkjadhsfas", "afgadlsfgaksfjads", "Bfdxxxxs");
            
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void ContainsAnyTrueWithOneMatchInMany()
        {
            //Arrange
            string value = "AxBfd-afd_ sf";
            
            //Act
            bool result = value.ContainsAny("adgfkjadhsfas", "afgadlsfgaksfjads", "Bfd");
            Assert.IsTrue(result);
        }

        [Test]
        [Category("Unit")]
        public void RemoveRegexMatchesRemovesMoreThanOneMatch()
        {
            //Arrange
            string value = "AxBfd-afd_ sf";
            string expected="AxBfdafdsf";
            
            //Act
            string result = value.RemoveRegexMatches("[ _-]");
            
            //Assert
            Assert.AreEqual(expected,result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWithSpacesWhenRemoveWhitespaceThenStringWithoutWhitespaceReturned()
        {
            //Arrange
            string value = " A  b C    d   E   f  G";
            string expected = "AbCdEfG";

            //Act
            string result = value.RemoveWhitespace();
            
            //Arrange
            Assert.AreEqual(expected,result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWithSlashRSlashNWhenRemoveNewlinesThenBothNewlineCharactersRemoved()
        {
            //Arrange
            string value = "FirstLine\r\nSecondLine";
            string expected = "FirstLineSecondLine";

            //Act
            string result = value.RemoveNewlines();
            
            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWithSlashNWhenRemoveNewlinesThenSlashNIsRemoved()
        {
            //Arrange
            string value = "FirstLine\nSecondLine";
            string expected = "FirstLineSecondLine";

            //Act
            string result = value.RemoveNewlines();
            
            //Assert
            Assert.AreEqual(expected,result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWithSlashRWhenRemoveNewlinesThenSlashRIsRemoved()
        {
            //Arrange
            string value = "FirstLine\rSecondLine";
            string expected = "FirstLineSecondLine";

            //Act
            string result = value.RemoveNewlines();
            
            //Assert
            Assert.AreEqual(expected,result);
        }

        [Test]
        [Category("Unit")]
        public void SplitEmptyStrIsSingletonEmptyStr()
        {
            Assert.AreEqual("".Split("dfasfadsf").Length, 1);
        }

        //non-mvp: more split test methods


        [Test]
        [Category("Unit")]
        public void EqualsIgnoreDigitsTrueWhenBothSidesHaveDifferentDigits()
        {
            //Arrange
            string value1 = "s000df11asd1hf23las";
            string value2 = "sdf33asdhf057la12389s";

            //Act
            bool result = value1.EqualsIgnoreDigits(value2);
            
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWhenStringValueConvertedToIntGreaterThanOrEqualToCalledThenCorrectValueReturned()
        {
            //Arrange
            string value = "2";
            //Act
            bool result = value.StringValueConvertedToIntGreaterThanOrEqualTo(1);
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWhenStringValueConvertedToIntGreaterThanOrEqualToOneCalledThenCorrectValueReturned()
        {
            //Arrange
            string value = "1";
            //Act
            bool result = value.StringValueConvertedToIntGreaterThanOrEqualToOne();
            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        [Category("Unit")]
        public void GivenStringWhenStringValueConvertedToIntGreaterThanOrEqualToZeroCalledThenCorrectValueReturned()
        {
            //Arrange
            string value = "0";
            //Act
            bool result = value.StringValueConvertedToIntGreaterThanOrEqualToZero();
            //Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        [Category("Unit")]
        [TestCase(" 12",12)]
        [TestCase("+352",352)]
        [TestCase("2001",2001)]
        public void GivenStringWhenIsAIntegerValueThenReturnTrue(string value, int expected)
        {
            //Act
            bool result = value.IsInt();
            //Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        [Category("Unit")]
        [TestCase("1212s123")]
        [TestCase("24124a")]
        [TestCase("1234 12")]
        [TestCase("1.0")]
        [TestCase("2.0")]
        [TestCase("16,2")]
        [TestCase(null)]
        [TestCase("1235987345983402759234085702349857342098573245343243298754302958734208954750347850234895750288888888888888888888888888888888888888888888888888888888888888888888888888888888888888888887532954720938570239485732094857023498572304985732094857")]
        public void GivenStringWhenIsNotAIntegerValueThenReturnFalse(string value)
        {
            //Act
            bool result = value.IsInt();
            //Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        [Category("Unit")]
        [TestCase(" 12",12)]
        [TestCase("+352",352)]
        [TestCase("2001",2001)]
        public void GivenStringWhenIsAIntegerValueThenReturnStringAsAInteger(string value, int expected)
        {
            //Act
            int result = value.ToInt();
            //Assert
            Assert.AreEqual(expected,result);
        }
        
        [Test]
        [Category("Unit")]
        [TestCase("1212s123")]
        [TestCase("24124a")]
        [TestCase("1234 12")]
        [TestCase("1.0")]
        [TestCase("2.0")]
        [TestCase("16,2")]
        [TestCase(null)]
        [TestCase("1235987345983402759234085702349857342098573245343243298754302958734208954750347850234895750288888888888888888888888888888888888888888888888888888888888888888888888888888888888888888887532954720938570239485732094857023498572304985732094857")]
        public void GivenStringWhenIsNotAIntegerValueThenThrowsInvalidOperationException(string value)
        {
            //Act
            Assert.Throws<InvalidOperationException>(() => value.ToInt());
        }
    }
}

