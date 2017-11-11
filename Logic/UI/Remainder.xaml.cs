/*********************************************************************************************************************************************
 * Author : Nivetha Sathyarajan
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * Displays a reminder for a task in a new window and a music is played in the background to grab the attention of the user
 * If the user enters values in the textboxes against postpone , then the task is postponed accordingly and
 * reminder by default is set to 1 hour before the new postponed time
 * If OK is pressed then the task is not affected and no more reminders will be displayed in the future for the task 
 *********************************************************************************************************************************************/
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
        int id;
        int days;
        int hours;
        int minutes;
        int seconds;
        string EditCommand;
        public Remainder(string s)
        {
            InitializeComponent();
            string IdOfTaskThatNeedsReminder = null;
            bool IdIdentified = false;
            EditCommand = null;
            int i = 0;
            int days = hours = minutes = seconds = 0;
            for (i = 0; i < s.Length; i++)
            {
                if (s[i] == '-')
                {
                    IdIdentified = true;
                }
                if (IdIdentified == false)
                {
                    IdOfTaskThatNeedsReminder += s[i];
                }
                else if (IdIdentified == true)
                {
                    id = Convert.ToInt32(IdOfTaskThatNeedsReminder.Trim());
                    break;
                }
            }
            label2.Content = null;
            string ContentOfLabel = null;
            i++;
            for (; i < s.Length; i++)
            {
                ContentOfLabel += s[i];
            }
            label2.Content = ContentOfLabel;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //if postponed is clicked then the start time or the end time is changed
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Conversions objectConversions = new Conversions();
            Parser parserObject = new Parser("display");
            List<Event> datagriditemSource = parserObject.taskToBePerformed();
            TaskDetails taskObj = objectConversions.convertEventsIntoTaskdetails(datagriditemSource[id - 1]);
            string DateToBePostponedTo = taskObj.getStartDate();
            string TimeToBePostponedTo = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();
            if (days != 0)
            {
                DateTime sDate = objectConversions.ConvertStringToDate(DateToBePostponedTo);
                DateToBePostponedTo = sDate.AddDays(days).ToShortDateString();
                EditCommand = "edit id" + " \"" + id.ToString() + "\"" + " start" + " \"" + DateToBePostponedTo + "\"";
            }
            if (taskObj.getEndTime() != "null")
            {
                string endTime= (TimeSpan.Parse(taskObj.getEndTime()) + TimeSpan.Parse(TimeToBePostponedTo)).ToString();
                EditCommand = "edit id" + " \"" + id.ToString() + "\"" + " to" + " \"" + endTime + "\"" + " start" + " \"" + DateToBePostponedTo + "\"" + "reminder" + " \"1H" + "\"";
            }
            if (taskObj.getStartTime() != "null")
            {
                string startTime = (TimeSpan.Parse(taskObj.getStartTime()) + TimeSpan.Parse(TimeToBePostponedTo)).ToString();
                EditCommand = "edit id" + " \"" + id.ToString() + "\"" + " from" + " \"" + startTime + "\"" + " start" + " \"" + DateToBePostponedTo + "\"" + "\"" + "reminder" + " \"1H" + "\"";
            }
            this.Close();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ContainsAllDigits = null;
            if (textBox1.Text.Length != 0)
            {
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    if (char.IsDigit(textBox1.Text[i]))
                    {
                        ContainsAllDigits += textBox1.Text[i];
                    }
                    else if (!(char.IsDigit(textBox1.Text[i])))
                    {
                        continue;
                    }
                }
            }
            if (ContainsAllDigits != null)
            {
                days = Convert.ToInt32(ContainsAllDigits);
            }
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ContainsAllDigits = null;
            if (textBox2.Text.Length != 0)
            {
                for (int i = 0; i < textBox2.Text.Length; i++)
                {
                    if (char.IsDigit(textBox2.Text[i]))
                    {
                        ContainsAllDigits += textBox2.Text[i];
                    }
                    else if (!(char.IsDigit(textBox2.Text[i])))
                    {
                        continue;
                    }
                }
            }
            if (ContainsAllDigits != null)
            {
                hours = Convert.ToInt32(ContainsAllDigits);
            }
        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ContainsAllDigits = null;
            if (textBox3.Text.Length != 0)
            {
                for (int i = 0; i < textBox3.Text.Length; i++)
                {
                    if (char.IsDigit(textBox3.Text[i]))
                    {
                        ContainsAllDigits += textBox3.Text[i];
                    }
                    else if (!(char.IsDigit(textBox3.Text[i])))
                    {
                        continue;
                    }
                }
            }
            if (ContainsAllDigits != null)
            {
                minutes = Convert.ToInt32(ContainsAllDigits);
            }
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ContainsAllDigits = null;
            if (textBox4.Text.Length != 0)
            {
                for (int i = 0; i < textBox4.Text.Length; i++)
                {
                    if (char.IsDigit(textBox4.Text[i]))
                    {
                        ContainsAllDigits += textBox1.Text[i];
                    }
                    else if (!(char.IsDigit(textBox4.Text[i])))
                    {
                        continue;
                    }
                }
            }
            if (ContainsAllDigits != null)
            {
                hours = Convert.ToInt32(ContainsAllDigits);
            }
        }
        public string GetEditCommand()
        {
            return EditCommand;
        }
    }

}