using CsharpExtras.Api;
using CsharpExtras.Map.Sparse;
using CsharpExtras.Map.Sparse.TwoDimensional;
using CsharpExtras.ValidatedType.Numeric.Integer;
using NUnit.Framework;
using System;
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
            SparseArrayImpl<string> invalidBackingArray
                = new SparseArrayImpl<string>((PositiveInteger)dimension, Api, (x, i) => true, "");

            //Act / Assert
            Assert.Throws<ArgumentException>(() => new SparseArray2DImpl<string>(Api, invalidBackingArray));
        }
    }
}
