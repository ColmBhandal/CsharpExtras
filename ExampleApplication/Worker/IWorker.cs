using System.Collections.Generic;

namespace ExampleApplication.Worker
{
    internal interface IWorker
    {
        IDictionary<string, string> RunSampleAlgorithmsAndGetResults();
    }
}