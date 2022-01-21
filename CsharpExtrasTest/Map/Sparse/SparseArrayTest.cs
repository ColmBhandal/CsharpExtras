using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Map.Sparse;
using CsharpExtras.Map.Sparse.Builder;
using CsharpExtras.ValidatedType.Numeric.Integer;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Map.Sparse
{
    [TestFixture, Category("Unit")]
    public class SparseArrayTest
    {

        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_ArrayWithValueLooselyMatchingDefault_WHEN_CompareToEmpty_THEN_NotEqual()
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            //This matches, but is not equal to the default value, so it will be explicitly stored in the array
            const string matchingValue = "default";
            ISparseArrayBuilder<string> builderPopulated = Api.NewSparseArrayBuilder((PositiveInteger)2, DefaultValue)
                .WithValue(matchingValue, 0, 0);
            ISparseArrayBuilder<string> builderEmpty = Api.NewSparseArrayBuilder((PositiveInteger)2, DefaultValue);
            ISparseArray<string> populated = builderPopulated.Build();
            ISparseArray<string> empty = builderEmpty.Build();

            string actualPopulatedValue = populated[0, 0];
            Assert.AreEqual(matchingValue, actualPopulatedValue, "GIVEN: Expected matching value to be inside the array to begin with");
            Assert.AreEqual((NonnegativeInteger)1, populated.UsedValuesCount,"GIVEN: Expected populated array to have exactly 1 used value");

            static bool isEqualIgnoreCase(string s1, string s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(isEqualIgnoreCase(DefaultValue, matchingValue),
                "GIVEN: The value populating the array should match according to the equality function under test");

            //Act
            IComparisonResult comparison = populated.CompareUsedValues(empty, isEqualIgnoreCase);
            Assert.IsFalse(comparison.IsEqual, "Arrays should not be equal. Only used values should be compared.");
        }

        [Test]
        public void GIVEN_TwoDistinctEmptyArraysWithDifferentDefaults_WHEN_CompareWithFalseComparisonFunction_THEN_Equal()
        {
            //Arrange
            ISparseArrayBuilder<string> builderEmpty1 = Api.NewSparseArrayBuilder((PositiveInteger)2, "default1");
            ISparseArray<string> empty1 = builderEmpty1.Build();
            ISparseArrayBuilder<string> builderEmpty2 = Api.NewSparseArrayBuilder((PositiveInteger)2, "default2");
            ISparseArray<string> empty2 = builderEmpty2.Build();

            //Act
            IComparisonResult comparison = empty1.CompareUsedValues(empty2, (s1, s2) => false);

            //Assert
            //Empty arrays should always be equal across used values, even if the comparison function is false & defaults differ
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_FilledArray_WHEN_CompareToItselfWithFalseFunction_THEN_NotEqual()
        {
            //Arrange                
            ISparseArrayBuilder<string> builderPopulated = Api.NewSparseArrayBuilder((PositiveInteger)2, "DEFAULT")
                .WithValue("blah", 0, 0);
            ISparseArray<string> populated = builderPopulated.Build();

            //Act
            IComparisonResult comparison = populated.CompareUsedValues(populated, (s1, s2) => false);

            //Assert
            Assert.IsFalse(comparison.IsEqual, "Populated array will not equal itself if the comparison function is false");
        }

        [Test]
        public void GIVEN_DistinctEquivalentFilledArrays_WHEN_Compare_THEN_Equal()
        {
            //Arrange                
            const string Value = "VALUE0,0";
            const string Value11 = "VALUE1,1";
            ISparseArrayBuilder<string> builderUpper = Api.NewSparseArrayBuilder((PositiveInteger)2, "DEFAULT")
                .WithValue(Value, 0, 0)
                .WithValue(Value11, 1, 1);
            ISparseArrayBuilder<string> builderLower = Api.NewSparseArrayBuilder((PositiveInteger)2, "DEFAULT")
                .WithValue(Value.ToLower(), 0, 0)
                .WithValue(Value11.ToLower(), 1, 1);

            ISparseArray<string> upper = builderUpper.Build();
            ISparseArray<string> lower = builderLower.Build();

            static bool isEqualIgnoreCase(string s1, string s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

            //Act
            IComparisonResult comparison = upper.CompareUsedValues(lower, isEqualIgnoreCase);

            //Assert
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_EmptyArray_WHEN_SetThenGet_THEN_RetrievedValueIsEqualToValuePutIn()
        {
            //Arrange                
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)2, "DEFAULT");
            ISparseArray<string> array = builder.Build();

            string valueToWrite = "Hello Sparse World";

            //Act
            array[3, 4] = valueToWrite;
            string valueRead = array[3, 4];

            //Assert
            Assert.AreEqual(valueToWrite, valueRead);
        }

        [Test]
        public void GIVEN_EmptyArray_WHEN_SetValueToDefault_THEN_CountDoesNotChange()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)2, DefaultValue);
            ISparseArray<string> array = builder.Build();

            Assert.AreEqual((NonnegativeInteger)0, array.UsedValuesCount, "GIVEN: Count should be zero to begin with");

            //Act
            array[3, 4] = DefaultValue;

            //Assert
            Assert.AreEqual((NonnegativeInteger)0, array.UsedValuesCount);
        }

        [Test]
        public void GIVEN_EmptyArray_WHEN_SetValueToDefault_THEN_CompareToEmptyIsEqual()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)2, DefaultValue);
            ISparseArray<string> array = builder.Build();
            ISparseArray<string> emptyArray = builder.Build();

            Assert.AreEqual((NonnegativeInteger)0, array.UsedValuesCount, "GIVEN: Count should be zero to begin with");

            //Act
            array[3, 4] = DefaultValue;

            //Assert
            IComparisonResult comparison = emptyArray.CompareUsedValues(array, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_ArrayWithImpureValidation_WHEN_MutateValidationFunctionAfterWriteNonDefault_THEN_NoExceptionThrown()
        {
            //Arrange
            int uniqueInvalidIndex = 7;
            IntWrapper wrappedUniqueInvalidIndex = new IntWrapper();
            //Initially, the validation function validates against a different index to the unique value index
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex+1;
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(i => i != wrappedUniqueInvalidIndex.Val, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();

            const string WrittenValue = "Something";

            //Act
            array[uniqueInvalidIndex] = WrittenValue;
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex;

            //Assert
            string value = array[uniqueInvalidIndex];
            Assert.Pass("No exception was thrown retrieving the value from the array - test passed");
        }

        [Test]
        public void GIVEN_ArrayWithImpureValidation_WHEN_MutateValidationFunctionAfterWriteDefault_THEN_NoExceptionThrown()
        {
            //Arrange
            int uniqueInvalidIndex = 7;
            IntWrapper wrappedUniqueInvalidIndex = new IntWrapper();
            //Initially, the validation function validates against a different index to the unique value index
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex + 1;
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(i => i != wrappedUniqueInvalidIndex.GetVal(), (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();

            //Act
            array[uniqueInvalidIndex] = DefaultValue;
            //At this point, the function is now mutated and the index is invalid. But since we've already accessed it we expect it to be cached as valid.
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex;

            //Assert
            string value = array[uniqueInvalidIndex];            
            Assert.Pass("No exception was thrown retrieving the value from the array - test passed");
        }

        [Test]
        public void GIVEN_ArrayWithImpureValidation_WHEN_MutateValidationFunctionBeforeWriteNonDefault_THEN_ArgumentException()
        {
            //Arrange
            int uniqueInvalidIndex = 7;
            IntWrapper wrappedUniqueInvalidIndex = new IntWrapper();
            //Initially, the validation function validates against a different index to the unique value index
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex + 1;
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(i => i != wrappedUniqueInvalidIndex.Val, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();

            const string WrittenValue = "Something";

            //Act
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex;

            //Assert
            Assert.Throws<ArgumentException>(() => array[uniqueInvalidIndex] = WrittenValue);
        }

        [Test]
        public void GIVEN_ArrayWithImpureValidation_WHEN_MutateValidationFunctionBeforeWriteDefault_THEN_ArgumentException()
        {
            //Arrange
            int uniqueInvalidIndex = 7;
            IntWrapper wrappedUniqueInvalidIndex = new IntWrapper();
            //Initially, the validation function validates against a different index to the unique value index
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex + 1;
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(i => i != wrappedUniqueInvalidIndex.Val, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();

            //Act
            wrappedUniqueInvalidIndex.Val = uniqueInvalidIndex;

            //Assert
            Assert.Throws<ArgumentException>(() => array[uniqueInvalidIndex] = DefaultValue);
        }

        [Test, TestCaseSource(nameof(TwoIndexOperationsCasesDimensionOne))]
        public void GIVEN_ValidIndex_WHEN_PerformTwoOperationsAtThatIndex_THEN_ValidationCalledExactlyOnceForGivenIndex
            ((string testCaseDescription, Action<ISparseArray<string>, int> beforeAction,
            Action<ISparseArray<string>, int> afterAction, bool shouldThrowBefore, bool shouldThrowAfter) testCase)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            Mock<Func<int, bool>> mockValidationFunc = new Mock<Func<int, bool>>();
            const int UniqueInvalidIndex = 77;
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i != UniqueInvalidIndex))).Returns(true);
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i == UniqueInvalidIndex))).Returns(false);
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(mockValidationFunc.Object, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();
            const int ValidIndex = 1;

            //Act
            testCase.beforeAction(array, ValidIndex);
            testCase.afterAction(array, ValidIndex);

            //Assert
            mockValidationFunc.Verify(f => f.Invoke(ValidIndex), Times.Once());
            mockValidationFunc.Verify(f => f.Invoke(It.IsAny<int>()), Times.Once());
        }

        [Test, TestCaseSource(nameof(TwoIndexOperationsCasesDimensionOne))]
        public void GIVEN_InvalidIndex_WHEN_PerformTwoOperationsAtThatIndex_THEN_ArgumentExceptionsAndValidationCalledOnce
            ((string testCaseDescription, Action<ISparseArray<string>, int> beforeAction,
            Action<ISparseArray<string>, int> afterAction, bool shouldThrowBefore, bool shouldThrowAfter) testCase)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            Mock<Func<int, bool>> mockValidationFunc = new Mock<Func<int, bool>>();
            const int UniqueInvalidIndex = 77;
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i != UniqueInvalidIndex))).Returns(true);
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i == UniqueInvalidIndex))).Returns(false);
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(mockValidationFunc.Object, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();

            //Act
            if (testCase.shouldThrowBefore)
            {
                Assert.Throws<ArgumentException>(() => testCase.beforeAction(array, UniqueInvalidIndex));
            }
            else
            {
                testCase.beforeAction(array, UniqueInvalidIndex);
            }
            if (testCase.shouldThrowAfter)
            {

                Assert.Throws<ArgumentException>(() => testCase.afterAction(array, UniqueInvalidIndex));
            }
            else
            {
                testCase.afterAction(array, UniqueInvalidIndex);
            }

            //Assert
            mockValidationFunc.Verify(f => f.Invoke(UniqueInvalidIndex), Times.Once());
            mockValidationFunc.Verify(f => f.Invoke(It.IsAny<int>()), Times.Once());
        }
        
        private static IEnumerable<(string, Action<ISparseArray<string>, int> beforeAction, Action<ISparseArray<string>, int> afterAction,
            bool shouldThrowBefore, bool shouldThrowAfter)>
            TwoIndexOperationsCasesDimensionOne
        {
            get
            {
                static void Set(ISparseArray<string> array, int i) => array[i] = "Hello";
                static void Get(ISparseArray<string> array, int i) { string s = array[i]; }
                static void IsValid(ISparseArray<string> array, int i) => array.IsValid(i, (NonnegativeInteger)0);
                yield return ("Set then Get", Set, Get, true, true);
                yield return ("Get then Set", Get, Set, true, true);
                yield return ("IsValid then Get", IsValid, Get, false, true);
                yield return ("Get then IsValid", Get, IsValid, true, false);
                yield return ("IsValid then Set", IsValid, Set, false, true);
                yield return ("Set then IsValid", Set, IsValid, true, false);
            }
        }

        [Test]
        public void GIVEN_ValidIndex_WHEN_IsValid_THEN_True()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            Mock<Func<int, bool>> mockValidationFunc = new Mock<Func<int, bool>>();
            const int UniqueInvalidIndex = 77;
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i != UniqueInvalidIndex))).Returns(true);
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i == UniqueInvalidIndex))).Returns(false);
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(mockValidationFunc.Object, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();
            const int ValidIndex = 1;

            //Act
            bool isValid = array.IsValid(ValidIndex, (NonnegativeInteger)0);

            //Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void GIVEN_InValidIndex_WHEN_IsValid_THEN_False()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            Mock<Func<int, bool>> mockValidationFunc = new Mock<Func<int, bool>>();
            const int UniqueInvalidIndex = 77;
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i != UniqueInvalidIndex))).Returns(true);
            mockValidationFunc.Setup(f => f.Invoke(It.Is<int>(i => i == UniqueInvalidIndex))).Returns(false);
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)1, DefaultValue)
                .WithValidationFunction(mockValidationFunc.Object, (NonnegativeInteger)0);
            ISparseArray<string> array = builder.Build();

            //Act
            bool isValid = array.IsValid(UniqueInvalidIndex, (NonnegativeInteger)0);

            //Assert
            Assert.IsFalse(isValid);
        }

        private class IntWrapper { public int Val { get; set; } public int GetVal() => Val; };
    }
}
