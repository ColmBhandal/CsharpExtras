using CsharpExtras.Map;
using GeneralPurpose;
using Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPurpose
{
    [TestFixture]
    public class LazyFunctionMapTest
    {
        [Test]
        [Category("Unit")]
        public void TestThatDictionaryMatchesPureFunction()
        {
            ILazyFunctionMap<string, string> lazyDict = new LazyFunctionMapImpl<string, string>(TestFunction);
            var input = "AaaaaA xox";
            string expected = TestFunction(input);
            string actual = lazyDict[input];
            Assert.AreEqual(expected, actual, "Expected lazy dictionary to equal function on first access");
            expected = TestFunction(input);
            Assert.AreEqual(expected, actual, "Expected lazy dictionary to equal function on second access");
            lazyDict.Clear();
            expected = TestFunction(input);
            Assert.AreEqual(expected, actual, "Expected lazy dictionary to equal function after clear");
        }

        private string TestFunction(string input)
        {
            return input.ToLower().Replace("a", "b");
        }

    }
}
