using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Com.Research.TwitterTrendingAutoExtraction.Utils
{
    /// <summary>
    /// Utility class for extracting hashtags from the text and 
    /// </summary>
    public static class ExtractHashTags
    {

        /// <summary>
        /// Returns the list of Hashtags from the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> ExtractTags(string input)
        {
            List<string> hashtags = new List<string>();
            var regex = new Regex(@"(?<=#)\w+");
            var matches = regex.Matches(input);

            foreach (Match m in matches)
            {
                hashtags.Add(m.Value);
                
            }

            return new List<string>(hashtags.Distinct());

        }
    }
}
