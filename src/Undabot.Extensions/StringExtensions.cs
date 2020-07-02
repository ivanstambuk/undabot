using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Undabot.Extensions
{
    public static class StringExtensions
    {
        const string RegexMatchWords = @"\w+[^\s]*\w+|\w";

        /// <summary>
        /// Splits a running text into words, by punctuation and whitespace
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitToWords(this string text)
        {
            return Regex.Matches(text, RegexMatchWords)
                .Cast<Match>()
                .Select(w => w.Value.ToLowerInvariant());
        }

        /// <summary>
        /// Wraps matched strings in HTML elements
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keywords">Comma-separated list of strings to be tagged</param>
        /// <param name="htmlTag">The HTML tag to apply</param>
        /// <param name="fullMatch">false for returning all matches, true for whole word matches only</param>
        /// <returns>string</returns>
        public static string TagKeywords(
            this string text,
            string keywords,
            string htmlTag,
            bool fullMatch = true)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keywords) || string.IsNullOrEmpty(htmlTag))
            {
                return text;
            }

            var keywordsList = keywords.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string result;
            string replacement = $"<{htmlTag}>{"$0"}</{htmlTag}>";

            if (fullMatch)
            {
                result = keywordsList.Select(word => @"\b" + word.Trim() + @"\b")
                        .Aggregate(text, (current, pattern) =>
                            Regex.Replace(current, pattern, replacement, RegexOptions.IgnoreCase));
            }
            // We don't match full words
            else
            {
                result = keywordsList.Select(word => word.Trim())
                                .Aggregate(text, (current, pattern) =>
                                    Regex.Replace(current, pattern, replacement, RegexOptions.IgnoreCase));

            }

            return result;
        }
    }
}