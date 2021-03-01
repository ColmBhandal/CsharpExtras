using CsharpExtras.Api;
using CsharpExtras.Map.Dictionary.Curry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Map.Dictionary.Curry
{
    [TestFixture]
    public class CurryDictionaryTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        /// <summary>
        /// In loving memory of Mikhail Volkov, a brilliant coder who first introduced me to C# and delegates.
        /// </summary>
        delegate string MikhailsDelegate(string input);

        [Test]
        [Category("Unit")]
        public void Given_DelegateDictionary_When_AddDelegate_Then_DelegateFoundAtGivenKeyAndReturnsExpectedValue()
        {
            //Assemble
            ICurryDictionary<int, MikhailsDelegate> dict = Api.NewCurryDictionary<int, MikhailsDelegate>(2);

            //Act
            dict.Add(s => $"Delegates are awesome! {s}", 13, 22);

            //Assert
            MikhailsDelegate f = dict[13, 22];
            string result = f("RIP Mikhail");
            Assert.AreEqual(result, "Delegates are awesome! RIP Mikhail");
        }

        [Test]
        [Category("Unit")]
        public void Given_PositiveInteger_When_ConstructCurryDictionary_Then_ArityMatchesCtorParameter()
        {
            //Assemble / Act
            const int ExpectedArity = 23;
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(ExpectedArity);

            //Assert
            Assert.AreEqual(ExpectedArity, (int)dict.Arity);
        }

        [Test]
        [Category("Unit")]
        public void Given_NewCurryDictionary_When_Add_Then_KeysMatchExpectation()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            dict.Add("Hello World 11", 1, 1);
            dict.Add("Hello World 12", 1, 2);
            dict.Add("Hello World 21", 2, 1);

            //Assert
            IEnumerable<IList<int>> keyset = dict.Keys;
            IEnumerable<IList<int>> expectedKeyset = new List<IList<int>>
                {new List<int>{1,1}, new List<int>{1,2}, new List<int>{2,1} };
            Assert.AreEqual(expectedKeyset, keyset);
        }

        [Test]
        [Category("Unit")]
        public void Given_NewCurryDictionary_When_Add_Then_KeyValuePairsMatchExpectation()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            dict.Add("Hello World 11", 1, 1);
            dict.Add("Hello World 12", 1, 2);
            dict.Add("Hello World 21", 2, 1);

            //Assert
            IEnumerable<(IList<int>, string)> pairs = dict.KeyValuePairs;
            IEnumerable<(IList<int>, string)> expectedPairs = new List<(IList<int>, string)>
                {(new List<int>{1,1}, "Hello World 11"),
                (new List<int>{1,2}, "Hello World 12"),
                (new List<int>{2,1}, "Hello World 21")
            };
            Assert.AreEqual(expectedPairs, pairs);
        }

        [Test]
        [Category("Unit")]
        public void Given_NewCurryDictionary_When_Add_Then_ValuesMatchExpectation()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            dict.Add("Hello World 11", 1, 1);
            dict.Add("Hello World 12", 1, 2);
            dict.Add("Hello World 21", 2, 1);

            //Assert
            IEnumerable<string> values = dict.Values;
            IEnumerable<string> expectedPairs = new List<string>
            {"Hello World 11", "Hello World 12", "Hello World 21"};
            Assert.AreEqual(expectedPairs, values);
        }

        [Test]
        [Category("Unit")]
        public void Given_NewCurryDictionary_When_Add_Then_DictContainsKeyTuple()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            dict.Add("Hello World", 1, 2);

            //Assert
            Assert.IsTrue(dict.ContainsKeyTuple(1, 2));
        }

        [Test]
        [Category("Unit")]
        public void Given_NewCurryDictionary_When_Add_Then_DictContainsKeyTuplePrefix()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act
            dict.Add("Hello World", 1, 2, 3);

            //Assert
            Assert.IsTrue(dict.ContainsKeyTuplePrefix(1, 2));
        }

        [Test]
        [Category("Unit")]
        public void Given_CurryDictionary_When_PrefixNotThere_Then_ContainsKeyTuplePrefixReturnsFalse()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Hello World", 1, 2, 3);
            dict.Add("Hello World again", 2, 17, 34);

            //Act
            bool containsPrefix = dict.ContainsKeyTuplePrefix(1, 145);

            //Assert
            Assert.IsFalse(containsPrefix, "Expected non-existent prefix contains to return false");
        }

        [Test]
        [Category("Unit")]
        public void Given_EmptyCurryDictionary_When_ContainsKeyTuplePrefix_Then_FalseReturned()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act
            bool containsPrefix = dict.ContainsKeyTuplePrefix(1, 145);

            //Assert
            Assert.IsFalse(containsPrefix, "Expected non-existent prefix contains to return false");
        }

        [Test]
        [Category("Unit")]
        public void Given_EmptyCurryDictionary_When_ContainsKeyTuple_Then_FalseReturned()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act
            bool containsPrefix = dict.ContainsKeyTuple(1, 145, 27);

            //Assert
            Assert.IsFalse(containsPrefix, "Expected non-existent prefix contains to return false");
        }

        [Test]
        [Category("Unit")]
        public void Given_NewCurryDictionary_When_Add_Then_GetReturnsValue()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            const string InsertValue = "Hello World";
            dict.Add(InsertValue, 1, 2);

            //Assert
            string value = dict[1, 2];
            Assert.AreEqual(InsertValue, value);
        }

        [Test]
        [Category("Unit")]
        public void Given_CurryDictionary_When_Curry_Then_KeysetMatchesExpectation()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Hello World 11", 1, 1, 3);
            dict.Add("Hello World 12", 1, 2, 3);
            dict.Add("Hello World 21", 2, 1, 3);

            //Act

            ICurryDictionary<int, string> curried = dict.GetCurriedDictionary(1);

            //Assert
            IEnumerable<IList<int>> keyset = curried.Keys;
            IEnumerable<IList<int>> expectedKeyset = new List<IList<int>>
                {new List<int>{1,3}, new List<int>{2,3} };
            Assert.AreEqual(expectedKeyset, keyset);
        }

        [Test]
        [Category("Unit")]
        public void Given_EmptyCurryDictionary_When_GetKeys_Then_ResultIsEmptyEnumerable()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act

            IEnumerable<IList<int>> keys = dict.Keys;

            //Assert
            IEnumerable<IList<int>> expectedKeyset = new List<IList<int>>();
            Assert.AreEqual(expectedKeyset, keys);
        }

        [Test]
        [Category("Unit")]
        public void Given_EmptyCurryDictionary_When_GetValues_Then_ResultIsEmptyEnumerable()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act

            IEnumerable<string> values = dict.Values;

            //Assert
            IEnumerable<string> expectedKeyset = new List<string>();
            Assert.AreEqual(expectedKeyset, values);
        }

        [Test]
        [Category("Unit")]
        public void Given_EmptyCurryDictionary_When_GetKeyValuePairs_Then_ResultIsEmptyEnumerable()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act

            IEnumerable<(IList<int>, string)> pairs = dict.KeyValuePairs;

            //Assert
            IEnumerable<(IList<int>, string)> expectedKeyset = new List<(IList<int>, string)>();
            Assert.AreEqual(expectedKeyset, pairs);
        }
    }
}
