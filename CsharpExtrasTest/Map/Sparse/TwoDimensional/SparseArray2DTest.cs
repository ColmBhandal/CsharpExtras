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

    }
}
