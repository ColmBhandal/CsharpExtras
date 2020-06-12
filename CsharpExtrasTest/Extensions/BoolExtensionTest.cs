using CsharpExtras.Extensions;
using NUnit.Framework;

namespace CsharpExtrasTest.Extensions
{
    [TestFixture]
    public class BoolExtensionTest
    {
        [Test]
        [Category("Unit")]
        public void Given_Yes_String_When_IsYes_Then_True_Returned()
        {
            var result = BoolExtension.IsYes("Yes");
            Assert.IsTrue(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_Random_String_When_IsYes_Then_True_Returned()
        {
            var result = BoolExtension.IsYes("fake");
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_No_String_When_IsYes_Then_False_Returned()
        {
            var result = BoolExtension.IsYes("No");
            Assert.False(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_Yes_String_When_TryParseYesNo_Then_True_Returned_Result_True_Returned()
        {
            var response = BoolExtension.TryParseYesNo("Yes", out var result);
            Assert.IsTrue(result);
            Assert.IsTrue(response);
        }

        [Test]
        [Category("Unit")]
        public void Given_No_String_When_TryParseYesNo_Then_True_Returned_Result_False_Returned()
        {
            var response = BoolExtension.TryParseYesNo("No", out var result);
            Assert.IsTrue(response);
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_Random_String_When_TryParseYesNo_Then_False_Returned_Result_False_Returned()
        {
            var response = BoolExtension.TryParseYesNo("Fake", out var result);
            Assert.IsFalse(response);
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_True_When_ToYesNoString_Then_Yes_String_Returned()
        {
            var response = true.ToYesNoString();
            Assert.AreEqual("Yes", response);
        }

        [Test]
        [Category("Unit")]
        public void Given_False_When_ToYesNoString_Then_No_String_Returned()
        {
            var response = false.ToYesNoString();
            Assert.AreEqual("No", response);
        }
    }
}