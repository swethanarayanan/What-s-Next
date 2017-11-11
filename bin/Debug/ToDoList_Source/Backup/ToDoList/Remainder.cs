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
    public class Remainder : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal Label label2;
        internal MediaElement mediaElement1;

        public Remainder(string s)
        {
            this.InitializeComponent();
            this.mediaElement1.Play();
            this.label2.Content = s;
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/remainder.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        private void mediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.mediaElement1 = (MediaElement) target;
                    this.mediaElement1.MediaOpened += new RoutedEventHandler(this.mediaElement1_MediaOpened);
                    break;

                case 2:
                    this.label2 = (Label) target;
                    break;

                default:
                    this._contentLoaded = true;
                    break;
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
        }
    }
}

