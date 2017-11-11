namespace ToDoList
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;

    internal class Operation
    {
        private string commandOperation;
        private string contentOperation;
        private string displayText;
        private string fileToBeDealtWith;
        private string[] listOfKeywords;
        private Database objectDatabase;
        private TaskDetails objectDetailsOfTask;
        private string result;
        private Database snapshots;

        public Operation(string scommand, string scontent)
        {
            this.commandOperation = scommand;
            this.contentOperation = scontent;
            this.objectDetailsOfTask = new TaskDetails();
            this.objectDatabase = new Database("ListOfTasksToDoFile.txt");
            this.result = "Invalid command";
            this.listOfKeywords = new string[] { " id ", " event ", " at ", " start ", " end ", " from ", " to ", " duration ", " priority ", " reminder ", " repeat " };
            this.fileToBeDealtWith = "ListOfTasksToDoFile.txt";
            this.snapshots = new Database("f1.txt");
        }

        private void AddContentToFileThatisOpened()
        {
            string s = this.BreakdownContentIntoAptDetails();
            if (s != null)
            {
                this.objectDatabase.writeIntoFile(s);
                this.SortContentsOfFile();
                this.displayText = "Added!";
            }
        }

        private void addToIntellisenseDatabase(string s)
        {
            string[] listOfIntellisense = s.Split(new char[] { ' ' });
            IntellisenseDatabase objectIntellisenseDatabase = new IntellisenseDatabase();
            for (int i = 0; i < listOfIntellisense.Count<string>(); i++)
            {
                if (listOfIntellisense[i].CompareTo("null") != 0)
                {
                    objectIntellisenseDatabase.addWordToIntellisenseList(listOfIntellisense[i]);
                }
            }
        }

        private string aggregateTaskDetailsParameterInAString(string parameterSeparator, int i)
        {
            string content = null;
            if (i == 1)
            {
                return (this.objectDetailsOfTask.getEventname() + parameterSeparator + this.objectDetailsOfTask.getLocation() + parameterSeparator + this.objectDetailsOfTask.getStartDate() + parameterSeparator + this.objectDetailsOfTask.getEndDate() + parameterSeparator + this.objectDetailsOfTask.getStartTime() + parameterSeparator + this.objectDetailsOfTask.getEndTime() + parameterSeparator + this.objectDetailsOfTask.getDuration() + parameterSeparator + this.objectDetailsOfTask.getPriority() + parameterSeparator + this.objectDetailsOfTask.getRemainderTime());
            }
            if (i == 2)
            {
                content = "ID-" + this.objectDetailsOfTask.getID() + parameterSeparator + "Event Name-" + this.objectDetailsOfTask.getEventname() + parameterSeparator + "Location-" + this.objectDetailsOfTask.getLocation() + parameterSeparator + "Start Date-" + this.objectDetailsOfTask.getStartDate() + parameterSeparator + "End Date-" + this.objectDetailsOfTask.getEndDate() + parameterSeparator + "Start Time-" + this.objectDetailsOfTask.getStartTime() + parameterSeparator + "End Time-" + this.objectDetailsOfTask.getEndTime() + parameterSeparator + "Duration-" + this.objectDetailsOfTask.getDuration() + parameterSeparator + "Priority-" + this.objectDetailsOfTask.getPriority() + parameterSeparator + "Reminder-" + this.objectDetailsOfTask.getRemainderTime() + parameterSeparator;
            }
            return content;
        }

        private void ArchiveTaskDone()
        {
            HistoryDatabase historyObj = new HistoryDatabase();
            string[] words = this.contentOperation.Split(new char[] { '"' })[1].Trim().Split(new char[] { ',' });
            int numberOfTasksToBeDeleted = words.Count<string>();
            int i = 0;
            int LineNumberToBeArchived = Convert.ToInt32(words[i]);
            int line_number = 0;
            string LineThatIsArchived = null;
            string FileName = "Completed.txt";
            using (StreamReader reader = new StreamReader(this.fileToBeDealtWith))
            {
                using (StreamWriter writer = new StreamWriter(FileName, true))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line_number++;
                        if (line_number == LineNumberToBeArchived)
                        {
                            LineThatIsArchived = line;
                            writer.WriteLine(line);
                            i++;
                            if (i < words.Count<string>())
                            {
                                LineNumberToBeArchived = Convert.ToInt32(words[i]);
                            }
                        }
                    }
                }
            }
            this.DeleteContentOfFile();
            this.result = "Task successfully completed and archived!";
        }

        private string BreakdownContentIntoAptDetails()
        {
            string DetailOfEventToBeWrittenIntoFile = null;
            this.contentOperation = " event " + this.contentOperation;
            this.InitialiseTaskDetailParameter();
            Conversions objectConversions = new Conversions();
            this.CheckIfAllEssentialDetailsAreEntered();
            string s = this.CheckIfRepeatIsSet(this.objectDetailsOfTask);
            return (DetailOfEventToBeWrittenIntoFile + objectConversions.convertTaskDetailToWriteIntoFile(this.objectDetailsOfTask) + s);
        }

        private void CheckIfAllEssentialDetailsAreEntered()
        {
            Conversions objectConversions = new Conversions();
            DateTime startDate = objectConversions.ConvertStringToDate(this.objectDetailsOfTask.getStartDate());
            DateTime endDate = objectConversions.ConvertStringToDate(this.objectDetailsOfTask.getEndDate());
            if ((this.objectDetailsOfTask.getEventname() == "null") || (startDate.CompareTo(endDate) > 0))
            {
                this.DisplayErrorMessage();
            }
            else if ((this.objectDetailsOfTask.getEndTime() == "null") && ((this.objectDetailsOfTask.getStartTime() != "null") && (this.objectDetailsOfTask.getDuration() != "null")))
            {
                this.objectDetailsOfTask.CalculateEndTime();
            }
            else if ((this.objectDetailsOfTask.getDuration() == "null") && ((this.objectDetailsOfTask.getEndTime() != "null") && (this.objectDetailsOfTask.getStartTime() != "null")))
            {
                this.objectDetailsOfTask.CalculateDuration();
            }
        }

        private bool checkIfautoschedulingNeeded(TaskDetails objectDetailsOfTask)
        {
            string date;
            Conversions objectConversions = new Conversions();
            List<TaskDetails> tasksAlreadyInFile = this.DisplayContentofFileThatIsOpened();
            List<Event> eventsAlreadyInFile = objectConversions.convertTaskdetailsIntoEvent(tasksAlreadyInFile);
            List<TaskDetails> tasks = new List<TaskDetails> {
                objectDetailsOfTask
            };
            List<Event> s = new List<Event>();
            s = objectConversions.convertTaskdetailsIntoEvent(tasks);
            List<Event> listOfEvents = new List<Event>();
            if (s[0].StartDate == s[0].EndDate)
            {
                date = objectConversions.ConvertStringToDate(s[0].StartDate).Date.ToShortDateString();
                listOfEvents = this.retrieveEventsThatAreRelatedTo_date(date, eventsAlreadyInFile);
            }
            DateTime today = objectConversions.ConvertStringToDate(s[0].StartDate);
            TimeSpan difference = (TimeSpan) (objectConversions.ConvertStringToDate(s[0].EndDate) - today);
            List<DateTime> days = new List<DateTime>();
            int i = 0;
            while (i <= difference.Days)
            {
                days.Add(today.AddDays((double) i));
                i++;
            }
            foreach (DateTime dateTime in days)
            {
                date = dateTime.Date.ToShortDateString();
                listOfEvents = this.retrieveEventsThatAreRelatedTo_date(date, eventsAlreadyInFile);
                TimeSpan startTime = new TimeSpan(0, 0, 0);
                TimeSpan endTime = new TimeSpan(0, 0, 0);
                TimeSpan otherEventStartTime = new TimeSpan(0, 0, 0);
                TimeSpan otherEventEndTime = new TimeSpan(0, 0, 0);
                TimeSpan otherEventDuration = otherEventEndTime - otherEventStartTime;
                TimeSpan duration = endTime - startTime;
                bool isAutoSchedulingNeeded = false;
                bool isMidNight = false;
                bool isStartTimeNull = false;
                bool isEndTimeNull = false;
                for (i = 0; i < listOfEvents.Count<Event>(); i++)
                {
                    if (listOfEvents[i].startTime != " ")
                    {
                        if (listOfEvents[i].startTime == "00:00:00")
                        {
                            isMidNight = true;
                        }
                        otherEventStartTime = TimeSpan.Parse(listOfEvents[i].startTime);
                        if (listOfEvents[i].endTime != " ")
                        {
                            otherEventEndTime = TimeSpan.Parse(listOfEvents[i].endTime);
                        }
                        else
                        {
                            otherEventEndTime = TimeSpan.Parse(listOfEvents[i].startTime);
                        }
                    }
                    else
                    {
                        isStartTimeNull = true;
                        if (listOfEvents[i].endTime != " ")
                        {
                            if (listOfEvents[i].startTime != " ")
                            {
                                otherEventStartTime = TimeSpan.Parse(listOfEvents[i].startTime);
                                otherEventEndTime = TimeSpan.Parse(listOfEvents[i].startTime);
                            }
                            if (listOfEvents[i].endTime == "00:00:00")
                            {
                                isMidNight = true;
                            }
                        }
                        else
                        {
                            isEndTimeNull = true;
                        }
                    }
                    otherEventDuration = otherEventEndTime - otherEventStartTime;
                    if ((objectDetailsOfTask.getStartTime() == "null") && (objectDetailsOfTask.getEndTime() == "null"))
                    {
                        isAutoSchedulingNeeded = false;
                    }
                    else if ((objectDetailsOfTask.getStartTime() != "null") && (objectDetailsOfTask.getEndTime() != "null"))
                    {
                        startTime = TimeSpan.Parse(objectDetailsOfTask.getStartTime());
                        endTime = TimeSpan.Parse(objectDetailsOfTask.getEndTime());
                        if (otherEventDuration.CompareTo(new TimeSpan(0, 0, 0)) == 0)
                        {
                            if (!isMidNight)
                            {
                                isAutoSchedulingNeeded = false;
                            }
                            else if (isStartTimeNull)
                            {
                                if ((otherEventEndTime > startTime) && (otherEventEndTime < endTime))
                                {
                                    isAutoSchedulingNeeded = true;
                                }
                            }
                            else if (isEndTimeNull && (otherEventStartTime == startTime))
                            {
                                isAutoSchedulingNeeded = true;
                            }
                        }
                        else if (((startTime >= otherEventStartTime) && (startTime < otherEventEndTime)) || ((endTime > otherEventStartTime) && (endTime <= otherEventEndTime)))
                        {
                            isAutoSchedulingNeeded = true;
                        }
                    }
                    else if (objectDetailsOfTask.getStartTime() != "null")
                    {
                        startTime = TimeSpan.Parse(objectDetailsOfTask.getStartTime());
                        if (otherEventDuration.CompareTo(new TimeSpan(0, 0, 0)) == 0)
                        {
                            if (!isMidNight)
                            {
                                isAutoSchedulingNeeded = false;
                            }
                            else if (isEndTimeNull && (otherEventStartTime == startTime))
                            {
                                isAutoSchedulingNeeded = true;
                            }
                        }
                        else if (otherEventStartTime < otherEventEndTime)
                        {
                            if ((startTime >= otherEventStartTime) && (startTime < otherEventEndTime))
                            {
                                isAutoSchedulingNeeded = true;
                            }
                        }
                        else
                        {
                            otherEventDuration = otherEventStartTime - otherEventEndTime;
                            TimeSpan CS$0$0004 = startTime - otherEventDuration;
                            if (CS$0$0004.CompareTo(otherEventStartTime) <= 0)
                            {
                                isAutoSchedulingNeeded = true;
                            }
                        }
                    }
                    else if (objectDetailsOfTask.getEndTime() != "null")
                    {
                        endTime = TimeSpan.Parse(objectDetailsOfTask.getEndTime());
                        if (otherEventDuration.CompareTo(new TimeSpan(0, 0, 0)) == 0)
                        {
                            if (!isMidNight)
                            {
                                isAutoSchedulingNeeded = false;
                            }
                            else if (isEndTimeNull && (otherEventStartTime == startTime))
                            {
                                isAutoSchedulingNeeded = true;
                            }
                        }
                        else if ((endTime > otherEventStartTime) && (endTime <= otherEventEndTime))
                        {
                            isAutoSchedulingNeeded = true;
                        }
                    }
                    if (isAutoSchedulingNeeded)
                    {
                        MessageBox.Show("You Need to Re-schedule.Click on " + date + "on the calendar to check the available slots");
                        return false;
                    }
                }
            }
            return true;
        }

        private string CheckIfRepeatIsSet(TaskDetails t)
        {
            Conversions objectConversions = new Conversions();
            TaskDetails task = new TaskDetails();
            string tasksThatAreToBeAdded = null;
            string s_repeat = t.getRepeat();
            string date = t.getStartDate();
            DateTime startDateOfEvent = DateTime.Parse(date);
            int next1Year = startDateOfEvent.Year + 1;
            int yearToBeChecked = startDateOfEvent.Year;
            int count = 1;
            DateTime endDateOfEvent = DateTime.Parse(t.getEndDate());
            int NoOfDays = objectConversions.interpretRepeatAsNumberOfDays(s_repeat, date);
            if (NoOfDays != 0)
            {
                while (yearToBeChecked <= next1Year)
                {
                    task.setEventname(t.getEventname());
                    task.setlocation(t.getLocation());
                    task.setPriority(t.getPriority());
                    task.setRepeat(t.getRepeat());
                    task.setReminder(t.getRemainderTime());
                    task.setStartTime(t.getStartTime());
                    task.setEndTime(t.getEndTime());
                    task.setDuration(t.getDuration());
                    task.setRepeat(t.getRepeat());
                    task.setStartDate(startDateOfEvent.AddDays((double) (count * NoOfDays)).ToShortDateString());
                    task.setEndDate(endDateOfEvent.AddDays((double) (count * NoOfDays)).ToShortDateString());
                    tasksThatAreToBeAdded = tasksThatAreToBeAdded + "\n" + objectConversions.convertTaskDetailToWriteIntoFile(task);
                    count++;
                    yearToBeChecked = DateTime.Parse(task.getStartDate()).Year;
                }
            }
            return tasksThatAreToBeAdded;
        }

        private void ClearAllTheContentFromTheFile()
        {
            this.result = "Display Box Cleared";
        }

        public DateEvent creatDateEventObject(TimeSpan time, string name)
        {
            Conversions objectConversions = new Conversions();
            return new DateEvent { Time = objectConversions.Convert24HourFormatTo12HourFormat(time), EventName = name };
        }

        public List<TaskDetails> decideOntask()
        {
            if (string.Compare(this.commandOperation, "add", true) == 0)
            {
                this.rememberoldfiles();
                this.AddContentToFileThatisOpened();
            }
            else if (string.Compare(this.commandOperation, "delete", true) == 0)
            {
                this.rememberoldfiles();
                this.DeleteContentOfFile();
                this.displayText = "Deleted!";
            }
            else if (string.Compare(this.commandOperation, "search", true) == 0)
            {
                this.SearchFileForTheExactMatch();
                this.displayText = this.result;
            }
            else if (string.Compare(this.commandOperation, "view", true) == 0)
            {
                this.displayText = this.result;
            }
            else if (string.Compare(this.commandOperation, "display", true) == 0)
            {
                if (this.contentOperation == null)
                {
                    this.DisplayContentofFileThatIsOpened();
                }
            }
            else
            {
                if (string.Compare(this.commandOperation, "clear", true) == 0)
                {
                    if (this.contentOperation == null)
                    {
                        this.rememberoldfiles();
                        this.ClearAllTheContentFromTheFile();
                    }
                    return null;
                }
                if (string.Compare(this.commandOperation, "edit", true) == 0)
                {
                    this.rememberoldfiles();
                    this.EditTheContentInTheFile();
                    this.displayText = this.result;
                }
                else if (string.Compare(this.commandOperation, "done", true) == 0)
                {
                    this.rememberoldfiles();
                    this.ArchiveTaskDone();
                    this.displayText = this.result;
                }
                else if (string.Compare(this.commandOperation, "undo", true) == 0)
                {
                    if (this.contentOperation == null)
                    {
                        this.UndoTask();
                    }
                }
                else if (string.Compare(this.commandOperation, "exit", true) == 0)
                {
                    if (this.contentOperation == null)
                    {
                        this.Exit();
                    }
                }
                else if (string.Compare(this.commandOperation, "help", true) == 0)
                {
                    if (this.contentOperation == null)
                    {
                        this.Help();
                    }
                }
                else if (string.Compare(this.commandOperation, "history", true) == 0)
                {
                    if (this.contentOperation == null)
                    {
                        this.ViewHistory();
                    }
                }
                else if (string.Compare(this.commandOperation, "emptySlotsOn", true) == 0)
                {
                }
            }
            return this.DisplayContentofFileThatIsOpened();
        }

        private void DeleteContentOfFile()
        {
            string[] words = this.contentOperation.Split(new char[] { '"' })[1].Trim().Split(new char[] { ',' });
            Array.Sort<string>(words);
            int numberOfTasksToBeDeleted = words.Count<string>();
            int i = 0;
            int LineNumberToBeDeleted = Convert.ToInt32(words[i]);
            int line_number = 0;
            string LineThatIsDeleted = null;
            string tempFileName = "output.txt";
            using (StreamReader reader = new StreamReader(this.fileToBeDealtWith))
            {
                using (StreamWriter writer = new StreamWriter(tempFileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line_number++;
                        if (line_number == LineNumberToBeDeleted)
                        {
                            LineThatIsDeleted = line;
                            i++;
                            if (i < words.Count<string>())
                            {
                                LineNumberToBeDeleted = Convert.ToInt32(words[i]);
                            }
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            if (File.Exists(this.fileToBeDealtWith))
            {
                File.Delete(this.fileToBeDealtWith);
            }
            File.Move(tempFileName, this.fileToBeDealtWith);
            File.Delete(tempFileName);
            this.result = "delete from " + this.fileToBeDealtWith + " :  \"" + LineThatIsDeleted + " \"";
        }

        private List<TaskDetails> DisplayContentofFileThatIsOpened()
        {
            List<TaskDetails> listOfLinesInFile = new List<TaskDetails>();
            return this.objectDatabase.readFromFile();
        }

        private void DisplayErrorMessage()
        {
            MessageBox.Show("Invalid/Insufficient Detail");
        }

        public List<DateEvent> displayTimeLineWithEvents(List<DateEvent> dateEvents, Event events, TimeSpan s1, TimeSpan s2)
        {
            Conversions objectConversions = new Conversions();
            for (int j = 0; j < dateEvents.Count<DateEvent>(); j++)
            {
                int pos;
                TimeSpan s = objectConversions.Convert12HourFormatTo24HourFormat(dateEvents[j].Time);
                if ((s == s1) || ((s > s1) && (s < s2)))
                {
                    dateEvents.RemoveAt(j);
                    pos = j;
                    dateEvents.Insert(pos, this.creatDateEventObject(s, events.EventName));
                }
                else if ((s < s1) && (s.Hours == s1.Hours))
                {
                    pos = ++j;
                    dateEvents.Insert(pos, this.creatDateEventObject(s1, events.EventName));
                }
                if (((s1 != s2) && (s2 != s)) && (s2.Hours == s.Hours))
                {
                    pos = j + 1;
                    dateEvents.Insert(pos, this.creatDateEventObject(s2, null));
                    j = dateEvents.Count<DateEvent>();
                }
            }
            return dateEvents;
        }

        private void EditTheContentInTheFile()
        {
            List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
            Conversions objectConversions = new Conversions();
            listOfTasksInFile = this.objectDatabase.readFromFile();
            string[] words = this.contentOperation.Split(new char[] { '"' });
            string[] listOfIDsToBeEdited = this.retrieveIDsofARepeatingEvent(Convert.ToInt32(words[1]));
            int k = 0;
            int LineNumberToBeUpdated = Convert.ToInt32(listOfIDsToBeEdited[k]);
            int line_number = 0;
            double totalDays = 0.0;
            bool totalDaysComputed = false;
            string tempFileName = "temp.txt";
            using (StreamReader reader = new StreamReader(this.fileToBeDealtWith))
            {
                using (StreamWriter writer = new StreamWriter(tempFileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if ((line_number + 1) == LineNumberToBeUpdated)
                        {
                            int i = 2;
                            while (i <= (words.Count<string>() - 2))
                            {
                                string[] parts = line.Split(new char[] { ';' });
                                if (words[i].Trim() == "event")
                                {
                                    listOfTasksInFile[line_number].setEventname(words[i + 1]);
                                }
                                else if (words[i].Trim() == "at")
                                {
                                    listOfTasksInFile[line_number].setlocation(words[i + 1]);
                                }
                                else if (words[i].Trim() == "start")
                                {
                                    DateTime sdate = DateTime.Parse(listOfTasksInFile[line_number].getStartDate());
                                    string endDate = listOfTasksInFile[line_number].getEndDate();
                                    string s = words[i + 1];
                                    DateTime updatedDate = DateTime.Parse(s);
                                    if (!totalDaysComputed)
                                    {
                                        TimeSpan CS$0$0002 = (TimeSpan) (updatedDate - sdate);
                                        totalDays = CS$0$0002.TotalDays;
                                        totalDaysComputed = true;
                                    }
                                    sdate = sdate.AddDays(totalDays);
                                    listOfTasksInFile[line_number].setStartDate(sdate.ToShortDateString());
                                    string startDate = listOfTasksInFile[line_number].getStartDate();
                                    if (startDate.CompareTo(endDate) > 0)
                                    {
                                        listOfTasksInFile[line_number].setEndDate(startDate);
                                    }
                                }
                                else if (words[i].Trim() == "end")
                                {
                                    listOfTasksInFile[line_number].setEndDate(words[i + 1]);
                                }
                                else if (words[i].Trim() == "from")
                                {
                                    listOfTasksInFile[line_number].setStartTime(words[i + 1]);
                                }
                                else if (words[i].Trim() == "to")
                                {
                                    listOfTasksInFile[line_number].setEndTime(words[i + 1]);
                                }
                                else if (words[i].Trim() == "duration")
                                {
                                    listOfTasksInFile[line_number].setDuration(words[i + 1]);
                                }
                                else if (words[i].Trim() == "priority")
                                {
                                    listOfTasksInFile[line_number].setPriority(words[i + 1]);
                                }
                                else if (words[i].Trim() == "reminder")
                                {
                                    listOfTasksInFile[line_number].setReminder(words[i + 1]);
                                }
                                i += 2;
                                k++;
                                if (k < listOfIDsToBeEdited.Count<string>())
                                {
                                    LineNumberToBeUpdated = Convert.ToInt32(listOfIDsToBeEdited[k]);
                                }
                            }
                            writer.WriteLine(objectConversions.convertTaskDetailToWriteIntoFile(listOfTasksInFile[line_number]));
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                        line_number++;
                    }
                }
            }
            this.SortContentsOfFile();
            if (File.Exists(this.fileToBeDealtWith))
            {
                File.Delete(this.fileToBeDealtWith);
            }
            File.Move(tempFileName, this.fileToBeDealtWith);
            File.Delete(tempFileName);
            this.result = "File updated!";
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        public string getDisplayMessage()
        {
            return this.displayText;
        }

        private void Help()
        {
        }

        private void InitialiseTaskDetailParameter()
        {
            Conversions objectConversions = new Conversions();
            string[] splitContentByQuotes = this.contentOperation.Split(new char[] { '"' });
            for (int i = 1; i < this.listOfKeywords.Count<string>(); i++)
            {
                for (int j = 0; j < splitContentByQuotes.Length; j += 2)
                {
                    if (splitContentByQuotes[j].Trim().CompareTo(this.listOfKeywords[i].Trim()) == 0)
                    {
                        if ((splitContentByQuotes[j + 1] == "today") && ((i == 3) || (i == 4)))
                        {
                            splitContentByQuotes[j + 1] = DateTime.Today.ToShortDateString();
                        }
                        switch (i)
                        {
                            case 1:
                                this.objectDetailsOfTask.setEventname(splitContentByQuotes[j + 1]);
                                break;

                            case 2:
                                this.objectDetailsOfTask.setlocation(splitContentByQuotes[j + 1]);
                                break;

                            case 3:
                                this.objectDetailsOfTask.setStartDate(splitContentByQuotes[j + 1]);
                                break;

                            case 4:
                                this.objectDetailsOfTask.setEndDate(splitContentByQuotes[j + 1]);
                                break;

                            case 5:
                                this.objectDetailsOfTask.setStartTime(splitContentByQuotes[j + 1]);
                                break;

                            case 6:
                                this.objectDetailsOfTask.setEndTime(splitContentByQuotes[j + 1]);
                                break;

                            case 7:
                                this.objectDetailsOfTask.setDuration(splitContentByQuotes[j + 1]);
                                break;

                            case 8:
                                this.objectDetailsOfTask.setPriority(splitContentByQuotes[j + 1]);
                                break;

                            case 9:
                                this.objectDetailsOfTask.setReminder(splitContentByQuotes[j + 1]);
                                break;

                            case 10:
                                this.objectDetailsOfTask.setRepeat(splitContentByQuotes[j + 1]);
                                break;
                        }
                    }
                }
            }
        }

        public string ListOfEventsForTheDay(List<Event> listOfEvents, string _date)
        {
            List<Event> events = this.retrieveEventsThatAreRelatedTo_date(_date, listOfEvents);
            string eventsForDisplay = null;
            int j = 0;
            for (int i = 0; i < events.Count<Event>(); i++)
            {
                string CS$0$0002;
                string[] CS$0$0003;
                int CS$0$0004;
                Conversions objectConversions = new Conversions();
                bool areBothTimeParametersNull = (events[i].startTime == " ") && (events[i].endTime == " ");
                bool isEndTimeParameterNull = (events[i].startTime != " ") && (events[i].endTime == " ");
                bool isStartTimeParameterNull = (events[i].startTime == " ") && (events[i].endTime != " ");
                if (areBothTimeParametersNull)
                {
                    CS$0$0002 = eventsForDisplay;
                    CS$0$0003 = new string[8];
                    CS$0$0003[0] = CS$0$0002;
                    CS$0$0004 = j + 1;
                    CS$0$0003[1] = CS$0$0004.ToString();
                    CS$0$0003[2] = ". ";
                    CS$0$0003[3] = events[i].EventName;
                    CS$0$0003[4] = " ,starts on";
                    CS$0$0003[5] = events[i].StartDate;
                    CS$0$0003[6] = " and ends on ";
                    CS$0$0003[7] = events[i].EndDate;
                    eventsForDisplay = string.Concat(CS$0$0003);
                    j++;
                }
                else if (isEndTimeParameterNull)
                {
                    CS$0$0002 = eventsForDisplay;
                    CS$0$0003 = new string[10];
                    CS$0$0003[0] = CS$0$0002;
                    CS$0$0004 = j + 1;
                    CS$0$0003[1] = CS$0$0004.ToString();
                    CS$0$0003[2] = ". ";
                    CS$0$0003[3] = events[i].EventName;
                    CS$0$0003[4] = " ,starts on ";
                    CS$0$0003[5] = events[i].StartDate;
                    CS$0$0003[6] = " at ";
                    CS$0$0003[7] = objectConversions.Convert24HourFormatTo12HourFormat(TimeSpan.Parse(events[i].startTime));
                    CS$0$0003[8] = " and ends on ";
                    CS$0$0003[9] = events[i].EndDate;
                    eventsForDisplay = string.Concat(CS$0$0003);
                    j++;
                }
                else if (isStartTimeParameterNull)
                {
                    CS$0$0002 = eventsForDisplay;
                    CS$0$0003 = new string[] { CS$0$0002, (j + 1).ToString(), ". ", events[i].EventName, " ,starts on ", events[i].StartDate, " and ends on ", events[i].EndDate, " at ", objectConversions.Convert24HourFormatTo12HourFormat(TimeSpan.Parse(events[i].endTime)) };
                    eventsForDisplay = string.Concat(CS$0$0003);
                    j++;
                }
                eventsForDisplay = eventsForDisplay + "\n";
            }
            return eventsForDisplay;
        }

        public List<DateEvent> ListOfTasksForTheDay(List<Event> listOfEvents, string _date)
        {
            _date = _date.Replace('"', ' ').Trim();
            List<DateEvent> dateEvents = new List<DateEvent>();
            dateEvents = this.valuesToInitialiseDataGrid2();
            Conversions objectConversions = new Conversions();
            List<Event> events = this.retrieveEventsThatAreRelatedTo_date(_date, listOfEvents);
            string end_Time = null;
            TimeSpan s2 = new TimeSpan();
            TimeSpan s1 = new TimeSpan();
            for (int i = 0; i < events.Count<Event>(); i++)
            {
                string start_Time = events[i].startTime;
                if (start_Time != " ")
                {
                    s1 = objectConversions.Convert12HourFormatTo24HourFormat(start_Time);
                    if (events[i].endTime != " ")
                    {
                        end_Time = events[i].endTime;
                        s2 = objectConversions.Convert12HourFormatTo24HourFormat(end_Time);
                        dateEvents = this.displayTimeLineWithEvents(dateEvents, events[i], s1, s2);
                    }
                }
            }
            string eventsList = this.ListOfEventsForTheDay(listOfEvents, _date);
            DateEvent <>g__initLocal1 = new DateEvent {
                Time = null,
                EventName = eventsList
            };
            DateEvent dateEventObj = <>g__initLocal1;
            dateEvents.Add(dateEventObj);
            this.result = "events on" + _date;
            this.displayText = this.result;
            return dateEvents;
        }

        private void rememberoldfiles()
        {
            List<TaskDetails> listOfTasks = this.objectDatabase.readFromFile();
            this.snapshots.writeIntoFile("\n*******\n");
            string DetailOfEventToBeWrittenIntoFile = null;
            for (int i = 0; i < listOfTasks.Count<TaskDetails>(); i++)
            {
                DetailOfEventToBeWrittenIntoFile = listOfTasks[i].getEventname() + ";" + listOfTasks[i].getLocation() + ";" + listOfTasks[i].getStartDate() + ";" + listOfTasks[i].getEndDate() + ";" + listOfTasks[i].getStartTime() + ";" + listOfTasks[i].getEndTime() + ";" + listOfTasks[i].getDuration() + ";" + listOfTasks[i].getPriority() + ";" + listOfTasks[i].getRemainderTime();
                this.snapshots.writeIntoFile(DetailOfEventToBeWrittenIntoFile);
            }
        }

        private List<Event> retrieveEventsThatAreRelatedTo_date(string _date, List<Event> events)
        {
            Conversions objectConversions = new Conversions();
            for (int i = 0; i < events.Count<Event>(); i++)
            {
                string startDate = events[i].StartDate;
                string endDate = events[i].EndDate;
                bool startDateEqualsEndDate = (startDate.CompareTo(_date) == 0) && (endDate.CompareTo(_date) == 0);
                bool _dateBetweenStartDateAndEndDate = ((startDate.CompareTo(endDate) != 0) && (endDate.CompareTo(_date) >= 0)) && (startDate.CompareTo(_date) <= 0);
                bool c = endDate.CompareTo(_date) >= 0;
                bool d = startDate.CompareTo(_date) <= 0;
                if (!(startDateEqualsEndDate || _dateBetweenStartDateAndEndDate))
                {
                    events.RemoveAt(i);
                    i--;
                }
            }
            return events;
        }

        private string[] retrieveIDsofARepeatingEvent(int id)
        {
            List<TaskDetails> tasks = new List<TaskDetails>();
            List<string> listOfIDs = new List<string>();
            tasks = this.DisplayContentofFileThatIsOpened();
            int i = id;
            int j = 0;
            string startDate = tasks[id - 1].getStartDate();
            string eventName = tasks[id - 1].getEventname();
            string startTime = tasks[id - 1].getStartTime();
            string endTime = tasks[id - 1].getEndTime();
            listOfIDs.Insert(j, id.ToString());
            while (i < tasks.Count<TaskDetails>())
            {
                if ((((eventName == tasks[i].getEventname()) && (startTime == tasks[i].getStartTime())) && (endTime == tasks[i].getEndTime())) && (tasks[i].getStartDate().CompareTo(startDate) > 0))
                {
                    j++;
                    listOfIDs.Insert(j, (i + 1).ToString());
                }
                i++;
            }
            return listOfIDs.ToArray();
        }

        private void SearchFileForTheExactMatch()
        {
            List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
            listOfTasksInFile = this.objectDatabase.readFromFile();
            if (listOfTasksInFile.Count<TaskDetails>() == 0)
            {
                this.result = "File is empty.Therefore no match found";
            }
            else
            {
                this.result = null;
                for (int i = 0; i < listOfTasksInFile.Count<TaskDetails>(); i++)
                {
                    int j = i + 1;
                    for (int k = 1; k <= 10; k++)
                    {
                        string searchPattern = this.contentOperation.Replace('"', ' ').Trim();
                        if (Regex.IsMatch(listOfTasksInFile[i].getWord(k).Trim(), searchPattern, RegexOptions.IgnoreCase))
                        {
                            this.objectDetailsOfTask = listOfTasksInFile[i];
                            this.result = this.result + this.aggregateTaskDetailsParameterInAString("\n", 2) + Environment.NewLine;
                            break;
                        }
                    }
                }
            }
        }

        private void SortContentsOfFile()
        {
            int i;
            TaskDetails TemporaryTaskObj;
            List<TaskDetails> ListOfTaskDetails = new List<TaskDetails>();
            TaskDetails TaskDetailsObj = new TaskDetails();
            ListOfTaskDetails = this.DisplayContentofFileThatIsOpened();
            Conversions DateConversionObj = new Conversions();
            int[] DaysRemainingForEventEnd = new int[ListOfTaskDetails.Count];
            int[] DaysRemainingForEventStart = new int[ListOfTaskDetails.Count];
            int[] Priority = new int[ListOfTaskDetails.Count];
            int j = 0;
            while (j < ListOfTaskDetails.Count)
            {
                string StartDateInStringFormat = ListOfTaskDetails[j].getStartDate();
                TimeSpan DifferenceFromStartDate = (TimeSpan) (DateTime.Now - new Conversions().ConvertStringToDate(StartDateInStringFormat));
                DaysRemainingForEventStart[j] = Math.Abs(DifferenceFromStartDate.Days);
                if (DateTime.Now > new Conversions().ConvertStringToDate(StartDateInStringFormat))
                {
                    DaysRemainingForEventStart[j] = -1 * DifferenceFromStartDate.Days;
                }
                string priorityOfTask = ListOfTaskDetails[j].getPriority();
                if (priorityOfTask.Equals("high"))
                {
                    Priority[j] = 2;
                }
                else if (priorityOfTask.Equals("medium"))
                {
                    Priority[j] = 1;
                }
                else
                {
                    Priority[j] = 0;
                }
                j++;
            }
            for (i = 0; i < ListOfTaskDetails.Count; i++)
            {
                j = 0;
                while (j < ((ListOfTaskDetails.Count - i) - 1))
                {
                    if (DaysRemainingForEventStart[j] > DaysRemainingForEventStart[j + 1])
                    {
                        TemporaryTaskObj = ListOfTaskDetails[j];
                        ListOfTaskDetails[j] = ListOfTaskDetails[j + 1];
                        ListOfTaskDetails[j + 1] = TemporaryTaskObj;
                        int TemporaryDate = DaysRemainingForEventStart[j];
                        DaysRemainingForEventStart[j] = DaysRemainingForEventStart[j + 1];
                        DaysRemainingForEventStart[j + 1] = TemporaryDate;
                    }
                    j++;
                }
            }
            for (i = 0; i < ListOfTaskDetails.Count; i++)
            {
                for (j = 0; j < ((ListOfTaskDetails.Count - i) - 1); j++)
                {
                    if ((DaysRemainingForEventStart[j] == DaysRemainingForEventStart[j + 1]) && (Priority[j] < Priority[j + 1]))
                    {
                        TemporaryTaskObj = ListOfTaskDetails[j];
                        ListOfTaskDetails[j] = ListOfTaskDetails[j + 1];
                        ListOfTaskDetails[j + 1] = TemporaryTaskObj;
                        int TemporaryPriority = Priority[j];
                        Priority[j] = Priority[j + 1];
                        Priority[j + 1] = TemporaryPriority;
                    }
                }
            }
            int counter = 0;
            Conversions objectConversions = new Conversions();
            StreamWriter writer = new StreamWriter("ListOfTasksToDoFile.txt", false);
            while (counter < ListOfTaskDetails.Count)
            {
                string TaskDetailsInStringFormat = objectConversions.convertTaskDetailToWriteIntoFile(ListOfTaskDetails[counter]);
                writer.WriteLine(TaskDetailsInStringFormat);
                counter++;
            }
            writer.Close();
        }

        public string tasksThatNeedReminderToBeShownNow(DateTime s)
        {
            int i;
            List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
            listOfTasksInFile = this.DisplayContentofFileThatIsOpened();
            string listOfTasksThatNeedRemainder = null;
            Conversions objectConversions = new Conversions();
            TimeSpan t = s.TimeOfDay;
            bool wasReminderNeeded = false;
            for (i = 0; i < listOfTasksInFile.Count<TaskDetails>(); i++)
            {
                string remainder = listOfTasksInFile[i].getRemainderTime();
                string starttime = listOfTasksInFile[i].getStartTime();
                string startdate = listOfTasksInFile[i].getStartDate();
                string enddate = listOfTasksInFile[i].getEndDate();
                string endtime = listOfTasksInFile[i].getEndTime();
                if (((objectConversions.ConvertStringToDate(startdate) == DateTime.Today) || (objectConversions.ConvertStringToDate(enddate) == DateTime.Today)) && (listOfTasksInFile[i].getRemainderTime() != "0H"))
                {
                    TimeSpan remainderTime = new TimeSpan();
                    TimeSpan timeLeft = new TimeSpan();
                    if (endtime.CompareTo("null") != 0)
                    {
                        remainderTime = TimeSpan.Parse(endtime) - objectConversions.durationOrRemainderToTotalHours(remainder);
                        timeLeft = TimeSpan.Parse(endtime) - t;
                    }
                    if (starttime.CompareTo("null") != 0)
                    {
                        remainderTime = TimeSpan.Parse(starttime) - objectConversions.durationOrRemainderToTotalHours(remainder);
                        timeLeft = TimeSpan.Parse(starttime) - t;
                    }
                    if (remainderTime.CompareTo(t) <= 0)
                    {
                        string CS$0$0002 = listOfTasksThatNeedRemainder;
                        listOfTasksThatNeedRemainder = CS$0$0002 + listOfTasksInFile[i].getEventname() + ", just " + timeLeft.ToString(@"hh\:mm") + " left" + Environment.NewLine;
                        listOfTasksInFile[i].setReminder("0H");
                        wasReminderNeeded = true;
                    }
                }
            }
            if (wasReminderNeeded)
            {
                this.objectDatabase.ClearAllTheContentFromTheFile();
                for (i = 0; i < listOfTasksInFile.Count<TaskDetails>(); i++)
                {
                    this.objectDatabase.writeIntoFile(listOfTasksInFile[i].getEventname() + ";" + listOfTasksInFile[i].getLocation() + ";" + listOfTasksInFile[i].getStartDate() + ";" + listOfTasksInFile[i].getEndDate() + ";" + listOfTasksInFile[i].getStartTime() + ";" + listOfTasksInFile[i].getEndTime() + ";" + listOfTasksInFile[i].getDuration() + ";" + listOfTasksInFile[i].getPriority() + ";" + listOfTasksInFile[i].getRemainderTime());
                }
            }
            return listOfTasksThatNeedRemainder;
        }

        private void UndoTask()
        {
            string line;
            StreamReader reader;
            StreamWriter writer;
            List<TaskDetails> undoContent = this.snapshots.readFromFile();
            this.objectDatabase.ClearAllTheContentFromTheFile();
            string DetailOfEventToBeWrittenIntoFile = null;
            for (int i = 0; i < undoContent.Count<TaskDetails>(); i++)
            {
                DetailOfEventToBeWrittenIntoFile = undoContent[i].getEventname() + ";" + undoContent[i].getLocation() + ";" + undoContent[i].getStartDate() + ";" + undoContent[i].getEndDate() + ";" + undoContent[i].getStartTime() + ";" + undoContent[i].getEndTime() + ";" + undoContent[i].getDuration() + ";" + undoContent[i].getPriority() + ";" + undoContent[i].getRemainderTime();
                this.objectDatabase.writeIntoFile(DetailOfEventToBeWrittenIntoFile);
            }
            string tempFileName = "output.txt";
            int totalNoOfLinesInFile = 0;
            using (reader = new StreamReader("f1.txt"))
            {
                using (writer = new StreamWriter(tempFileName))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        totalNoOfLinesInFile++;
                        writer.WriteLine(line);
                    }
                }
            }
            if (totalNoOfLinesInFile == 0)
            {
                this.displayText = "Cannot Undo!";
            }
            else
            {
                int LineNumberToBeDeletedFrom = (totalNoOfLinesInFile - undoContent.Count<TaskDetails>()) - 3;
                int line_number = 0;
                string LineThatIsDeleted = null;
                string snapshotDividingString = "*******";
                bool snapshotDividerEncountered = false;
                using (reader = new StreamReader("f1.txt"))
                {
                    using (writer = new StreamWriter(tempFileName))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            line_number++;
                            if (line_number > LineNumberToBeDeletedFrom)
                            {
                                LineThatIsDeleted = line;
                            }
                            else
                            {
                                if (line.Contains("*******") || (line.CompareTo("") == 0))
                                {
                                    snapshotDividerEncountered = true;
                                    continue;
                                }
                                if (snapshotDividerEncountered)
                                {
                                    snapshotDividerEncountered = false;
                                    writer.WriteLine(snapshotDividingString);
                                }
                                writer.WriteLine(line);
                            }
                        }
                    }
                }
                if (File.Exists("f1.txt"))
                {
                    File.Delete("f1.txt");
                }
                File.Move(tempFileName, "f1.txt");
                File.Delete(tempFileName);
                this.displayText = "Previous Action Retrieved!";
            }
        }

        public List<DateEvent> valuesToInitialiseDataGrid2()
        {
            Conversions objectConversions = new Conversions();
            List<DateEvent> dateEvents = new List<DateEvent>();
            for (int k = 0; k <= 0x18; k++)
            {
                TimeSpan timeLine = new TimeSpan(k, 0, 0);
                dateEvents.Add(this.creatDateEventObject(timeLine, null));
            }
            return dateEvents;
        }

        private void ViewHistory()
        {
        }
    }
}

