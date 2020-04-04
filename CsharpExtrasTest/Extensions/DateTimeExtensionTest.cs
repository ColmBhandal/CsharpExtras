using CustomExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExtensions
{
    [TestFixture]
    class DateTimeExtensionTest
    {
        [Test]
        [Category("Quick")]
        [Category("Unit")]
        public void TestGivenKnownDateWhenGetShortDateStampThenExpectedStringReturned()
        {
            DateTime date = DateTime.ParseExact("21/09/2012", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string givenWarningUnformatted = "Given: {0} is not as expected before running test.";
            Assert.AreEqual(9, date.Month, string.Format(givenWarningUnformatted, "month"));
            Assert.AreEqual(21, date.Day, string.Format(givenWarningUnformatted, "day"));
            Assert.AreEqual(2012, date.Year, string.Format(givenWarningUnformatted, "year"));
            Assert.AreEqual("120921", date.GetShortDateStamp());
        }
    }
}
