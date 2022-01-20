using CsharpExtras.Api;
using CsharpExtras._Enumerable.Provider.Int;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest._Enumerable.Provider.Int
{
    [TestFixture, Category("Unit")]    
    public class SequentialIntProviderTest
    {
        private readonly ICsharpExtrasApi _csharpExtrasApi = new CsharpExtrasApi();

        [Test, TestCase(1), TestCase(7), TestCase(1026)]
        public void GIVEN_ProviderNearMaxRange_WHEN_NextToMax_THEN_MaxReturned
               (int step)
        {
            //Arrange
            ISequentialIntProvider provider = _csharpExtrasApi.NewSequentialIntProvider(int.MaxValue - step, step);

            //Act
            int next = provider.Next();

            //Assert
            Assert.AreEqual(int.MaxValue, next);
        }

        [Test, TestCase(2), TestCase(7), TestCase(1026)]
        public void GIVEN_ProviderNearMaxRange_WHEN_NextBeyondMax_THEN_IndexOutOfRangeException
              (int step)
        {
            //Arrange
            ISequentialIntProvider provider = _csharpExtrasApi.NewSequentialIntProvider(int.MaxValue - step + 1, step);

            //Act / Assert
            Assert.Throws<IndexOutOfRangeException>(() => provider.Next());
        }

        [Test, TestCase(-1), TestCase(-7), TestCase(-1026)]
        public void GIVEN_ProviderNearMinRange_WHEN_NextToMin_THEN_MinReturned
               (int step)
        {
            //Arrange
            ISequentialIntProvider provider = _csharpExtrasApi.NewSequentialIntProvider(int.MinValue - step, step);

            //Act
            int next = provider.Next();

            //Assert
            Assert.AreEqual(int.MinValue, next);
        }

        [Test, TestCase(-2), TestCase(-7), TestCase(-1026)]
        public void GIVEN_ProviderNearMinRange_WHEN_NextBeyondMin_THEN_IndexOutOfRangeException
              (int step)
        {
            //Arrange
            ISequentialIntProvider provider = _csharpExtrasApi.NewSequentialIntProvider(int.MinValue - step - 1, step);

            //Act / Assert
            Assert.Throws<IndexOutOfRangeException>(() => provider.Next());
        }

        [Test, TestCase(0, 0, 0, 1), TestCase(0, 1, 1, 1), TestCase(-1, 1, 0, 1), TestCase(0, -7, -7, 1),
            TestCase(-12, 14, 2, 1), TestCase(-20, 4, 0, 5), TestCase(13, -4, -7, 5)]
        public void GIVEN_ProviderWithStartAndStep_WHEN_NextCalledNTimes_THEN_ExpectedValueReturned
              (int start, int step, int expected, int iterationCount)
        {
            //Arrange
            ISequentialIntProvider provider = _csharpExtrasApi.NewSequentialIntProvider(start, step);
            //Purposely initialise the actual as different from the expected to ensure it actually gets set
            int actual = expected - 1;
            Assert.AreNotEqual(expected, actual, "GIVEN: Expected should not equal actual before Act phase of test");

            //Act
            for (int i = 0; i < iterationCount; i++)
            {
                actual = provider.Next();
            }            

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
