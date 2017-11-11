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
    public class add_event : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal Button button1;
        private string command;
        internal Image image2;
        internal Image image3;
        internal Label label1;
        internal Label label10;
        internal Label label2;
        internal Label label3;
        internal Label label4;
        internal Label label5;
        internal Label label7;
        internal Label label9;
        internal TextBox textBox1;
        internal TextBox textBox2;
        internal TextBox textBox3;
        internal TextBox textBox4;
        internal TextBox textBox5;
        internal TextBox textBox6;
        internal TextBox textBox8;
        internal TextBox textBox9;

        public add_event()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("Essential fields not filled!");
                flag = false;
            }
            if (flag)
            {
                string tempstring = "add \"" + this.textBox1.Text + "\" at \"" + this.textBox2.Text + " \" start \"" + this.textBox3.Text + " \" end \"" + this.textBox4.Text + " \" from \"" + this.textBox5.Text + " \" to \"" + this.textBox6.Text + " \" priority \"" + this.textBox9.Text + " \" reminder \"" + this.textBox8.Text + " \"";
                this.command = tempstring;
                MessageBox.Show("Event added successfully");
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
                Uri resourceLocater = new Uri("/ToDoList;component/ui/add_event.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocater);
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((add_event) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    break;

                case 2:
                    this.button1 = (Button) target;
                    this.button1.Click += new RoutedEventHandler(this.button1_Click);
                    break;

                case 3:
                    this.textBox1 = (TextBox) target;
                    this.textBox1.TextChanged += new TextChangedEventHandler(this.textBox1_TextChanged);
                    break;

                case 4:
                    this.textBox2 = (TextBox) target;
                    break;

                case 5:
                    this.textBox3 = (TextBox) target;
                    break;

                case 6:
                    this.textBox4 = (TextBox) target;
                    break;

                case 7:
                    this.label1 = (Label) target;
                    break;

                case 8:
                    this.label2 = (Label) target;
                    break;

                case 9:
                    this.label3 = (Label) target;
                    break;

                case 10:
                    this.label4 = (Label) target;
                    break;

                case 11:
                    this.textBox5 = (TextBox) target;
                    break;

                case 12:
                    this.label5 = (Label) target;
                    break;

                case 13:
                    this.textBox6 = (TextBox) target;
                    break;

                case 14:
                    this.textBox8 = (TextBox) target;
                    break;

                case 15:
                    this.textBox9 = (TextBox) target;
                    break;

                case 0x10:
                    this.label7 = (Label) target;
                    break;

                case 0x11:
                    this.label9 = (Label) target;
                    break;

                case 0x12:
                    this.label10 = (Label) target;
                    break;

                case 0x13:
                    this.image2 = (Image) target;
                    break;

                case 20:
                    this.image3 = (Image) target;
                    break;

                default:
                    this._contentLoaded = true;
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}

