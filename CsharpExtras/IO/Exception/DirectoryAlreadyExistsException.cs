using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsharpExtras.IO.Exception
{
    public class DirectoryAlreadyExistsException : IOException
    {
        public DirectoryAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
