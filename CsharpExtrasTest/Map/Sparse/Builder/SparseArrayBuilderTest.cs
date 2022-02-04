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
using Moq;

namespace CsharpExtrasTest.Map.Sparse.Builder
{
    [TestFixture, Category("Unit")]
    public class SparseArrayBuilderTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_Builder_WHEN_BuildTwice_THEN_DistinctObjectsCreated()
        {
            //Arrange

            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)7, "DEFAULT");

            //Act
            ISparseArray<string> array1 = builder.Build();
            ISparseArray<string> array2 = builder.Build();

            //Assert
            Assert.AreNotSame(array1, array2);
        }

        [Test, TestCase(1, 0, 3), TestCase(2, 0, 3), TestCase(3, 2, -79)]
        public void GIVEN_BuilderAndBuiltObject_WHEN_AddValidationToBuilderAfterBuild_THEN_GetInvalidIndexReturnsDefault
            (int dimension, int axisIndex, int uniqueInvalidIndex)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)dimension, DefaultValue);
            ISparseArray<string> array = builder.Build();
            Func<NonnegativeInteger, int, bool> validationFunction = (k, i) => i != uniqueInvalidIndex && k == axisIndex;
            Assert.IsFalse(validationFunction((NonnegativeInteger) axisIndex, uniqueInvalidIndex),
                "GIVEN: Validaiton function should return false on invalid index");

            //Act
            builder.WithValidationFunction(validationFunction);

            //Assert
            int[] coordinates = new int[dimension];
            coordinates[axisIndex] = uniqueInvalidIndex;
            string valueAtCoordinates = array[coordinates];
            Assert.AreEqual(DefaultValue, valueAtCoordinates);
        }

        [Test, TestCase(1, 0, 3), TestCase(2, 0, 3), TestCase(3, 2, -79)]
        /*The index we're getting is "invalid" only according to a validation function that was later added to the builder
         * What we're testing here is that said validation function does not come into effect for an object that's already been built*/
        public void GIVEN_BuilderAndBuiltObject_WHEN_AddAxisValidationsToBuilderAfterBuild_THEN_GetInvalidIndexReturnsDefault
            (int dimension, int axisIndex, int uniqueInvalidIndex)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)dimension, DefaultValue);
            ISparseArray<string> array = builder.Build();
            Func<int, bool> axisValidationFunction = i => i != uniqueInvalidIndex;
            Assert.IsFalse(axisValidationFunction(uniqueInvalidIndex),
                "GIVEN: Validaiton function should return false on invalid index");

            //Act
            builder.WithAxisValidationFunction(axisValidationFunction, (NonnegativeInteger)axisIndex);

            //Assert
            int[] coordinates = new int[dimension];
            coordinates[axisIndex] = uniqueInvalidIndex;
            string valueAtCoordinates = array[coordinates];
            Assert.AreEqual(DefaultValue, valueAtCoordinates);
        }

        [Test, TestCase(1, 0, 3), TestCase(2, 0, 3), TestCase(3, 2, -79)]
        public void GIVEN_BuilderWithValidations_WHEN_Build_THEN_GetInvalidIndexThrowsIndexOutOfRangeException
            (int dimension, int axisIndex, int uniqueInvalidIndex)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger) dimension, DefaultValue)
                .WithAxisValidationFunction(i => i != uniqueInvalidIndex, (NonnegativeInteger) axisIndex);

            //Act
            ISparseArray<string> array = builder.Build();

            //Assert
            int[] coordinates = new int[dimension];
            coordinates[axisIndex] = uniqueInvalidIndex;
            Assert.Throws<IndexOutOfRangeException>(() => { string _ = array[coordinates]; });
        }

        [Test, TestCase(1, 0, 3), TestCase(2, 0, 3), TestCase(3, 2, -79)]
        public void GIVEN_BuilderWithValidations_WHEN_Build_THEN_GetValidCoordinatesReturnsDefault
            (int dimension, int axisIndex, int uniqueInvalidIndex)
        {
            //Arrange
            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)dimension, DefaultValue)
                .WithAxisValidationFunction(i => i != uniqueInvalidIndex, (NonnegativeInteger)axisIndex);

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
            Assert.Throws<ArgumentException>(() => builder.WithAxisValidationFunction(i => true, (NonnegativeInteger)axisIndex));
        }

        [Test, TestCaseSource(nameof(ProviderForIndexinActionValidationTest))]
        public void GIVEN_AxisValidationAndValidation_WHEN_Access_THEN_AxisValidationNotCalled
            (Action<int, int, ISparseArray<string>> indexingAction)
        {
            //Arrange
            Mock<Func<NonnegativeInteger, int, bool>> mockValidator = new Mock<Func<NonnegativeInteger, int, bool>>();
            mockValidator.Setup(v => v.Invoke(It.IsAny<NonnegativeInteger>(), It.IsAny<int>())).Returns(true);

            Mock<Func<int, bool>> mockAxisValidator = new Mock<Func<int, bool>>();
            mockAxisValidator.Setup(v => v.Invoke(It.IsAny<int>())).Returns(true)
                .Verifiable();

            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)2, DefaultValue)
                .WithValidationFunction(mockValidator.Object)
                .WithAxisValidationFunction(mockAxisValidator.Object, (NonnegativeInteger)0)
            .WithAxisValidationFunction(mockAxisValidator.Object, (NonnegativeInteger)1);
            ISparseArray<string> zippedArray = builder.Build();

            //Act
            indexingAction(1, -20, zippedArray);

            //Assert
            mockAxisValidator.Verify(v => v.Invoke(It.IsAny<int>()), Times.Never());
        }

        [Test, TestCaseSource(nameof(ProviderForIndexinActionValidationTest))]
        public void GIVEN_AxisValidationAndValidation_WHEN_Access_THEN_ValidationCalledOncePerAxisIndex
            (Action<int, int, ISparseArray<string>> indexingAction)
        {
            //Arrange
            Mock<Func<NonnegativeInteger, int, bool>> mockValidator = new Mock<Func<NonnegativeInteger, int, bool>>();
            mockValidator.Setup(v => v.Invoke(It.IsAny<NonnegativeInteger>(), It.IsAny<int>())).Returns(true)
                .Verifiable();

            Mock<Func<int, bool>> mockAxisValidator = new Mock<Func<int, bool>>();
            mockAxisValidator.Setup(v => v.Invoke(It.IsAny<int>())).Returns(true);

            const string DefaultValue = "DEFAULT";
            ISparseArrayBuilder<string> builder = Api.NewSparseArrayBuilder((PositiveInteger)2, DefaultValue)
                .WithValidationFunction(mockValidator.Object)
                .WithAxisValidationFunction(mockAxisValidator.Object, (NonnegativeInteger) 0)
            .WithAxisValidationFunction(mockAxisValidator.Object, (NonnegativeInteger)1);
            ISparseArray<string> zippedArray = builder.Build();

            //Act
            indexingAction(1, -20, zippedArray);

            //Assert
            mockValidator.Verify(v => v.Invoke((NonnegativeInteger)0, 1), Times.Once());
            mockValidator.Verify(v => v.Invoke((NonnegativeInteger)1, -20), Times.Once());
            mockValidator.Verify(v => v.Invoke(It.IsAny<NonnegativeInteger>(), It.IsAny<int>()), Times.Exactly(2));
        }

        private static IEnumerable<Action<int, int, ISparseArray<string>>>
            ProviderForIndexinActionValidationTest()
        {
            return new List<Action<int, int, ISparseArray<string>>>()
            {
                (i, j, a) => {a[i, j] = "Hello";},
                (i, j, a) => { var _ = a[i, j];},
                (i, j, a) => { a.IsValid(i, (NonnegativeInteger)0); a.IsValid(j, (NonnegativeInteger)1);},
                (i, j, a) => a.IsUsed(i, j)
            };
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
