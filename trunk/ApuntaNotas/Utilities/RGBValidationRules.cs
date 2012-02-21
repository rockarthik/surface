using System.Windows.Controls;

namespace ApuntaNotas.Utilities
{
    /// <summary>
    /// We use this converter to check the RGB strings
    /// </summary>
    public class RGBValidationRules : ValidationRule
    {
        /// <summary>
        /// When overridden in a derived class, performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ValidationResult"/> object.
        /// </returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var rgb = value.ToString();

            const string valid = "1234567890abcdef";

            if (rgb.Length > 0)
            {
                if (rgb[0] == '#')
                    for (var i = 1; i < rgb.Length; i++)
                    {
                        if ((valid.IndexOf(char.ToLower(rgb[i]))) == -1)
                            return new ValidationResult(false, Resources.Strings.BadRGB);
                    }
                else
                    return new ValidationResult(false, Resources.Strings.BadRGB);
            }

            if (rgb.Length != 4 && rgb.Length != 5 && rgb.Length != 7 && rgb.Length != 9)
                return new ValidationResult(false, Resources.Strings.BadRGB);

            return new ValidationResult(true, null);
        }
    }
}
