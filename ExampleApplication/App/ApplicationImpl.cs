using ExampleApplication.Worker;

namespace ExampleApplication.App
{
    internal class ApplicationImpl : IApplication
    {
        private IWorker Worker => new WorkerImpl();

        public void RunPreApplication()
        {
            Worker.DoWork();
        }

        public void RunApplication()
        {
            for (int i = 0; i < 10; i++)
            {
                Worker.DoWork();
            }
        }
    }
}