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

        [Test]
        public void GIVEN_Array_WHEN_SetValidAreaWithDefaults_THEN_UsedValuesEqualsEmptyArray()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue);
            ISparseArray2D<string> array = builder.Build();
            string[,] areaToSet = new string[,]{ { DefaultValue, DefaultValue, DefaultValue },
                {DefaultValue, DefaultValue, DefaultValue }, { DefaultValue, DefaultValue, DefaultValue } };

            //Act / Assert
            array.SetArea(areaToSet, -1, -1);

            //Assert
            ISparseArray2D<string> expected = Api.NewSparseArray2DBuilder(DefaultValue).Build();
            IComparisonResult comparison = expected.CompareUsedValues(array, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test]
        public void GIVEN_Array_WHEN_SetValidArea_THEN_UsedValuesAsExpected()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue);
            ISparseArray2D<string> array = builder.Build();
            string[,] areaToSet = new string[,]{ { "TOP LEFT", "TOP MID", "TOP RIGHT" },
                { "MID LEFT", "CENTRE", "MID RIGHT" }, { "BOTTOM LEFT", "BOTTOM MID", "BOTTOM RIGHT" } };

            //Act / Assert
            array.SetArea(areaToSet, -1, -1);

            //Assert
            ISparseArray2D<string> expected = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithValue("TOP LEFT" , -1, -1)
                .WithValue("TOP MID" , -1, 0)
                .WithValue("TOP RIGHT", -1, 1)
                .WithValue("MID LEFT", 0, -1)
                .WithValue("CENTRE", 0, 0)
                .WithValue("MID RIGHT", 0, 1)
                .WithValue("BOTTOM LEFT", 1, -1)
                .WithValue("BOTTOM MID", 1, 0)
                .WithValue("BOTTOM RIGHT", 1, 1)
                .Build();
            IComparisonResult comparison = expected.CompareUsedValues(array, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        [Test,
            TestCase(1, 2), TestCase(1, -3), TestCase(2, 1), TestCase(-5, 1), TestCase(1, 1),
            TestCase(-2, -1), TestCase(-1, -6), TestCase(-8, -1),
            TestCase(0, 0), TestCase(3, 0), TestCase(0, 3)]
        public void GIVEN_Array_WHEN_SetInvalidArea_THEN_IndexOutOfRangeExceptionAndArrayUnchanged(int startRow, int startCol)
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            const int InvalidRow = 1;
            const int InvalidColumn = 1;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithColumnValidation(i => i != InvalidColumn)
                .WithRowValidation(i => i != InvalidRow);
            ISparseArray2D<string> array = builder.Build();
            string[,] areaToSet = new string[,]{ { "TOP LEFT", "TOP MID", "TOP RIGHT" },
                { "MID LEFT", "CENTRE", "MID RIGHT" }, { "BOTTOM LEFT", "BOTTOM MID", "BOTTOM RIGHT" } };

            //Act / Assert
            Assert.Throws<IndexOutOfRangeException>(() => array.SetArea(areaToSet, startRow, startCol));

            //Assert
            ISparseArray2D<string> expected = Api.NewSparseArray2DBuilder(DefaultValue).Build();
            IComparisonResult comparison = expected.CompareUsedValues(array, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }

        public void GIVEN_SparselyFilledArray_WHEN_GetValidArea_THEN_ExpectedArrayReturned()
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithValue("TOP LEFT", -1, -1)
                .WithValue("CENTRE", 0, 0)
                .WithValue("BOTTOM RIGHT", 1, 1);
            ISparseArray2D<string> array = builder.Build();

            //Act
            string[,] area = array.GetArea(-1, -1, 1, 1);

            //Assert
            Assert.AreEqual(new string[,]{ { "TOP LEFT", DefaultValue, DefaultValue },
                { DefaultValue, "CENTRE", DefaultValue }, { DefaultValue, DefaultValue, "BOTTOM RIGHT" } }, area);
        }

        public void GIVEN_EmptyArray_WHEN_GetValidArea_THEN_ArrayOfDefaultsReturned()
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue);
            ISparseArray2D<string> array = builder.Build();

            //Act
            string[,] area = array.GetArea(-1, -1, 1, 1);

            //Assert
            Assert.AreEqual(new string[,]{ { DefaultValue, DefaultValue, DefaultValue },
                { DefaultValue, DefaultValue, DefaultValue }, { DefaultValue, DefaultValue, DefaultValue } }, area);
        }

        [Test, 
            TestCase(1, 2, 4, 6), TestCase(1, -3, 4, 0), TestCase(2, 1, 5, 4), TestCase(-5, 1, -2, 4), TestCase(1, 1, 4, 4),
            TestCase(-2, -1, 1, 2), TestCase(-2, -6, 1, -3), TestCase(-1, -2, 2, 1), TestCase(-8, -2, -5, 1), TestCase(-1, -2, 1, 1),
            TestCase(0, 0, 0, 2), TestCase(0, 0, 2, 0), TestCase(0, 0, 3, 2), TestCase(3, 0, 4, 4), TestCase(0, 3, 6, 7)]
        public void GIVEN_Array_WHEN_GetInvalidArea_THEN_IndexOutOfRangeException(int startRow, int startCol, int endRow, int endCol)
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            const int InvalidRow = 1;
            const int InvalidColumn = 1;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithColumnValidation(i => i != InvalidColumn)
                .WithRowValidation(i => i != InvalidRow);
            ISparseArray2D<string> array = builder.Build();

            //Act / Assert
            Assert.Throws<IndexOutOfRangeException>(() => array.GetArea(startRow, startCol, endRow, endCol));
        }

        [Test,            
            //End row < start row cases
            TestCase(0, 0, -1, 2), TestCase(0, 0, -1, 0), TestCase(0, 0, -1, -9),
            //End col < start col cases
            TestCase(1, 3, 1, 2), TestCase(1, 3, 90, 2)]
        public void GIVEN_Array_WHEN_GetAreaWithInvalidCoordinates_THEN_ArgumentException(int startRow, int startCol, int endRow, int endCol)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const int InvalidRow = 1;
            const int InvalidColumn = 1;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithColumnValidation(i => i != InvalidColumn)
                .WithRowValidation(i => i != InvalidRow);
            ISparseArray2D<string> array = builder.Build();

            //Act / Assert
            Assert.Throws<ArgumentException>(() => array.GetArea(startRow, startCol, endRow, endCol));
        }

        [Test, TestCase(1), TestCase(3), TestCase(500)]
        public void GIVEN_BackingArrayWithWrongDimension_WHEN_ConstructSparseArray2D_THEN_ArgumentException
            (int dimension)
        {
            //Arrange
            ISparseArray<string> invalidBackingArray
                = new SparseArrayImpl<string>((PositiveInteger)dimension, Api, (x, i) => true, "");

            //Act / Assert
            Assert.Throws<ArgumentException>(() => new SparseArray2DImpl<string>(invalidBackingArray));
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
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            Mock<IComparisonResult> comparison = new Mock<IComparisonResult>();
            mockBackingArray.Setup(a => a.CompareUsedValues(It.IsAny<ISparseArray<string>>(),
                It.IsAny<Func<string, string, bool>>())).Returns(comparison.Object).Verifiable();
            mockBackingArray.Setup(a => a.Dimension).Returns((PositiveInteger)2);
            Mock<ISparseArray<string>> mockOtherBackingArray = new Mock<ISparseArray<string>>();
            mockOtherBackingArray.Setup(a => a.Dimension).Returns((PositiveInteger)2);

            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(mockBackingArray.Object);
            ISparseArray2D<string> otherArray = new SparseArray2DImpl<string>(mockOtherBackingArray.Object);

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
            const string ValueToSet = "Foo";
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            string mockSetValue = "Not Set Yet";
            mockBackingArray.SetupSet(a => a[It.IsAny<int[]>()] = It.IsAny<string>())
                .Callback((int[] coordinates, string v) => { mockSetValue = v;})
                .Verifiable();
            mockBackingArray.Setup(a => a.Dimension).Returns((PositiveInteger)2);
            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(mockBackingArray.Object);
            Assert.AreNotEqual(ValueToSet, mockSetValue, "GIVEN: Value to set should not be equal to the mock set value");

            //Act
            mockBackedArray[7, 12] = ValueToSet;

            //Assert
            Assert.AreEqual(ValueToSet, mockSetValue, "Value to set should be set after the outer setter is called");
            mockBackingArray.VerifySet(a => a[7, 12]=ValueToSet, Times.Once());
        }

        [Test]
        //Note: if backing implementation changes, then we'll need to explicitly test compare cases here
        //rather than just testing that mocked-out backing implementation is called
        public void GIVEN_Array_WHEN_Get_THEN_BackingArrayGetCalledOnce()
        {
            //Arrange
            const string MockReturnValue = "Returned Value";
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            mockBackingArray.SetupGet(a => a[It.IsAny<int>(), It.IsAny<int>()])
                .Returns(MockReturnValue)
                .Verifiable();
            mockBackingArray.Setup(a => a.Dimension).Returns((PositiveInteger)2);
            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(mockBackingArray.Object);

            //Act
            string actualValue = mockBackedArray[7, 12];

            //Assert
            Assert.AreEqual(MockReturnValue, actualValue);
            mockBackingArray.VerifyGet(a => a[7, 12], Times.Once());
        }

        [Test]
        //Note: if backing implementation changes, then we'll need to explicitly add test cases here
        //rather than just testing that mocked-out backing implementation is called
        public void GIVEN_Array_WHEN_InsertRows_THEN_BackingArrayShiftGetCalledOnce()
        {
            //Arrange
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            mockBackingArray.Setup(a => a.Shift(It.IsAny<NonnegativeInteger>(), It.IsAny<int>(), It.IsAny<int>()))
                .Verifiable();
            mockBackingArray.Setup(a => a.Dimension).Returns((PositiveInteger)2);
            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(mockBackingArray.Object);

            //Act
            mockBackedArray.InsertRows(2, -4);

            //Assert
            mockBackingArray.Verify(a => a.Shift(It.IsAny<NonnegativeInteger>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            mockBackingArray.Verify(a => a.Shift((NonnegativeInteger)0, 2, -4), Times.Once());
        }

        [Test]
        //Note: if backing implementation changes, then we'll need to explicitly add test cases here
        //rather than just testing that mocked-out backing implementation is called
        public void GIVEN_Array_WHEN_InsertColumns_THEN_BackingArrayShiftGetCalledOnce()
        {
            //Arrange
            Mock<ISparseArray<string>> mockBackingArray = new Mock<ISparseArray<string>>();
            mockBackingArray.Setup(a => a.Shift(It.IsAny<NonnegativeInteger>(), It.IsAny<int>(), It.IsAny<int>()))
                .Verifiable();
            mockBackingArray.Setup(a => a.Dimension).Returns((PositiveInteger)2);
            ISparseArray2D<string> mockBackedArray = new SparseArray2DImpl<string>(mockBackingArray.Object);

            //Act
            mockBackedArray.InsertColumns(-1, 7);

            //Assert
            mockBackingArray.Verify(a => a.Shift(It.IsAny<NonnegativeInteger>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            mockBackingArray.Verify(a => a.Shift((NonnegativeInteger)1, -1, 7), Times.Once());
        }

    }
}
