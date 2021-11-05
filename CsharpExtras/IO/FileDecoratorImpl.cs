using CsharpExtras.IO.FileNameCheck;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsharpExtras.IO
{
    class FileDecoratorImpl : IFileDecorator
    {
        public IFileApiWrapper FileApiWrapper { get; set; } = new FileApiWrapperImpl();
        public IDirectoryDecorator DirectoryDecorator { get; set; } = new DirectoryDecoratorImpl();
        public IFileNameChecker FileNameChecker { get; set; } = new SystemFileNameChecker();
        public FileStream Create(string path)
        {
            return FileApiWrapper.Create(path);
        }

        public string[] ReadAllLines(string path)
        {
            return FileApiWrapper.ReadAllLines(path);
        }

        public string ReadAllText(string path)
        {
            return FileApiWrapper.ReadAllText(path);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            FileApiWrapper.WriteAllLines(path, contents);
        }

        public void WriteAllText(string path, string contents)
        {
            FileApiWrapper.WriteAllText(path, contents);
        }

        public bool Exists(string path)
        {
            return FileApiWrapper.Exists(path);
        }

        public void TrimEmptyLinesFromEndOfFile(string filePath)
        {
            TrimEmptyLinesFromEndOfFile(filePath, str => string.IsNullOrWhiteSpace(str));
        }

        public void TrimEmptyLinesFromEndOfFile(string filePath, Func<string, bool> isLineEmpty)
        {
            string[] lines = ReadAllLines(filePath);
            int maxIndex = lines.Count();

            for (int i = lines.Count() - 1; i > 0; i--)
            {
                if (!isLineEmpty(lines[i]))
                {
                    break;
                }
                maxIndex--;
            }

            WriteAllLines(filePath, lines.Take(maxIndex));
        }

        public bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }
            return !FileNameChecker.DoesFileNameContainInvalidCharacters(fileName);
        }
    }

    /// <summary>
    /// The purpose of this is to wrap static methods from System.IO.File allowing our decorator to be decoupled from them
    /// This allows, for example, mocking out File system behaviour for testing without having to use a real file system
    /// </summary>
    internal class FileApiWrapperImpl : IFileApiWrapper
    {
        public FileStream Create(string path)
        {
            return File.Create(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            File.WriteAllLines(path, contents);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }

    public interface IFileApiWrapper
    {
        FileStream Create(string path);
        bool Exists(string path);
        string[] ReadAllLines(string path);
        string ReadAllText(string path);
        void WriteAllLines(string path, IEnumerable<string> contents);
        void WriteAllText(string path, string contents);
    }
}
