using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
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
            //替换代码中的workpath变量
            CodeParsers.Add(s =>
            {
                //string workpath="";
                //var _p = @$"var _parameters= new {{WorkingDirectory = @"" {_parameters.WorkingDirectory} "" }};";
                //return _p + Environment.NewLine + s;
                return System.Text.RegularExpressions.Regex.Replace(s, @"string\s+_workpath\s*=\s*""""\s*;", $"string _workPath=@\"{_parameters.WorkingDirectory}\";", RegexOptions.IgnoreCase);


            });

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

    //public class ExecutionHostConsole
    //{
    //    public static void RunScript(string code, string platform, ExecutionHostParameters para)
    //    {
    //        // await _executionHost.ExecuteAsync(code, ShowIL, OptimizationLevel).ConfigureAwait(true);
    //        var _executionHost = new ExecutionHost(para
    //        , new RoslynHost(ImmutableArray.Create(
    //       typeof(RoslynHost).Assembly),
    //           RoslynHostReferences.NamespaceDefault.With(typeNamespaceImports: new[] { typeof(Runtime.ObjectExtensions) }),
    //           disabledDiagnostics: ImmutableArray.Create("CS1701", "CS1702")));
    //        switch (platform)
    //        {
    //            case "net462":
    //            case "net472":
    //            default:
    //                _executionHost.Platform = new ExecutionPlatform(".NET Framework x64", platform, string.Empty, Architecture.X64, isCore: false);
    //                break;
    //            case "netcore6":
    //                //case "netcore5":
    //                _executionHost.Platform = new ExecutionPlatform(".NET Core", "netcoreapp6.0", "6.0.400", Architecture.X64, isCore: true);
    //                break;
    //        }
    //        _executionHost.ExecuteAsync(code, true, Microsoft.CodeAnalysis.OptimizationLevel.Release).Wait();
    //    }
    //}
}
