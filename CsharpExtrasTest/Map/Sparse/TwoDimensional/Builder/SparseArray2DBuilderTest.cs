using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Map.Sparse;
using CsharpExtras.Map.Sparse.TwoDimensional;
using CsharpExtras.Map.Sparse.TwoDimensional.Builder;
using CsharpExtras.ValidatedType.Numeric.Integer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Map.Sparse.TwoDimensional.Builder
{
    [TestFixture, Category("Unit")]
    public class SparseArray2DBuilderTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_EmptyBuilder_WHEN_BuildTwice_THEN_DistinctObjectsCreated()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue);

            //Act
            ISparseArray2D<string> array1 = builder.Build();
            ISparseArray2D<string> array2 = builder.Build();

            //Assert
            Assert.AreNotSame(array1, array2);
        }

        [Test]
        /*The index we're getting is "invalid" only according to a validation function that was later added to the builder
         * What we're testing here is that said validation function does not come into effect for an object that's already been built*/
        public void GIVEN_BuilderAndBuiltObject_WHEN_AddRowValidationToBuilderAfterBuild_THEN_GetInvalidIndexReturnsDefault()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const int UniqueInvalidRowIndex = 7;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue);
            ISparseArray2D<string> array = builder.Build();

            //Act
            builder.WithRowValidation(i => i != UniqueInvalidRowIndex);

            //Assert
            int arbitraryColumnIndex = 20;
            string valueAtCoordinates = array[UniqueInvalidRowIndex, arbitraryColumnIndex];
            Assert.AreEqual(DefaultValue, valueAtCoordinates);
        }

        [Test]
        /*The index we're getting is "invalid" only according to a validation function that was later added to the builder
         * What we're testing here is that said validation function does not come into effect for an object that's already been built*/
        public void GIVEN_BuilderAndBuiltObject_WHEN_AddColumnValidationToBuilderAfterBuild_THEN_GetInvalidIndexReturnsDefault()
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const int UniqueInvalidColumnIndex = 7;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue);
            ISparseArray2D<string> array = builder.Build();

            //Act
            builder.WithColumnValidation(i => i != UniqueInvalidColumnIndex);

            //Assert
            int arbitraryRowIndex = 20;
            string valueAtCoordinates = array[UniqueInvalidColumnIndex, arbitraryRowIndex];
            Assert.AreEqual(DefaultValue, valueAtCoordinates);
        }

        [Test, TestCase(0, 7), TestCase(-2, -4), TestCase(-2, 7)]
        public void GIVEN_BuilderWithValidations_WHEN_Build_THEN_GetInvalidIndexThrowsIndexOutOfRangeException
            (int rowIndex, int colIndex)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const int UniqueInvalidRowIndex = -2;
            const int UniqueInvalidColumnIndex = 7;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithRowValidation(i => i != UniqueInvalidRowIndex)
                .WithColumnValidation(i => i != UniqueInvalidColumnIndex);

            //Act
            ISparseArray2D<string> array = builder.Build();

            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => { string _ = array[rowIndex, colIndex]; });
        }

        [Test, TestCase(0, 8), TestCase(-3, -4), TestCase(1, 6)]
        public void GIVEN_BuilderWithValidations_WHEN_Build_THEN_GetValidIndexReturnsDefault
            (int rowIndex, int colIndex)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            const int UniqueInvalidRowIndex = -2;
            const int UniqueInvalidColumnIndex = 7;
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithRowValidation(i => i != UniqueInvalidRowIndex)
                .WithColumnValidation(i => i != UniqueInvalidColumnIndex);

            //Act
            ISparseArray2D<string> array = builder.Build();

            //Assert
            string actualValue = array[rowIndex, colIndex];
            Assert.AreEqual(DefaultValue, actualValue);
        }

        [Test]
        public void GIVEN_BuilderWithValues_WHEN_Build_THEN_ResultantArrayMatchesExpectedUsedValues()
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            ISparseArray2DBuilder<string> builder = Api.NewSparseArray2DBuilder(DefaultValue)
                .WithValue("0,0", 0, 0)
                .WithValue("1,2", 1, 2)
                .WithValue("25,29", 25, 29);

            ISparseArray2D<string> expected = new SparseArray2DImpl<string>(
                new SparseArrayImpl<string>((PositiveInteger)2, Api, (x, i) => true, DefaultValue), Api);
            expected[0, 0] = "0,0";
            expected[1, 2] = "1,2";
            expected[25, 29] = "25,29";

            //Act
            ISparseArray2D<string> array = builder.Build();

            //Assert
            IComparisonResult comparison = expected.CompareUsedValues(array, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }
    }
}
