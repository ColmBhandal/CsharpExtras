using CsharpExtras.Api;
using CsharpExtras.Extensions.Helper.Dictionary;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.ValidatedType.Numeric.Integer;
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
        public void GIVEN_NullaryDictsWithDifferentValue_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> nullaryDict1 = dict1.GetCurriedDictionary(5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("Different World", 5, 6, 7, 8);
            ICurryDictionary<int, string> nullaryDict2 = dict2.GetCurriedDictionary(5, 6, 7, 8);

            //Act
            IDictionaryComparison result = nullaryDict1.Compare(nullaryDict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_NullaryDictsWithSameValue_WHEN_Compare_THEN_IsEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> nullaryDict1 = dict1.GetCurriedDictionary(5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> nullaryDict2 = dict2.GetCurriedDictionary(5, 6, 7, 8);

            //Act
            IDictionaryComparison result = nullaryDict1.Compare(nullaryDict2, string.Equals);

            //Assert            
            Assert.IsTrue(result.IsEqual, result.Message);
        }

        [Test]
        public void GIVEN_NullaryDict_WHEN_CompareToSelf_THEN_IsEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> nullaryDict = dict.GetCurriedDictionary(5, 6, 7, 8);

            //Act
            IDictionaryComparison result = nullaryDict.Compare(nullaryDict, string.Equals);

            //Assert            
            Assert.IsTrue(result.IsEqual, result.Message);
        }

        [Test]
        public void GIVEN_NullaryDictAndParentSingletonDict_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> nullaryDict = dict.GetCurriedDictionary(5, 6, 7, 8);

            //Act
            IDictionaryComparison result = dict.Compare(nullaryDict, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_EmptyDictAndFilledDict_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("World", 5, 6, 7, 8);
            dict2.Add("Hello Different", 3, 4, 5, 6);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_FilledDictAndEmptyDict_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("Hello", 3, 4, 5, 6);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_DictsWithSameKeysButNotAllValuesTheSame_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("Hello", 3, 4, 5, 6);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("World", 5, 6, 7, 8);
            dict2.Add("Hello Different", 3, 4, 5, 6);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_TwoNonContainedDicts_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("Hello", 3, 4, 5, 6);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("Hello", 3, 4, 5, 6);
            dict1.Add("World", 9, 10, 11, 14);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_DictStrictSupersetOfOther_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("Hello", 3, 4, 5, 6);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("Hello", 3, 4, 5, 6);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_DictStrictSubsetOfOther_WHEN_Compare_THEN_NotEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);
            dict1.Add("Hello", 3, 4, 5, 6);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("World", 5, 6, 7, 8);
            dict2.Add("Hello", 3, 4, 5, 6);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsFalse(result.IsEqual);
        }

        [Test]
        public void GIVEN_DictsWithSameMappingsAddedInDifferentOrder_WHEN_Compare_THEN_IsEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict1 = Api.NewCurryDictionary<int, string>(4);            
            dict1.Add("Hello", 3, 4, 5, 6);
            dict1.Add("World", 5, 6, 7, 8);
            ICurryDictionary<int, string> dict2 = Api.NewCurryDictionary<int, string>(4);
            dict2.Add("World", 5, 6, 7, 8);
            dict2.Add("Hello", 3, 4, 5, 6);

            //Act
            IDictionaryComparison result = dict1.Compare(dict2, string.Equals);

            //Assert            
            Assert.IsTrue(result.IsEqual, result.Message);
        }

        [Test]
        public void GIVEN_Dict_WHEN_CompareToItself_THEN_IsEqual()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);

            //Act
            IDictionaryComparison result = dict.Compare(dict, string.Equals);

            //Assert            
            Assert.IsTrue(result.IsEqual, result.Message);
        }

        [Test]
        public void GIVEN_RemovedDict_WHEN_Remove_THEN_ExParentCountNotAffected()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            ICurryDictionary<int, string> subDict = dict.GetCurriedDictionary(3, 4);
            Assert.AreEqual((NonnegativeInteger)1, dict.Count, "GIVEN: Count of dictionary should start at 1");
            int removeCount = dict.Remove(3, 4);
            Assert.AreEqual((NonnegativeInteger)1, removeCount, "GIVEN: Expected 1 element to be removed from dictionary for test setup");
            Assert.AreEqual((NonnegativeInteger)0, dict.Count, "GIVEN: Expected 1 element to be removed from dictionary for test setup");
            Assert.AreEqual((NonnegativeInteger)1, subDict.Count, "GIVEN: Sub-dictionary count should be 1 before test");

            //Act
            subDict.Remove(5, 6);

            //Assert            
            Assert.AreEqual((NonnegativeInteger)0, dict.Count, "Dictionary count should be unchanged after removing from an element that was removed");
        }


        [Test]
        public void GIVEN_RemovedDict_WHEN_Add_THEN_ExParentCountNotAffected()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            ICurryDictionary<int, string> subDict = dict.GetCurriedDictionary(3, 4);
            Assert.AreEqual((NonnegativeInteger)1, dict.Count, "GIVEN: Count of dictionary should start at 1");
            int removeCount = dict.Remove(3, 4);
            Assert.AreEqual((NonnegativeInteger) 1, removeCount, "GIVEN: Expected 1 element to be removed from dictionary for test setup");
            Assert.AreEqual((NonnegativeInteger)0, dict.Count, "GIVEN: Expected 1 element to be removed from dictionary for test setup");
            Assert.AreEqual((NonnegativeInteger)1, subDict.Count, "GIVEN: Sub-dictionary count should be 1 before test");

            //Act
            subDict.Add("Other Value", 6, 7);

            //Assert            
            Assert.AreEqual((NonnegativeInteger)0, dict.Count, "Dictionary count should be unchanged after adding to an element that was removed");
        }

        [Test]
        public void GIVEN_SingletonDict_WHEN_RemovePrefix_THEN_DictDoesNotContainPrefixOfPrefix()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);
            Assert.IsTrue(dict.ContainsKeyTuplePrefix(3, 4), "GIVEN: Dictionary should contain prefix to begin with");

            //Act
            dict.Remove(3, 4);

            //Assert
            Assert.IsFalse(dict.ContainsKeyTuplePrefix(3), "Dictionary should not contain prefix of prefix after singleton has been removed");
        }

        [Test]
        public void GIVEN_SingletonDict_WHEN_RemovePrefix_THEN_DictDoesNotContainPrefix()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);
            Assert.IsTrue(dict.ContainsKeyTuplePrefix(3, 4), "GIVEN: Dictionary should contain prefix to begin with");

            //Act
            dict.Remove(3, 4);

            //Assert
            Assert.IsFalse(dict.ContainsKeyTuplePrefix(3, 4), "Dictionary should not contain prefix after it's been removed");
        }

        [Test]
        public void GIVEN_SingletonDict_WHEN_RemovePrefix_THEN_DictCountIsZero()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);
            Assert.AreEqual((NonnegativeInteger)1, dict.Count, "GIVEN: Dictionary should have count of 1 to begin with");

            //Act
            dict.Remove(3, 4);

            //Assert
            Assert.AreEqual((NonnegativeInteger)0, dict.Count, "Diciontary count should be zero after remove");
        }

        [Test]
        public void GIVEN_SingletonDict_WHEN_RemovePrefix_THEN_RemoveCountIsOne()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);

            //Act
            int removeCount = dict.Remove(3, 4);

            //Assert            
            Assert.AreEqual(1, removeCount, "No elements should be removed if key prefix is empty");
        }

        [Test]
        public void GIVEN_Dict_WHEN_RemoveEmptyPrefix_THEN_ZeroElementsRemoved()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);

            //Act
            int removeCount = dict.Remove();

            //Assert            
            Assert.AreEqual(0, removeCount, "No elements should be removed if key prefix is empty");
            string element = dict[3, 4, 5, 6];
            Assert.AreEqual(Value, element, "Value in the dictionary should be unchanged by remove");
        }

        [Test, TestCase(3), TestCase(3, 2), TestCase(4, 2, 1, 1)]
        public void GIVEN_NullaryDict_WHEN_RemoveWrongArity_THEN_ZeroElementsRemoved(params int[] prefix)
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);
            ICurryDictionary<int, string> nullaryDict = dict.GetCurriedDictionary(3, 4, 5, 6);

            //Act
            int removeCount = nullaryDict.Remove(prefix);

            //Assert            
            Assert.AreEqual(0, removeCount, "No elements should be removed from a nullary dictionary");
            string element = nullaryDict.GetValueFromTuple();
            Assert.AreEqual(Value, element, "Value in the nullary dictionary should be unchanged by remove");
        }

        [Test]
        public void GIVEN_NullaryDict_WHEN_RemoveEmptyPrefix_THEN_ZeroElementsRemoved()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            const string Value = "Hello";
            dict.Add(Value, 3, 4, 5, 6);
            ICurryDictionary<int, string> nullaryDict = dict.GetCurriedDictionary(3, 4, 5, 6);
            int nullaryArity = nullaryDict.Arity;
            Assert.AreEqual(0, nullaryArity, "GIVEN: Unexpected arity for nullary dict");

            //Act
            int removeCount = nullaryDict.Remove();

            //Assert            
            Assert.AreEqual(0, removeCount, "No elements should be removed from a nullary dictionary");
            string element = nullaryDict.GetValueFromTuple();
            Assert.AreEqual(Value, element, "Value in the nullary dictionary should be unchanged by remove");
        }

        [Test,
            //Key exists cases
            TestCase(3), TestCase(3, 2), TestCase(4, 2, 1, 1),
            //Key doesn't exist cases
            TestCase(5), TestCase(3, 4, 6), TestCase(4, 2, 1, 2)]
        public void GIVEN_Dict_WHEN_RemoveAnyPrefix_THEN_DictDoesNotContainPrefixAfterRemove(params int[] prefix)
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            dict.Add("Goodbye", 3, 2, 1, 0);
            dict.Add("To Remove", 3, 2, 1, 1);
            dict.Add("To Remove", 4, 2, 1, 1);
            int count = dict.Count;
            Assert.AreEqual(4, count, "GIVEN: Curried dictionary count is not as expected to begin with");

            //Act
            dict.Remove(prefix);

            //Assert
            bool isContained = dict.ContainsKeyTuplePrefix(prefix);
            Assert.IsFalse(isContained, "Dictionary should not contain a key-tuple prefix that has been removed");
        }

        [Test, TestCase(9), TestCase(3, 6), TestCase(4, 2, 1, 8)]
        public void GIVEN_Dict_WHEN_RemoveNonExistingPrefix_THEN_RemoveCountIsZero(params int[] prefix)
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            dict.Add("Goodbye", 3, 2, 1, 0);
            dict.Add("To Remove", 3, 2, 1, 1);
            dict.Add("To Remove", 4, 2, 1, 1);

            //Act
            int removeCount = dict.Remove(prefix);

            //Assert
            Assert.AreEqual(0, removeCount);            
        }

        [Test, TestCase(3), TestCase(3, 2), TestCase(4, 2, 1, 1)]
        public void GIVEN_Dict_WHEN_RemoveExistingPrefix_THEN_RemoveCountIsNonZeroAndEqualsCountDifference(params int[] prefix)
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            dict.Add("Goodbye", 3, 2, 1, 0);
            dict.Add("To Remove", 3, 2, 1, 1);
            dict.Add("To Remove", 4, 2, 1, 1);
            int originalCount = dict.Count;
            Assert.AreEqual(4, originalCount, "GIVEN: Curried dictionary count is not as expected to begin with");

            //Act
            int removeCount = dict.Remove(prefix);

            //Assert
            int newCount = dict.Count;
            int delta = originalCount - newCount;
            Assert.AreNotEqual(0, removeCount, "Non-zero amount of mappings should have been removed for a matching key");
            Assert.AreEqual(delta, removeCount, "Count of removed items should match difference between original and new counts");
        }

        [Test,
            //Key exists cases
            TestCase(1, 3), TestCase(2, 3, 2), TestCase(3, 4, 2, 1, 1),
            //Key doesn't exist cases
            TestCase(4, 5), TestCase(4, 3, 4, 6), TestCase(4, 4, 2, 1, 2)]
        public void GIVEN_Dict_WHEN_Remove_THEN_CountIsAsExpected(int expectedCount, params int[] prefix)
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            dict.Add("Goodbye", 3, 2, 1, 0);
            dict.Add("To Remove", 3, 2, 1, 1);
            dict.Add("To Remove", 4, 2, 1, 1);
            int count = dict.Count;
            Assert.AreEqual(4, count, "GIVEN: Curried dictionary count is not as expected to begin with");

            //Act
            dict.Remove(prefix);

            //Assert
            count = dict.Count;
            Assert.AreEqual(expectedCount, count);
        }

        [Test]
        public void GIVEN_CurriedAndDoublyCurried_WHEN_RemoveFromDoublyCurried_THEN_BothCountsUpdate()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(4);
            dict.Add("Hello", 3, 4, 5, 6);
            dict.Add("Goodbye", 3, 2, 1, 0);
            dict.Add("To Remove", 3, 2, 1, 1);
            ICurryDictionary<int, string> curried = dict.GetCurriedDictionary(3);
            ICurryDictionary<int, string> doublyCurried = dict.GetCurriedDictionary(3, 2);
            int curriedCount = curried.Count;
            int doublyCurriedCount = doublyCurried.Count;

            Assert.AreEqual(3, curriedCount, "GIVEN: Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(2, doublyCurriedCount, "GIVEN: Doubly curried dictionary count is not as expected to begin with");

            //Act
            doublyCurried.Remove(1);

            //Assert
            curriedCount = curried.Count;
            doublyCurriedCount = doublyCurried.Count;
            Assert.AreEqual(1, curriedCount);
            Assert.AreEqual(0, doublyCurriedCount);
        }

        [Test]
        public void GIVEN_CurriedDict_WHEN_RemoveFromCurried_THEN_OriginalAndCurriedCountsUpdate()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Hello", 3, 4, 5);
            dict.Add("Goodbye", 3, 2, 1);
            dict.Add("To Remove", 3, 2, 2);
            ICurryDictionary<int, string> curried = dict.GetCurriedDictionary(3);
            int curriedCount = curried.Count;
            int count = dict.Count;

            Assert.AreEqual(3, curriedCount, "GIVEN: Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(3, count, "GIVEN: Curried dictionary count is not as expected to begin with");

            //Act
            curried.Remove(2);

            //Assert
            curriedCount = curried.Count;
            count = dict.Count;
            Assert.AreEqual(1, curriedCount);
            Assert.AreEqual(1, count);
        }

        [Test, 
            //Prefix mismatch before arity runs out
            TestCase(1, 2, 6, 7), TestCase(9, 4, 6, 7, 10, 11),
            //Prefix matches all the way to nulllary dictionary, then some extra arguments
            TestCase(1, 2, 3, 9), TestCase(1, 2, 3, 4, 6, 7, 10, 11)]
        public void GIVEN_CurryDictionary_WHEN_RemoveKeyWithTooLargeArity_Then_RemovedCountEqualsZero(params int[] prefix)
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);

            //Act
            int removedCount = dict.Remove(prefix);

            //Assert
            Assert.AreEqual(0, removedCount);
        }

        [Test, TestCase(1), TestCase(1, 2), TestCase(9, 4, 6, 7)]
        public void GIVEN_CurryDictionary_WHEN_UpdateWithWrongArity_Then_ArgumentException(params int[] keyTuple)
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);

            //Act / Assert
            Assert.Throws<ArgumentException>(() => dict.Update("Blah", keyTuple));
        }

        [Test]
        public void GIVEN_CurryDictionary_WHEN_UpdateNonExisting_Then_NoValueAddedAndFalseReturned()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);
            int dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "GIVEN: Initial count not as expected");
            bool hasKey = dict.ContainsKeyTuple(1, 2, 9);
            Assert.IsFalse(hasKey, "GIVEN: Dictionary should not contain the given key");

            //Act

            bool isSuccess = dict.Update("This update will get lost", 1, 2, 9);

            //Assert
            Assert.IsFalse(isSuccess);
            hasKey = dict.ContainsKeyTuple(1, 2, 9);
            Assert.IsFalse(hasKey, "Dictionary should not contain the given key");
        }

        [Test]
        public void GIVEN_CurryDictionary_WHEN_UpdateNonExisting_Then_CountDoesNotChange()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);
            int dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "GIVEN: Initial count not as expected");
            bool hasKey = dict.ContainsKeyTuple(1, 2, 9);
            Assert.IsFalse(hasKey, "GIVEN: Dictionary should not contain the given key");

            //Act

            dict.Update("This update will get lost", 1, 2, 9);

            //Assert
            dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "Count should not change after update");
        }

        [Test]
        public void GIVEN_CurryDictionary_WHEN_UpdateExsting_Then_ValueUpdatedAndTrueReturned()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);
            int dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "GIVEN: Initial count not as expected");

            //Act

            const string UpdatedValue = "Updated value";
            bool isSuccess = dict.Update(UpdatedValue, 1, 2, 3);

            //Assert
            Assert.IsTrue(isSuccess);
            string currentValue = dict[1, 2, 3];
            Assert.AreEqual(UpdatedValue, currentValue);
        }

        [Test]
        public void GIVEN_CurryDictionary_WHEN_UpdateExsting_Then_CountDoesNotChange()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);
            int dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "GIVEN: Initial count not as expected");

            //Act

            dict.Update("Updated value", 1, 2, 3);

            //Assert
            dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "Count should not change after update");
        }

        [Test]
        public void GIVEN_CurryDictionary_WHEN_AddToSameKeyTwice_Then_CountDoesNotChange()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Blah", 1, 2, 3);
            int dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "GIVEN: Initial count not as expected");

            //Act

            dict.Add("Attempted second add - expect it to fail", 1, 2, 3);

            //Assert
            dictCount = dict.Count;
            Assert.AreEqual(1, dictCount, "Count should not change after trying to add to an existing element");
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

            int doubleCurriedCount = doublyCurried.Count;
            int singleCurriedCount = singlyCurried.Count;
            Assert.AreEqual(2, doubleCurriedCount, "GIVEN: Doubly Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(3, singleCurriedCount, "GIVEN: Singly Curried dictionary count is not as expected to begin with");

            //Act
            doublyCurried.Add("More data", 9);

            //Assert
            Assert.AreEqual(3, (int)doublyCurried.Count);
            Assert.AreEqual(4, (int)singlyCurried.Count);
        }

        [Test]
        public void GIVEN_CurriedDict_WHEN_AddToCurried_THEN_OriginalAndCurriedCountsUpdated()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);
            dict.Add("Hello", 3, 4);
            dict.Add("Goodbye", 3, 5);
            ICurryDictionary<int, string> curried = dict.GetCurriedDictionary(3);
            int curriedCount = curried.Count;
            int count = dict.Count;

            Assert.AreEqual(2, curriedCount, "GIVEN: Curried dictionary count is not as expected to begin with");
            Assert.AreEqual(2, count, "GIVEN: Curried dictionary count is not as expected to begin with");

            //Act
            curried.Add("More data", 2);

            //Assert
            curriedCount = curried.Count;
            count = dict.Count;
            Assert.AreEqual(3, curriedCount);
            Assert.AreEqual(3, count);
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
            int count = dict.Count;
            Assert.AreEqual(2, count);
        }

        [Test]
        public void GIVEN_EmptyDict_WHEN_GetCount_THEN_CountEquals0()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act / Assert
            int count = dict.Count;
            Assert.AreEqual(0, count);
        }
        
        [Test]
        public void GIVEN_NullaryDict_WHEN_GetCount_THEN_CountEquals1()
        {
            //Arrange
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);
            dict.Add("Hello", 3, 4);
            ICurryDictionary<int, string> nullary = dict.GetCurriedDictionary(3, 4);

            //Act / Assert
            int count = nullary.Count;
            Assert.AreEqual(1, count);
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
        public void Given_NewCurryDictionary_When_AddSameObjectToTwoDifferentKeys_Then_ObjectsAtEachKeyAreSame()
        {
            //Assemble
            object obj = new object();
            ICurryDictionary<int, object> dict = Api.NewCurryDictionary<int, object>(2);

            //Act
            dict.Add(obj, 1, 1);
            dict.Add(obj, 1, 2);

            //Assert
            object objKey1 = dict[1, 1];
            var objKey2 = dict[1, 2];
            Assert.AreSame(objKey1, objKey2);
        }

        [Test]
        public void Given_NewCurryDictionary_When_AddSameObjectToTwoDifferentKeys_Then_CountIsTwo()
        {
            //Assemble
            object obj = new object();
            ICurryDictionary<int, object> dict = Api.NewCurryDictionary<int, object>(2);

            //Act
            dict.Add(obj, 1, 1);
            dict.Add(obj, 1, 2);

            //Assert
            Assert.AreEqual((NonnegativeInteger)2, dict.Count);
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
        public void Given_CurryDictionary_When_Curry_Then_KeyTuplesMatchExpectation()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);
            dict.Add("Hello World 11", 1, 1, 3);
            dict.Add("Hello World 12", 1, 2, 3);
            dict.Add("Hello World 21", 2, 1, 3);

            //Act

            ICurryDictionary<int, string> curried = dict.GetCurriedDictionary(1);

            //Assert
            IEnumerable<IList<int>> keyTuples = curried.KeyTuples;
            IEnumerable<IList<int>> expectedKeyTuples = new List<IList<int>>
                {new List<int>{1,3}, new List<int>{2,3} };
            Assert.AreEqual(expectedKeyTuples, keyTuples);
        }

        [Test]
        public void Given_NewCurryDictionary_When_Add_Then_KeyTuplesMatchExpectation()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(2);

            //Act
            dict.Add("Hello World 11", 1, 1);
            dict.Add("Hello World 12", 1, 2);
            dict.Add("Hello World 21", 2, 1);

            //Assert
            IEnumerable<IList<int>> keyTuples = dict.KeyTuples;
            IEnumerable<IList<int>> expectedKeyTuples = new List<IList<int>>
                {new List<int>{1,1}, new List<int>{1,2}, new List<int>{2,1} };
            Assert.AreEqual(expectedKeyTuples, keyTuples);
        }

        [Test]
        public void Given_EmptyCurryDictionary_When_GetKeys_Then_ResultIsEmptyEnumerable()
        {
            //Assemble
            ICurryDictionary<int, string> dict = Api.NewCurryDictionary<int, string>(3);

            //Act

            IEnumerable<IList<int>> keyTuples = dict.KeyTuples;

            //Assert
            IEnumerable<IList<int>> expectedKeyTuples = new List<IList<int>>();
            Assert.AreEqual(expectedKeyTuples, keyTuples);
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
