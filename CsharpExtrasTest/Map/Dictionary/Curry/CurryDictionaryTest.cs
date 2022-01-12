using CsharpExtras.Api;
using CsharpExtras.Map.Dictionary.Curry;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Map.Dictionary.Curry
{
    [TestFixture, Category("Unit")]
    public class CurryDictionaryTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();
        
        [Test]
        public void GIVEN_CurryDictionary_WHEN_AddToSameKeyTwice_Then_CountDoesNotChange()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);
            Assert.AreEqual(1, dict.Count, "GIVEN: Initial count not as expected");

            //Act

            dict.Add("Attempted second add - expect it to fail", 1, 2, 3);

            //Assert
            Assert.AreEqual(1, dict.Count, "Count should not change after trying to add to an existing element");
        }

        [Test]
        public void GIVEN_DoublyCurried_WHEN_AddToDoublyCurried_THEN_SinglyCurriedCountIsUpdated()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Hello", 1, 2, 3);
            dict.Add("Goodbye", 1, 2, 4);
            dict.Add("Goodbye", 1, 5, 6);
            ICurryDictionary<int, string> doublyCurried = dict.GetCurriedDictionary(1, 2);
            ICurryDictionary<int, string> singlyCurried = dict.GetCurriedDictionary(1);

            Assert.AreEqual(2, doublyCurried.Count, "GIVEN: Doubly Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(3, singlyCurried.Count, "GIVEN: Singly Curried dictionary count is not as expected to begin with");

            //Act
            doublyCurried.Add("More data", 9);

            //Assert
            Assert.AreEqual(3, doublyCurried.Count);
            Assert.AreEqual(4, singlyCurried.Count);
        }

        [Test]
        public void GIVEN_CurriedDict_WHEN_AddToCurried_THEN_OriginalAndCurriedCountsUpdate()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);
            dict.Add("Hello", 3, 4);
            dict.Add("Goodbye", 3, 5);
            ICurryDictionary<int, string> curried = dict.GetCurriedDictionary(3);

            Assert.AreEqual(2, curried.Count, "GIVEN: Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(2, dict.Count, "GIVEN: Curried dictionary count is not as expected to begin with");

            //Act
            curried.Add("More data", 2, 2);

            //Assert
            Assert.AreEqual(3, curried.Count, "GIVEN: Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(3, dict.Count, "GIVEN: Curried dictionary count is not as expected to begin with");
        }

        [Test]
        public void GIVEN_EmptyDict_WHEN_AddMultipleElements_THEN_CountIsAsExpected()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            dict.Add("Hello", 3, 4);
            dict.Add("Goodbye", 3, 5);

            //Assert
            Assert.AreEqual(2, dict.Count);
        }

        [Test]
        public void GIVEN_EmptyDict_WHEN_GetCount_THEN_CountEquals0()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act / Assert
            Assert.AreEqual(0, dict.Count);
        }
        
        [Test]
        public void GIVEN_NullaryDict_WHEN_GetCount_THEN_CountEquals1()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);
            dict.Add("Hello", 3, 4);
            ICurryDictionary<int, string> nullary = dict.GetCurriedDictionary(3, 4);

            //Act / Assert
            Assert.AreEqual(1, nullary.Count);
        }

        /// <summary>
        /// In loving memory of Mikhail Volkov, a brilliant coder who first introduced me to C# and delegates.
        /// </summary>
        delegate string MikhailsDelegate(string input);

        [Test]
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
        public void Given_PositiveInteger_When_ConstructCurryDictionary_Then_ArityMatchesCtorParameter()
        {
            //Assemble / Act
            const int ExpectedArity = 23;
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(ExpectedArity);

            //Assert
            Assert.AreEqual(ExpectedArity, (int)dict.Arity);
        }

        [Test]
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
        public void GIVEN_CurryDictionary_WHEN_AddToSameKeyTwice_Then_FirstAddSucceedsAndSecondFails()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act
            const string OriginalValue = "Orignal value";
            bool isInitialSuccessful = dict.Add(OriginalValue, 1, 2, 3);
            bool isSecondSuccessful = dict.Add("Hello World again", 1, 2, 3);

            //Assert
            Assert.IsTrue(isInitialSuccessful, "Expected initial add to succeed");
            Assert.IsFalse(isSecondSuccessful, "Expected add to existing key to return false");
            string value = dict[1, 2, 3];
            Assert.AreEqual(OriginalValue, value);
        }

        [Test]
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
