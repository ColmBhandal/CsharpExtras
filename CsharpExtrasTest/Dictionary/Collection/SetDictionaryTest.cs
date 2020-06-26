using CsharpExtras.Extensions;
using CsharpExtras.Dictionary;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using CsharpExtras.Dictionary.Collection;

namespace CsharpExtrasTest.Dictionary
{
    [TestFixture]
    class SetDictionaryTest
    {
        [Test]
        [Category("Unit")]
        public void GivenEmptyMapWhenTwoItemsAddedToSameKeyThenBothItemsAreInSetForThatKey()
        {
            ISetValuedDictionary<string, int> setDictionary = new SetValuedDictionaryImpl<string, int>();
            string key = "bob";
            Assert.IsFalse(setDictionary.ContainsKey(key), string.Format(
                "Given: expected test to start with empty map entry for key {0}, but map appears to contain value for that key."
                , key));
            const int value1 = 1;
            setDictionary.Add(key, value1);
            const int value2 = 2;
            setDictionary.Add(key, value2);
            Assert.AreEqual(setDictionary[key], new int[] { value1, value2 });
        }

        [Test]
        [Category("Unit")]
        public void Given_TwoMaps_When_DifferentNumberOfElementsAdded_Then_MapsAreNotEqual()
        {
            //Assemble
            ISetValuedDictionary<int, string> map1 = new SetValuedDictionaryImpl<int, string>();
            ISetValuedDictionary<int, string> map2 = new SetValuedDictionaryImpl<int, string>();

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
            ISetValuedDictionary<int, string> map1 = new SetValuedDictionaryImpl<int, string>();
            ISetValuedDictionary<int, string> map2 = new SetValuedDictionaryImpl<int, string>();

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
            ISetValuedDictionary<int, string> map1 = new SetValuedDictionaryImpl<int, string>();
            ISetValuedDictionary<int, string> map2 = new SetValuedDictionaryImpl<int, string>();

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
            Assert.IsTrue(map1.DictEquals(map2));
        }

        [Test]
        [Category("Unit")]
        public void GivenMap_WhenTransform_ThenResultIsOfCorrectLengthAndContainsExpectedMappings()
        {
            ISetValuedDictionary<int, string> testMap = new SetValuedDictionaryImpl<int, string>();
            testMap.Add(1, "Unity");
            testMap.Add(1, "One");
            testMap.Add(2, "Two");
            //Intentionally add a value that'll map to the same as another value
            testMap.Add(2, "Dha");
            testMap.Add(3, "Three");
            ISetValuedDictionary<int, int> transformedMap = testMap.TransformValues(s => s.Length);
            Assert.AreEqual(3, transformedMap.Count, "Transformed map should be the same length as original");
            Assert.IsTrue(new HashSet<int>(new int[] {5, 3}).SetEquals(transformedMap[1]));
            Assert.IsTrue(new HashSet<int>(new int[] {3}).SetEquals(transformedMap[2]));
            Assert.IsTrue(new HashSet<int>(new int[] {5}).SetEquals(transformedMap[3]));
        }

        [Test]
        [Category("Unit")]
        public void Given_OneLargeMap_When_HashCodeGenerated_Then_NoExceptionThrown()
        {
            //Assemble
            ISetValuedDictionary<int, string> map = new SetValuedDictionaryImpl<int, string>();

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
