using ExampleApplication.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IApplication app = new ApplicationImpl();
            IDictionary<string, string> results = app.RunApplicationAndGetResults();

            // Print out the results
            PrintResults(results);
        }

        private static void PrintResults(IDictionary<string, string> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****** Printing results of sample application ******");
            foreach (string key in results.Keys)
            {
                sb.AppendLine(string.Format("Result of {0}: {1}", key, results[key]));
            }
            sb.AppendLine("****** Completed printing results of sample application ******");
            string output = sb.ToString();
            Console.WriteLine(output);
        }
    }
}