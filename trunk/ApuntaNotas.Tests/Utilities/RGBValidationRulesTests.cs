using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ApuntaNotas.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApuntaNotas.Tests.Utilities
{
    [TestClass]
    public class RGBValidationRulesTests
    {
        [TestMethod]
        public void ShouldValidateCorrectColors()
        {
            var val = new RGBValidationRules();

            var valid1 = val.Validate("#FFAAFFAA", CultureInfo.CurrentCulture );
            var valid2 = val.Validate("#FFAAFF", CultureInfo.CurrentCulture);

            Assert.IsTrue(valid1.IsValid);
            Assert.IsTrue(valid2.IsValid);
        }

        [TestMethod]
        public void ShouldFailWithInvalidColorsOrStrings()
        {
            var val = new RGBValidationRules();

            var aString = val.Validate("Hello", CultureInfo.CurrentCulture);
            var invalidColor = val.Validate("#FFAAFFA", CultureInfo.CurrentCulture);

            Assert.IsFalse(aString.IsValid);
            Assert.IsFalse(invalidColor.IsValid);
        }
    }
}
