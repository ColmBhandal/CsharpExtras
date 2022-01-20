using CsharpExtras.Api;
using CsharpExtras.Map.Sparse;
using CsharpExtras.Map.Sparse.Builder;
using CsharpExtras.ValidatedType.Numeric.Integer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CsharpExtras.Compare;

namespace CsharpExtrasTest.Map.Sparse.Builder
{
    [TestFixture, Category("Unit")]
    public class SparseArrayBuilderTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();
        
        [Test, TestCase(1, 0, 3), TestCase(2, 0, 3), TestCase(3, 2, -79)]
        public void GIVEN_BuilderWithValidations_WHEN_Build_THEN_GetInvalidIndexThrowsArgumentException
            (int dimension, int axisIndex, int uniqueInvalidIndex)
        {
            //Arrange

            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger) dimension, "DEFAULT")
                .WithValidationFunction(i => i != uniqueInvalidIndex, (NonnegativeInteger) axisIndex);

            //Act
            ISparseArray<string> array = builder.Build();

            //Assert
            int[] coordinates = new int[dimension];
            coordinates[axisIndex] = uniqueInvalidIndex;
            Assert.Throws<ArgumentException>(() => { string _ = array[coordinates]; });
        }

        [Test, TestCase(1, 0, 3), TestCase(2, 0, 3), TestCase(3, 2, -79)]
        public void GIVEN_BuilderWithValidations_WHEN_Build_THEN_GetValidCoordinatesReturnsDefault
            (int dimension, int axisIndex, int uniqueInvalidIndex)
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)dimension, DefaultValue)
                .WithValidationFunction(i => i != uniqueInvalidIndex, (NonnegativeInteger)axisIndex);

            //Act
            ISparseArray<string> array = builder.Build();

            //Assert
            int[] validCoordinates = Enumerable.Range(uniqueInvalidIndex+1, dimension).ToArray();
            string actualValue = array[validCoordinates];
            Assert.AreEqual(DefaultValue, actualValue);
        }

        [Test, TestCase(1, 1), TestCase(5, 6), TestCase(2, 75)]
        public void GIVEN_Builder_WHEN_AddValidationFunctionForInvalidAxis_THEN_ArgumentException
            (int dimension, int axisIndex)
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)dimension, DefaultValue);

            //Act / Assert
            Assert.Throws<ArgumentException>(() => builder.WithValidationFunction(i => true, (NonnegativeInteger)axisIndex));
        }

        [Test]
        public void GIVEN_BuilderWithValues_WHEN_Build_THEN_ResultantArrayMatchesExpectedUsedValues()            
        {
            //Arrange

            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)3, DefaultValue)
                .WithValue("0,0,0", 0, 0, 0)
                .WithValue("1,2,3", 1, 2, 3);

            SparseArrayImpl<string> expected = new SparseArrayImpl<string>((PositiveInteger)3, Api, (x, y) => true, DefaultValue);
            expected[0, 0, 0] = "0,0,0";
            expected[1, 2, 3] = "1,2,3";

            //Act
            ISparseArray<string> array = builder.Build();

            //Assert
            IComparisonResult comparison = expected.CompareUsedValues(array, string.Equals);
            Assert.IsTrue(comparison.IsEqual, comparison.Message);
        }
    }
}
