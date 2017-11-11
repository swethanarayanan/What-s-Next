namespace ToDoList
{
    using System;
    using System.Collections.Generic;

    internal class Parser
    {
        private string commandTextUI;
        private string contentTextUI;
        private int NumberOfContentWords;
        private Operation OperationObject;
        private string sentence;
        private string[] words;

        public Parser(string s)
        {
            this.sentence = s;
            this.NumberOfContentWords = 0;
            this.commandTextUI = null;
            this.contentTextUI = null;
            if (s != null)
            {
                this.ShouldSentenceBeModified();
                this.words = this.sentence.Split(new char[] { ' ' });
                this.checkForWhatCommand();
            }
            else
            {
                this.words = null;
            }
        }

        public void checkForWhatCommand()
        {
            this.countNumberofContentWords();
            this.commandTextUI = this.words[0];
            for (int i = 1; i <= this.NumberOfContentWords; i++)
            {
                this.contentTextUI = this.contentTextUI + " ";
                this.contentTextUI = this.contentTextUI + this.words[i];
            }
        }

        public void CheckIfCommandAndContentValid()
        {
            if (this.commandTextUI.IndexOf('"') >= 0)
            {
                string trimContentWord = null;
                if (this.contentTextUI != null)
                {
                    trimContentWord = this.contentTextUI.Trim();
                }
                if (string.Compare(trimContentWord, "done", true) == 0)
                {
                    string swap = this.commandTextUI;
                    this.commandTextUI = trimContentWord;
                    this.contentTextUI = swap;
                }
                else
                {
                    this.contentTextUI = this.commandTextUI + " " + this.contentTextUI;
                    this.commandTextUI = "add";
                }
            }
        }

        public string CheckIfRemainderNeeded(DateTime s)
        {
            this.OperationObject = new Operation(this.commandTextUI, this.contentTextUI);
            return this.OperationObject.tasksThatNeedReminderToBeShownNow(s);
        }

        private void countNumberofContentWords()
        {
            this.NumberOfContentWords = this.words.Length - 1;
        }

        public string getCommand()
        {
            return this.commandTextUI;
        }

        public string getContent()
        {
            return this.contentTextUI;
        }

        public string getDisplay()
        {
            this.OperationObject = new Operation(this.commandTextUI, this.contentTextUI);
            return this.OperationObject.getDisplayMessage();
        }

        public string HelpUserWriteCommand(string AppendThisWithQuotes)
        {
            if ((this.NumberOfContentWords == 1) && (this.sentence.IndexOf('"') != 0))
            {
                return "\"\"";
            }
            if (this.NumberOfContentWords >= 2)
            {
                string[] splitContentByQuotes = this.contentTextUI.Split(new char[] { '"' });
                string LastStringOfsplitContentByQuotes = splitContentByQuotes[splitContentByQuotes.Length - 1];
                string[] listOfKeyWords = new string[] { " id ", " event ", " from ", " at ", " start ", " end ", " to ", " priority ", " reminder ", " duration ", " repeat " };
                for (int i = 0; i < listOfKeyWords.Length; i++)
                {
                    if (LastStringOfsplitContentByQuotes.CompareTo(listOfKeyWords[i]) == 0)
                    {
                        return "\"\"";
                    }
                }
            }
            return null;
        }

        private void ShouldSentenceBeModified()
        {
        }

        public List<Event> taskToBePerformed()
        {
            this.OperationObject = new Operation(this.commandTextUI, this.contentTextUI);
            List<TaskDetails> tasks = new List<TaskDetails>();
            Conversions objectConversions = new Conversions();
            List<Event> events = new List<Event>();
            tasks = this.OperationObject.decideOntask();
            return objectConversions.convertTaskdetailsIntoEvent(tasks);
        }

        public List<DateEvent> taskToBePerformed(List<Event> events, string _date)
        {
            this.OperationObject = new Operation(this.commandTextUI, this.contentTextUI);
            return this.OperationObject.ListOfTasksForTheDay(events, _date);
        }
    }
}

