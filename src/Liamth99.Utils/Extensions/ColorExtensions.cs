using System.Drawing;
using System.Text;

namespace Liamth99.Utils.Extensions;

/// <summary>
/// Provides extension methods for operations related to colors.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Converts a <see cref="Color"/> object to its hexadecimal string representation
    /// based on the specified format.
    /// </summary>
    /// <param name="color">The color to be converted to a hexadecimal string.</param>
    /// <param name="format">
    /// A format string that determines the layout of the hexadecimal representation.
    /// </param>
    /// <returns>A string containing the hexadecimal representation of the color.</returns>
    /// <exception cref="ArgumentException">Thrown when the format string is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="FormatException">
    /// Thrown when the format string contains invalid token lengths.
    /// Only single or double character channel tokens are allowed.
    /// </exception>
    /// <remarks>
    /// Use 'R', 'G', 'B' for red, green, and blue channels, and 'A' for alpha channel.
    /// Tokens can be single (e.g., 'R') for shorthand representation or double (e.g., 'RR')
    /// for full byte representation. Non-channel characters are included as literals.
    /// </remarks>
    public static string ToHex(this Color color, string format = "#RRGGBB")
    {
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Format cannot be null or empty.", nameof(format));

        if (format is "#RRGGBB")
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        if (format is "#AARRGGBB")
            return color.ToArgb().ToString("X");

        var sb = new StringBuilder(format.Length * 2);

        for (int i = 0; i < format.Length;)
        {
            char c = format[i];

            byte value;
            switch (c)
            {
                case 'A':
                    value = color.A;
                    break;
                case 'R':
                    value = color.R;
                    break;
                case 'G':
                    value = color.G;
                    break;
                case 'B':
                    value = color.B;
                    break;
                default:
                    // Is other token to be included
                    i++;
                    sb.Append(c);
                    continue;
            }

            // Count consecutive identical chars
            int count = 1;
            while (i + count < format.Length && format[i + count] == c)
                count++;

            if (count is not (1 or 2))
                throw new FormatException($"Invalid token length '{new string(c, count)}'. Only single or double channel tokens are allowed.");

            if (count == 1)
                sb.Append((value >> 4).ToString("X1")); // shorthand
            else
                sb.Append(value.ToString("X2")); // full byte

            i += count;
        }

        return sb.ToString();
    }
}