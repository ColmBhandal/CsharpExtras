using System;

namespace CsharpExtras.Log
{
    internal class InactiveLoggerImpl : ILogger
    {
        public void Debug(string message)
        {
        }

        public void DebugFormat(string message, params string[] formatArgs)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception ex)
        {
        }

        public void Warn(string message)
        {
        }
    }
}