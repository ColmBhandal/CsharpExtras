using System;

namespace CsharpExtras.Log
{
    public interface ILogger
    {
        void Error(string message);

        void Error(string message, Exception ex);
    }
}