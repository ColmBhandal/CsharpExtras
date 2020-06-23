using CsharpExtras.IO;
using IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    [TestFixture]
    public class DirectoryFacadeTest
    {
        private readonly IDirectoryFacade _directoryFacade = new DirectoryFacadeImpl();

        [Test, Category("Unit")]
        public void TestGivenRootAndRelativeSubDirWhenCombineThenGetDifferenceThenResultEqualsRelativeSubDir()
        {
            string root = Directory.GetCurrentDirectory();
            string[] relativeSubDirs = new string[] { "level1", "level2" };
            string relativeSubDir = Path.Combine(relativeSubDirs);
            string absoluteSubDir = Path.Combine(root, relativeSubDir);
            string diff = _directoryFacade.GetRelativeDifferenceBetweenPaths(root, absoluteSubDir);
            Assert.AreEqual(relativeSubDir, diff);
        }
    }
}
