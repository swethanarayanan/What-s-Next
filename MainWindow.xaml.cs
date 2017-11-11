/********************************************************************************************************
 * Authors : Monika  Puhazhendhi, Nivetha Sathyarajan ,Swetha Narayanan,Venkatesh Sridharan
 * -----------------Functionalty Implemented In This Project---------------------------------------
 * (a)add a new task with/without repeat feature
 * (b)edit a task
 * (c)delete a task
 * (d)Undo/Redo
 * (e)Search for a task by date,or event name,etc
 * (f)View the list of tasks on a particular date in timeline format
 * (g)Reminder for an event
 * (h)Re-schedule a task
 * (e)Intellisense
 * (f)Archive a Task as completed
 * (g)View list of completed tasks
 * (h)Postpone a task
 * *******************************************************************************************************/
/*********************************************************************************************************
 * -----------------------------------Class Descripton----------------------------------------------------
 * Acts as the user interface
 * Passes the user entered command to Parser class for further interpretation
 * Any click event on the GUI element is captured and the appropriate function is performed
 *******************************************************************************************************/

//To-do list
using System.Windows;
using System.Windows.Threading;
using System;
using System.IO;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        string lastInput;
        Parser parserObject;
        public MainWindow()
        {
            InitializeComponent();
            /***logging in***/
            StreamWriter loggingWriter = new StreamWriter("Logging.txt");
            Debug.Assert(loggingWriter != null);
            loggingWriter.Close();
            test.Visibility = Visibility.Hidden;
            InitialiseNotificationIcon();

           // InitialiseNotifyUserAbtQuotes();
            clearTheContentOfUndoFileAtTheStart();
            clearTheContentOfRedoFileAtTheStart();
            //*************************************Datagrid Display*******************************************
            parserObject = new Parser("display");
            datagrid.ItemsSource = parserObject.taskToBePerformed();
           

            //hide listbox
            listBox1.Visibility = Visibility.Hidden;

            dataGrid1.Visibility = Visibility.Collapsed;
            datagrid.Visibility = Visibility.Visible;
             
            DisplayReminder();          
            UpdateSystemTime();
       }
        private void DisplayReminder()
        {
            DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Tick += new EventHandler(displayRemainder);
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer1.Start();
        }
        private void UpdateSystemTime()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
       
       
        

        //*************************************NotificationIcon**********************************************
        private void InitialiseNotificationIcon()
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.notifyIcon.BalloonTipText = "What'sNext has been minimised. Click the tray icon to show.";
            this.notifyIcon.BalloonTipTitle = "What'sNext";
            this.notifyIcon.Text = "What'sNext";
            this.notifyIcon.Icon = new System.Drawing.Icon("Logo.ico");
            this.notifyIcon.Click += new EventHandler(notifyIcon_Click);

           
        }
        void notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = storedWindowState;
        }
        void OnClose(object sender, CancelEventArgs args)
        {
            notifyIcon.Dispose();
            notifyIcon = null;
        }

        private WindowState storedWindowState = WindowState.Normal;
        void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (notifyIcon != null)
                    notifyIcon.ShowBalloonTip(2000);
            }
            else
                storedWindowState = WindowState;
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (notifyIcon != null)
                notifyIcon.Visible = show;
        }
        void maximiseWindow()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Show();
            }
        }
        //*************************************NotificationIcon**********************************************
        private void clearTheContentOfUndoFileAtTheStart()
        {
            string fileToBeDealtWith = "undo.txt";
            string tempFileName = "output.txt";
            using (StreamWriter writer = new StreamWriter(tempFileName))
                Debug.Assert(writer != null);
                if (File.Exists(fileToBeDealtWith))
                    File.Delete(fileToBeDealtWith);
            File.Move(tempFileName, fileToBeDealtWith);
            File.Delete(tempFileName);
        
        }
        private void clearTheContentOfRedoFileAtTheStart()
        {
            string fileToBeDealtWith = "redo.txt";
            string tempFileName = "output.txt";
            using (StreamWriter writer = new StreamWriter(tempFileName))
                Debug.Assert(writer != null);
                if (File.Exists(fileToBeDealtWith))
                    File.Delete(fileToBeDealtWith);
            File.Move(tempFileName, fileToBeDealtWith);
            File.Delete(tempFileName);

        }
        //************************************Reminder*******************************************************
        private void displayRemainder(object sender, EventArgs e)
        {
            //displays a remainder when the task is added....yet to implement it to meet the actual requirement.

            parserObject = new Parser(null);
            string s = parserObject.CheckIfRemainderNeeded(DateTime.Now);
            if (s != null)
            {
                Remainder form1 = new Remainder(s);
                form1.ShowDialog();
                string command = form1.GetEditCommand();
                if (command != null)
                {
                    Parser objectParser1 = new Parser(command);
                    dataGrid1.Visibility = Visibility.Collapsed;
                    datagrid.Visibility = Visibility.Visible;
                    datagrid.ItemsSource = objectParser1.taskToBePerformed();
                    ToastMsg.Text = "Postponed!";
                    parserObject = new Parser("display");
                    datagrid.ItemsSource = parserObject.taskToBePerformed();
                }
            }
            //parserObject = new Parser("display");
            //datagrid.ItemsSource = parserObject.taskToBePerformed();

        }
        //************************************Reminder*******************************************************
        // displays the system time
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            label4.Content = DateTime.Now.ToString("f") + "\n\t" + DateTime.Now.ToString("t");
        }

        //this function is used to get the content of the textbox before intellisense is required
        private string ExtractStringUntilLastSpecialCharacterInTextbox(int i)
        {
            //loops until special characters like white space,quotes are encountered
            string s = null;
            for (; i >= 0; i--)
            {
                if (char.IsLetter(textBox1.Text[i]) || char.IsNumber(textBox1.Text[i]))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            for (int j = 0; j <= i; j++)
            {
                s += textBox1.Text[j];
            }
            return s;
        }
        //this function is used to get the item from the listbox and write it into the textbox
        private void addSelectedItemToTextbox(string currentItemInListBox)
        {
            string s = null;
            int i; //for loop variable
            string appendWithCloseQuotes = null;
            if (textBox1.Text[textBox1.Text.Length - 1] == '\"')
            {
                i = textBox1.Text.Length - 2;
                appendWithCloseQuotes = "\"";
            }
            else
            {
                i = textBox1.Text.Length - 1;
            }

            s = ExtractStringUntilLastSpecialCharacterInTextbox(i);

            //if s is null , it implies that the first word of the textbox is choosen from the listbox
            if (listBox1.SelectedIndex != -1 && s != null)
            {
                textBox1.Text = s + currentItemInListBox + " " + appendWithCloseQuotes;
            }
            else if (listBox1.SelectedIndex != -1)
            {
                textBox1.Text = currentItemInListBox + " " + appendWithCloseQuotes;
            }
            //positions the cursor at the appropriate position in the textbox
            textBox1.Select(textBox1.Text.Length - 2, 0);
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                listBox1.Visibility = Visibility.Collapsed;
            }
            else
            {
                listBox1.Visibility = Visibility.Visible;
            }
            //select from a listbox
            if (e.Key == Key.Down)
            {
                ++listBox1.SelectedIndex;
            }
            else if (e.Key == Key.Up && listBox1.SelectedIndex != -1)
            {
                --listBox1.SelectedIndex;
            }
            //choose the item user had selected from the listbox
            if (e.Key == Key.Right && listBox1.SelectedIndex != -1)
            {
                listBox1.Visibility = Visibility.Collapsed;
                int indexOfItemSelected = listBox1.SelectedIndex;
                string currentItemInListBox = listBox1.Items[indexOfItemSelected].ToString();
                addSelectedItemToTextbox(currentItemInListBox);

            }

        }
       
        //calls member function of class objectTextUI to process the command entered by the user in the command box
        private void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            // List<TaskDetails> temp = new List<TaskDetails>();

            if (e.Key == Key.Return)
            {
                listBox1.Visibility = Visibility.Hidden;
                string textboxContent = textBox1.Text;
                Parser objectTextUI = new Parser(textboxContent);
                string s = objectTextUI.SyntaxOfTextboxText(textBox1.Text);
                if (textBox1.Text != s)
                    ToastMsg.Text = s;
                else
                {
                    objectTextUI.CheckIfCommandAndContentValid();
                    if (String.Compare(objectTextUI.getCommand(), "view", true) == 0)
                    {
                        retrieveEventList(objectTextUI.taskToBePerformed(), objectTextUI.getContent());
                    }
                    
                    else
                    {
                        dataGrid1.Visibility = Visibility.Collapsed;
                        datagrid.Visibility = Visibility.Visible;
                      //  PerformanceChart performanceChart1 = new PerformanceChart();
                        if (String.Compare(objectTextUI.getCommand(), "performance", true) == 0)
                        {
                   //       performanceLabel.Visibility = Visibility.Visible;
                     //   performanceButton.Visibility = Visibility.Visible;
                       //  performanceChart1.Visibility = Visibility.Visible;
                        }
                        datagrid.ItemsSource = objectTextUI.taskToBePerformed();
                        
                        lastInput = textboxContent;
                        ToastMsg.Text = objectTextUI.getDisplay();
                    }

                    textBox1.Text = "\"\"";
                }
            }
        }
        //controls the appearance of the listbox
        private void appearanceOfListBox()
        {
            listBox1.Items.Clear();
            ListBoxContent objectListBoxContent = new ListBoxContent();
            string[] s = textBox1.Text.Split(' '); // holds all the words in the textbox
            List<string> listOfIntellisenseStrings = objectListBoxContent.addItemToListBox(s[s.Length - 1]);
            for (int i = 0; i < listOfIntellisenseStrings.Count; i++)
            {
                listBox1.Items.Add(listOfIntellisenseStrings[i]);
            }
        }
        //integrates the appearance of listbox and "" after keywords by calling appropriate functions
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
            listBox1.SelectedIndex = -1;
            appearanceOfListBox();
            if ((textBox1.Text.Length - 1) > 0)
            {
                if (textBox1.Text[textBox1.Text.Length - 1].CompareTo(' ') == 0 && (textBox1.Text.Length != 0))
                {
                    Parser objectTextUI = new Parser(textBox1.Text);
                    int len = textBox1.Text.Length;
                    string appendThisToText = objectTextUI.HelpUserWriteCommand();
                    textBox1.Text += appendThisToText;
                    textBox1.SelectionStart = textBox1.Text.Length;
                    if (appendThisToText != null && appendThisToText.CompareTo("\"\"") == 0)
                    {
                        textBox1.Select(textBox1.Text.Length - 1, 0);
                    }
                    int pos = textBox1.SelectionStart;
                    textBox1.ScrollToEnd();
                    if (listBox1.SelectedIndex != -1)
                    {
                        textBox1.Text.Insert(pos, listBox1.Items[listBox1.SelectedIndex].ToString());
                    }
                }
   
            }
        }
        
        private void retrieveEventList(List<Event> events ,string clickedDateTime)
        {
            if (clickedDateTime != null)
            {
                if (clickedDateTime.IndexOf('\"') != -1)
                {
                    clickedDateTime = clickedDateTime.Replace('\"', ' ').Trim();
                }
                ErrorHandling objectErrorHandling = new ErrorHandling();
                clickedDateTime = objectErrorHandling.CheckDate(clickedDateTime);
                if (!(clickedDateTime.IndexOf('/') != -1))
                {
                    ToastMsg.Text = clickedDateTime;
                    return ;
                }
                string[] date = clickedDateTime.Trim().Split(' ');
                List<DateEvent> dateEvents = parserObject.taskToBePerformed(events, date[0].ToString());
                datagrid.Visibility = Visibility.Collapsed;
                dataGrid1.Visibility = Visibility.Visible;
                dataGrid1.ItemsSource = dateEvents;
            }
            else
            {
                ToastMsg.Text = "Please enter the date";
            }
        }
            
        private void Calendar_Opened(object sender, MouseButtonEventArgs e)
        {
            Conversions objectConversions = new Conversions(); 
            DateTime clickedDateTime =DateTime.Parse(calendar1.SelectedDate.ToString());
            string sClickedDateTime = (clickedDateTime).ToString("dd/MM/yyyy");
            parserObject = new Parser("display");
            List<Event> events = parserObject.taskToBePerformed();
            retrieveEventList(events, sClickedDateTime.Replace("-","/"));

        }
        //************short-cut for commands************************************
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void undoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Parser parserObject = new Parser("undo");
            datagrid.ItemsSource = parserObject.taskToBePerformed();
            ToastMsg.Text = parserObject.getDisplay();
        }
        private void redoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Parser parserObject = new Parser("redo");
            datagrid.ItemsSource = parserObject.taskToBePerformed();
            ToastMsg.Text = parserObject.getDisplay();
        }
        //******************************************Menu Functionality**************************************
        private void View_Click(object sender, RoutedEventArgs e)
        {
            History.Visibility = Visibility.Visible;
           // Performance.Visibility = Visibility.Visible;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            Parser objectTextUI = new Parser("history");
            dataGrid1.Visibility = Visibility.Collapsed;
            datagrid.Visibility = Visibility.Visible;
            datagrid.ItemsSource = objectTextUI.taskToBePerformed();
            ToastMsg.Text = objectTextUI.getDisplay();
            datagrid.Visibility = Visibility.Visible;          
          
          
        }
        private void Performance_Click(object sender, RoutedEventArgs e)
        {
       //     performanceLabel.Visibility = Visibility.Visible;
         //     performanceButton.Visibility = Visibility.Visible;
           //  performanceChart1.Visibility = Visibility.Visible;
        
            
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            About.Visibility = Visibility.Visible;
            UserGuide.Visibility = Visibility.Visible;
            Commands.Visibility = Visibility.Visible;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            String MessageBoxText = "Are you sure you want to exit What's Next?";
            String MessageBoxHeader = "What's Next?";
            MessageBoxButton YesNo = MessageBoxButton.YesNo;
            MessageBoxImage Warning = MessageBoxImage.Warning;
            MessageBoxResult userDecision = MessageBox.Show(MessageBoxText, MessageBoxHeader, YesNo, Warning);
            if (userDecision == MessageBoxResult.Yes)
                System.Environment.Exit(0);

        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new Window1();
            StreamReader userguidereader = new StreamReader(@"About.txt");
            Debug.Assert(userguidereader != null);
            string lineFromUserGuide, UserGuide = "";
            lineFromUserGuide = userguidereader.ReadLine();
            while (lineFromUserGuide != null)
            {
                UserGuide += lineFromUserGuide + "\n";
                lineFromUserGuide = userguidereader.ReadLine();
            }
            aboutWindow.Display.Text += UserGuide;
            aboutWindow.Show();
        }
       
        private void UserGuide_Click(object sender, RoutedEventArgs e)
        {
            var userGuideWindow = new Window1();
            StreamReader userGuideReader = new StreamReader(@"UserGuide.txt");
            Debug.Assert(userGuideReader != null);
            String LineFromUserGuide, UserGuide = "";
            LineFromUserGuide = userGuideReader.ReadLine();
            while (LineFromUserGuide != null)
            {
                UserGuide += LineFromUserGuide + "\n";
                LineFromUserGuide = userGuideReader.ReadLine();
            }
            userGuideWindow.Display.Text += UserGuide;
            userGuideWindow.Show();
        }

        private void Commands_Click(object sender, RoutedEventArgs e)
        {
            var commandsWindow = new Window1();
            StreamReader userguidereader = new StreamReader(@"commands.txt");
            Debug.Assert(userguidereader != null);
            string linefromuserguide, userguide = "";
            linefromuserguide = userguidereader.ReadLine();
            while (linefromuserguide != null)
            {
                userguide += linefromuserguide + "\n";
                linefromuserguide = userguidereader.ReadLine();
            }
            commandsWindow.Display.Text += userguide;
            commandsWindow.Show();
        }
        //******************************************Menu Functionality**************************************
     
        //******************************************Button Functionality**************************************
       
        private void Delete_Click(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem == null)
                MessageBox.Show("Please select the event row to be deleted");
              
            else
            {
                int eventid = datagrid.SelectedIndex + 1;
                string sr = "delete id" + " \"" + eventid.ToString() + "\"";
                Parser objectParser = new Parser(sr);
                dataGrid1.Visibility = Visibility.Collapsed;
                datagrid.Visibility = Visibility.Visible;
                datagrid.ItemsSource = objectParser.taskToBePerformed();
               
                ToastMsg.Text = "Deleted!";
            }
        }
        private void Edit_Click(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem == null)
             ToastMsg.Text = "Please select the event which you want to edit";
            else
            {
                int eventid = datagrid.SelectedIndex;
                edit_event subform = new edit_event(eventid);
                string command;
                subform.ShowDialog();
                command = subform.getCommand();
                Parser objectParser = new Parser(command);
                dataGrid1.Visibility = Visibility.Collapsed;
                datagrid.Visibility = Visibility.Visible;
                datagrid.ItemsSource = objectParser.taskToBePerformed();
                ToastMsg.Text = "Edited!";
                
            }
        }
        private void Done_Click(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem == null)
                MessageBox.Show("Please select the event which you have completed");
            else
            {
                int eventid = datagrid.SelectedIndex + 1;
                string sr = "done id" + " \"" + eventid.ToString() + "\"";
                Parser objectParser = new Parser(sr);
                dataGrid1.Visibility = Visibility.Collapsed;
                datagrid.Visibility = Visibility.Visible;
                datagrid.ItemsSource = objectParser.taskToBePerformed();
             
                ToastMsg.Text = "Done!";
            }
        }
        private void ShowNextView(object sender, MouseButtonEventArgs e)
        {
            dataGrid1.Visibility = Visibility.Collapsed;
            datagrid.Visibility = Visibility.Visible;

        }  

        //******************************************Button Functionality**************************************
        private void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGrid1.Visibility = Visibility.Collapsed;
            datagrid.Visibility = Visibility.Visible;
            datagrid.RowBackground = Brushes.BlanchedAlmond;
        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DateEvent RowDataContext = e.Row.DataContext as DateEvent;
            if (RowDataContext != null)
            {
                if (RowDataContext.EventName != null)
                    e.Row.Background = Brushes.Red; //FindResource("GreenBackgroundBrush") as Brush;
                if((RowDataContext.Time != null))
                    e.Row.Background = Brushes.Yellow;
            }
        }
        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            DateEvent RowDataContaxt = e.Row.DataContext as DateEvent;
            if (RowDataContaxt != null)
            {
                if (RowDataContaxt.EventName != null) { }
                  //  e.Row.Background = Brushes.Red;
            }
        }
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{

        //}

        private void performanceButton_Click(object sender, RoutedEventArgs e)
        {
    //        performanceChart1.Visibility = Visibility.Hidden;
      //     performanceButton.Visibility = Visibility.Hidden;
        //  performanceLabel.Visibility = Visibility.Hidden;
        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            
            AutomatedTesting AutomatedTestingObj = new AutomatedTesting();
            AutomatedTestingObj.StartTesting();
            MessageBox.Show("Automated Testing Done!");
            CompareOutputs();
        }
        private void CompareOutputs()
        {
            StreamReader file1 = new StreamReader("AutomatedTestingOutput.txt");
            Debug.Assert(file1 != null);
            StreamReader file2 = new StreamReader("ExpectedOutput.txt");
            Debug.Assert(file2 != null);
            bool areEqual=true;
            string line1, line2;
            while ((line1 = file1.ReadLine()) != null)
            {
                line2 = file2.ReadLine();
                if (line1 != line2)
                {
                    areEqual = false;
                    break;
                }            
            }
            file1.Close();
            file2.Close();
            if (areEqual)
                MessageBox.Show("All test cases passed!");
            else
                MessageBox.Show("Atleast one test case failed.Check log file for errors!");
        }
        private void Got_Focus(object sender, RoutedEventArgs e)
        {
            test.Visibility = Visibility.Visible;
        }

        private void mouseover(object sender, MouseEventArgs e)
        {
            test.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }    
    }
}

