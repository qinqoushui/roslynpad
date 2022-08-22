using System;
using System.Runtime;
using System.Windows;

namespace RoslynPad
{
    public partial class App
    {
        private const string ProfileFileName = "RoslynPad.jitprofile";

        public App()
        {
            ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory!);
            ProfileOptimization.StartProfile(ProfileFileName);
        }
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    MainWindow window = new MainWindow(e.Args);
        //    window.Show();

        //}
    }
}
