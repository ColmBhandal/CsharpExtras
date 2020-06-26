using CsharpExtras.Api;
using CsharpExtras.Dictionary.Collection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CsharpExtrasTest.Dictionary.Collection
{
    [TestFixture, Category("Unit")]
    public class ListValuedDictionaryTest
    {
        [Test]
        public void Given_ListValuedDictionary_When_ElementsAddedToKey_Then_ListAtThatKeyPreservedInsertionOrder()
        {
            //Assemble
            IListValuedDictionary<int, int> dict1 = new CsharpExtrasApi().NewListValuedDictionary<int, int>();

            //Act
            dict1.Add(1, 11);
            dict1.Add(1, 12);
            dict1.Add(1, 13);
            dict1.Add(1, 14);
            dict1.Add(1, 13);
            dict1.Add(1, 12);
            dict1.Add(1, 11);

            //Assert
            Assert.IsTrue(dict1[1].SequenceEqual(new List<int>{11, 12, 13, 14, 13, 12, 11}),
                "Dictionary sequence at given key does not match the expected sequence");
        }

        [Test]
        public void Given_ListDictionaries_When_SameElementsAddedInSameOrderPerKey_Then_DictionariesEqual()
        {
            //Assemble
            IListValuedDictionary<int, int> dict1 = new CsharpExtrasApi().NewListValuedDictionary<int, int>();
            IListValuedDictionary<int, int> dict2 = new CsharpExtrasApi().NewListValuedDictionary<int, int>();

            //Act
            dict1.Add(1, 11);
            dict1.Add(1, 12);
            dict1.Add(2, 21);
            dict1.Add(2, 22);

            dict2.Add(1, 11);
            dict2.Add(2, 21);
            dict2.Add(2, 22);
            dict2.Add(1, 12);

            //Assert
            Assert.IsTrue(dict1.DictEquals(dict2), "Expected dictionaries to be equal");
        }

        [Test]
        public void Given_ListDictionaries_When_SameElementsAddedInSameOrderPerKey_Then_ValueSequencesEqualPerKey()
        {
            //Assemble
            IListValuedDictionary<int, int> dict1 = new CsharpExtrasApi().NewListValuedDictionary<int, int>();
            IListValuedDictionary<int, int> dict2 = new CsharpExtrasApi().NewListValuedDictionary<int, int>();

            //Act
            dict1.Add(1, 11);
            dict1.Add(1, 12);
            dict1.Add(2, 21);
            dict1.Add(2, 22);

            dict2.Add(1, 11);
            dict2.Add(2, 21);
            dict2.Add(1, 12);
            dict2.Add(2, 22);

            //Assert
            Assert.IsTrue(dict1.Keys.ToHashSet().SetEquals(dict2.Keys.ToHashSet()),
                "Expected keysets of the two dictionaries to be the same");
            foreach(int key in dict1.Keys)
            {
                Assert.IsTrue(dict1[key].SequenceEqual(dict2[key]));
            }            
        }

        [Test]
        public void Given_ListDictionaries_When_DifferentElementsAddedToSameKey_Then_ValueSetsNotEqual()
        {
            //Assemble
            IListValuedDictionary<int, int> dict1 = new ListValuedDictionaryImpl<int, int>();
            IListValuedDictionary<int, int> dict2 = new ListValuedDictionaryImpl<int, int>();

            //Act
            dict1.Add(1, 11);
            dict1.Add(1, 12);
            dict2.Add(1, 11);
            dict2.Add(1, 13);

            //Assert
            Assert.IsFalse(dict1[1].ToHashSet().SetEquals(dict2[1].ToHashSet()));
        }

        [Test]
        public void Given_ListDictionaries_When_SameElementsAddedInDifferentOrderToSameKey_Then_ValueSequencesNotEqual()
        {
            //Assemble
            IListValuedDictionary<int, int> dict1 = new ListValuedDictionaryImpl<int, int>();
            IListValuedDictionary<int, int> dict2 = new ListValuedDictionaryImpl<int, int>();

            //Act
            dict1.Add(1, 11);
            dict1.Add(1, 12);
            dict2.Add(1, 12);
            dict2.Add(1, 11);

            //Assert
            Assert.IsFalse(dict1[1].SequenceEqual(dict2[1]));
        }

        [Test]
        public void Given_ListDictionaries_When_SameElementsAddedInDifferentOrderToSameKey_Then_ValueSetsAreEqual()
        {
            //Assemble
            IListValuedDictionary<int, int> dict1 = new ListValuedDictionaryImpl<int, int>();
            IListValuedDictionary<int, int> dict2 = new ListValuedDictionaryImpl<int, int>();
            
            //Act
            dict1.Add(1, 11);
            dict1.Add(1, 12);
            dict2.Add(1, 12);
            dict2.Add(1, 11);

            //Assert
            Assert.IsTrue(dict1[1].ToHashSet().SetEquals(dict2[1].ToHashSet()));
        }
    }
}
