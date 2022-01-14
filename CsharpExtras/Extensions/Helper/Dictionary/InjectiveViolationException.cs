using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Extensions.Helper.Dictionary
{
    public class InjectiveViolationException : Exception
    {
        public InjectiveViolationException(string message) : base(message)
        {
        }
    }
}
