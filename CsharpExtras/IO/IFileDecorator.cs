using CsharpExtras.IO.FileNameCheck;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsharpExtras.IO
{
    /// <summary>
    /// Provides extra methods on top of the methods in System.IO.File.
    /// Also provides alternative error-handling to System.IO.File.
    /// </summary>
    public interface IFileDecorator
    {
        IFileNameChecker FileNameChecker { get; set; }
        IDirectoryDecorator DirectoryDecorator { get; set; }
        IFileApiWrapper FileApiWrapper { get; set; }

        void TrimEmptyLinesFromEndOfFile(string filePath);

        void TrimEmptyLinesFromEndOfFile(string filePath, Func<string, bool> isLineEmpty);

        bool IsValidFileName(string fileName);
        FileStream Create(string path);

        string[] ReadAllLines(string path);

        string ReadAllText(string path);

        void WriteAllLines(string path, IEnumerable<string> contents);

        void WriteAllText(string path, string contents);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path is valid, the file is found, and the user has read access to the file. False otherwise.</returns>
        bool Exists(string path);
    }
}
