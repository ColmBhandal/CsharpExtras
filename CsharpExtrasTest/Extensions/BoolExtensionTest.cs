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
            bool result = BoolExtension.IsYes("Yes");
            Assert.IsTrue(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_Random_String_When_IsYes_Then_True_Returned()
        {
            bool result = BoolExtension.IsYes("fake");
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_No_String_When_IsYes_Then_False_Returned()
        {
            bool result = BoolExtension.IsYes("No");
            Assert.False(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_Yes_String_When_TryParseYesNo_Then_True_Returned_Result_True_Returned()
        {
            bool response = BoolExtension.TryParseYesNo("Yes", out bool result);
            Assert.IsTrue(result);
            Assert.IsTrue(response);
        }

        [Test]
        [Category("Unit")]
        public void Given_No_String_When_TryParseYesNo_Then_True_Returned_Result_False_Returned()
        {
            bool response = BoolExtension.TryParseYesNo("No", out bool result);
            Assert.IsTrue(response);
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_Random_String_When_TryParseYesNo_Then_False_Returned_Result_False_Returned()
        {
            bool response = BoolExtension.TryParseYesNo("Fake", out bool result);
            Assert.IsFalse(response);
            Assert.IsFalse(result);
        }

        [Test]
        [Category("Unit")]
        public void Given_True_When_ToYesNoString_Then_Yes_String_Returned()
        {
            string response = true.ToYesNoString();
            Assert.AreEqual("Yes", response);
        }

        [Test]
        [Category("Unit")]
        public void Given_False_When_ToYesNoString_Then_No_String_Returned()
        {
            string response = false.ToYesNoString();
            Assert.AreEqual("No", response);
        }
    }
}