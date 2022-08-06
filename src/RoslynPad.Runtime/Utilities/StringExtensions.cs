using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RoslynPad.Utilities
{
    public static class StringExtensions
    {
        public static string Join(this IEnumerable<string> source, string separator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (separator == null) throw new ArgumentNullException(nameof(separator));

            return string.Join(separator, source);
        }

       
    }
}
