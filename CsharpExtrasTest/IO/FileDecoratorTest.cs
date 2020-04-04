using CsharpExtras.Api;
using NUnit.Framework;
using System.Collections.Generic;

namespace IO
{
    [TestFixture]
    class FileDecoratorTest
    {
        [Test]
        [Category("Unit")]
        [TestCaseSource("StringProviderForFileNameCheck")]
        public void TestGivenStringWhenCheckIfValidFileNameThenCorrectResultReturned(
            string fileName, bool expectedResult)
        {
            ICsharpExtrasApi api = new CsharpExtrasApi();
            IFileDecorator fileDecorator = api.NewFileDecorator();

            Assert.AreEqual(expectedResult, fileDecorator.IsValidFileName(fileName), 
                "Expected the file name check to match expected result");
        }

        public static IEnumerable<object[]> StringProviderForFileNameCheck()
        {
            return new List<object[]>
            {
                new object[] { "", false },
                new object[] { "test", true },
                new object[] { "test-123", true },
                new object[] { "test.json", true },
                new object[] { "test.txt.json", true },
                new object[] { "ab:cd", false },
                new object[] { "C:\\test", false },
                new object[] { "A|b.txt", false },
                new object[] { "<23>", false },
                new object[] { "a/b.txt", false },
                new object[] { "test?", false },
            };
        }
    }
}
