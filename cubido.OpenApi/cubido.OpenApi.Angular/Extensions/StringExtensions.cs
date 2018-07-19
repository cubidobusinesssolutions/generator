using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cubido.OpenApi.Angular.Extensions
{
    public static class StringExtensions
    {
        /// <summary>Returns the string in TypeScript interpolated string format.</summary>
        /// <param name="s">Interpolated format string</param>
        public static string ToTypeScriptInterpolated(this string s)
        {
            // replaces {identifier} by ${identifier}
            return Regex.Replace(s, @"\{\w+\}", @"$$$0", RegexOptions.Compiled);
        }

    }
}
