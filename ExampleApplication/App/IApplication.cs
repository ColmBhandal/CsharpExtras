using System.Collections.Generic;

namespace ExampleApplication.App
{
    internal interface IApplication
    {
        IDictionary<string, string> RunApplicationAndGetResults();
    }
}