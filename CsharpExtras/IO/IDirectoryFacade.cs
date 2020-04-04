using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.IO
{
    public interface IDirectoryFacade
    {
        bool Exists(string path);

        System.IO.DirectoryInfo CreateDirectory(string path);
        void CreateIfNonExistent(string returnPath);
        string GetParentDirectoryOrThisDirectory(string path);
        /// <summary>
        /// The parent directory must be a prefix of the child directory or this function will throw an exception.
        /// </summary>
        /// <param name="pathToRootDir">The path to some directory that contains others.</param>
        /// <param name="pathToNestedDir">The path to any directory recursively contained underneath the root directory </param>
        /// <returns>Returns the relative path of the nested directory, relative to the parent directory, excluding any trailing separator characters</returns>
        string GetRelativeDifferenceBetweenPaths(string pathToRootDir, string pathToNestedDir);
    }
}
