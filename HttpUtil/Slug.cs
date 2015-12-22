using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtil
{
    /// <summary>
    /// Provides helper methods for efficiently generate a SEO-friendly slugs.
    /// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls/6740497#6740497
    /// </summary>
    public static class Slug
    {
        /// <summary>
        /// Creates a slug from one or more string parameters.
        /// </summary>
        /// <param name="toLower"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Create(params string[] values)
        {
            return Create(string.Join("-", values));
        }

        /// <summary>
        /// Creates a slug.
        /// References:
        /// http://www.unicode.org/reports/tr15/tr15-34.html
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// http://stackoverflow.com/questions/25259/how-do-you-include-a-webpage-title-as-part-of-a-webpage-url/25486#25486
        /// http://stackoverflow.com/questions/3769457/how-can-i-remove-accents-on-a-string
        /// </summary>
        /// <param name="toLower"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Create(string value)
        {
            if (value == null)
                return "";

            // Approach that is purely lookup based misses many characters found in examples while researching on Stack Overflow. 
            // To counter this, first peform a normalisation pass (AKA collation mentioned in Meta Stack Overflow question:
            // Non US-ASCII characters dropped from full (profile) http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696), 
            // and then ignore any characters outside the acceptable ranges. This works most of the time...
            var normalised = value.Normalize(NormalizationForm.FormKD);

            const int maxlen = 80;
            int len = normalised.Length;
            bool prevDash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = normalised[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    if (prevDash)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (prevDash)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }
                    // Tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevDash && sb.Length > 0)
                    {
                        prevDash = true;
                    }
                }
                else
                {
                    string swap = ConvertEdgeCases(c);

                    if (swap != null)
                    {
                        if (prevDash)
                        {
                            sb.Append('-');
                            prevDash = false;
                        }
                        sb.Append(swap);
                    }
                }

                if (sb.Length == maxlen)
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Lookup table for when normalization pass failed. 
        /// Some characters don’t map to a low ASCII value when normalised. 
        /// The normalisation code was inspired by Jon Hanna’s great post in Stack Overflow question: 
        /// How can I remove accents on a string? http://stackoverflow.com/questions/3769457
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static string ConvertEdgeCases(char c)
        {
            string swap = null;
            switch (c)
            {
                case 'ı':
                    swap = "i";
                    break;
                case 'ł':
                    swap = "l";
                    break;
                case 'Ł':
                    swap = "l";
                    break;
                case 'đ':
                    swap = "d";
                    break;
                case 'ß':
                    swap = "ss";
                    break;
                case 'ø':
                    swap = "o";
                    break;
                case 'Þ':
                    swap = "th";
                    break;
            }
            return swap;
        }
    }
}
