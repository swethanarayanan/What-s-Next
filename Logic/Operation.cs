/*********************************************************************************************************************************************
 * Author : Monika  Puhazhendhi,Nivetha Sathyarajan,Swetha Narayanan,Venkatesh Sridharan
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * This class extensively deals with the logic behind implementation of every functionality like,
 * (a)adding a new task with/without repeat feature
 * (b)editing a task
 * (c)deleting a task
 * (d)undoing/redoing the action
 * (e)search for a task by date,or event name,etc
 * (f)view the list of tasks on a particular date in timeline format
 * (g)reminder for an event
 * (h)check if a task needs to be re-scheduled or not
 * (e)updates the intellisense database with a nnew word
 * (f)Archiving a Task as completed
 * (g)View list of completed tasks
 * decideOntask() : crucial function in Operation that returns the appropriate list of tasks to the Parser Class
 * It maps the command entered by the user with the corresponding function defined in this class
 * Eg : if the command word entered is add , it calls AddContentToFileThatisOpened(), to add the task into the storage file 
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;


namespace ToDoList
{
    class Operation
    {

        private string commandOperation;
        private string contentOperation;
        private string result;
        private string displayText;
        private string fileToBeDealtWith;
        string[] listOfKeywords;
        Database objectDatabase;
        Database snapshots;
        Database snapshotsForRedo;
        TaskDetails objectDetailsOfTask;
        //construcor of class 
        public Operation(string scommand, string scontent)
        {
            commandOperation = scommand;
            contentOperation = scontent;
            objectDetailsOfTask = new TaskDetails();
            objectDatabase = new Database("ListOfTasksToDoFile.txt");
            result = "Invalid command";
            listOfKeywords = new string[] { " id "," event ", " at ",
                                            " start ", " end "," from ", " to ", " duration "," priority ", " reminder " ," repeat ", " eRepeat "};
            fileToBeDealtWith = "ListOfTasksToDoFile.txt";
            snapshots = new Database("undo.txt");
            snapshotsForRedo = new Database("redo.txt");
        }

        //-----------------------------------code for timeline display-----------------------------

        public DateEvent creatDateEventObject(TimeSpan time, string name)
        {
            try
            {
                Conversions objectConversions = new Conversions();
                DateEvent dateEventObj = new DateEvent()
                {
                    Time = objectConversions.Convert24HourFormatTo12HourFormat(time),
                    EventName = name
                };
                return dateEventObj;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }

        public List<DateEvent> valuesToInitialiseDataGrid2()
        {
            try
            {
                Conversions objectConversions = new Conversions();
                List<DateEvent> dateEvents = new List<DateEvent>();
                for (int k = 0; k <= 24; k++)
                {
                    TimeSpan timeLine = new TimeSpan(k, 0, 0);
                    dateEvents.Add(creatDateEventObject(timeLine, null));
                }
                return dateEvents;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }
        private List<Event> retrieveEventsThatAreRelatedTo_date(string _date, List<Event> events)
        {
            try
            {
                if (_date.IndexOf('\"') != -1)
                {
                    _date = _date.Replace('\"', ' ').Trim();
                }
                ErrorHandling objectErrorHandling = new ErrorHandling();
                _date = objectErrorHandling.CheckDate(_date);
                if (!(_date.IndexOf('/') != -1))
                {
                    return null;
                }

                Conversions objectConversions = new Conversions();
                List<TaskDetails> tasks = new List<TaskDetails>();
                for (int i = 0; i < events.Count(); i++)
                {
                    tasks.Add(objectConversions.convertEventsIntoTaskdetails(events[i]));
                }
                for (int i = 0; i < tasks.Count(); i++)
                {
                    //if the event has the same start and end date as _date 
                    string startDate = tasks[i].getStartDate();
                    string endDate = tasks[i].getEndDate();
                    DateTime sDate = objectConversions.ConvertStringToDate(startDate);// DateTime.Parse(startDate);
                    DateTime eDate = objectConversions.ConvertStringToDate(endDate);//DateTime.Parse(endDate);
                    DateTime date = objectConversions.ConvertStringToDate(_date);//DateTime.Parse(_date);
                    bool startDateEqualsEndDate = ((sDate).CompareTo(date) == 0 && (eDate).CompareTo(date) == 0);
                    bool _dateBetweenStartDateAndEndDate = ((sDate).CompareTo(eDate) != 0 && (eDate).CompareTo(date) >= 0 && (sDate.CompareTo(date) <= 0));
                    bool c = (endDate).CompareTo(_date) >= 0;
                    bool d = (startDate.CompareTo(_date) <= 0);
                    if (!(startDateEqualsEndDate || _dateBetweenStartDateAndEndDate))
                    {
                        events.RemoveAt(i);
                        tasks.RemoveAt(i);
                        --i;
                    }
                    if (_dateBetweenStartDateAndEndDate)
                    {
                        if ((eDate).CompareTo(date) == 0)
                        {
                            events[i].Start = _date + ",00:00:00";
                        }
                        if ((sDate).CompareTo(date) == 0)
                        {
                            events[i].End = _date + ",00:00:00";
                        }
                    }
                }

                return events;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }

        }
        public List<DateEvent> displayTimeLineWithEvents(List<DateEvent> dateEvents, Event events, TimeSpan s1, TimeSpan s2)
        {
            try
            {
                int pos;
                Conversions objectConversions = new Conversions();

                for (int j = 0; j < dateEvents.Count(); j++)
                {
                    TimeSpan s = objectConversions.Convert12HourFormatTo24HourFormat(dateEvents[j].Time);
                    string tempEventName = null;
                    string tempEventName1 = null;
                    string tempEventName2 = null;
                    string[] temp = null;
                    string[] temp1 = null;
                    if (dateEvents[j].EventName != null)
                    {
                        tempEventName = ", " + dateEvents[j].EventName;
                    }
                    if ((j < (dateEvents.Count() - 1)))
                    {
                        if (dateEvents[j + 1].EventName != null)
                            tempEventName1 = ", " + dateEvents[j + 1].EventName;
                    }
                    if (tempEventName != null)
                    {
                        temp = tempEventName.Replace(",", " ").Trim().Split(' ');
                    }
                    if (tempEventName1 != null)
                    {
                        temp1 = tempEventName1.Replace(",", " ").Trim().Split(' ');
                    }
                    if (temp != null && temp1 != null)
                    {
                        for (int i = 0; i < temp.Count(); i++)
                        {
                            for (int k = 0; k < temp1.Count(); k++)
                            {
                                if (temp[i] == temp1[k])
                                {
                                    tempEventName2 = temp[i] + ",";
                                }
                            }
                        }
                    }
                    if (s == s1 || (s > s1 && s < s2))
                    {

                        dateEvents.RemoveAt(j);
                        pos = j;
                        dateEvents.Insert(pos, creatDateEventObject(s, events.Event_Name + tempEventName));

                    }
                    else if (s < s1 && s.Hours == s1.Hours)
                    {
                        pos = ++j;
                        dateEvents.Insert(pos, creatDateEventObject(s1, events.Event_Name + tempEventName));

                    }
                    if (s1 != s2 && s2 != s && s2.Hours == s.Hours)
                    {
                        if (s2 > s1)
                        {
                            pos = j + 1;

                            dateEvents.Insert(pos, creatDateEventObject(s2, tempEventName2));
                            j = dateEvents.Count();
                        }
                        else
                        {
                            if (j == (dateEvents.Count() - 1))
                            {
                                pos = j + 1;

                                dateEvents.Insert(pos, creatDateEventObject(s2, tempEventName2));
                                j = dateEvents.Count();

                            }
                        }
                    }
                }
                for (int j = 0; j < (dateEvents.Count() - 1); j++)
                {
                    if (dateEvents[j].Time == dateEvents[j + 1].Time)
                    {
                        if (dateEvents[j].EventName == null)
                        {
                            dateEvents.RemoveAt(j);
                        }
                        else if (dateEvents[j + 1].EventName == null)
                        {
                            dateEvents.RemoveAt(j + 1);
                        }
                    }
                }
                return dateEvents;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }
        public List<DateEvent> ListOfTasksForTheDay(List<Event> listOfEvents, string _date)
        {
            try
            {
                if (_date.IndexOf('\"') != -1)
                {
                    _date = _date.Replace('\"', ' ').Trim();
                }

                List<DateEvent> dateEvents = new List<DateEvent>();
                dateEvents = valuesToInitialiseDataGrid2();

                Conversions objectConversions = new Conversions();

                //List with all the tasks that do not have any relation with _date in events removed
                List<Event> events = retrieveEventsThatAreRelatedTo_date(_date, listOfEvents);
                List<TaskDetails> tasks = new List<TaskDetails>();
                for (int i = 0; i < events.Count(); i++)
                {
                    tasks.Add(objectConversions.convertEventsIntoTaskdetails(events[i]));
                }

                string start_Time; //holds the start time of an event 
                string end_Time = null; //holds the end time of an event if both start time and duration have some valid values
                TimeSpan s2 = new TimeSpan();
                TimeSpan s1 = new TimeSpan();
                for (int i = 0; i < tasks.Count(); i++)
                {
                    start_Time = tasks[i].getStartTime();
                    if (start_Time != "null")
                    {
                        s1 = objectConversions.Convert12HourFormatTo24HourFormat(start_Time);
                        //These two consitions are satisfied by the below statement(s2=s1)
                        //startdate =  enddate,starttime!= null ,duration and end time null
                        //startdate !=  enddate,start_time!= null ,duration and end time null
                        //s2 = s1;
                        if (tasks[i].getEndTime() != "null")
                        {
                            end_Time = tasks[i].getEndTime();
                            s2 = objectConversions.Convert12HourFormatTo24HourFormat(end_Time);
                            dateEvents = displayTimeLineWithEvents(dateEvents, events[i], s1, s2);
                        }

                    }
                }
                dateEvents.Add(ListOfEventsForTheDay(listOfEvents, _date));
                return dateEvents;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }

        }
        public DateEvent ListOfEventsForTheDay(List<Event> listOfEvents, string _date)
        {
            try
            {
                Conversions objectConversions = new Conversions();
                List<Event> events = retrieveEventsThatAreRelatedTo_date(_date, listOfEvents);

                List<TaskDetails> tasks = new List<TaskDetails>();
                for (int i = 0; i < events.Count(); i++)
                {
                    tasks.Add(objectConversions.convertEventsIntoTaskdetails(events[i]));
                }
                string eventsForDisplay = null;
                int j = 0;
                for (int i = 0; i < tasks.Count(); i++)
                {
                    string startTime = tasks[i].getStartTime();
                    string endTime = tasks[i].getEndTime();
                    bool areBothTimeParametersNull = (startTime == "null" && endTime == "null");
                    bool isEndTimeParameterNull = (startTime != "null" && endTime == "null");
                    bool isStartTimeParameterNull = (startTime == "null" && endTime != "null");
                    if (areBothTimeParametersNull || isEndTimeParameterNull || isStartTimeParameterNull)
                    {
                        eventsForDisplay += (j + 1).ToString() + ". " + events[i].Event_Name + " starts on" + events[i].Start + " and ends on " + events[i].End;
                        j++;
                    }
                    eventsForDisplay += "\n";
                }

                return new DateEvent() { Time = null, EventName = eventsForDisplay };
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }

        }
        //--------------code for timeline display ends here-----------------------------------------------------------


        //--------------------------code for implementing remainder function-------------------------------------------
        public string tasksThatNeedReminderToBeShownNow(DateTime s)
        {
            try
            {
                List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
                listOfTasksInFile = DisplayContentofFileThatIsOpened();

                string listOfTasksThatNeedRemainder = null;
                Conversions objectConversions = new Conversions();

                TimeSpan t = s.TimeOfDay;

                bool wasReminderNeeded = false;
                bool isDeadlineOvershot = false;
                bool endTimePresent = false;
                bool startTimePresent = false;

                for (int i = 0; i < listOfTasksInFile.Count(); i++)
                {
                    string remainder = listOfTasksInFile[i].getRemainderTime();
                    string starttime = listOfTasksInFile[i].getStartTime();
                    string startdate = listOfTasksInFile[i].getStartDate();
                    string enddate = listOfTasksInFile[i].getEndDate();
                    string endtime = listOfTasksInFile[i].getEndTime();
                    string SystemDate = objectConversions.ConvertSystemDateToddmmyyyy();
                    DateTime Today = new DateTime(Convert.ToInt32(SystemDate.Split('/')[2]), Convert.ToInt32(SystemDate.Split('/')[1]), Convert.ToInt32(SystemDate.Split('/')[0]));
                    if (objectConversions.ConvertStringToDate(startdate) == Today || objectConversions.ConvertStringToDate(enddate) == Today)
                    {
                        if (listOfTasksInFile[i].getRemainderTime() != "0H")
                        {
                            TimeSpan remainderTime = new TimeSpan();
                            TimeSpan timeLeft = new TimeSpan();
                            DateTime d1;
                            if (endtime.CompareTo("null") != 0)
                            {
                                bool endTimeCheck = DateTime.TryParse(endtime, out d1);

                                if (endTimeCheck)
                                {
                                    remainderTime = (TimeSpan.Parse(endtime) - objectConversions.durationOrRemainderToTotalHours(remainder));
                                    if (TimeSpan.Parse(endtime).CompareTo(t) < 0)
                                        isDeadlineOvershot = true;
                                    timeLeft = TimeSpan.Parse(endtime) - t;
                                    endTimePresent = true;
                                }
                            }
                            if (starttime.CompareTo("null") != 0)
                            {
                                 bool startTimeCheck = DateTime.TryParse(starttime, out d1);

                                 if (startTimeCheck)
                                 {
                                     remainderTime = (TimeSpan.Parse(starttime) - objectConversions.durationOrRemainderToTotalHours(remainder));
                                     timeLeft = TimeSpan.Parse(starttime) - t;
                                     if (TimeSpan.Parse(starttime).CompareTo(t) < 0)
                                         isDeadlineOvershot = true;
                                     startTimePresent = true;
                                 }
                            }

                            if (remainderTime.CompareTo(t) <= 0 && (endTimePresent || startTimePresent))
                            {
                                if (isDeadlineOvershot)
                                {
                                    listOfTasksThatNeedRemainder += (i + 1).ToString() + "-" + listOfTasksInFile[i].getEventname() + ", overshot by " + timeLeft.ToString(@"hh\:mm") + Environment.NewLine;
                                }
                                else
                                {
                                    listOfTasksThatNeedRemainder += (i + 1).ToString() + "-" + listOfTasksInFile[i].getEventname() + ", just " + timeLeft.ToString(@"hh\:mm") + " left" + Environment.NewLine;
                                }
                                listOfTasksInFile[i].setReminder("0H");
                                wasReminderNeeded = true;
                                break;
                            }
                        }
                    }
                }
                if (wasReminderNeeded)
                {
                    objectDatabase.ClearAllTheContentFromTheFile();
                    for (int i = 0; i < listOfTasksInFile.Count(); i++)
                    {
                        objectDatabase.writeIntoFile(listOfTasksInFile[i].getEventname() + ";" + listOfTasksInFile[i].getLocation() + ";"
                                                          + listOfTasksInFile[i].getStartDate() + ";" + listOfTasksInFile[i].getEndDate() + ";" +
                                                           listOfTasksInFile[i].getStartTime() + ";" + listOfTasksInFile[i].getEndTime() + ";" +
                                                           listOfTasksInFile[i].getDuration() + ";" + listOfTasksInFile[i].getPriority() + ";" +
                                                           listOfTasksInFile[i].getRemainderTime());
                    }
                }
                return listOfTasksThatNeedRemainder;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }

        }
        //---------------------end of code for implementing remainder----------------------------------------------


        //----------------------code for implementing add,edit, search,redo,undo,display,sort--------------------------------

        //This function ensures whether all essential features have been provided by the user 
        //It at the same time tries to fill in other details based on the available information given by the user.
        private bool CheckIfAllEssentialDetailsAreEntered()
        {
            try
            {
                bool allDetailsAreThere = true;
                Conversions objectConversions = new Conversions();
                DateTime startDate = objectConversions.ConvertStringToDate(objectDetailsOfTask.getStartDate());
                DateTime endDate = objectConversions.ConvertStringToDate(objectDetailsOfTask.getEndDate());

                if (objectDetailsOfTask.getEventname() == "null")
                {
                    displayText = "Please enter a valid Event Name";
                    allDetailsAreThere = false;
                }
                else if (startDate.CompareTo(endDate) > 0)
                {
                    displayText = "Start date occurs after end date";
                    allDetailsAreThere = false;
                }
                else if (objectDetailsOfTask.getEndTime() == "null" && (objectDetailsOfTask.getStartTime() != "null" && objectDetailsOfTask.getDuration() != "null"))
                {
                    objectDetailsOfTask.CalculateEndTime();
                }
                else if (objectDetailsOfTask.getDuration() == "null" && (objectDetailsOfTask.getEndTime() != "null" && objectDetailsOfTask.getStartTime() != "null"))
                {
                    objectDetailsOfTask.CalculateDuration();
                }
                return allDetailsAreThere;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return false;
            }
        }
        private void addToIntellisenseDatabase(string s)
        {
            try
            {
                string[] listOfIntellisense = s.Split(' ');
                IntellisenseDatabase objectIntellisenseDatabase = new IntellisenseDatabase();
                for (int i = 0; i < listOfIntellisense.Count(); i++)
                {
                    if (listOfIntellisense[i].CompareTo("null") != 0)
                    {
                        objectIntellisenseDatabase.addWordToIntellisenseList(listOfIntellisense[i]);
                    }
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        //------------------------------check if auto-scheduling needed before storing it in the file----------------------------------------------------
        private bool checkIfautoschedulingNeeded(TaskDetails objectDetailsOfTask)
        {
            try
            {
                Conversions objectConversions = new Conversions();

                List<TaskDetails> tasksAlreadyInFile = DisplayContentofFileThatIsOpened();
                List<Event> eventsAlreadyInFile = objectConversions.convertTaskdetailsIntoEvent(tasksAlreadyInFile);

                List<TaskDetails> tasks = new List<TaskDetails>();
                tasks.Add(objectDetailsOfTask);
                List<Event> s = new List<Event>();
                s = objectConversions.convertTaskdetailsIntoEvent(tasks);
                string startDate = s[0].Start.Split(',')[0];
                string endDate = s[0].End.Split(',')[0];
                List<Event> listOfEvents = new List<Event>();
                if (startDate == endDate)
                {
                    string date = startDate;
                    listOfEvents = retrieveEventsThatAreRelatedTo_date(date, eventsAlreadyInFile);
                }
                {
                    DateTime today = objectConversions.ConvertStringToDate(startDate);
                    DateTime nextWeek = objectConversions.ConvertStringToDate(endDate);

                    TimeSpan difference = nextWeek - today;

                    List<DateTime> days = new List<DateTime>();

                    for (int i = 0; i <= difference.Days; i++)
                    {
                        days.Add(today.AddDays(i));
                    }

                    foreach (var dateTime in days)
                    {
                        string date = (objectConversions.ConvertStringDateToddmmyyyy(dateTime.Date.ToString("dd/MM/yyyy")));
                        listOfEvents = retrieveEventsThatAreRelatedTo_date(date, eventsAlreadyInFile);
                        for (int i = 0; i < listOfEvents.Count(); i++)
                        {
                            tasks.Add(objectConversions.convertEventsIntoTaskdetails(listOfEvents[i]));
                        }
                        TimeSpan startTime = new TimeSpan(0, 0, 0);
                        TimeSpan endTime = new TimeSpan(0, 0, 0);
                        TimeSpan otherEventStartTime = new TimeSpan(0, 0, 0);
                        TimeSpan otherEventEndTime = new TimeSpan(0, 0, 0);
                        TimeSpan otherEventDuration = otherEventEndTime - otherEventStartTime;
                        TimeSpan duration = endTime - startTime;

                        bool isAutoSchedulingNeeded = false;
                        bool isMidNight = false; //set to true if some task is scheduled at 12 A.M
                        bool isStartTimeNull = false;
                        bool isEndTimeNull = false;
                        TimeSpan t1;
                        TimeSpan t2;

                        bool startTimeCheck = TimeSpan.TryParse(startDate, out t1);
                        bool endTimeCheck = TimeSpan.TryParse(endDate, out t2);
                        if (startTimeCheck && endTimeCheck)
                        {
                            for (int i = 0; i < listOfEvents.Count(); i++)
                            {
                                if (tasks[i].getStartTime() != "null")
                                {
                                    if (tasks[i].getStartTime() == "00:00:00")
                                        isMidNight = true;
                                    otherEventStartTime = TimeSpan.Parse(tasks[i].getStartTime());
                                    if (tasks[i].getEndTime() != "null")
                                        otherEventEndTime = TimeSpan.Parse(tasks[i].getEndTime());
                                    else
                                        otherEventEndTime = TimeSpan.Parse(tasks[i].getStartTime());
                                }
                                else
                                {
                                    isStartTimeNull = true;
                                    if (tasks[i].getEndTime() != "null")
                                    {
                                        if (tasks[i].getStartTime() != "null")
                                        {
                                            otherEventStartTime = TimeSpan.Parse(tasks[i].getStartTime());
                                            otherEventEndTime = TimeSpan.Parse(tasks[i].getStartTime());
                                        }
                                        if (tasks[i].getEndTime() == "00:00:00")
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
                                //In case the event to be added does not have any start time or end time
                                if (objectDetailsOfTask.getStartTime() == "null" && objectDetailsOfTask.getEndTime() == "null")
                                {
                                    //there is no need to auto-schedule
                                    isAutoSchedulingNeeded = false;
                                }
                                else if (objectDetailsOfTask.getStartTime() != "null" && objectDetailsOfTask.getEndTime() != "null")
                                {
                                    //In case event to be added has both start time and End time
                                    startTime = TimeSpan.Parse(objectDetailsOfTask.getStartTime());
                                    endTime = TimeSpan.Parse(objectDetailsOfTask.getEndTime());

                                    if (otherEventDuration.CompareTo(new TimeSpan(0, 0, 0)) == 0)
                                    {
                                        if (!isMidNight)//If this event doesn't have a start time or end time
                                        {
                                            //there is no need to auto-schedule
                                            isAutoSchedulingNeeded = false;
                                        }
                                        else if (isStartTimeNull) //this implies the event has only endtime
                                        {
                                            if (otherEventEndTime > startTime && otherEventEndTime < endTime)
                                            {
                                                isAutoSchedulingNeeded = true;
                                            }
                                        }
                                        else if (isEndTimeNull) //this implies event has start time only
                                        {
                                            if (otherEventStartTime == startTime)
                                            {
                                                isAutoSchedulingNeeded = true;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //if duration is not 0 ,i.e, the event has different start time and end time
                                        if ((startTime >= otherEventStartTime && startTime < otherEventEndTime) || (endTime > otherEventStartTime && endTime <= otherEventEndTime))
                                            isAutoSchedulingNeeded = true;
                                    }
                                }
                                else if (objectDetailsOfTask.getStartTime() != "null")
                                {
                                    //If the event to be added has start time only
                                    startTime = TimeSpan.Parse(objectDetailsOfTask.getStartTime());
                                    if (otherEventDuration.CompareTo(new TimeSpan(0, 0, 0)) == 0)
                                    {
                                        if (!isMidNight)//If this event doesn't have a start time or end time
                                        {
                                            //there is no need to auto-schedule
                                            isAutoSchedulingNeeded = false;
                                        }
                                        else if (isEndTimeNull) //this implies event has start time only
                                        {
                                            if (otherEventStartTime == startTime)
                                            {
                                                isAutoSchedulingNeeded = true;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (otherEventStartTime < otherEventEndTime)
                                        {
                                            if (startTime >= otherEventStartTime && startTime < otherEventEndTime)
                                                isAutoSchedulingNeeded = true;
                                        }
                                        else
                                        {
                                            otherEventDuration = otherEventStartTime - otherEventEndTime;
                                            if ((startTime - otherEventDuration).CompareTo(otherEventStartTime) <= 0)
                                                isAutoSchedulingNeeded = true;
                                        }

                                    }
                                }
                                else if (objectDetailsOfTask.getEndTime() != "null")
                                {
                                    //If the event to be added has end time only
                                    endTime = TimeSpan.Parse(objectDetailsOfTask.getEndTime());
                                    if (otherEventDuration.CompareTo(new TimeSpan(0, 0, 0)) == 0)
                                    {
                                        if (!isMidNight)//If this event doesn't have a start time or end time
                                        {
                                            //there is no need to auto-schedule
                                            isAutoSchedulingNeeded = false;
                                        }
                                        else if (isEndTimeNull) //this implies event has start time only
                                        {
                                            if (otherEventStartTime == startTime)
                                            {
                                                isAutoSchedulingNeeded = true;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (endTime > otherEventStartTime && endTime <= otherEventEndTime)
                                            isAutoSchedulingNeeded = true;
                                    }
                                }

                                if (isAutoSchedulingNeeded)
                                {
                                    displayText = ("You may want to Re-schedule.Check the available slots on " + date);
                                    return false;
                                }
                            }
                        }
                    }
                }
                
                return true;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return false;
            }
        }
        //-------------------------end of function checking for re-scheduling--------------------------------------------

        //--------------------------------repeat feature-----------------------------------------------------------------
        private string CheckIfRepeatIsSet(TaskDetails t)
        {

            try
            {
                Conversions objectConversions = new Conversions();
                TaskDetails task = new TaskDetails();
                string tasksThatAreToBeAdded = null;
                string s_repeat = t.getRepeat();
                string date = t.getStartDate();
                DateTime startDateOfEvent = objectConversions.ConvertStringToDate(date);
                DateTime next1Year = startDateOfEvent.AddYears(1);
                DateTime d1;
                if (t.getEndRepeat() != "null")
                {
                    bool endRepeatCheck = DateTime.TryParse(t.getEndRepeat(), out d1);

                    if (endRepeatCheck)
                    {
                        next1Year = objectConversions.ConvertStringToDate(t.getEndRepeat());
                    }
                }
                DateTime yearToBeChecked = startDateOfEvent;
                int count = 1;
                bool endDateOfEventCheck = DateTime.TryParse(t.getEndDate(), out d1);

                if (endDateOfEventCheck)
                {
                    DateTime endDateOfEvent = objectConversions.ConvertStringToDate(t.getEndDate());
                    int NoOfDays = objectConversions.interpretRepeatAsNumberOfDays(s_repeat, date);
                    if (NoOfDays != 0)
                    {
                        while (yearToBeChecked < next1Year)
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
                            task.setEndRepeat(t.getEndRepeat());
                            task.setStartDate(objectConversions.ConvertStringDateToddmmyyyy(startDateOfEvent.AddDays((count * NoOfDays)).ToString("dd/MM/yyyy")));
                            task.setEndDate(objectConversions.ConvertStringDateToddmmyyyy(endDateOfEvent.AddDays((count * NoOfDays)).ToString("dd/MM/yyyy")));
                            tasksThatAreToBeAdded += "\n" + objectConversions.convertTaskDetailToWriteIntoFile(task);
                            count++;
                            yearToBeChecked = objectConversions.ConvertStringToDate(task.getStartDate());
                        }
                    }
                }
                return tasksThatAreToBeAdded;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }
        private string[] retrieveIDsofARepeatingEvent(int id)
        {
            try
            {
                List<TaskDetails> tasks = new List<TaskDetails>();
                List<string> listOfIDs = new List<string>();
                tasks = DisplayContentofFileThatIsOpened();
                int i = id;
                int j = 0;
                string startDate = tasks[id - 1].getStartDate();
                string eventName = tasks[id - 1].getEventname();
                string startTime = tasks[id - 1].getStartTime();
                string endTime = tasks[id - 1].getEndTime();
                listOfIDs.Insert(j, id.ToString());
                DateTime d1,d2;
                Conversions objectConversions = new Conversions();
                for (; i < tasks.Count(); i++)
                {
                    if (eventName == tasks[i].getEventname() && startTime == tasks[i].getStartTime() && endTime == tasks[i].getEndTime())
                    {
                        bool startDateCheck1= DateTime.TryParse(tasks[i].getStartDate(), out d1);
                        bool startDateCheck2 = DateTime.TryParse(startDate, out d2);


                        if (startDateCheck1 && startDateCheck2)
                        {
                            if (objectConversions.ConvertStringToDate(tasks[i].getStartDate()).CompareTo(objectConversions.ConvertStringToDate(startDate)) > 0)
                            {
                                j++;
                                listOfIDs.Insert(j, (i + 1).ToString());
                            }
                        }

                    }
                }
                return listOfIDs.ToArray();
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }
        //----------------------end of repeat---------------------------------
        //extracts the details entered by the user regarding the task to be done
        private void InitialiseTaskDetailParameter()
        {
            // details are stored in the following order : id-0,event-1,at-2,start-3,end-4,from-5,to-6,duration-7,priority-8,reminder-9,10 -repeat
            //every non-keyword(which are the event details) are enclosed in quotesstring[] splitContentByQuotes = contentOperation.Split('\"');
            try
            {
                Conversions objectConversions = new Conversions(); 
                string SystemDate = objectConversions.ConvertSystemDateToddmmyyyy();
                DateTime Today = new DateTime(Convert.ToInt32(SystemDate.Split('/')[2]), Convert.ToInt32(SystemDate.Split('/')[1]), Convert.ToInt32(SystemDate.Split('/')[0]));
                    
                string[] splitContentByQuotes = contentOperation.Split('\"');

                //traverses the whole of listOfKeywords array
                for (int i = 1; i < listOfKeywords.Count(); i++)
                {
                    //checks if the keyword matches with the parameter of the event entered by user
                    for (int j = 0; j < splitContentByQuotes.Length; j += 2)
                    {
                        if (String.Compare(splitContentByQuotes[j].Trim(), listOfKeywords[i].Trim(), true) == 0)
                        {
                            if ((j + 1) < splitContentByQuotes.Count())
                            {
                                int count = splitContentByQuotes[j + 1].Trim().Length - splitContentByQuotes[j + 1].Trim().Replace("\"", "").Length - splitContentByQuotes[j + 1].Trim().Replace(" ", "").Length;
                                
                                if (splitContentByQuotes[j + 1] == "today" && (i == 3 || i == 4))
                                {
                                    splitContentByQuotes[j + 1] = (SystemDate);
                                }
                                switch (i)
                                {
                                    //reminder of order ;) - id-0,event-1,at-2,start-3,end-4,from-5,to-6,duration-7,priority-8,reminder-9
                                    case 1:
                                        if (count == 0)
                                        {
                                            continue;
                                        }
                                        objectDetailsOfTask.setEventname(splitContentByQuotes[j + 1]);
                                        //addToIntellisenseDatabase(splitContentByQuotes[j + 1]);
                                        break;
                                    case 2:
                                        if (count == 0)
                                        {
                                            continue;
                                        }
                                        objectDetailsOfTask.setlocation(splitContentByQuotes[j + 1]);
                                        //addToIntellisenseDatabase(splitContentByQuotes[j + 1]);
                                        break;
                                    case 3:

                                        objectDetailsOfTask.setStartDate(splitContentByQuotes[j + 1]);
                                        if (!(objectDetailsOfTask.getStartDate().IndexOf('/') != -1))
                                        {
                                            result = "Start date has " + objectDetailsOfTask.getStartDate() + ".Please check with your system date format";
                                        }
                                        break;
                                    case 4:
                                        if (objectDetailsOfTask.getStartDate().IndexOf('/') != -1)
                                        {
                                            objectDetailsOfTask.setEndDate(splitContentByQuotes[j + 1]);
                                            if (!(objectDetailsOfTask.getEndDate().IndexOf('/') != -1))
                                            {
                                                result = "End date has " + objectDetailsOfTask.getEndDate() + ".Please check with your system date format";
                                            }
                                        }
                                        break;
                                    case 5:
                                        if (count == 0)
                                        {
                                               continue;
                                        }
                                        objectDetailsOfTask.setStartTime(splitContentByQuotes[j + 1]);
                                        if (objectDetailsOfTask.getStartTime() != "null" && objectDetailsOfTask.getStartTime().IndexOf(':') == -1)
                                        {
                                            result = "Start time has " + objectDetailsOfTask.getStartTime() + ".";
                                        }
                                        break;
                                    case 6:
                                        if (count == 0)
                                        {
                                               continue;
                                        }
                                        objectDetailsOfTask.setEndTime(splitContentByQuotes[j + 1]);
                                        if (objectDetailsOfTask.getEndTime() != "null" && objectDetailsOfTask.getEndTime().IndexOf(':') == -1)
                                        {
                                            result = "End time has " + objectDetailsOfTask.getEndTime() + ".";
                                        }
                                        break;
                                    case 7:
                                        if (count == 0)
                                        {
                                               continue;
                                        }
                                        objectDetailsOfTask.setDuration(splitContentByQuotes[j + 1]);
                                        break;
                                    case 8:
                                        objectDetailsOfTask.setPriority(splitContentByQuotes[j + 1]);
                                        break;
                                    case 9:
                                        objectDetailsOfTask.setReminder(splitContentByQuotes[j + 1]);
                                        break;
                                    case 10:
                                        objectDetailsOfTask.setRepeat(splitContentByQuotes[j + 1]);
                                        break;
                                    case 11:
                                        if (count == 0)
                                        {
                                            continue;
                                        }
                                        objectDetailsOfTask.setEndRepeat(splitContentByQuotes[j + 1]);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        private void checkIfTaskIsShiftingToAnotherDay(TaskDetails taskObject)
        {
            try
            {
                //end date is changed appropriately
                Conversions objectConversions = new Conversions(); 
                if (taskObject.getStartTime() != "null" && taskObject.getEndTime() != "null")
                {
                        DateTime d1;
                        if (taskObject.getEndTime().CompareTo(taskObject.getStartTime()) < 0)
                        {
                                                               
                           bool startDateCheck = DateTime.TryParse(taskObject.getStartDate(),out d1);
                           if (startDateCheck)
                           {
                               DateTime start = objectConversions.ConvertStringToDate(taskObject.getStartDate());
                               taskObject.setEndDate(start.AddDays(1).ToString("dd/MM/yyyy"));
                           }
                        }
                   
                    
                }
                //remider is also set here for events that do not have a start time and end time
                if (taskObject.getStartTime() == "null" && taskObject.getEndTime() == "null")
                {
                    taskObject.setReminder("0H");
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        private string BreakdownContentIntoAptDetails()
        {
            try
            {
                string DetailOfEventToBeWrittenIntoFile = null;
                contentOperation = " event " + contentOperation;
                InitialiseTaskDetailParameter();
                Conversions objectConversions = new Conversions();
                // id-0,event-1,at-2,start-3,end-4,from-5,to-6,duration-7,priority-8,reminder-9,10 - repeat
                if (result != "Invalid command")
                {
                    displayText = result;
                    return null;
                }

                if (!CheckIfAllEssentialDetailsAreEntered())  // checks if all details have been entered by user 
                {
                    return null;
                }
                checkIfTaskIsShiftingToAnotherDay(objectDetailsOfTask);
                string s = CheckIfRepeatIsSet(objectDetailsOfTask);

                bool canWriteIntoFile = checkIfautoschedulingNeeded(objectDetailsOfTask);//checks if the entered task can be fit into the schedule

                // String with all the parameters of the task separated by semi-colon
                if (canWriteIntoFile)
                    displayText = "Added!";

                DetailOfEventToBeWrittenIntoFile += objectConversions.convertTaskDetailToWriteIntoFile(objectDetailsOfTask) + s;
                return DetailOfEventToBeWrittenIntoFile;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }

        //This function does the following
        //1.It firsts breaks down the information given by the user in the command box into appropriate feature of the task by invoking
        //BreakdownContentIntoAptDetails()
        //2.It then writes into the file  the  string which contains all the parameters of the task separated by semi-colon
        private void AddContentToFileThatisOpened()
        {
            try
            {
                string s = BreakdownContentIntoAptDetails();
                if (s != null)
                {
                    objectDatabase.writeIntoFile(s);
                    SortContentsOfFile(); 
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }


        }
        //Sort is done based on three parameters
        //1.Start Date
        //2.Start Time if start time  exists otherwise end Time
        //3.Priority
        //The tasks on the same date are first grouped together , then within every group they are sorted based on time,they are further sorted by priority
        private void SortContentsOfFile()
        {
            try
            {
                List<TaskDetails> ListOfTaskDetails = new List<TaskDetails>();
                TaskDetails TaskDetailsObj = new TaskDetails();
                ListOfTaskDetails = DisplayContentofFileThatIsOpened();
                String StartDateInStringFormat;
                Conversions DateConversionObj = new Conversions();
                TimeSpan DifferenceFromStartDate;
                int[] DaysRemainingForEventEnd = new int[ListOfTaskDetails.Count], DaysRemainingForEventStart = new int[ListOfTaskDetails.Count], Priority = new int[ListOfTaskDetails.Count];


                for (int j = 0; j < ListOfTaskDetails.Count; j++)
                {
                    StartDateInStringFormat = ListOfTaskDetails[j].getStartDate();
                    DifferenceFromStartDate = (DateTime.Now - new Conversions().ConvertStringToDate(StartDateInStringFormat));
                    DaysRemainingForEventStart[j] = Math.Abs(DifferenceFromStartDate.Days);
                    if (DateTime.Now > new Conversions().ConvertStringToDate(StartDateInStringFormat))
                    {
                        DaysRemainingForEventStart[j] = -1 * DifferenceFromStartDate.Days;
                    }
                    String priorityOfTask = ListOfTaskDetails[j].getPriority();
                    if (priorityOfTask.Equals("high"))
                        Priority[j] = 2;
                    else if (priorityOfTask.Equals("medium"))
                        Priority[j] = 1;
                    else
                        Priority[j] = 0;
                }
                for (int i = 0; i < ListOfTaskDetails.Count; i++)
                    for (int j = 0; j < ListOfTaskDetails.Count - i - 1; j++)
                        if (DaysRemainingForEventStart[j] > DaysRemainingForEventStart[j + 1])
                        {
                            TaskDetails TemporaryTaskObj = ListOfTaskDetails[j];
                            ListOfTaskDetails[j] = ListOfTaskDetails[j + 1];
                            ListOfTaskDetails[j + 1] = TemporaryTaskObj;

                            int TemporaryDate = DaysRemainingForEventStart[j];
                            DaysRemainingForEventStart[j] = DaysRemainingForEventStart[j + 1];
                            DaysRemainingForEventStart[j + 1] = TemporaryDate;
                        }
                for (int i = 0; i < ListOfTaskDetails.Count; i++)
                    for (int j = 0; j < ListOfTaskDetails.Count - i - 1; j++)
                        if (DaysRemainingForEventStart[j] == DaysRemainingForEventStart[j + 1])
                        {
                            if (Priority[j] < Priority[j + 1])
                            {
                                TaskDetails TemporaryTaskObj = ListOfTaskDetails[j];
                                ListOfTaskDetails[j] = ListOfTaskDetails[j + 1];
                                ListOfTaskDetails[j + 1] = TemporaryTaskObj;

                                int TemporaryPriority = Priority[j];
                                Priority[j] = Priority[j + 1];
                                Priority[j + 1] = TemporaryPriority;
                            }
                        }
                int counter = 0;
                Conversions objectConversions = new Conversions();
                StreamWriter writer = new StreamWriter("ListOfTasksToDoFile.txt", false);
                while (counter < ListOfTaskDetails.Count)
                {
                    String TaskDetailsInStringFormat = objectConversions.convertTaskDetailToWriteIntoFile(ListOfTaskDetails[counter]);
                    writer.WriteLine(TaskDetailsInStringFormat);
                    counter++;
                }
                writer.Close();
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }

        }
        //displays the list os all tasks that need to be completed 
        private List<TaskDetails> DisplayContentofFileThatIsOpened()
        {
            try
            {
                List<TaskDetails> listOfLinesInFile = new List<TaskDetails>();
                listOfLinesInFile = objectDatabase.readFromFile();
                return listOfLinesInFile;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }
        //deletes the task indicated by the id.
        //If id is "all" then all tasks are deleted
        private void DeleteContentOfFile()
        {
            try
            {
                string[] words;
                words = contentOperation.Split('\"');
                int totalNumberOfTasks = DisplayContentofFileThatIsOpened().Count();
                ErrorHandling objectErrorHandling = new ErrorHandling();
                if (string.Compare(words[1].Trim(), "all") == 0)
                {
                    words[1] = "";
                    for (int i = 1; i <= totalNumberOfTasks; i++)
                        words[1] += i.ToString() + ",";
                }
                words[1] = objectErrorHandling.CheckIfDeleteParameterInProperFormat(words[1]);
                words = words[1].Trim().Split(' ');
                if (objectErrorHandling.CheckIfANumber(words, totalNumberOfTasks) != null)
                {
                    displayText = objectErrorHandling.CheckIfANumber(words, totalNumberOfTasks);
                    return;
                }
                else
                {
                    Conversions objectConversions = new Conversions();
                    List<int> id = objectConversions.ConvertArrayOfStringIntoListOfInt(words);
                    id.Sort();
                    id = objectErrorHandling.HandleInvalidID(id, totalNumberOfTasks);
                    int numberOfTasksToBeDeleted = id.Count();
                    int i = 0;
                    int LineNumberToBeDeleted = id[i];
                    int line_number = 0;
                    string line;
                    string LineThatIsDeleted = null;
                    string tempFileName = "output.txt";
                    using (StreamReader reader = new StreamReader(fileToBeDealtWith))
                    {
                        using (StreamWriter writer = new StreamWriter(tempFileName))
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                line_number++;
                                if (line_number == LineNumberToBeDeleted)
                                {
                                    LineThatIsDeleted = line;
                                    i++;
                                    if (i < id.Count())
                                    {
                                        LineNumberToBeDeleted = id[i];
                                    }
                                    continue;
                                }

                                writer.WriteLine(line);
                            }
                        }
                    }


                    if (File.Exists(fileToBeDealtWith))
                        File.Delete(fileToBeDealtWith);
                    File.Move(tempFileName, fileToBeDealtWith);
                    File.Delete(tempFileName);
                    result = "delete from " + fileToBeDealtWith + " : " + " \"" + LineThatIsDeleted + " \"";
                    displayText = "Deleted!";
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        //clears the display box but does not delete all the task
        private void ClearAllTheContentFromTheFile()
        {
            try
            {
                result = "Display Box Cleared";
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
            }
        }
        
        private List<TaskDetails> SearchFileForTheExactMatch()
        {
            try
            {
                List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
                bool wasMatchFound = false;
                listOfTasksInFile = objectDatabase.readFromFile();
                if (contentOperation.Trim().Replace(" ", "").Replace("\"", "").Length == 0)
                {
                    displayText = "Please enter text to be searched!";
                }
                else if (listOfTasksInFile.Count() == 0)
                {
                    displayText = "File is empty.Therefore no match found";
                }
                else
                {
                    displayText = "No matches found!";
                    for (int i = 0; i < listOfTasksInFile.Count(); i++)
                    {
                        listOfTasksInFile[i].setID((i + 1).ToString());
                    }
                    for (int i = 0; i < listOfTasksInFile.Count(); i++)
                    {
                        int j = i + 1;
                        for (int k = 1; k < listOfKeywords.Count(); k++)
                        {
                            wasMatchFound = false;
                            String searchPattern = contentOperation.Replace('\"', ' ');
                            searchPattern = searchPattern.Trim();


                            if (System.Text.RegularExpressions.Regex.IsMatch(listOfTasksInFile[i].getWord(k).Trim(), searchPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                wasMatchFound = true;
                                displayText = "Search Complete";
                                break;
                            }

                        }
                        if (!wasMatchFound)
                        {
                            listOfTasksInFile.RemoveAt(i);
                            i--;
                        }
                    }
                }
                return listOfTasksInFile;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }
        private void EditTheContentInTheFile()
        {
            try
            {
                bool wasEdited = false;

                string[] listOfIDsToBeEdited;
                List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
                Conversions objectConversions = new Conversions();
                listOfTasksInFile = objectDatabase.readFromFile();


                int totalNumberOfTasks = DisplayContentofFileThatIsOpened().Count();
                ErrorHandling objectErrorHandling = new ErrorHandling();
                string[] words = null;//= { "0", "0", "0" };
                string[] wordsinit = contentOperation.Trim().Split('\"');
                List<string> ListWord = new List<string>();
                for (int i = 0; i < wordsinit.Count(); i++)
                {
                    if (!(wordsinit[i].CompareTo("") == 0))
                    {
                        ListWord.Add(wordsinit[i]);
                    }
                }
                words = ListWord.ToArray();
                if (words != null && words[0] != null)
                {
                    
                    words[0] = objectErrorHandling.CheckIfDeleteParameterInProperFormat(words[0]);
                    listOfIDsToBeEdited = words[0].Trim().Split(' ');
                    if (objectErrorHandling.CheckIfANumber(listOfIDsToBeEdited, totalNumberOfTasks) != null)
                    {
                        displayText = objectErrorHandling.CheckIfANumber(listOfIDsToBeEdited, totalNumberOfTasks);
                        return;
                    }

                    else
                    {
                        listOfIDsToBeEdited = retrieveIDsofARepeatingEvent(Convert.ToInt32(listOfIDsToBeEdited[0]));
                        int k = 0;
                        int LineNumberToBeUpdated = Convert.ToInt32(listOfIDsToBeEdited[k]);
                        int line_number = 0;
                        double totalDays = 0;
                        bool totalDaysComputed = false;
                        string tempFileName = "temp.txt";

                        string line;
                        using (StreamReader reader = new StreamReader(fileToBeDealtWith))
                        {
                            using (StreamWriter writer = new StreamWriter(tempFileName))
                            {
                                while ((line = reader.ReadLine()) != null)
                                {

                                    if (line_number + 1 == LineNumberToBeUpdated)
                                    {
                                        int i = 1;
                                        while (i <= words.Count() - 2)
                                        {
                                            string[] parts = line.Split(';');
                                            int count = words[i + 1].Trim().Length - words[i + 1].Trim().Replace("\"", "").Length - words[i + 1].Trim().Replace(" ", "").Length;
                                            if (count == 0)
                                            {
                                                words[i + 1] = "null";
                                            }
                                            if (words[i].Trim() == "event")
                                            {
                                                if (words[i + 1] == "null")
                                                    continue;
                                                listOfTasksInFile[line_number].setEventname(words[i + 1]);
                                                wasEdited = true;
                                            }
                                            else if (words[i].Trim() == "at")
                                            {
                                                listOfTasksInFile[line_number].setlocation(words[i + 1]);
                                                wasEdited = true;
                                            }
                                            else if (words[i].Trim() == "start")
                                            {
                                                string s = words[i + 1];
                                                s = objectErrorHandling.CheckDate(s);
                                                if (s.IndexOf("/") != -1)
                                                {
                                                    DateTime d1;
                                                    string startDate = listOfTasksInFile[line_number].getStartDate();

                                                    bool startDateCheck = DateTime.TryParse(startDate, out d1);

                                                    if (startDateCheck)
                                                    {
                                                        DateTime sdate = objectConversions.ConvertStringToDate(startDate);
                                                        string endDate = listOfTasksInFile[line_number].getEndDate();

                                                        DateTime updatedDate = objectConversions.ConvertStringToDate(s);
                                                        if (!totalDaysComputed)
                                                        {
                                                            totalDays = (updatedDate - sdate).TotalDays;
                                                            totalDaysComputed = true;
                                                        }
                                                        sdate = sdate.AddDays(totalDays);
                                                        listOfTasksInFile[line_number].setStartDate(objectConversions.ConvertStringDateToddmmyyyy(sdate.ToString("dd/MM/yyyy")));
                                                        startDate = listOfTasksInFile[line_number].getStartDate();
                                                        if (objectConversions.ConvertStringToDate(startDate).CompareTo(objectConversions.ConvertStringToDate(endDate)) > 0)
                                                        {
                                                            listOfTasksInFile[line_number].setEndDate(startDate);
                                                        }
                                                        wasEdited = true;
                                                    }
                                                }
                                                else if (s == "start time value has been entered")
                                                {
                                                    listOfTasksInFile[line_number].setStartTime(words[i + 1]);
                                                    listOfTasksInFile[line_number].CalculateEndTime();
                                                    listOfTasksInFile[line_number].CalculateDuration();
                                                    wasEdited = true;
                                                }
                                            }
                                            else if (words[i].Trim() == "end")
                                            {
                                                string s = objectErrorHandling.CheckDate(words[i + 1]);
                                                if (s.IndexOf("/") != -1)
                                                {
                                                    listOfTasksInFile[line_number].setEndDate(words[i + 1]);
                                                    wasEdited = true;
                                                }
                                                else if (s == "start time value has been entered")
                                                {
                                                    listOfTasksInFile[line_number].setEndTime(words[i + 1]);
                                                    listOfTasksInFile[line_number].CalculateDuration();
                                                    wasEdited = true;
                                                }
                                            }
                                            else if (words[i].Trim() == "from")
                                            {
                                                listOfTasksInFile[line_number].setStartTime(words[i + 1]);
                                                listOfTasksInFile[line_number].CalculateEndTime();
                                                listOfTasksInFile[line_number].CalculateDuration();
                                                wasEdited = true;
                                            }
                                            else if (words[i].Trim() == "to")
                                            {
                                                listOfTasksInFile[line_number].setEndTime(words[i + 1]);
                                                listOfTasksInFile[line_number].CalculateDuration();
                                                wasEdited = true;
                                            }
                                            else if (words[i].Trim() == "duration")
                                            {
                                                listOfTasksInFile[line_number].setDuration(words[i + 1]);
                                                listOfTasksInFile[line_number].CalculateEndTime();
                                                wasEdited = true;
                                            }
                                            else if (words[i].Trim() == "priority")
                                            {
                                                listOfTasksInFile[line_number].setPriority(words[i + 1]);
                                                wasEdited = true;
                                            }
                                            else if (words[i].Trim() == "reminder")
                                            {
                                                listOfTasksInFile[line_number].setReminder(words[i + 1]);
                                                wasEdited = true;
                                            }

                                            i += 2;
                                            k++;
                                            if (k < listOfIDsToBeEdited.Count())
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
                        if (wasEdited)
                            displayText = "Taskupdated!";
                        else
                            displayText = "Nothing was affected!";
                        SortContentsOfFile();
                        if (File.Exists(fileToBeDealtWith))
                            File.Delete(fileToBeDealtWith);
                        File.Move(tempFileName, fileToBeDealtWith);
                        File.Delete(tempFileName);
                    }
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        
        }
        

        private void ArchiveTaskDone()
        {
            try
            {
                HistoryDatabase historyObj = new HistoryDatabase(); string[] words;
                string[] listOfIDsToBeEdited;
                List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
                Conversions objectConversions = new Conversions();
                listOfTasksInFile = objectDatabase.readFromFile();

                words = contentOperation.Split('\"');
                int totalNumberOfTasks = DisplayContentofFileThatIsOpened().Count();
                ErrorHandling objectErrorHandling = new ErrorHandling();

                words[1] = objectErrorHandling.CheckIfDeleteParameterInProperFormat(words[1]);
                listOfIDsToBeEdited = words[1].Trim().Split(' ');
                if (objectErrorHandling.CheckIfANumber(listOfIDsToBeEdited, totalNumberOfTasks) != null)
                {
                    displayText = objectErrorHandling.CheckIfANumber(listOfIDsToBeEdited, totalNumberOfTasks);
                    return;
                }
                else
                {
                    int i = 0;
                    int LineNumberToBeArchived = Convert.ToInt32(listOfIDsToBeEdited[i]);
                    int numberOfTasksToBeDeleted = words.Count();
                    int line_number = 0;
                    string line;
                    string LineThatIsArchived = null;
                    string FileName = "Completed.txt";
                    using (StreamReader reader = new StreamReader(fileToBeDealtWith))
                    {
                        using (StreamWriter writer = new StreamWriter(FileName, true))
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                line_number++;

                                if (line_number == LineNumberToBeArchived)
                                {
                                    LineThatIsArchived = line;
                                    writer.WriteLine(line);
                                    if (i < words.Count())
                                    {
                                        LineNumberToBeArchived = Convert.ToInt32(listOfIDsToBeEdited[i]);
                                        i++;
                                    }
                                    continue;
                                }


                            }
                        }
                    }
                }
                DeleteContentOfFile();
                displayText = "Task successfully completed and archived!";
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }

        }
        private void rememberoldfiles()
        {
            try
            {
                List<TaskDetails> listOfTasks = objectDatabase.readFromFile();
                snapshots.writeIntoFile("\n*******\n");
                String DetailOfEventToBeWrittenIntoFile = null;
                for (int i = 0; i < listOfTasks.Count(); i++)
                {
                    DetailOfEventToBeWrittenIntoFile = listOfTasks[i].getEventname() + ";" + listOfTasks[i].getLocation() + ";"
                                                     + listOfTasks[i].getStartDate() + ";" + listOfTasks[i].getEndDate() + ";" +
                                                      listOfTasks[i].getStartTime() + ";" + listOfTasks[i].getEndTime() + ";" +
                                                      listOfTasks[i].getDuration() + ";" + listOfTasks[i].getPriority() + ";" +
                                                      listOfTasks[i].getRemainderTime();
                    snapshots.writeIntoFile(DetailOfEventToBeWrittenIntoFile);
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
            }
        }
        private void writeIntoRedoFileBeforeUndo()
        {
            try
            {
                List<TaskDetails> listOfTasks = objectDatabase.readFromFile();
                snapshotsForRedo.writeIntoFile("\n*******\n");
                String DetailOfEventToBeWrittenIntoFile = null;
                for (int i = 0; i < listOfTasks.Count(); i++)
                {
                    DetailOfEventToBeWrittenIntoFile = listOfTasks[i].getEventname() + ";" + listOfTasks[i].getLocation() + ";"
                                                     + listOfTasks[i].getStartDate() + ";" + listOfTasks[i].getEndDate() + ";" +
                                                      listOfTasks[i].getStartTime() + ";" + listOfTasks[i].getEndTime() + ";" +
                                                      listOfTasks[i].getDuration() + ";" + listOfTasks[i].getPriority() + ";" +
                                                      listOfTasks[i].getRemainderTime();
                    snapshotsForRedo.writeIntoFile(DetailOfEventToBeWrittenIntoFile);
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        private void UndoTask()
        {
            try
            {
                writeIntoRedoFileBeforeUndo();
                List<TaskDetails> undoContent = snapshots.readFromFile();
                objectDatabase.ClearAllTheContentFromTheFile();
                String DetailOfEventToBeWrittenIntoFile = null;
                for (int i = 0; i < undoContent.Count(); i++)
                {
                    DetailOfEventToBeWrittenIntoFile = undoContent[i].getEventname() + ";" + undoContent[i].getLocation() + ";"
                                                     + undoContent[i].getStartDate() + ";" + undoContent[i].getEndDate() + ";" +
                                                      undoContent[i].getStartTime() + ";" + undoContent[i].getEndTime() + ";" +
                                                      undoContent[i].getDuration() + ";" + undoContent[i].getPriority() + ";" +
                                                      undoContent[i].getRemainderTime();
                    objectDatabase.writeIntoFile(DetailOfEventToBeWrittenIntoFile);
                }

                string tempFileName = "output.txt";
                string line;
                int totalNoOfLinesInFile = 0;
                using (StreamReader reader = new StreamReader("undo.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(tempFileName))
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
                    displayText = "Cannot Undo!";
                    return;
                }
                int LineNumberToBeDeletedFrom = totalNoOfLinesInFile - undoContent.Count() - 3;
                int line_number = 0;
                string LineThatIsDeleted = null;
                string snapshotDividingString = "" + "*******" + "";
                bool snapshotDividerEncountered = false;
                using (StreamReader reader = new StreamReader("undo.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(tempFileName))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            line_number++;
                            if (line_number > LineNumberToBeDeletedFrom)
                            {
                                LineThatIsDeleted = line;
                                continue;
                            }
                            else if ((line.Contains("*******")) || line.CompareTo("") == 0)
                            {
                                snapshotDividerEncountered = true;
                                continue;
                            }
                            else
                            {
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
                if (File.Exists("undo.txt"))
                    File.Delete("undo.txt");
                File.Move(tempFileName, "undo.txt");
                File.Delete(tempFileName);
                displayText = "Previous Action Retrieved!";
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        private void RedoTask()
        {
            try
            {
                List<TaskDetails> redoContent = snapshotsForRedo.readFromFile();
                objectDatabase.ClearAllTheContentFromTheFile();
                String DetailOfEventToBeWrittenIntoFile = null;
                for (int i = 0; i < redoContent.Count(); i++)
                {
                    DetailOfEventToBeWrittenIntoFile = redoContent[i].getEventname() + ";" + redoContent[i].getLocation() + ";"
                                                     + redoContent[i].getStartDate() + ";" + redoContent[i].getEndDate() + ";" +
                                                      redoContent[i].getStartTime() + ";" + redoContent[i].getEndTime() + ";" +
                                                      redoContent[i].getDuration() + ";" + redoContent[i].getPriority() + ";" +
                                                      redoContent[i].getRemainderTime();
                    objectDatabase.writeIntoFile(DetailOfEventToBeWrittenIntoFile);
                }

                string tempFileName = "output.txt";
                string line;
                int totalNoOfLinesInFile = 0;
                using (StreamReader reader = new StreamReader("redo.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(tempFileName))
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
                    displayText = "Cannot Redo!";
                    return;
                }
                int LineNumberToBeDeletedFrom = totalNoOfLinesInFile - redoContent.Count() - 3;
                int line_number = 0;
                string LineThatIsDeleted = null;
                string snapshotDividingString = "" + "*******" + "";
                bool snapshotDividerEncountered = false;
                using (StreamReader reader = new StreamReader("redo.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(tempFileName))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            line_number++;
                            if (line_number > LineNumberToBeDeletedFrom)
                            {
                                LineThatIsDeleted = line;
                                continue;
                            }
                            else if ((line.Contains("*******")) || line.CompareTo("") == 0)
                            {
                                snapshotDividerEncountered = true;
                                continue;
                            }
                            else
                            {
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
                if (File.Exists("redo.txt"))
                    File.Delete("redo.txt");
                File.Move(tempFileName, "redo.txt");
                File.Delete(tempFileName);
                displayText = "Re-Done!";
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";

            }
        }
        private void Help()
        {

        }
        private List<TaskDetails> ViewHistory()
        {
            try
            {
                HistoryDatabase HistoryDatabaseObject = new HistoryDatabase();
                return HistoryDatabaseObject.ViewCompletedEvents();
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }
        }

        public List<Efficiency> ViewPerformance()
        {
            try
            {
                int CurrentMonth = System.DateTime.Now.Month, TotalEventsForCurrentMonth = 0;
                HistoryDatabase HistoryDatabaseObj = new HistoryDatabase();
                int CompletedEventsForCurrentMonth = HistoryDatabaseObj.EventCountForCurrentMonth();
                List<TaskDetails> EventsReadFromMainFile = DisplayContentofFileThatIsOpened();
                for (int i = 0; i < EventsReadFromMainFile.Count; i++)
                {
                    String EndDateInStringFormat = EventsReadFromMainFile[i].getEndDate();
                    DateTime EndDateInDateFormat = new Conversions().ConvertStringToDate(EndDateInStringFormat);
                    if (EndDateInDateFormat.Month.CompareTo(CurrentMonth) == 0)
                        TotalEventsForCurrentMonth++;
                }

                CalculateMonthlyEfficiency PerformanceObj = new CalculateMonthlyEfficiency();
                Efficiency efficiencyObj = new Efficiency();
                efficiencyObj.efficiency = 100.0 * CompletedEventsForCurrentMonth / (CompletedEventsForCurrentMonth + TotalEventsForCurrentMonth);
                PerformanceObj.setDataForEfficencyCalc(efficiencyObj);
                PerformanceObj.WriteToFile(efficiencyObj);
                PerformanceObj.MonthlyTaskCount = PerformanceObj.readFromFile();
                return PerformanceObj.MonthlyTaskCount;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result = "error!";
                return null;
            }



        }

        private void Exit()
        {
            Environment.Exit(0);
        }
        public string getDisplayMessage()
        {
            return displayText;

        }

        //---------------------- end of code for implementing add,edit, search,redo, undo,display,sort--------------------------------

        public List<TaskDetails> decideOntask()
        {
            try
            {
                if (String.Compare(commandOperation, "add", true) == 0)
                {
                    rememberoldfiles();
                    this.AddContentToFileThatisOpened();
                }

                else if (String.Compare(commandOperation, "delete", true) == 0) 
                {
                    if (contentOperation != null)
                    {
                        rememberoldfiles();
                        this.DeleteContentOfFile();
                    }
                    else
                        displayText = "Please enter valid ID!";
                }

                else if (String.Compare(commandOperation, "search", true) == 0)
                {
                    if (contentOperation != null)
                    {
                        return this.SearchFileForTheExactMatch();
                    }
                    else
                    {
                        displayText = "Please enter the text to be searched!";
                    }
                }
                else if (String.Compare(commandOperation, "view", true) == 0)
                {
                    if (contentOperation != null)
                    {
                        displayText = result;
                    }
                    else
                        displayText = "Please enter the text to be searched!";
                }
                else if (String.Compare(commandOperation, "display", true) == 0) 
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        this.DisplayContentofFileThatIsOpened();
                    }
                }
                else if (String.Compare(commandOperation, "clear", true) == 0) 
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        rememberoldfiles();
                        this.ClearAllTheContentFromTheFile();
                    }
                    return null;
                }
                else if (String.Compare(commandOperation, "edit", true) == 0) 
                {
                    if (contentOperation != null)
                    {
                        rememberoldfiles();
                        this.EditTheContentInTheFile();
                    }
                    else
                        displayText = "Please provide valid ID";
                }
                else if (String.Compare(commandOperation, "done", true) == 0)
                {
                    rememberoldfiles();
                    if (contentOperation != null)
                    {
                        this.ArchiveTaskDone();
                    }
                    else
                        displayText = "Please enter ID of task completed within quotes";
                }
                else if (String.Compare(commandOperation, "undo", true) == 0)
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        this.UndoTask();
                    }

                }
                else if (String.Compare(commandOperation, "redo", true) == 0)
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        this.RedoTask();
                    }

                }
                else if (String.Compare(commandOperation, "exit", true) == 0)
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        this.Exit();
                    }

                }
                else if (String.Compare(commandOperation, "help", true) == 0)
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        this.Help();
                    }

                }
                else if (String.Compare(commandOperation, "history", true) == 0)
                {
                    contentOperation = null;
                    if (contentOperation == null)
                    {
                        return this.ViewHistory();
                    }

                }
                else if (String.Compare(commandOperation, "performance", true) == 0)
                {
                    if (contentOperation == null)
                    {
                        this.ViewPerformance();
                    }

                }
                return this.DisplayContentofFileThatIsOpened();
            }  
             catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                result="error!";
                return null;
            }
        }
    }
}
