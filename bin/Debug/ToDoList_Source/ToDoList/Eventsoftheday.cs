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
    using System.Windows.Media;

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class Eventsoftheday : Window, IComponentConnector
    {
        private bool _contentLoaded;
        private string _date;
        internal DataGrid dataGrid2;
        internal Label label1;
        internal Label label2;
        internal Label label3;
        internal Label label4;
        private Conversions objectConversions;

        public Eventsoftheday(string date)
        {
            this.InitializeComponent();
            this.objectConversions = new Conversions();
            this._date = date;
            this.label1.Content = "Events for the date " + this._date;
            this.label2.Content = "Tasks";
            this.label3.Content = " ";
        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DateEvent RowDataContaxt = e.Row.DataContext as DateEvent;
            if ((RowDataContaxt != null) && (RowDataContaxt.EventName != null))
            {
                e.Row.Background = Brushes.Red;
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void dataGrid2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
        }

        public void initialiseDatagrid(List<DateEvent> dateEvents, List<int> Items)
        {
            this.dataGrid2.ItemsSource = dateEvents;
        }

        public void initialiseTasks(string eventsForTheDay)
        {
            this.label3.Content = eventsForTheDay;
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/eventsoftheday.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        private void label_loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.label1 = (Label) target;
                    this.label1.Loaded += new RoutedEventHandler(this.label_loaded);
                    break;

                case 2:
                    this.dataGrid2 = (DataGrid) target;
                    this.dataGrid2.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.dataGrid2_LoadingRow);
                    this.dataGrid2.SelectionChanged += new SelectionChangedEventHandler(this.dataGrid2_SelectionChanged_1);
                    break;

                case 3:
                    this.label4 = (Label) target;
                    break;

                case 4:
                    this.label2 = (Label) target;
                    break;

                case 5:
                    this.label3 = (Label) target;
                    break;

                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}

