using System;
using System.IO;
using System.Linq;

namespace CsharpExtras.IO
{
    class FileDecoratorImpl : IFileDecorator
    {
        public IFileFacade FileFacade { get; }

        public FileDecoratorImpl(IFileFacade fileFacade)
        {
            FileFacade = fileFacade;
        }

        public void TrimEmptyLinesFromEndOfFile(string filePath)
        {
            TrimEmptyLinesFromEndOfFile(filePath, str => string.IsNullOrWhiteSpace(str));
        }

        public void TrimEmptyLinesFromEndOfFile(string filePath, Func<string, bool> isLineEmpty)
        {
            string[] lines = FileFacade.ReadAllLines(filePath);
            int maxIndex = lines.Count();

            for (int i = lines.Count() - 1; i > 0; i--)
            {
                if (!isLineEmpty(lines[i]))
                {
                    break;
                }
                maxIndex--;
            }

            FileFacade.WriteAllLines(filePath, lines.Take(maxIndex));
        }

        public bool IsValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }
            return !(fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0);
        }
    }
}
