using CsharpExtras.Map.Dictionary.Variant;
using CsharpExtras.Map.Dictionary.Variant.Semi;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CsharpExtrasTest.Map
{
    [TestFixture]
   public class VariantDictionaryTests
    {
        
        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_KeyValueAddedToVariantDictionary_Then_DictionaryContainsAddedKeyValue()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Add(4, "four");
            int expectedCount = 4;
            int actualCount = variantDict.Count;
            bool actualResult = variantDict.ContainsKey(4);
            string value = variantDict[1];
            variantDict[2] = "Five";

            //Assert
            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsTrue(actualResult);
            Assert.IsTrue(value?.Equals("One"));
            Assert.IsTrue("Five".Equals(variantDict[2]));
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_ElementIsPresentInDictionary_Then_ContainsIsTrue()
        {
            //Arrange
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(5, "test");
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Add(keyValuePair);
            bool actualResult = variantDict.Contains(keyValuePair);

            //Assert
            Assert.IsTrue(actualResult);
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_ElementIsRemovedFromDictionary_Then_ContainsIsFalse()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(2, "two");

            //Act
            variantDict.Remove(keyValuePair);
            bool actualResult = variantDict.Contains(keyValuePair);

            //Assert
            Assert.IsFalse(actualResult);
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_ElementIsRemovedUsingKey_Then_ContainsIsFalse()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Remove(1);
            bool actualResult = variantDict.ContainsKey(1);

            //Assert
            Assert.IsFalse(actualResult);
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_DictionaryIsCleared_Then_DictionaryIsEmpty()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Clear();
            int expectedCount = 0;
            int actualCount = variantDict.Count;

            //Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionaryAndArray_When_DictionaryIsCopiedToArray_Then_ArrayContainsDictionaryElements()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());
            KeyValuePair<int, string>[] keyValuePairArray = new KeyValuePair<int, string>[4];
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(2, "Two");

            //Act
            variantDict.CopyTo(keyValuePairArray, 1);
            bool actualResult = keyValuePairArray.Contains(keyValuePair);

            //Assert
            Assert.IsTrue(actualResult);
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_ElementIsPresentInDictionary_Then_GetValueIsTrue()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            string value = "One";
            bool actualResult = variantDict.TryGetValue(1,out value);
            ISuccessTuple<string> successTuple = variantDict.TryGetValue(1);

            //Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual("One", successTuple.Value);

        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_DictionaryIsNotReadOnly_Then_ReadOnlyPropertyIsFalse()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Assert
            Assert.IsFalse(variantDict.IsReadOnly);
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_DictionaryGetKeys_Then_KeysShouldBeReturned()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            List<int> expectedKeys = new List<int>();
            expectedKeys.Add(1);
            expectedKeys.Add(2);
            expectedKeys.Add(3);
            List<int> actualKeys = variantDict.Keys.ToList();

            //Assert
            Assert.AreEqual(3, actualKeys.Count);
            for (int i = 0; i < actualKeys.Count; i++)
            {
                Assert.AreEqual(actualKeys.ElementAt(i), expectedKeys.ElementAt(i));
            }
        }

        [Test]
        [Category("Unit")]
        public void Given_VariantDictionary_When_DictionaryGetValues_Then_ValuesShouldBeReturned()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            List<string> expectedValus = new List<string>();
            expectedValus.Add("One");
            expectedValus.Add("Two");
            expectedValus.Add("Three");
            List<string> actualValues = variantDict.Values.ToList();

            //Assert
            Assert.AreEqual(3, actualValues.Count);
            for(int i = 0; i < actualValues.Count; i++)
            {
                Assert.AreEqual(actualValues.ElementAt(i), expectedValus.ElementAt(i));
            }

        }

        private static IDictionary<int,string> MockDictionary()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "One");
            dict.Add(2, "Two");
            dict.Add(3, "Three");
            
            return dict;
        }
    }
}
