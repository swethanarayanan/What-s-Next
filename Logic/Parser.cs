/*********************************************************************************************************************************
 * Author : Swetha Narayanan
 * -----------------------------------Descripton---------------------------------------------------------------------------------- 
 * Parses the input/command entered by user 
 * The command word is extracted 
 * The validity of the command and the parameter following the command word is checked
 * TaskToBePerformed() of Operation Class is called to implement the functionality corresponding to the extracted command word
 *********************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ToDoList
{
    class Parser
    {
        private string sentence;
        private string commandTextUI; //stores command entered by user 
        private string contentTextUI; //stores the text following the command word
        private string[] words; //stores the words entered in the command box
        private int NumberOfContentWords; // holds the number of words in contentTextUI
        Operation OperationObject;

        //string s has the entire text entered by the user in the command box
        public Parser(string s)
        {
            sentence = s;
            NumberOfContentWords = 0;
            commandTextUI = null;
            contentTextUI = null;
            if (s != null)
            {
                words = sentence.Split(' ');//splits sentence with space as a delimiter 
                checkForWhatCommand();
            }
            else
            {
                words = null;
            }
        } 
        private void countNumberofContentWords()
        {
            NumberOfContentWords = words.Length - 1;
        }
        //Checks for Syntax
        public string SyntaxOfTextboxText(string s)
        {
            try
            {
                string[] OneWordCommands = { "exit", "help", "undo", "performance", "history", "redo", "clear" };
                for (int i = 0; i < OneWordCommands.Count(); i++)
                {
                    if (string.Compare(commandTextUI, OneWordCommands[i], true) == 0)
                    {
                        contentTextUI = null;
                        return s;
                    }
                }
                string ERROR_MESSAGE = "Please ensure that all parameters have been entered within quotes!";
                int count = s.Trim().Length - s.Trim().Replace("\"", "").Length - s.Trim().Replace(" ", "").Length;
                if (count == 0)
                {
                    return ERROR_MESSAGE;
                }
                count = s.Length - s.Replace("\"", "").Length;
                if (count % 2 == 1)
                {
                    return ERROR_MESSAGE;
                }
                else if (count == 2)
                {
                    for (int i = 0; i < s.Length - 1; i++)
                    {
                        if (s[i] == '\"' && s[i + 1] == '\"')
                            return ERROR_MESSAGE;
                    }
                }
                return s;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
        //initialse commandTextUI and  contentTextUI with appropriate values
        //Therefore extracts the command word!
        public void checkForWhatCommand()
        {
            try
            {
                countNumberofContentWords();
                bool ConsecutiveQuotesFound = false;
                commandTextUI = words[0];
                for (int i = 0; i < commandTextUI.Length - 1; i++)
                {
                    if (commandTextUI[i] == '\"' && commandTextUI[i + 1] == '\"')
                    {
                        commandTextUI = commandTextUI.Replace("\"", "");
                        ConsecutiveQuotesFound = true;
                        break;
                    }
                }
                for (int i = 1; i <= NumberOfContentWords; i++)
                {
                    contentTextUI += " ";
                    contentTextUI += words[i];
                }
                if (contentTextUI == null && ConsecutiveQuotesFound)
                {
                    contentTextUI += " ";
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
            }
        }
        public void CheckIfCommandAndContentValid()
        {
            try
            {
                if (commandTextUI.IndexOf('\"') >= 0)
                {
                    string trimContentWord = null;
                    if (contentTextUI != null)
                    {
                        trimContentWord = contentTextUI.Trim();
                    }
                    if (String.Compare(trimContentWord, "done", true) == 0)
                    {
                        string swap = commandTextUI;
                        commandTextUI = trimContentWord;
                        contentTextUI = swap;
                    }
                    else
                    {
                        contentTextUI = commandTextUI + " " + contentTextUI;
                        commandTextUI = "add";
                    }
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
            }
        }
        //Adds quotes after every keyword and the user is expected to enter the event details inside these quotes
        public string HelpUserWriteCommand()
        {
            try
            {
                if ((NumberOfContentWords == 1 && sentence.IndexOf('\"') != 0))// && contentTextUI.Trim().CompareTo("add") == 0)
                {
                    return "\"\"";
                }
                else if (NumberOfContentWords >= 2)
                {
                    string[] splitContentByQuotes = contentTextUI.Split('\"');
                    string LastStringOfsplitContentByQuotes = splitContentByQuotes[splitContentByQuotes.Length - 1];
                    string[] listOfKeyWords = { " id ", " event ", " from ", " at ", " start ", " end ", " to ", " priority ", " reminder ", " duration ", " repeat ", " eRepeat ", "add" };
                    for (int i = 0; i < listOfKeyWords.Length; i++)
                    {
                        if (String.Compare(LastStringOfsplitContentByQuotes, listOfKeyWords[i], true) == 0)
                        {
                            {
                                return "\"\"";
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
        //calls the function of class Operation to decide what action is to be performed :)
        public List<Event> taskToBePerformed()
        {
            try
            {
                OperationObject = new Operation(commandTextUI, contentTextUI);
                if (String.Compare(commandTextUI, "performance") == 0)
                {
                    OperationObject.decideOntask();
                    
                }

                else
                {
                    List<TaskDetails> tasks = new List<TaskDetails>();

                    Conversions objectConversions = new Conversions();
                    List<Event> events = new List<Event>();
                    tasks = OperationObject.decideOntask();
                    events = objectConversions.convertTaskdetailsIntoEvent(tasks);
                    return events;
                }
                return null;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
        //Events for the day
        public List<DateEvent> taskToBePerformed(List<Event> events,string _date)
        {
            //list of all events is retrieved
            //OperationObject = new Operation(commandTextUI, contentTextUI);
            return OperationObject.ListOfTasksForTheDay(events, _date);
        }
        public string getCommand()
        {
            return commandTextUI;
        }
        public string getContent()
        {
            return contentTextUI;
        }
        public string getDisplay()
        {
          //  OperationObject = new Operation(commandTextUI, contentTextUI); 
            return OperationObject.getDisplayMessage();
        }
        public string CheckIfRemainderNeeded(DateTime s)
        {
            OperationObject = new Operation(commandTextUI, contentTextUI);
            string s1 = OperationObject.tasksThatNeedReminderToBeShownNow(s);
            return s1;
        }
    }
}
