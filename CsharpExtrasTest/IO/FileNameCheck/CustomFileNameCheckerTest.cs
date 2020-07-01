using CsharpExtras.IO.FileNameCheck;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.IO.FileNameCheck
{
    class CustomFileNameCheckerTest
    {
        [Test]
        [Category("Unit")]
        [TestCaseSource("ProviderForInvalidCharCheck")]
        public void Given_CustomFileNameChecker_When_CheckIfFileNameContainsInvalidChars_Then_ReturnCorrectResult(
            string fileName, char[] invalidChars, bool expectedResult)
        {
            //Act
            IFileNameChecker fileNameChecker = new CustomFileNameChecker(invalidChars);
            
            //Assert
            Assert.AreEqual(expectedResult, fileNameChecker.DoesFileNameContainInvalidCharacters(fileName),
                "Expected the file name check to match expected result");
        }

        public static IEnumerable<object[]> ProviderForInvalidCharCheck()
        {
            return new List<object[]>
            {
                new object[] { "test", new char[] { }, false },
                new object[] { "test", new char[] { 't' }, true },
                new object[] { "<23>", new char[] { }, false },
                new object[] { "<23>", new char[] { 't' }, false },
                new object[] { "<23>", new char[] { '<' }, true },
            };
        }
    }
}
