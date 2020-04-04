using ExampleApplication.Worker;
using System.Collections.Generic;

namespace ExampleApplication.App
{
    internal class ApplicationImpl : IApplication
    {
        private IWorker Worker => new WorkerImpl();

        public IDictionary<string, string> RunApplicationAndGetResults()
        {
            return Worker.RunSampleAlgorithmsAndGetResults();
        }
    }
}