using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ApuntaNotas.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApuntaNotas.Tests.Utilities
{
    [TestClass]
    public class StringToColorExtensionMethodTests
    {
        [TestMethod]
        public void ShouldConvertACorrectStringToColor()
        {
            var stringColor = "#FFFFAAFF";
            Color color = stringColor.ToColor();

            color.ShouldEqual(stringColor.ToColor());
            Assert.AreNotSame(color, Colors.White); // If the string is not a color, just return white.
        }

        [TestMethod]
        public void ANonColorShouldReturnWhite()
        {
            var stringMessage = "Hello, World";
            Color color = stringMessage.ToColor();

            color.ShouldEqual(stringMessage.ToColor());
        }
    }
}
