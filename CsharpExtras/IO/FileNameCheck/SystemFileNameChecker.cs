using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsharpExtras.IO.FileNameCheck
{
    class SystemFileNameChecker : CustomFileNameChecker, IFileNameChecker
    {
        public SystemFileNameChecker()
            : base(Path.GetInvalidFileNameChars())
        {
        }
    }
}
