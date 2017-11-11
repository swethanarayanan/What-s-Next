using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for Eventsoftheday.xaml
    /// </summary>

    public partial class Eventsoftheday : Window 
    {
        string _date;

        Conversions objectConversions;
        public Eventsoftheday(string date)
        {
            InitializeComponent();

            objectConversions = new Conversions();
            _date = date;
            label1.Content = "Events for the date " + _date;
            label2.Content = "Tasks";
            label3.Content = " ";
           
        }

        private void label_loaded(object sender, RoutedEventArgs e)
        {

        }

        public void initialiseTasks(string eventsForTheDay)
        {
            label3.Content = eventsForTheDay;
        }
        public void initialiseDatagrid(List<DateEvent> dateEvents, List<int> Items)
        {
            dataGrid2.ItemsSource = dateEvents;
          //  Grid1.ItemsSource = dateEvents;
           

        }
        
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        
        //}

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DateEvent RowDataContaxt = e.Row.DataContext as DateEvent;
            if (RowDataContaxt != null)
            {
                if (RowDataContaxt.EventName != null)
                    e.Row.Background = Brushes.Red; //FindResource("GreenBackgroundBrush") as Brush;
                //datagrid.RowBackground = Brushes.Yellow;
            }
        }
        private void dataGrid2_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
