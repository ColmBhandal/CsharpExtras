using CsharpExtras.Extensions;
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
        public void Given_TwoMaps_When_DifferentNumberOfElementsAdded_Then_MapsAreNotEqual()
        {
            //Assemble
            IMultiValueMap<int, string> map1 = new MultiValueMapImpl<int, string>();
            IMultiValueMap<int, string> map2 = new MultiValueMapImpl<int, string>();

            //Act
            map1.Add(1, "Unity");
            map1.Add(1, "One");
            map1.Add(1, "Uno");
            map1.Add(2, "Two");

            map2.Add(1, "Unity");
            map2.Add(1, "One");
            map2.Add(1, "Uno");

            //Assert
            Assert.IsFalse(map1.Equals(map2));
        }

        [Test]
        [Category("Unit")]
        public void Given_TwoMaps_When_DifferentElementsAdded_Then_MapsAreNotEqual()
        {
            //Assemble
            IMultiValueMap<int, string> map1 = new MultiValueMapImpl<int, string>();
            IMultiValueMap<int, string> map2 = new MultiValueMapImpl<int, string>();

            //Act
            map1.Add(1, "Unity");
            map1.Add(1, "One");
            map1.Add(1, "Uno-Different");
            map1.Add(2, "Two");

            map2.Add(1, "Unity");
            map2.Add(2, "Two");
            map2.Add(1, "Uno");
            map2.Add(1, "One");

            //Assert
            Assert.IsFalse(map1.Equals(map2));
        }

        [Test]
        [Category("Unit")]
        public void Given_TwoMaps_When_SameElementsAddedInDifferentOrder_Then_MapsAreEqual()
        {
            //Assemble
            IMultiValueMap<int, string> map1 = new MultiValueMapImpl<int, string>();
            IMultiValueMap<int, string> map2 = new MultiValueMapImpl<int, string>();

            //Act
            map1.Add(1, "Unity");
            map1.Add(1, "One");
            map1.Add(1, "Uno");
            map1.Add(2, "Two");

            map2.Add(1, "Unity");
            map2.Add(2, "Two");
            map2.Add(1, "Uno");
            map2.Add(1, "One");

            //Assert
            Assert.IsTrue(map1.Equals(map2));
        }

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
