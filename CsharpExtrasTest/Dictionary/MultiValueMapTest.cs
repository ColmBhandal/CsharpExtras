using CsharpExtras.CustomExtensions;
using CsharpExtras.Dictionary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Dictionary
{
    [TestFixture]
    class MultiValueMapTest
    {
        [Test]
        [Category("Unit")]
        public void GivenMap_WhenTransform_ThenResultIsOfCorrectLengthAndContainsExpectedMappings()
        {
            IMultiValueMap<int, string> testMap = new MultiValueMapImpl<int, string>();
            testMap.Add(1, "Unity");
            testMap.Add(1, "One");
            testMap.Add(2, "Two");
            //Intentionally add a value that'll map to the same as another value
            testMap.Add(2, "Dha");
            testMap.Add(3, "Three");
            IMultiValueMap<int, int> transformedMap = testMap.TransformValues(s => s.Length);
            Assert.AreEqual(3, transformedMap.Count, "Transformed map should be the same length as original");
            Assert.IsTrue(new HashSet<int>(new int[] {5, 3}).SetEquals(transformedMap[1]));
            Assert.IsTrue(new HashSet<int>(new int[] {3}).SetEquals(transformedMap[2]));
            Assert.IsTrue(new HashSet<int>(new int[] {5}).SetEquals(transformedMap[3]));
        }
    }
}
