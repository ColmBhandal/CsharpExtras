using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.IO.FileNameCheck
{
    class CustomFileNameChecker : IFileNameChecker
    {
        private readonly char[] _invalidCharacters;

        public CustomFileNameChecker(char[] invalidCharacters)
        {
            _invalidCharacters = invalidCharacters ?? throw new ArgumentNullException(nameof(invalidCharacters));
        }

        public bool DoesFileNameContainInvalidCharacters(string fileName)
        {
            return (fileName.IndexOfAny(_invalidCharacters) >= 0);
        }
    }
}
