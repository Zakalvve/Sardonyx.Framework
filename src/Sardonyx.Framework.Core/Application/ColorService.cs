namespace Sardonyx.Framework.Core.Application
{
    public class ColorService : IColorService
    {
        private const double RED_LUMINANCE_WEIGHT = 0.2126;
        private const double GREEN_LUMINANCE_WEIGHT = 0.7152;
        private const double BLUE_LUMINANCE_WEIGHT = 0.0722;

        private const double RGB_MAX = 255.0;
        private const double LINEAR_THRESHOLD = 0.03928;
        private const double LINEAR_DIVISOR = 12.92;
        private const double GAMMA_OFFSET = 0.055;
        private const double GAMMA_DIVISOR = 1.055;
        private const double GAMMA_EXPONENT = 2.4;

        /// <summary>
        /// Determines a high-contrast color (black or white) for the given hex color.
        /// </summary>
        public string GetContrastingColor(string color)
        {
            (int red, int green, int blue) = GetRGBFromHex(color);

            double luminance = CalculateLuminance(red, green, blue);

            return luminance > 0.5 ? "#000000" : "#FFFFFF";
        }

        /// <summary>
        /// Parses a hex color string into its RGB components.
        /// Supports formats: #RRGGBB, RRGGBB, #RGB, or RGB.
        /// </summary>
        private (int red, int green, int blue) GetRGBFromHex(string hexColor)
        {
            hexColor = hexColor.Trim().TrimStart('#');

            if (hexColor.Length == 3) // Handle shorthand hex colors (e.g., #FFF)
            {
                hexColor = $"{hexColor[0]}{hexColor[0]}{hexColor[1]}{hexColor[1]}{hexColor[2]}{hexColor[2]}";
            }
            else if (hexColor.Length != 6)
            {
                throw new ArgumentException("Invalid hex color format. Use #RRGGBB or #RGB.", nameof(hexColor));
            }

            return (
                Convert.ToInt32(hexColor.Substring(0, 2), 16),
                Convert.ToInt32(hexColor.Substring(2, 2), 16),
                Convert.ToInt32(hexColor.Substring(4, 2), 16)
            );
        }

        /// <summary>
        /// Calculates the relative luminance of a color in RGB space.
        /// </summary>
        private double CalculateLuminance(int red, int green, int blue)
        {
            double r = ApplyGammaCorrection(red / RGB_MAX);
            double g = ApplyGammaCorrection(green / RGB_MAX);
            double b = ApplyGammaCorrection(blue / RGB_MAX);

            return (RED_LUMINANCE_WEIGHT * r) +
               (GREEN_LUMINANCE_WEIGHT * g) +
               (BLUE_LUMINANCE_WEIGHT * b);
        }

        /// <summary>
        /// Applies gamma correction to a normalized RGB component.
        /// </summary>
        private double ApplyGammaCorrection(double channel)
        {
            return channel <= LINEAR_THRESHOLD
                ? channel / LINEAR_DIVISOR
                : Math.Pow((channel + GAMMA_OFFSET) / GAMMA_DIVISOR, GAMMA_EXPONENT);
        }
    }
}
