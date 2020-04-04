using System;

namespace CsharpExtras.Log
{
    internal class InactiveLoggerImpl : ILogger
    {
        public void Error(string message)
        {
        }

        public void Error(string message, Exception ex)
        {
        }
    }
}