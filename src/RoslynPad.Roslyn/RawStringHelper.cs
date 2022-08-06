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
        Regex regString = new Regex(@"(?<a>"""""")(?<b>[\w\W]*?)(?<c>"""""")", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        /// <summary>
        /// 处理"""的写法
        /// </summary>
        /// <param name="code"></param>
        /// <param name="positions">起止位置</param>
        /// <returns></returns>
        public string ParseRawString(string code, out List<(int s, int e)> positions)
        {
            positions = new List<(int s, int e)>();
            //查找Raw string literals
            var ms = regString.Matches(code);
            if (ms == null || ms.Count == 0)
                return code;
            StringBuilder sb = new StringBuilder();
            int offset = 0;
            for (int i = 0; i < ms.Count; i++)
            {
                var m = ms[i]!;
                sb.Append(code.Substring(offset, m.Index - offset));
                sb.Append("@\"");
                sb.Append(m.Groups["b"].Value.Replace("\"", "\"\""));
                offset = m.Index + m.Length;
                sb.Append("\"");
                positions.Add((m.Index, m.Index + m.Length));
            }

            sb.Append(code.Substring(offset));
            return sb.ToString();
        }
        #endregion

    }
}
