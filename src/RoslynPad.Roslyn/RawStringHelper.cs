using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RoslynPad.Roslyn
{
    internal class RawStringHelper
    {
        static RawStringHelper _ = new RawStringHelper();
        public static RawStringHelper Instance { get; } = _;
        #region  rawstring 
        Regex regString = new Regex(@"(?<a>(?<d>\$?)"""""")(?<b>[\w\W]*?)(?<c>"""""")", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        /// <summary>
        /// 处理"""的写法
        /// </summary>
        /// <param name="code"></param>
        /// <param name="positions">起止位置</param>
        /// <returns></returns>
        public string ParseRawString(string code, out List<(int s, int e)> positions)
        {
            return findStringByRegex(regString, code, out positions, m =>
            {
                
                return $"{m.Groups["d"]?.Value}@\"" + m.Groups["b"].Value.Replace("\"", "\"\"") + "\"";
            });
        }
        public string findStringByRegex(Regex reg, string code, out List<(int s, int e)> positions, Func<Match, string> func)
        {
            positions = new List<(int s, int e)>();
            //查找Raw string literals
            var ms = reg.Matches(code);
            if (ms == null || ms.Count == 0)
                return code;
            StringBuilder sb = new StringBuilder();
            int offset = 0;
            for (int i = 0; i < ms.Count; i++)
            {
                var m = ms[i]!;
                sb.Append(code.Substring(offset, m.Index - offset));
                sb.Append(func(m)); //替换代码
                offset = m.Index + m.Length;
                positions.Add((m.Index, m.Index + m.Length));
            }

            sb.Append(code.Substring(offset));
            return sb.ToString();
        }
        #endregion

        #region razor
        Regex regRazor = new Regex(@"(?<a>#region\s+razor)(?<b>[\w\W]*?)(?<c>#endregion)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        /// <summary>
        /// 处理razor部分
        /// </summary>
        /// <param name="code"></param>
        /// <param name="positions"></param>
        /// <returns></returns>
        public string ParseRazorSection(string code, out List<(int s, int e)> positions,Func<Match,string> runRazor)
        {
            return findStringByRegex(regRazor, code, out positions, m =>
            {
                return "@\"" + runRazor(m) + "\"";
            });
        }
        

        #endregion
    }
}
