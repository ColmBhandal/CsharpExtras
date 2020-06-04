using CsharpExtras.IO.FileNameCheck;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.IO.FileNameCheck
{
    class MockWindowsSpecificFileNameChecker : CustomFileNameChecker
    {
        private static readonly char[] _windowsSpecificInvalidChars =
            new char[] { '"', '<', '>', '|', '\u0000', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\u0007', '\u0008', '	', '\n', '\u000b', '\u000c', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f', ':', '*', '?', '\\', '/' };

        public MockWindowsSpecificFileNameChecker() : base(_windowsSpecificInvalidChars)
        {
        }
    }
}
