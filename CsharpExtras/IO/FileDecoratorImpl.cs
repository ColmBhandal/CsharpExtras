using CsharpExtras.IO.FileNameCheck;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsharpExtras.IO
{
    class FileDecoratorImpl : IFileDecorator
    {
        public IFileNameChecker FileNameChecker { get; set; } = new SystemFileNameChecker();
        public void Create(string path)
        {
            File.Create(path);
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

        public bool Exists(string path)
        {
            return File.Exists(path);
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
}
