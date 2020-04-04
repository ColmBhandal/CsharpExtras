using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public interface IFileDecorator
    {
        IFileFacade FileFacade { get; }
        
        void TrimEmptyLinesFromEndOfFile(string filePath);

        void TrimEmptyLinesFromEndOfFile(string filePath, Func<string, bool> isLineEmpty);

        bool IsValidFileName(string fileName);
    }
}
