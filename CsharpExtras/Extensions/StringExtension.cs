using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CsharpExtras.Extensions
{
    public static class StringExtension
    {
        public static string InsertSpaceBetweenEachLetternAndFollowingUppercaseLetter(this string str)
        {
            return Regex.Replace(str, "((?<=[A-z])[A-Z])", " $1", RegexOptions.Compiled);
        }
        public static ISet<string> Lines(this string str)
        {
            StringReader reader = new StringReader(str);
            string nextLine = reader.ReadLine();
            ISet<string> set = new HashSet<string>();
            while (nextLine != null)
            {
                set.Add(nextLine);
                nextLine = reader.ReadLine();
            }
            return set;
        }

        public static string[] Split(this string str, string separator)
        {
            return str.Split(new string[] { separator }, StringSplitOptions.None);
        }

        public static string RemoveDigits(this string str)
        {
            //non-mvp: could make that one or more times
            string digitPattern = "[0-9]*";
            return str.RemoveRegexMatches(digitPattern);
        }

        public static string RemoveRegexMatches(this string str, string regex)
        {
            return str.RemoveRegexMatches(new Regex(regex));
        }

        public static string RemoveRegexMatches(this string str, Regex regex)
        {
            return regex.Replace(str, "");
        }

        public static string RemoveWhitespace(this string str)
        {
            return str.RemoveRegexMatches(@"\s+");
        }

        public static string RemoveNewlines(this string str)
        {
            return str.RemoveRegexMatches(@"\r|\n");
        }

        public static bool EqualsIgnoreDigits(this string str, string other)
        {
            return RemoveDigits(str) == RemoveDigits(other);
        }

        public static bool ContainsAny(this string str, params string[] strList)
        {
            foreach (string s in strList)
            {
                if (str.Contains(s)) return true;
            }
            return false;
        }

        public static bool EqualsIgnoreWhitespaceAndCase(this string str, string other)
        {
            return str.RemoveWhitespace().Equals(other.RemoveWhitespace(), StringComparison.OrdinalIgnoreCase);
        }

        //Non-mvp: Test
        /// <summary>
        /// Converts a date in OAD format, e.g. such as one read from an Excel sheet, to a date in a known format
        /// </summary>
        /// <param name="oadDate">The date in OAD format that we want to convet. This is the date format that gets read from an Excel cell.</param>
        /// <returns>Empty string if conversion fails, or a date value in ISO format otherwise</returns>
        public static string TryConvertOadDateStringToFormattedDate(this string oadDate)
        {
            if (double.TryParse(oadDate, out double parsedDate))
            {
                DateTime conv = DateTime.FromOADate(parsedDate);
                return conv.ToString("yyyy-MM-dd");
            }
            return "";
        }

        public static string RemoveAllCharactersExceptNumbersAndLetters(this string str)
        {
            return str.RemoveRegexMatches(@"[^a-zA-Z0-9]");
        }

        public static string GetFirstRegexCaptureGroupInFirstMatchOrBlank(this string str, string regexString)
        {
            return GetRegexCaptureGroupInFirstMatchOrBlank(str, regexString, 1);
        }


        /// <summary>
        /// Gets the regex capture of the group at the given index in the regex for the first match of the regex to the given string
        /// </summary>
        /// <param name="str">This string</param>
        /// <param name="regexString">A regex against which to match this string</param>
        /// <param name="groupIndex">The 1 based index of the group within the match</param>
        /// <returns>The value of the regex capture group at the first match of the regex in the string</returns>
        public static string GetRegexCaptureGroupInFirstMatchOrBlank(this string str, string regexString, int groupIndex)
        {
            Regex regex = new Regex(regexString);
            Match match = regex.Match(str);
            string capture = GetRegexCaptureInGroupOrBlank(match, groupIndex);
            return capture;
        }

        /// <summary>
        /// Gets a group capture from a match of the given regex in the given string
        /// </summary>
        /// <param name="str">Search in here for matches</param>
        /// <param name="regexString">This is the pattern used to match - it should contain at least one capture group</param>
        /// <param name="matchIndex">The 0 based index of the match in our search string</param>
        /// <param name="groupIndex">The 1 based index of the group within the match</param>
        /// <returns>If a match exists at the given index and a capture exists within that match, then return that capture. Else return empty string.</returns>
        public static string GetRegexCaptureInGroupOrBlank(this string str, string regexString, int matchIndex, int groupIndex)
        {
            Regex regex = new Regex(regexString);
            MatchCollection matches = regex.Matches(str);
            int minimumRequiredMatches = matchIndex + 1;
            if (matches.Count < minimumRequiredMatches) return string.Empty;
            Match match = matches[matchIndex];
            string capture = GetRegexCaptureInGroupOrBlank(match, groupIndex);
            return capture;
        }

        private static string GetRegexCaptureInGroupOrBlank(Match match, int groupIndex)
        {
            if (match == null || !match.Success || match.Groups.Count < groupIndex)
            {
                return string.Empty;
            }
            return match.Groups[groupIndex].Value;
        }

        public static bool StringValueConvertedToIntGreaterThanOrEqualTo(this string str, int value)
        {
            if (int.TryParse(str, out int strInt))
            {
                return strInt >= value;
            }
            return false;
        }

        public static bool StringValueConvertedToIntGreaterThanOrEqualToOne(this string str)
        {
            return StringValueConvertedToIntGreaterThanOrEqualTo(str, 1);
        }

        public static bool StringValueConvertedToIntGreaterThanOrEqualToZero(this string str)
        {
            return StringValueConvertedToIntGreaterThanOrEqualTo(str, 0);
        }

        public static bool IsInt(this string str)
        {
            return int.TryParse(str, out int _);
        }

        public static int ToInt(this string str)
        {
            if (int.TryParse(str, out int strInt))
            {
                return strInt;
            }

            throw new InvalidOperationException("The string should be a integer.");
        }
    }
}

