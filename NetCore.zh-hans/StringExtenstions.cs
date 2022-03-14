using System.Text.RegularExpressions;

namespace NetCore.zh_hans
{
    public static class StringExtensions
    {
        /// <summary>
        /// 把\n \r 替换成空格
        /// 把多个空格替换成一个空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSpace(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var text = str
                .Replace('\r', ' ')
                .Replace('\n', ' ');

            var replaceSpaceRegex = new Regex(@"\s{1,}", RegexOptions.IgnoreCase);
            return replaceSpaceRegex.Replace(text, " ").Trim();
        }
    }
}