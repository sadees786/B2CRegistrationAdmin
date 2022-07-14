using RegistrationAdmin.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAdmin.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Joins all the items that are not null, or just whitespace into one string
        /// </summary>
        public static string JoinNonEmpty(this IEnumerable<string> items, string separator)
        {
            return string.Join(separator, items.Where(i => !string.IsNullOrWhiteSpace(i)));
        }

        /// <summary>
        /// Show the nullDefault string if the item is null, empty or whitespace
        /// </summary>
        /// <param name="item"></param>
        /// <param name="nullDefault"></param>
        /// <returns></returns>
        public static string OrShow(this string item, string nullDefault)
        {
            return string.IsNullOrWhiteSpace(item)
                ? nullDefault
                : item;
        }

        public static string ReplaceCarriageReturnAndLineFeedWithCarriageReturn(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str.Trim().Replace("\r\n", "\n");
        }

        public static string ReplaceLineFeedWithCarriageReturnAndLineFeed(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str.Trim().Replace("\n", "\r\n");
        }

        public static string ShortDateGbFormat(this DateTime dt)
        {
            return dt.ToString("d", new CultureInfo("en - GB"));
        }

        public static string TitleCase(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : new CultureInfo("en-GB", false).TextInfo.ToTitleCase(str);
        }

        public static TrimString TitleCase(this TrimString str)
        {
            return TitleCase((string)str);
        }

        /// <summary>
        /// True values will show "Yes" or provided alternative.
        /// False (or null) values will show "No" or provided alternative
        /// </summary>
        public static string ToYesNo(this bool? val, string yes = "Yes", string no = "No")
        {
            return ToYesNo(val == true, yes, no);
        }

        /// <summary>
        /// True values will show "Yes" or provided alternative.
        /// False values will show "No" or provided alternative
        /// </summary>
        public static string ToYesNo(this bool val, string yes = "Yes", string no = "No")
        {
            return val ? yes : no;
        }

        /// <summary>
        /// Retrieve text from inside of html element like <label id='dd'>some text here</label>
        /// </summary>
        public static string GetTextInsideHtmlElement(this string htmlInput)
        {
            int tag1 = htmlInput.IndexOf(">");
            int tag2 = htmlInput.IndexOf("</");
            string rr = htmlInput.Substring(tag1 + 1, tag2 - tag1 - 1);
            return rr;
        }
    }
}
