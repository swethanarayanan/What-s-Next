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
    /// Interaction logic for edit_event.xaml
    /// </summary>
    public partial class edit_event : Window
    {
        int id;
        string command;
        public edit_event(int eventid)
        {
            InitializeComponent();
            id = eventid;
            Parser obj = new Parser("display");
            List<Event> currentevents = new List<Event>();
            currentevents = obj.taskToBePerformed();
            TaskDetails TaskDetailsObject=new TaskDetails();
            Conversions Conversionsobject = new Conversions();
            TaskDetailsObject = Conversionsobject.convertEventsIntoTaskdetails(currentevents[id]);

            textBox2.Text = TaskDetailsObject.getEventname()== "null" ? " " : TaskDetailsObject.getEventname();
            textBox3.Text = TaskDetailsObject.getLocation() == "null" ? " " : TaskDetailsObject.getLocation();
            textBox4.Text = TaskDetailsObject.getStartDate() == "null" ? " " : TaskDetailsObject.getStartDate();
            textBox5.Text = TaskDetailsObject.getEndDate() == "null" ? " " : TaskDetailsObject.getEndDate();
            textBox6.Text = TaskDetailsObject.getStartTime() == "null" ? " " : TaskDetailsObject.getStartTime();
            textBox7.Text = TaskDetailsObject.getEndTime() == "null" ? " " : TaskDetailsObject.getEndTime(); 
            textBox9.Text = currentevents[id].Remind_Me_Before == "null" ? " " : TaskDetailsObject.getRemainderTime();
            textBox10.Text = currentevents[id].Priority == "null" ? " " : TaskDetailsObject.getPriority();
            command = "";
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (textBox2.Text == "")
            {
                MessageBox.Show("Event name not filled!");
                flag = false;
            }
            if (flag)
            {
                id++;
                string tempstring = "edit id" + " \"" + id.ToString() + "\"" + " event" + " \"" + textBox2.Text + "\"" + " at" + " \"" + textBox3.Text + "\"" + " start" + " \"" + textBox4.Text + "\"" + " end" + " \"" + textBox5.Text + "\"" + " from" + " \"" + textBox6.Text + "\"" + " to" + " \"" + textBox7.Text + "\"" + " priority" + " \"" + textBox10.Text + "\"" + " reminder" + " \"" + textBox9.Text + "\"";
                command = tempstring;
                MessageBox.Show("Event edited successfully");
                //Edit ID “11” event “Dinner” at “Pizza Hut” start “2/9/11” end “2/9/11” from “8.00 pm” to “10.00 pm” duration “0.5 hr” priority “high” reminder “1 hr”
                this.Close();
            }
 

        }
        public string getCommand()
        {
            return command;
        }
    }
}
