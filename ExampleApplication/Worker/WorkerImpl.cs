

using CsharpExtras.Api;
using OneBased;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleApplication.Worker
{
    internal class WorkerImpl : IWorker
    {
        ICsharpExtrasApi _csharpExtrasApi;
        ICsharpExtrasApi CsharpExtrasApi => _csharpExtrasApi ?? (_csharpExtrasApi = new CsharpExtrasApi());
        //non-mvp: Add usages of more of the library types here to show how they can be used
        public IDictionary<string, string> RunSampleAlgorithmsAndGetResults()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            AddResultsForFindInverseOfOneBasedArray(dict);
            return dict;
        }

        private void AddResultsForFindInverseOfOneBasedArray(IDictionary<string, string> dict)
        {
            string[] zeroBasedBackingArray = new string[] { "One", "Two", "Buzz", "Four", "Five", "Buzz"};
            IOneBasedArray<string> array = CsharpExtrasApi.NewOneBasedArray(zeroBasedBackingArray);
            IDictionary<string, IList<int>> inverseMap = array.InverseMap();
            StringBuilder sb = new StringBuilder("[ ");
            foreach((string key, IList<int> indices) in inverseMap)
            {
                sb.Append("(").Append(key).Append(" -> ").Append(indices[0]);
                for(int i = 1; i < indices.Count; i++)
                {
                    sb.Append(", ").Append(indices[i]);
                }
                sb.Append(") ");
            }
            sb.Append("]");
            string resultString = sb.ToString();
            dict.Add("One-based inverse", resultString);
        }
    }
}