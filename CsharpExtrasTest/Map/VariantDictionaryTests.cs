using CsharpExtras.Map.Dictionary.Variant;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtrasTest.Map
{
    [TestFixture]
   public class VariantDictionaryTests
    {
        
        [Test]
        [Category("Unit")]
        public void AddKeyValueToVariantDictionaryTest()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Add(4, "four");
            int expectedCount = 4;
            int actualCount = variantDict.Count;
            bool expectedResult = true;
            bool actualResult = variantDict.ContainsKey(4);

            //Assert
            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Unit")]
        public void ContainsKeyValuePairTest()
        {
            //Arrange
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(5, "test");
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Add(keyValuePair);
            bool expectedResult = true;
            bool actualResult = variantDict.Contains(keyValuePair);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Unit")]
        public void RemoveKeyValuePairFromVariantDictionaryTest()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(2, "two");

            //Act
            variantDict.Remove(keyValuePair);
            bool expectedResult = false;
            bool actualResult = variantDict.Contains(keyValuePair);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Unit")]
        public void RemoveFromVariantDicitonaryUsingKeyTest()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            variantDict.Remove(1);
            bool expectedResult = false;
            bool actualResult = variantDict.ContainsKey(1);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Unit")]
        public void ClearVariantDicitonaryTest()
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
        public void CopyToArrayFromVariantDictionaryTest()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());
            KeyValuePair<int, string>[] keyValuePairArray = new KeyValuePair<int, string>[4];
            KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(2, "Two");

            //Act
            variantDict.CopyTo(keyValuePairArray, 1);
            bool expectedResult = true;
            bool actualResult = keyValuePairArray.Contains(keyValuePair);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [Category("Unit")]
        public void TryGetValueTest()
        {
            //Arrange
            IVariantDictionary<int, string> variantDict = new VariantDictionaryImpl<int, string>(MockDictionary());

            //Act
            bool expectedResult = true;
            string value = "One";
            bool actualResult = variantDict.TryGetValue(1,out value);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);

        }

        private IDictionary<int,string> MockDictionary()
        {
            IDictionary<int, string> dict = new Dictionary<int, string>();
            dict.Add(1, "One");
            dict.Add(2, "Two");
            dict.Add(3, "Three");
            
            return dict;
        }
    }
}
