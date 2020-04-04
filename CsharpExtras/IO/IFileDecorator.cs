using System;

namespace CsharpExtras.IO
{
    public interface IFileDecorator
    {
        IFileFacade FileFacade { get; }
        
        void TrimEmptyLinesFromEndOfFile(string filePath);

        void TrimEmptyLinesFromEndOfFile(string filePath, Func<string, bool> isLineEmpty);

        bool IsValidFileName(string fileName);
    }
}
