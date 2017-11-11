namespace ToDoList
{
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Windows;

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class App : Application
    {
        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
        }

        [STAThread, DebuggerNonUserCode]
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}

