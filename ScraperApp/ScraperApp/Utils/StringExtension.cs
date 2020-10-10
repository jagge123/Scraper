using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ScraperApp.Utils
{
    public static class StringExtension
    {
        public static string RemoveStartingSlash(this string input)
        {
            int index = input.IndexOf("/");
            return (index < 0)
                ? input
                : input.Remove(index, 1);
        }

        public static string ForwardSlashToDoubleBack(this string input)
        {
            return input.Replace("/", "\\");
        }

        public static string RemoveHtmlEnding(this string inmput)
        {
            return Regex.Replace(inmput, ".html", "");
        }
    }
}
