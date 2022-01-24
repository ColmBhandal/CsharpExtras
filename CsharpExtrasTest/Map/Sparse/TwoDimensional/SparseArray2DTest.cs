using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Map.Sparse;
using CsharpExtras.Map.Sparse.TwoDimensional;
using CsharpExtras.Map.Sparse.TwoDimensional.Builder;
using CsharpExtras.ValidatedType.Numeric.Integer;
using NUnit.Framework;
using System;
using Moq;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Map.Sparse.TwoDimensional
{
    [TestFixture, Category("Unit")]
    public class SparseArray2DTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test, TestCase(1), TestCase(3), TestCase(500)]
        public void GIVEN_BackingArrayWithWrongDimension_WHEN_ConstructSparseArray2D_THEN_ArgumentException
            (int dimension)
        {
            //Arrange
            ISparseArray<string> invalidBackingArray
                = new SparseArrayImpl<string>((PositiveInteger)dimension, Api, (x, i) => true, "");

            //Act / Assert
            Assert.Throws<ArgumentException>(() => new SparseArray2DImpl<string>(Api, invalidBackingArray));
        }

        [Test]
        public void GIVEN_ArrayWithValueLooselyMatchingDefault_WHEN_CompareToEmpty_THEN_NotEqual()
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            //This matches, but is not equal to the default value, so it will be explicitly stored in the array
            const string matchingValue = "default";
            ISparseArray2DBuilder<string> builderPopulated = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithValue(matchingValue, 0, 0);
            ISparseArray2DBuilder<string> builderEmpty = Api.NewSparseArray2DBuilder(DefaultValue);
            ISparseArray2D<string> populatedArray = builderPopulated.Build();
            ISparseArray2D<string> emptyArray = builderEmpty.Build();
            static bool isEqualIgnoreCase(string s1, string s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

            //Act
            IComparisonResult comparison = populatedArray.CompareUsedValues(emptyArray, isEqualIgnoreCase);

            //Assert
            Assert.IsFalse(comparison.IsEqual, "Arrays should not be equal. Only used values should be compared.");
        }

        [Test]
        //Note: if backing implementation changes, then we'll need to explicitly test compare cases here
        //rather than just testing that mocked-out backing implementation is called
        public void GIVEN_Array_WHEN_Compare_THEN_BackingArrayCompareCalledOnce()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            Mock<IComparisonResult> comparison = new Mock<IComparisonResult>();
            mockBackingArray.Setup(a => a.CompareUsedValues(It.IsAny<ISparseArray<string>>(),
                It.IsAny<Func<string, string, bool>>())).Returns(comparison.Object).Verifiable();
            Mock<ISparseArray<string>> mockOtherBackingArray = new Mock<ISparseArray<string>>();

            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(Api, mockBackingArray.Object);
            ISparseArray2D<string> otherArray = new SparseArray2DImpl<string>(Api, mockOtherBackingArray.Object);

            //Act
            IComparisonResult result = mockBackedArray.CompareUsedValues(otherArray, string.Equals);

            //Assert
            mockBackingArray.Verify(a => a.CompareUsedValues(mockOtherBackingArray.Object, string.Equals), Times.Once());
        }

        [Test]
        //Note: if backing implementation changes, then we'll need to explicitly test compare cases here
        //rather than just testing that mocked-out backing implementation is called
        public void GIVEN_Array_WHEN_Set_THEN_BackingArraySetCalledOnceWithCorrectValue()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const string ValueToSet = "Foo";
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            string mockSetValue = "Not Set Yet";
            mockBackingArray.SetupSet(a => a[It.IsAny<int[]>()] = It.IsAny<string>())
                .Callback((int[] coordinates, string v) => { mockSetValue = v;})
                .Verifiable();
            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(Api, mockBackingArray.Object);
            Assert.AreNotEqual(ValueToSet, mockSetValue, "GIVEN: Value to set should not be equal to the mock set value");

            //Act
            mockBackedArray[7, 12] = ValueToSet;

            //Assert
            Assert.AreNotEqual(ValueToSet, mockSetValue, "Value to set should be set after the outer setter is called");
            mockBackingArray.VerifySet(a => a[7, 12]=ValueToSet, Times.Once());
        }

        [Test]
        //Note: if backing implementation changes, then we'll need to explicitly test compare cases here
        //rather than just testing that mocked-out backing implementation is called
        public void GIVEN_Array_WHEN_Get_THEN_BackingArrayGetCalledOnce()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const string MockReturnValue = "Returned Value";
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            mockBackingArray.SetupGet(a => a[It.IsAny<int>(), It.IsAny<int>()])
                .Returns(MockReturnValue)
                .Verifiable();
            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(Api, mockBackingArray.Object);

            //Act
            string actualValue = mockBackedArray[7, 12];

            //Assert
            Assert.AreEqual(MockReturnValue, actualValue);
            mockBackingArray.VerifyGet(a => a[7, 12], Times.Once());
        }

    }
}
