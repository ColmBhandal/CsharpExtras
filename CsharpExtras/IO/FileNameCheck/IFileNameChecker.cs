using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.IO.FileNameCheck
{
    public interface IFileNameChecker
    {
        bool DoesFileNameContainInvalidCharacters(string fileName);
    }
}
