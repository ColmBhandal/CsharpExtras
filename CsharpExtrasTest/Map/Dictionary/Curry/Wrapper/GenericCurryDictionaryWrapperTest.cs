using CsharpExtras.Api;
using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.Map.Dictionary.Curry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Map.Dictionary.Curry.Wrapper
{
    internal class GenericCurryDictionaryWrapperTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();
        [Test]
        public void GIVEN_WrappedDict_WHEN_Compare_THEN_IsEqualToExpectedDict()
        {
            //Arrange
            ICurryDictionary<int, string> backingDict = Api.NewCurryDictionary<int, string>(4);
            backingDict.Add("1024", 3, 8, 15, 24);
            backingDict.Add("512", 5, 12, 21, 32);

            ICurryDictionary<string, int> expectedDict = Api.NewCurryDictionary<string, int>(4);
            expectedDict.Add(1024, "3", "4", "5", "6");
            expectedDict.Add(512, "5", "6", "7", "8");

            ICurryDictionary<string, int> wrappedDict = Api.NewGenericCurryDictionaryWrapper
                (backingDict, (s, i) => (i + 1) * int.Parse(s), (k, i) => (k/ (i + 1)).ToString(), i => i.ToString(), int.Parse);

            //Act

            IDictionaryComparison comparison = expectedDict.Compare(wrappedDict, (i, j) => i == j);

            //Assert            
            Assert.IsTrue(comparison.IsEqual, comparison.Message);

        }
    }
}
