using System;
using RoslynPad.Roslyn;
using RoslynPad.Build;
using System.Collections.Immutable;
using Microsoft.Win32;
using RoslynPad;
using System.Runtime.InteropServices;

namespace RoslynConsole
{
    internal class Program
    {
        /// <summary>
        /// 指定文件和目标
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //string file = args[0];
            //string framework = args[1];
            //Console.WriteLine($"run {args[0]} as {args[1]}");
            string file = "D:\\code\\Zooy\\PatrolCore\\src\\PatrolDeviceServer\\DeviceAgent\\Publish\\publishenigma.csx"; 
            string framework = "net462";
            var BuildPath = Path.Combine(Path.GetTempPath(), "roslynpad", "build", DateTime.Now.Ticks.ToString());
            Directory.CreateDirectory(BuildPath);
            string nugetConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Roaming", "NuGet");
            var para = new ExecutionHostParameters(
            BuildPath,
                nugetConfig,
               RoslynHostReferences.NamespaceDefault.With(typeNamespaceImports: new[] { typeof(RoslynPad.Runtime.ObjectExtensions) }).Imports,
                ImmutableArray.Create("CS1701", "CS1702"),
                Path.GetDirectoryName(file)!);
            ExecutionHostConsole.RunScript(File.ReadAllText(file, System.Text.Encoding.UTF8), framework, para);
        }
    }


}
