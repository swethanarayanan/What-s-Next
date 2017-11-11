namespace ToDoList
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class History : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal DataGrid datagrid;
        private List<string> listOfCompletedEvents;
        private List<Event> listOfEvents;
        private List<TaskDetails> listOfTaskDetails;

        public History()
        {
            this.InitializeComponent();
            this.listOfCompletedEvents = new List<string>();
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/history.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        public void SetEventDetails(string CompletedEvent)
        {
            this.listOfCompletedEvents.Add(CompletedEvent);
            Conversions objectConversions = new Conversions();
            this.listOfTaskDetails = objectConversions.convertStringIntoTaskDetails(this.listOfCompletedEvents);
            this.listOfEvents = objectConversions.convertTaskdetailsIntoEvent(this.listOfTaskDetails);
            this.datagrid.ItemsSource = this.listOfEvents;
            foreach (DataGridColumn item in this.datagrid.Columns)
            {
                MessageBox.Show(item.Header.ToString());
                if (item.Header.ToString().Equals("selectTask"))
                {
                    item.Visibility = Visibility.Hidden;
                }
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                this.datagrid = (DataGrid) target;
                this.datagrid.SelectionChanged += new SelectionChangedEventHandler(this.datagrid_SelectionChanged);
            }
            else
            {
                this._contentLoaded = true;
            }
        }
    }
}

