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
    public class edit_event : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal Button button1;
        private string command;
        private int id;
        internal Image image2;
        internal Image image3;
        internal Label label10;
        internal Label label2;
        internal Label label3;
        internal Label label4;
        internal Label label5;
        internal Label label6;
        internal Label label7;
        internal Label label9;
        internal TextBox textBox10;
        internal TextBox textBox2;
        internal TextBox textBox3;
        internal TextBox textBox4;
        internal TextBox textBox5;
        internal TextBox textBox6;
        internal TextBox textBox7;
        internal TextBox textBox9;

        public edit_event(int eventid)
        {
            this.InitializeComponent();
            this.id = eventid;
            Parser obj = new Parser("display");
            List<Event> currentevents = new List<Event>();
            currentevents = obj.taskToBePerformed();
            this.textBox2.Text = currentevents[this.id].EventName;
            this.textBox4.Text = currentevents[this.id].StartDate;
            this.textBox5.Text = currentevents[this.id].EndDate;
            this.textBox6.Text = currentevents[this.id].startTime;
            this.textBox7.Text = currentevents[this.id].endTime;
            this.textBox9.Text = currentevents[this.id].reminderBefore;
            this.textBox10.Text = currentevents[this.id].priority;
            this.command = "";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (this.textBox2.Text == "")
            {
                MessageBox.Show("Event name not filled!");
                flag = false;
            }
            if (flag)
            {
                this.id++;
                string tempstring = "edit id \"" + this.id.ToString() + "\" event \"" + this.textBox2.Text + "\" at \"" + this.textBox3.Text + "\" start \"" + this.textBox4.Text + "\" end \"" + this.textBox5.Text + "\" from \"" + this.textBox6.Text + "\" to \"" + this.textBox7.Text + "\" priority \"" + this.textBox10.Text + "\" reminder \"" + this.textBox9.Text + "\"";
                this.command = tempstring;
                MessageBox.Show("Event edited successfully");
            }
        }

        public string getCommand()
        {
            return this.command;
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/ui/edit_event.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.button1 = (Button) target;
                    this.button1.Click += new RoutedEventHandler(this.button1_Click);
                    break;

                case 2:
                    this.textBox2 = (TextBox) target;
                    break;

                case 3:
                    this.textBox3 = (TextBox) target;
                    break;

                case 4:
                    this.textBox4 = (TextBox) target;
                    break;

                case 5:
                    this.textBox5 = (TextBox) target;
                    break;

                case 6:
                    this.label2 = (Label) target;
                    break;

                case 7:
                    this.label3 = (Label) target;
                    break;

                case 8:
                    this.label4 = (Label) target;
                    break;

                case 9:
                    this.label5 = (Label) target;
                    break;

                case 10:
                    this.textBox6 = (TextBox) target;
                    break;

                case 11:
                    this.label6 = (Label) target;
                    break;

                case 12:
                    this.textBox7 = (TextBox) target;
                    break;

                case 13:
                    this.textBox9 = (TextBox) target;
                    break;

                case 14:
                    this.textBox10 = (TextBox) target;
                    break;

                case 15:
                    this.label7 = (Label) target;
                    break;

                case 0x10:
                    this.label9 = (Label) target;
                    break;

                case 0x11:
                    this.label10 = (Label) target;
                    break;

                case 0x12:
                    this.image2 = (Image) target;
                    break;

                case 0x13:
                    this.image3 = (Image) target;
                    break;

                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}

