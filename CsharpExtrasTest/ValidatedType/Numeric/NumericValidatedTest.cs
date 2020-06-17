using CsharpExtras.ValidatedType.Numeric.Integer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.ValidatedType.Numeric
{
    [TestFixture]
    [Category("Unit")]
    class NumericValidatedTest
    {
        [Test, TestCase(-7423), TestCase(-1), TestCase(0)]
        public void Given_NonPositiveInteger_When_AssignToPositiveInteger_Then_ExceptionThrown(int value)
        {
            PositiveInteger posInt;
            Assert.Catch(() => posInt = (PositiveInteger) value);
        }

        [Test, TestCase(7423), TestCase(1)]
        public void Given_RawPositiveInteger_When_AssignToPositiveInteger_Then_ValueMatchesInput
            (int value)
        {
            //Assemble
            PositiveInteger posInt;
            //Act
            posInt = (PositiveInteger)value;
            //Assert
            int actual = posInt;
            Assert.AreEqual(value, actual);
        }

        [Test, TestCase(-228), TestCase(-1)]
        public void Given_NonNonnegativeInteger_When_AssignToNonnegativeInteger_Then_ExceptionThrown(int value)
        {
            NonnegativeInteger posInt;
            Assert.Catch(() => posInt = (NonnegativeInteger)value);
        }

        [Test, TestCase(241), TestCase(1), TestCase(0)]
        public void Given_RawNonnegativeInteger_When_AssignToNonnegativeInteger_Then_ValueMatchesInput
            (int value)
        {
            //Assemble
            NonnegativeInteger posInt;
            //Act
            posInt = (NonnegativeInteger)value;
            //Assert
            int actual = posInt;
            Assert.AreEqual(value, actual);
        }
    }
}
