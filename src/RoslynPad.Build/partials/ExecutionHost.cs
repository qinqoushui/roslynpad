using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RoslynPad.Roslyn;

namespace RoslynPad.Build
{
    internal partial class ExecutionHost
    {
        List<Func<string, string>> CodeParsers = new List<Func<string, string>>();
        void InitCodeParser()
        {
            //https://github.com/fouadmess/RazorEngine
            CodeParsers.Add(s => RawStringHelper.Instance.ParseRawString(s, out var positions));

            TemplateServiceConfiguration config = new TemplateServiceConfiguration();
            var service = RazorEngineService.Create(config);
            CodeParsers.Add(s => RawStringHelper.Instance.ParseRazorSection(s, out var positions, m =>
            {
                try
                {
                    //@Model.x
                    string key = $"razor{DateTime.Now.Millisecond}";
                    string s = service.RunCompile(m.Groups["b"].Value, key);
                    // var tpl = config.TemplateManager;
                    return s;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }));



        }
    }
}
