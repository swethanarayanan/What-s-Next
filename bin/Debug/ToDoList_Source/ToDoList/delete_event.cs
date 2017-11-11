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
    public class delete_event : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal TextBox textBox1;

        public delete_event()
        {
            this.InitializeComponent();
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/delete_event.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                this.textBox1 = (TextBox) target;
            }
            else
            {
                this._contentLoaded = true;
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }
    }
}

