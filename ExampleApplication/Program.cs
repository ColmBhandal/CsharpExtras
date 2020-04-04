using ExampleApplication.App;
using System;

namespace ExampleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IApplication app = new ApplicationImpl();
            app.RunPreApplication();
            app.RunApplication();

            // Print out the results
            PrintResults();
        }

        private static void PrintResults()
        {
            Console.WriteLine();
            Console.Write("Hello World");
        }
    }
}