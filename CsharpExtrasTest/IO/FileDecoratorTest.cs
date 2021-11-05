using CsharpExtras.Api;
using CsharpExtras.IO;
using CsharpExtras.IO.Exception;
using CsharpExtrasTest.IO.FileNameCheck;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IO
{
    [TestFixture]
    class FileDecoratorTest
    {
        [Test]
        [Category("Unit")]
        public void GIVEN_MockDirectoryWhereAllPathsAreDirectories_WHEN_CreateFile_THEN_DirectoryAlreadyExistsExceptionThrownBeforeUnderlyingFileCreateCalled()
        {
            //Assemble
            ICsharpExtrasApi api = new CsharpExtrasApi();
            IFileDecorator fileDecorator = api.NewFileDecorator();
            Mock<IFileApiWrapper> mockFileWrapperNoOpCreate = new Mock<IFileApiWrapper>();
            mockFileWrapperNoOpCreate
                .Setup(_ => _.Create(It.IsAny<string>()))
                .Returns(() => throw new InvalidOperationException("Logic should never actually reach the underlying create method. Failure should happend before then."));
            Mock<IDirectoryDecorator> mockDirectoryDecorator = new Mock<IDirectoryDecorator>();
            //Pretend like all possible strings represent an existing directory in the filesystem
            mockDirectoryDecorator
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(true);

            fileDecorator.DirectoryDecorator = mockDirectoryDecorator.Object;
            fileDecorator.FileApiWrapper = mockFileWrapperNoOpCreate.Object;

            //Act /Assert
            Assert.Throws<DirectoryAlreadyExistsException>(
                () => fileDecorator.Create("C:/Arbitrary String Representing an existing directory"));
        }


        [Test]
        [Category("Unit")]
        [TestCaseSource("StringProviderForFileNameCheck")]
        public void TestGivenStringWhenCheckIfValidFileNameThenCorrectResultReturned(
            string fileName, bool expectedResult)
        {
            //Arrange
            ICsharpExtrasApi api = new CsharpExtrasApi();
            IFileDecorator fileDecorator = api.NewFileDecorator();
            
            //Act
            fileDecorator.FileNameChecker = new MockWindowsSpecificFileNameChecker();

            //Assert
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
