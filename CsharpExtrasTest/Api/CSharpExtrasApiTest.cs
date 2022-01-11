using CsharpExtras.Api;
using CsharpExtras.Enumerable.OneBased;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtrasTest.Api
{
    [TestFixture, Category("Unit")]
    public class CSharpExtrasApiTest
    {
        ICsharpExtrasApi? _csharpExtrasApi;
        ICsharpExtrasApi CsharpExtrasApi => _csharpExtrasApi ??= new CsharpExtrasApi();

        [Test]
        public void GIVEN_IndexPairInitialisationFunction_WHEN_Initialise1DOneBasedArray_THEN_ResultIsAsExpected()
        {
            //Assemble

            Func<int, int, (int, int)> func = (i, j) => (i, j);

            //Act
            IOneBasedArray2D<(int, int)> result = CsharpExtrasApi.NewOneBasedArray2D(2, 2, func);

            //Assert
            Assert.AreEqual(new (int, int)[,] { { (1, 1), (1, 2) }, { (2,1), (2, 2) } }, result.ZeroBasedEquivalent);
        }

        [Test]
        public void GIVEN_Constant_WHEN_Initialise2DOneBasedArray_THEN_ResultantOneBasedArrayIsAsExpected()
        {
            //Assemble

            const int C = 7;

            //Act
            IOneBasedArray2D<int> result = CsharpExtrasApi.NewOneBasedArray2D(2, 2, C);

            //Assert
            Assert.AreEqual(new int[,] { { C, C }, { C, C } }, result.ZeroBasedEquivalent);
        }

        [Test]
        public void GIVEN_IdentityInitialisationFunction_WHEN_Initialise1DOneBasedArray_THEN_ResultIsAsExpected()
        {
            //Assemble

            Func<int, int> func = i => i;

            //Act
            IOneBasedArray<int> result = CsharpExtrasApi.NewOneBasedArray(3, func);

            //Assert
            Assert.AreEqual(new int[] { 1, 2, 3 }, result.ZeroBasedEquivalent);
        }

        [Test]
        public void GIVEN_Constant_WHEN_Initialise1DOneBasedArray_THEN_ResultantOneBasedArrayIsAsExpected()
        {
            //Assemble

            const int C = 7;

            //Act
            IOneBasedArray<int> result = CsharpExtrasApi.NewOneBasedArray(3, C);

            //Assert
            Assert.AreEqual(new int[] { C, C, C }, result.ZeroBasedEquivalent);
        }
    }
}
