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
    public class done : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal Label label1;

        public done()
        {
            this.InitializeComponent();
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/done.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                this.label1 = (Label) target;
            }
            else
            {
                this._contentLoaded = true;
            }
        }
    }
}

