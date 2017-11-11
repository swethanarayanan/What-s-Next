using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for Remainder.xaml
    /// </summary>
    public partial class Remainder : Window
    {
        public Remainder(string s)
        {
            InitializeComponent();
           // mediaElement1.Play();
            label2.Content = s;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            //return "closed";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}