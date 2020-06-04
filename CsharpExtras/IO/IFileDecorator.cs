using CsharpExtras.IO.FileNameCheck;
using System;

namespace CsharpExtras.IO
{
    public interface IFileDecorator
    {
        IFileFacade FileFacade { get; }

        IFileNameChecker FileNameChecker { get; set; }

        void TrimEmptyLinesFromEndOfFile(string filePath);

        void TrimEmptyLinesFromEndOfFile(string filePath, Func<string, bool> isLineEmpty);

        bool IsValidFileName(string fileName);
    }
}
