using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace ApuntaNotas.Utilities
{
    /// <summary>
    /// This a helper Extension Method that help us transforming a string like #ffffff to a Color instance
    /// </summary>
    public static class StringToColorExtensionMethod
    {
        /// <summary>
        /// The EM itself that does the job
        /// </summary>
        /// <param name="colorString">The color string.</param>
        /// <returns></returns>
        public static Color ToColor(this string colorString)
        {
            colorString = ExtractHexDigits(colorString);

            Color color = Colors.White;

            if (colorString.Length == 6)
            {
                var r = colorString.Substring(0, 2);
                var g = colorString.Substring(2, 2);
                var b = colorString.Substring(4, 2);

                try
                {
                    byte rc = Byte.Parse(r, NumberStyles.HexNumber);
                    byte gc = Byte.Parse(g, NumberStyles.HexNumber);
                    byte bc = Byte.Parse(b, NumberStyles.HexNumber);
                    color = Color.FromRgb(rc, gc, bc);
                }
                catch (Exception)
                {
                    return Colors.White;
                    throw;
                }
            }
            if (colorString.Length == 8)
            {
                var a = colorString.Substring(0, 2);
                var r = colorString.Substring(2, 2);
                var g = colorString.Substring(4, 2);
                var b = colorString.Substring(6, 2);

                try
                {
                    byte ac = Byte.Parse(a, NumberStyles.HexNumber);
                    byte rc = Byte.Parse(r, NumberStyles.HexNumber);
                    byte gc = Byte.Parse(g, NumberStyles.HexNumber);
                    byte bc = Byte.Parse(b, NumberStyles.HexNumber);
                    color = Color.FromArgb(ac, rc, gc, bc);
                }
                catch (Exception)
                {
                    return Colors.White;
                    throw;
                }
            }
            return color;
        }

        /// <summary>
        /// Extracts the hex digits from the string.
        /// </summary>
        /// <param name="colorString">The color string.</param>
        /// <returns></returns>
        private static string ExtractHexDigits(string colorString)
        {
            Regex HexDigits = new Regex(@"[abcdefABCDEF\d]+", RegexOptions.Compiled);

            var hexnum = new StringBuilder();
            foreach (char c in colorString)
                if (HexDigits.IsMatch(c.ToString()))
                    hexnum.Append(c.ToString());

            return hexnum.ToString();
        }
    }
}
