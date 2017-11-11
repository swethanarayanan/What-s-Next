namespace ToDoList
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Threading;

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public class MainWindow : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal System.Windows.Controls.MenuItem About;
        internal Calendar calendar1;
        internal System.Windows.Controls.MenuItem Commands;
        internal System.Windows.Controls.DataGrid datagrid;
        internal System.Windows.Controls.DataGrid dataGrid1;
        internal System.Windows.Controls.MenuItem Help;
        internal System.Windows.Controls.MenuItem History;
        internal System.Windows.Controls.Image image1;
        internal System.Windows.Controls.Label label1;
        internal System.Windows.Controls.Label label2;
        internal System.Windows.Controls.Label label3;
        internal System.Windows.Controls.Label label4;
        internal System.Windows.Controls.Label label5;
        internal System.Windows.Controls.ListBox listBox1;
        internal System.Windows.Controls.Menu menu1;
        private NotifyIcon notifyIcon;
        private Parser parserObject;
        internal System.Windows.Controls.MenuItem Quit;
        private WindowState storedWindowState = WindowState.Normal;
        internal System.Windows.Controls.TextBox textBox1;
        internal System.Windows.Controls.TextBox textBox2;
        internal System.Windows.Controls.MenuItem UserGuide;
        internal System.Windows.Controls.MenuItem View;

        public MainWindow()
        {
            this.InitializeComponent();
            this.InitialiseNotificationIcon();
            this.clearTheContentOfUndoFileAtTheStart();
            this.parserObject = new Parser("display");
            this.datagrid.ItemsSource = this.parserObject.taskToBePerformed();
            this.listBox1.Visibility = Visibility.Hidden;
            this.retrieveEventList(this.parserObject.taskToBePerformed(), DateTime.Today.ToShortDateString());
            this.dataGrid1.Visibility = Visibility.Collapsed;
            this.datagrid.Visibility = Visibility.Visible;
            this.DisplayReminder();
            this.UpdateSystemTime();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Window1 aboutWindow = new Window1();
            StreamReader userguidereader = new StreamReader("About.txt");
            string UserGuide = "";
            for (string lineFromUserGuide = userguidereader.ReadLine(); lineFromUserGuide != null; lineFromUserGuide = userguidereader.ReadLine())
            {
                UserGuide = UserGuide + lineFromUserGuide + "\n";
            }
            aboutWindow.Display.Text = aboutWindow.Display.Text + UserGuide;
            aboutWindow.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            add_event subform = new add_event();
            subform.ShowDialog();
            Parser objectParser = new Parser(subform.getCommand());
            this.dataGrid1.Visibility = Visibility.Collapsed;
            this.datagrid.ItemsSource = objectParser.taskToBePerformed();
            subform.Close();
        }

        private void addSelectedItemToTextbox(string currentItemInListBox)
        {
            string s = null;
            int i;
            string appendWithCloseQuotes = null;
            if (this.textBox1.Text[this.textBox1.Text.Length - 1] == '"')
            {
                i = this.textBox1.Text.Length - 2;
                appendWithCloseQuotes = "\"";
            }
            else
            {
                i = this.textBox1.Text.Length - 1;
            }
            s = this.ExtractStringUntilLastSpecialCharacterInTextbox(i);
            if ((this.listBox1.SelectedIndex != -1) && (s != null))
            {
                this.textBox1.Text = s + currentItemInListBox + " " + appendWithCloseQuotes;
            }
            else if (this.listBox1.SelectedIndex != -1)
            {
                this.textBox1.Text = currentItemInListBox + " " + appendWithCloseQuotes;
            }
            this.textBox1.Select(this.textBox1.Text.Length - 2, 0);
        }

        private void appearanceOfListBox()
        {
            this.listBox1.Items.Clear();
            ListBoxContent objectListBoxContent = new ListBoxContent();
            string[] s = this.textBox1.Text.Split(new char[] { ' ' });
            List<string> listOfIntellisenseStrings = objectListBoxContent.addItemToListBox(s[s.Length - 1]);
            for (int i = 0; i < listOfIntellisenseStrings.Count; i++)
            {
                this.listBox1.Items.Add(listOfIntellisenseStrings[i]);
            }
        }

        private void Calendar_Opened(object sender, MouseButtonEventArgs e)
        {
            string clickedDateTime = this.calendar1.SelectedDate.ToString();
            this.parserObject = new Parser("display");
            List<Event> events = this.parserObject.taskToBePerformed();
            this.retrieveEventList(events, clickedDateTime);
        }

        private void CheckTrayIcon()
        {
            this.ShowTrayIcon(!base.IsVisible);
        }

        private void clearTheContentOfUndoFileAtTheStart()
        {
            string fileToBeDealtWith = "f1.txt";
            string tempFileName = "output.txt";
            using (new StreamWriter(tempFileName))
            {
                if (File.Exists(fileToBeDealtWith))
                {
                    File.Delete(fileToBeDealtWith);
                }
            }
            File.Move(tempFileName, fileToBeDealtWith);
            File.Delete(tempFileName);
        }

        private void Commands_Click(object sender, RoutedEventArgs e)
        {
            Window1 commandsWindow = new Window1();
            StreamReader userguidereader = new StreamReader("commands.txt");
            string userguide = "";
            for (string linefromuserguide = userguidereader.ReadLine(); linefromuserguide != null; linefromuserguide = userguidereader.ReadLine())
            {
                userguide = userguide + linefromuserguide + "\n";
            }
            commandsWindow.Display.Text = commandsWindow.Display.Text + userguide;
            commandsWindow.Show();
        }

        private void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            this.dataGrid1.Visibility = Visibility.Collapsed;
            this.datagrid.Visibility = Visibility.Visible;
            this.datagrid.RowBackground = System.Windows.Media.Brushes.BlanchedAlmond;
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DateEvent RowDataContext = e.Row.DataContext as DateEvent;
            if ((RowDataContext != null) && (RowDataContext.EventName != null))
            {
                e.Row.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (this.datagrid.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Please select the event row to be deleted");
            }
            else
            {
                int eventid = this.datagrid.SelectedIndex + 1;
                Parser objectParser = new Parser("delete id \"" + eventid.ToString() + "\"");
                this.dataGrid1.Visibility = Visibility.Collapsed;
                this.datagrid.Visibility = Visibility.Visible;
                this.datagrid.ItemsSource = objectParser.taskToBePerformed();
                new delete_event().ShowDialog();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.label4.Content = DateTime.Now.ToString("g");
        }

        private void displayRemainder(object sender, EventArgs e)
        {
            this.parserObject = new Parser(null);
            string s = this.parserObject.CheckIfRemainderNeeded(DateTime.Now);
            if (s != null)
            {
                new Remainder(s).ShowDialog();
            }
        }

        private void DisplayReminder()
        {
            DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Tick += new EventHandler(this.displayRemainder);
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer1.Start();
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (this.datagrid.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Please select the event which you have completed");
            }
            else
            {
                int eventid = this.datagrid.SelectedIndex + 1;
                Parser objectParser = new Parser("done id \"" + eventid.ToString() + "\"");
                this.dataGrid1.Visibility = Visibility.Collapsed;
                this.datagrid.Visibility = Visibility.Visible;
                this.datagrid.ItemsSource = objectParser.taskToBePerformed();
                new done().ShowDialog();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (this.datagrid.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Please select the event which you want to edit");
            }
            else
            {
                edit_event subform = new edit_event(this.datagrid.SelectedIndex);
                subform.ShowDialog();
                Parser objectParser = new Parser(subform.getCommand());
                this.dataGrid1.Visibility = Visibility.Collapsed;
                this.datagrid.Visibility = Visibility.Visible;
                this.datagrid.ItemsSource = objectParser.taskToBePerformed();
                subform.Close();
            }
        }

        private string ExtractStringUntilLastSpecialCharacterInTextbox(int i)
        {
            string s = null;
            while (i >= 0)
            {
                if (!char.IsLetter(this.textBox1.Text[i]) && !char.IsNumber(this.textBox1.Text[i]))
                {
                    break;
                }
                i--;
            }
            for (int j = 0; j <= i; j++)
            {
                s = s + this.textBox1.Text[j];
            }
            return s;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            this.About.Visibility = Visibility.Visible;
            this.UserGuide.Visibility = Visibility.Visible;
            this.Commands.Visibility = Visibility.Visible;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            new HistoryDatabase().ViewCompletedEvents();
        }

        private void InitialiseNotificationIcon()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "What'sNext has been minimised. Click the tray icon to show.";
            this.notifyIcon.BalloonTipTitle = "What'sNext";
            this.notifyIcon.Text = "What'sNext";
            this.notifyIcon.Icon = new Icon("Logo.ico");
            this.notifyIcon.Click += new EventHandler(this.notifyIcon_Click);
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocater = new Uri("/ToDoList;component/mainwindow.xaml", UriKind.Relative);
                System.Windows.Application.LoadComponent(this, resourceLocater);
            }
        }

        private void maximiseWindow()
        {
            if (base.WindowState == WindowState.Minimized)
            {
                base.Show();
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            base.Show();
            base.WindowState = this.storedWindowState;
        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            this.notifyIcon.Dispose();
            this.notifyIcon = null;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            this.CheckTrayIcon();
        }

        private void OnStateChanged(object sender, EventArgs args)
        {
            if (base.WindowState == WindowState.Minimized)
            {
                base.Hide();
                if (this.notifyIcon != null)
                {
                    this.notifyIcon.ShowBalloonTip(0x7d0);
                }
            }
            else
            {
                this.storedWindowState = base.WindowState;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            string MessageBoxText = "Are you sure you want to exit What's Next?";
            string MessageBoxHeader = "What's Next?";
            MessageBoxButton YesNo = MessageBoxButton.YesNo;
            MessageBoxImage Warning = MessageBoxImage.Exclamation;
            if (System.Windows.MessageBox.Show(MessageBoxText, MessageBoxHeader, YesNo, Warning) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void retrieveEventList(List<Event> events, string clickedDateTime)
        {
            string[] date = clickedDateTime.Trim().Split(new char[] { ' ' });
            List<DateEvent> dateEvents = this.parserObject.taskToBePerformed(events, date[0].ToString());
            this.datagrid.Visibility = Visibility.Collapsed;
            this.dataGrid1.Visibility = Visibility.Visible;
            this.dataGrid1.ItemsSource = dateEvents;
        }

        private void ShowTrayIcon(bool show)
        {
            if (this.notifyIcon != null)
            {
                this.notifyIcon.Visible = show;
            }
        }

        [DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((MainWindow) target).Closing += new CancelEventHandler(this.OnClose);
                    ((MainWindow) target).StateChanged += new EventHandler(this.OnStateChanged);
                    ((MainWindow) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.OnIsVisibleChanged);
                    ((MainWindow) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
                    break;

                case 2:
                    this.datagrid = (System.Windows.Controls.DataGrid) target;
                    this.datagrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.datagrid_LoadingRow);
                    this.datagrid.SelectionChanged += new SelectionChangedEventHandler(this.datagrid_SelectionChanged);
                    break;

                case 3:
                    this.label5 = (System.Windows.Controls.Label) target;
                    break;

                case 4:
                    this.label1 = (System.Windows.Controls.Label) target;
                    break;

                case 5:
                    this.label2 = (System.Windows.Controls.Label) target;
                    break;

                case 6:
                    this.dataGrid1 = (System.Windows.Controls.DataGrid) target;
                    break;

                case 7:
                    this.calendar1 = (Calendar) target;
                    break;

                case 8:
                    this.label4 = (System.Windows.Controls.Label) target;
                    break;

                case 9:
                    this.label3 = (System.Windows.Controls.Label) target;
                    break;

                case 10:
                    this.listBox1 = (System.Windows.Controls.ListBox) target;
                    break;

                case 11:
                    this.textBox1 = (System.Windows.Controls.TextBox) target;
                    this.textBox1.TextChanged += new TextChangedEventHandler(this.textBox1_TextChanged);
                    this.textBox1.KeyDown += new System.Windows.Input.KeyEventHandler(this.textbox_KeyDown);
                    this.textBox1.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.textBox1_PreviewKeyDown);
                    break;

                case 12:
                    this.menu1 = (System.Windows.Controls.Menu) target;
                    break;

                case 13:
                    this.View = (System.Windows.Controls.MenuItem) target;
                    this.View.Click += new RoutedEventHandler(this.View_Click);
                    break;

                case 14:
                    this.History = (System.Windows.Controls.MenuItem) target;
                    this.History.Click += new RoutedEventHandler(this.History_Click);
                    break;

                case 15:
                    this.Help = (System.Windows.Controls.MenuItem) target;
                    this.Help.Click += new RoutedEventHandler(this.Help_Click);
                    break;

                case 0x10:
                    this.About = (System.Windows.Controls.MenuItem) target;
                    this.About.Click += new RoutedEventHandler(this.About_Click);
                    break;

                case 0x11:
                    this.UserGuide = (System.Windows.Controls.MenuItem) target;
                    this.UserGuide.Click += new RoutedEventHandler(this.UserGuide_Click);
                    break;

                case 0x12:
                    this.Commands = (System.Windows.Controls.MenuItem) target;
                    this.Commands.Click += new RoutedEventHandler(this.Commands_Click);
                    break;

                case 0x13:
                    this.Quit = (System.Windows.Controls.MenuItem) target;
                    this.Quit.Click += new RoutedEventHandler(this.Quit_Click);
                    break;

                case 20:
                    this.image1 = (System.Windows.Controls.Image) target;
                    break;

                case 0x15:
                    this.textBox2 = (System.Windows.Controls.TextBox) target;
                    break;

                default:
                    this._contentLoaded = true;
                    break;
            }
        }

        private void textbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.listBox1.Visibility = Visibility.Hidden;
                Parser objectTextUI = new Parser(this.textBox1.Text);
                objectTextUI.CheckIfCommandAndContentValid();
                if (string.Compare(objectTextUI.getCommand(), "view", true) == 0)
                {
                    this.retrieveEventList(objectTextUI.taskToBePerformed(), objectTextUI.getContent());
                }
                else
                {
                    this.dataGrid1.Visibility = Visibility.Collapsed;
                    this.datagrid.Visibility = Visibility.Visible;
                    this.datagrid.ItemsSource = objectTextUI.taskToBePerformed();
                }
                this.textBox1.Text = "\"\"";
            }
        }

        private void textBox1_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key == Key.Left) || (e.Key == Key.Right))
            {
                this.listBox1.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.listBox1.Visibility = Visibility.Visible;
            }
            if (e.Key == Key.Down)
            {
                this.listBox1.SelectedIndex++;
            }
            if ((e.Key == Key.Right) && (this.listBox1.SelectedIndex != -1))
            {
                this.listBox1.Visibility = Visibility.Collapsed;
                int indexOfItemSelected = this.listBox1.SelectedIndex;
                string currentItemInListBox = this.listBox1.Items[indexOfItemSelected].ToString();
                this.addSelectedItemToTextbox(currentItemInListBox);
            }
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.listBox1.SelectedIndex = -1;
            this.appearanceOfListBox();
            if ((this.textBox1.Text.Length - 1) > 0)
            {
                char CS$0$0001 = this.textBox1.Text[this.textBox1.Text.Length - 1];
                if ((CS$0$0001.CompareTo(' ') == 0) && (this.textBox1.Text.Length != 0))
                {
                    Parser objectTextUI = new Parser(this.textBox1.Text);
                    int len = this.textBox1.Text.Length;
                    string appendThisToText = objectTextUI.HelpUserWriteCommand(this.textBox1.Text);
                    this.textBox1.Text = this.textBox1.Text + appendThisToText;
                    this.textBox1.SelectionStart = this.textBox1.Text.Length;
                    if ((appendThisToText != null) && (appendThisToText.CompareTo("\"\"") == 0))
                    {
                        this.textBox1.Select(this.textBox1.Text.Length - 1, 0);
                    }
                    int pos = this.textBox1.SelectionStart;
                    this.textBox1.ScrollToEnd();
                    if (this.listBox1.SelectedIndex != -1)
                    {
                        this.textBox1.Text.Insert(pos, this.listBox1.Items[this.listBox1.SelectedIndex].ToString());
                    }
                }
            }
        }

        private void UpdateSystemTime()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(this.dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void UserGuide_Click(object sender, RoutedEventArgs e)
        {
            Window1 userGuideWindow = new Window1();
            StreamReader userGuideReader = new StreamReader("UserGuide.txt");
            string UserGuide = "";
            for (string LineFromUserGuide = userGuideReader.ReadLine(); LineFromUserGuide != null; LineFromUserGuide = userGuideReader.ReadLine())
            {
                UserGuide = UserGuide + LineFromUserGuide + "\n";
            }
            userGuideWindow.Display.Text = userGuideWindow.Display.Text + UserGuide;
            userGuideWindow.Show();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            this.History.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}

