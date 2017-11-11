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

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for add_event.xaml
    /// </summary>
    public partial class add_event : Window
    {
        string command;
        public add_event()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (textBox1.Text == "")//||textBox6.Text == "")
            {
                MessageBox.Show("Essential fields not filled!");
                flag = false;
            }
            if (flag)
            {
                string tempstring = "add" + " \"" + textBox1.Text + "\"" + " at" + " \"" + textBox2.Text + " \"" + " start" + " \"" + textBox3.Text + " \"" + " end" + " \"" + textBox4.Text + " \"" + " from" + " \"" + textBox5.Text + " \"" + " to" + " \"" + textBox6.Text + " \"" + " priority" + " \"" + textBox9.Text + " \"" + " reminder" + " \"" + textBox8.Text + " \"";
                //TextUI obj = new TextUI(tempstring);
                //buttonAddEvent(command);
                command = tempstring;
                MessageBox.Show("Event added successfully");

            }
        }
        public string getCommand()
        {
            return command;
        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
