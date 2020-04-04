using System;

namespace CsharpExtras.Log
{
    public interface ILogger
    {
        void Error(string message);

        void Error(string message, Exception ex);
        void DebugFormat(string message, params string[] formatArgs);
        void Warn(string message);
        void Debug(string message);
    }
}