using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.IO
{
    /// <summary>
    /// Provides extra methods on top of the methods in System.IO.Directory.
    /// Also provides alternative error-handling to System.IO.Directory.
    /// </summary>
    public interface IDirectoryDecorator
    {
        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>true if path refers to an existing directory;
        /// false if the directory does not exist or an error occurs
        /// when trying to determine if the specified directory exists.</returns>
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
