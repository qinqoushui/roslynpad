using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RoslynPad.Roslyn;

namespace RoslynPad.Build
{
    internal partial class ExecutionHost
    {
        List<Func<string, string>> CodeParsers = new List<Func<string, string>>();
        void InitCodeParser()
        {
            CodeParsers.Add(s => RawStringHelper.Instance.ParseRawString(s, out var positions));
        }


    }
}
