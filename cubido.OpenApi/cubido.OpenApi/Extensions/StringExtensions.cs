using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cubido.OpenApi.Extensions
{
    public static class StringExtensions
    {
        /// <summary>TheQuickBrownFoxJumpsOverTheLazyDog</summary>
        /// <param name="s">A string in camel or kebap case.</param>
        public static string ToUpperCamelCase(this string s)
        {
            // convert kebap to camel case
            s = Regex.Replace(s, @"-\p{Ll}", match => match.Value.Substring(1).ToUpper());

            // see https://stackoverflow.com/a/42310740
            return Char.ToUpperInvariant(s[0]) + s.Substring(1);
        }

        /// <summary>theQuickBrownFoxJumpsOverTheLazyDog</summary>
        /// <param name="s">A string in camel or kebap case.</param>
        public static string ToLowerCamelCase(this string s)
        {
            // convert kebap to camel case
            s = Regex.Replace(s, @"-\p{Ll}", match => match.Value.Substring(1).ToUpper());

            // see https://stackoverflow.com/a/42310740
            return Char.ToLowerInvariant(s[0]) + s.Substring(1);
        }

        /// <summary>the-quick-brown-fox-jumps-over-the-lazy-dog</summary>
        /// <param name="s">A string in camel case.</param>
        public static string ToLowerKebapCase(this string s)
        {
            // credits to https://stackoverflow.com/a/21327150
            return String.IsNullOrWhiteSpace(s) 
                ? string.Empty
                // (?= ) positive lookahead
                // (?! ) negative lookahead
                // (?<! ) negative lookbehind
                // (?<= ) positive lookbehind
                // \p{Lu} Letter upperspace
                // \p{Ll} Letter lowerspace
                : Regex.Replace(s, @"(?<!^)(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})", "-", RegexOptions.Compiled).ToLowerInvariant();

        }
    }
}
