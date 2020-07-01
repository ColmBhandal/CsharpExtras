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
            //Arrange
            IMultiValueMap<int, string> testMap = new MultiValueMapImpl<int, string>();
            testMap.Add(1, "Unity");
            testMap.Add(1, "One");
            testMap.Add(2, "Two");
            //Intentionally add a value that'll map to the same as another value
            testMap.Add(2, "Dha");
            testMap.Add(3, "Three");
            
            //Act
            IMultiValueMap<int, int> transformedMap = testMap.TransformValues(s => s.Length);
            
            //Assert
            Assert.AreEqual(3, transformedMap.Count, "Transformed map should be the same length as original");
            Assert.IsTrue(new HashSet<int>(new int[] {5, 3}).SetEquals(transformedMap[1]));
            Assert.IsTrue(new HashSet<int>(new int[] {3}).SetEquals(transformedMap[2]));
            Assert.IsTrue(new HashSet<int>(new int[] {5}).SetEquals(transformedMap[3]));
        }

        [Test]
        [Category("Unit")]
        public void Given_TwoMaps_When_TheSameDataIsAddedInDifferentOrders_Then_MapHashCodesMatch()
        {
            //Assemble
            IMultiValueMap<int, string> map1 = new MultiValueMapImpl<int, string>();
            IMultiValueMap<int, string> map2 = new MultiValueMapImpl<int, string>();

            //Act
            map1.Add(1, "a");
            map1.Add(1, "b");
            map1.Add(1, "c");
            map1.Add(2, "d");
            map1.Add(2, "e");

            map2.Add(2, "e");
            map2.Add(1, "c");
            map2.Add(1, "a");
            map2.Add(2, "d");
            map2.Add(1, "b");

            //Assert
            Assert.IsTrue(map1.Equals(map2), "GIVEN: Maps should be equal");
            Assert.AreEqual(map1.GetHashCode(), map2.GetHashCode(), "Map hash codes should match when the maps have the same data");
        }

        [Test]
        [Category("Unit")]
        public void Given_OneLargeMap_When_HashCodeGenerated_Then_NoExceptionThrown()
        {
            //Assemble
            IMultiValueMap<int, string> map = new MultiValueMapImpl<int, string>();

            //Act
            for (int index1 = 0; index1 < 20; index1++)
            {
                for (int index2 = 0; index2 < 20; index2++)
                {
                    map.Add(index1, index1 + ":" + index2);
                }
            }

            //Assert
            Assert.DoesNotThrow(() => map.GetHashCode(), "GetHashCode should not throw an exception");
        }
    }
}
