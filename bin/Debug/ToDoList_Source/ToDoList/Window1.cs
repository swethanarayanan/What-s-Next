namespace ToDoList
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class Window1 : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal TextBox Display;

        public Window1()
        {
            this.InitializeComponent();
        }

        private void DisplayUserGuide_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/about.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                this.Display = (TextBox) target;
                this.Display.TextChanged += new TextChangedEventHandler(this.DisplayUserGuide_TextChanged);
            }
            else
            {
                this._contentLoaded = true;
            }
        }
    }
}

