/********************************************************************************************************
 * Author : Nivetha Sathyarajan,Swetha Narayanan
 * -----------------------------------Descripton--------------------------------------------------------- 
 * This class is meant for converting from 
 * (a) a predefined data type to another predefined/user defined data type
 * (b) a user defined data type to another predefined/user defined data type
 *******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;

namespace ToDoList
{
    class Conversions
    {

        public string ConvertSystemDateToddmmyyyy()
        {
            string date = System.DateTime.Today.Day.ToString() + "/" + System.DateTime.Today.Month.ToString() + "/" + System.DateTime.Today.Year.ToString();
            return date;
        }
        public string ConvertStringDateToddmmyyyy(string s)
        {
            if (s != "null" && s != null)
            {
                DateTime dt;
                bool dtCheck = DateTime.TryParse(s, out dt);
                if (dtCheck)
                {
                    DateTime date = DateTime.Parse(s);
                    string[] ddmmyyyy = s.Split('/');
                    string dateInString = s;
                    string[] pattern = { "dd/MM/yyyy" };
                    DateTime parsedDate;
                    bool check = DateTime.TryParseExact(date.ToString(), pattern, null, System.Globalization.DateTimeStyles.None, out parsedDate);
                    if (check)
                        dateInString = parsedDate.ToString("dd/MM/yyyy");
                    dateInString = dateInString.Replace("-", "/");
                    return dateInString;
                }
                return s;
            }
            return this.ConvertSystemDateToddmmyyyy();
        }
        public List<int> ConvertArrayOfStringIntoListOfInt(string[] s)
        {
            try
            {
                List<string> ids = s.ToList<string>();
                List<int> id = new List<int>();
                for (int x = 0; x < ids.Count(); x++)
                {
                    if (ids[x] == "")
                    {
                        ids.RemoveAt(x);
                        x--;
                    }
                    else
                        id.Add(Convert.ToInt32(ids[x]));
                }
                return id;
            }

            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
       

        public DateTime ConvertStringToDate(string s)
        {
            try
            {
                if (s != " ")
                {
                    string[] mmddyy = s.Trim().Split('/');
                    DateTime dt;
                    if (mmddyy != null)
                    {
                        try
                        {
                            ErrorHandling ObjectErrorHandling = new ErrorHandling();
                            dt = new DateTime(Convert.ToInt32(mmddyy[2]), Convert.ToInt32(mmddyy[1]), Convert.ToInt32(mmddyy[0]));
                            //convert into mmddyyyy if entered in ddmmyyyy format
                            string[] pattern = { "dd/MM/yyyy" };
                            DateTime parsedDate;
                            bool check = DateTime.TryParseExact(dt.ToString(), pattern, null, System.Globalization.DateTimeStyles.None, out parsedDate);
                            if (check)
                                dt = parsedDate;
                            return dt;
                        }
                        catch (IndexOutOfRangeException i)
                        {
                            StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                            loggingWriter.WriteLine(System.DateTime.Now + i.Message + "\n");
                            loggingWriter.Close();
                        }
                    }
                    return DateTime.Now;
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    return dt;
                }
            }
            
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return  DateTime.Today;
            }
               
        }
        public string ConvertDateTo_ddmmyyFormat(string _date)
        {
            try
            {
                string[] dateformat = _date.Split('/', '-', '.');
                _date = dateformat[1] + "/" + dateformat[0] + "/" + dateformat[2];
                return _date;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
        public double ConvertDaysToHours(string days)
        {
            try
            {
                double totalHours = TimeSpan.FromDays(Convert.ToDouble(days)).TotalHours;
                return totalHours;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return 0;
            }
        }
        public double ConvertSecondsToHours(string seconds)
        {
            try
            {
                return TimeSpan.FromSeconds(Convert.ToDouble(seconds)).TotalHours;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return 0;
            }
        }
        public double ConvertMinutesToHours(string minutes)
        {
            try
            {
                return TimeSpan.FromMinutes(Convert.ToDouble(minutes)).TotalHours;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return 0;
            }


        }      
        //converts time into 24 hours format
        public TimeSpan Convert12HourFormatTo24HourFormat(string s)
        {
            try
            {
                int indexOfAM = s.IndexOf("am");
                int indexOfam = s.IndexOf("AM");
                int indexOfPM = s.IndexOf("pm");
                int indexOfpm = s.IndexOf("PM");
                string[] HrMinSec = null;
                int HourMin = 0;
                s = s.Replace(" ", "");
                if (indexOfPM != -1 || indexOfpm != -1)
                {
                    if (indexOfPM != -1)
                    {
                        s = s.Replace("pm", null);
                    }
                    else
                    {
                        s = s.Replace("PM", null);
                    }
                    HrMinSec = s.Split('.', ':');
                    if (Convert.ToInt32(HrMinSec[0]) < 12)
                    {
                        HrMinSec[0] = (Convert.ToInt32(HrMinSec[0]) + 12).ToString();
                    }
                    HourMin = HrMinSec.Length;
                }

                else if (indexOfAM != -1 || indexOfam != -1)
                {

                    if (indexOfAM != -1)
                    {
                        s = s.Replace("am", null);
                    }
                    else
                    {
                        s = s.Replace("AM", null);
                    }
                    HrMinSec = s.Split('.', ':');
                    if (Convert.ToInt32(HrMinSec[0]) == 12)
                    {
                        HrMinSec[0] = (Convert.ToInt32("0").ToString());
                    }
                    HourMin = HrMinSec.Length;
                }
                if (indexOfAM == -1 && indexOfPM == -1 && indexOfam == -1 && indexOfpm == -1)
                {
                    HrMinSec = s.Split('.', ':');
                    HourMin = HrMinSec.Length;
                }
                if (HourMin <= 1 && HrMinSec[0] != string.Empty && HrMinSec[0] != null)
                    return new TimeSpan(Convert.ToInt32(HrMinSec[0]), 0, 0);
                else if (HourMin == 2)
                    return new TimeSpan(Convert.ToInt32(HrMinSec[0]), Convert.ToInt32(HrMinSec[1]), 0);
                else if (HourMin == 3)
                    return new TimeSpan(Convert.ToInt32(HrMinSec[0]), Convert.ToInt32(HrMinSec[1]), Convert.ToInt32(HrMinSec[2]));
                return new TimeSpan(0, 0, 0);
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return TimeSpan.Zero;
            }
        }

        public string Convert24HourFormatTo12HourFormat(TimeSpan s)
        {
            try
            {
                string timeLine = s.Hours.ToString() + ":" + s.Minutes.ToString() + " am";
                if (s.Hours == 12)
                {
                    timeLine = (s.Hours).ToString() + ":" + s.Minutes.ToString() + " pm";
                }
                else if (s.Hours == 0 || s.Hours == 24)
                {
                    timeLine = (s.Hours + 12).ToString() + ":" + s.Minutes.ToString() + " am";
                }
                else if (s.Hours > 12)
                {
                    timeLine = (s.Hours - 12).ToString() + ":" + s.Minutes.ToString() + " pm";
                }
                return timeLine;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }

        //need to convert duration and reminder to totalNumberOfHours
        public TimeSpan durationOrRemainderToTotalHours(string s)
        {
            try
            {
                string[] calculateHours = { "0", "0", "0", "0" };
                string[] splitDuration;
                if (s.IndexOf("H") != -1)
                {
                    splitDuration = s.Split('H');
                    calculateHours[1] = splitDuration[0];
                    s = splitDuration[1];
                }
                if (s.IndexOf("M") != -1)
                {
                    splitDuration = s.Split('M');
                    calculateHours[2] = splitDuration[0];
                    s = splitDuration[1];
                }
                if (s.IndexOf("S") != -1)
                {
                    splitDuration = s.Split('S');
                    calculateHours[3] = splitDuration[0];
                    s = splitDuration[1];
                }
                if ((s.Split(':')).Count() == 3)
                {
                    splitDuration = s.Split(':');
                    int Hours = Convert.ToInt32(splitDuration[0]);
                    if (Hours < 0)
                        splitDuration[0] = (24 + Hours).ToString();
                    return new TimeSpan(Convert.ToInt32(splitDuration[0]), Convert.ToInt32(splitDuration[1]), Convert.ToInt32(splitDuration[2]));
                }
                
                return new TimeSpan(Convert.ToInt32(calculateHours[1]), Convert.ToInt32(calculateHours[2]), Convert.ToInt32(calculateHours[3]));
            }
            catch (Exception e)
            {

                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return TimeSpan.Zero;
            }
        }
        public int interpretRepeatAsNumberOfDays(string s, string date)
        {

            try
            {
                int days = 0;
                DateTime dateOfTheEvent = ConvertStringToDate(date);
                if (String.Compare(s.Trim(), "daily", true) == 0)
                {
                    days = 1;
                }
                else if (String.Compare(s.Trim(), "weekly", true) == 0)
                {
                    days = 7;
                }
                else if (String.Compare(s.Trim(), "biweekly", true) == 0)
                {
                    days = 14;
                }
                else if (String.Compare(s.Trim(), "monthly", true) == 0)
                {
                    days = DateTime.DaysInMonth(dateOfTheEvent.Year, dateOfTheEvent.Month);
                }
                else if (String.Compare(s.Trim(), "yearly", true) == 0)
                {
                    if (DateTime.IsLeapYear(dateOfTheEvent.Year))
                        days = 366;
                    else
                        days = 365;
                }
                return days;
            }
            catch (Exception e)
            {

                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return 0;
            }
        }
        //user can have his own notation for representing hours, minutes , days - in such a situation they are converted to a standard
        //notation that the application can understand 
        public string replaceWithAppropriateShortForms(string s)
        {
            try
            {
                string[] checkForHours = { "hours", "hour", "hrs", "hr", "h" };
                string[] checkForWeeks = { "weeks", "week", "w" };
                string[] checkForMinutes = { "minutes", "mins", "min", "m" };
                string[] checkForDays = { "days", "day", "d" };
                string[] checkforSeconds = { "seconds", "secs", "sec", "s" };
                bool isItAday = false;
                if (!isItAday)
                {
                    if (s.IndexOf('w') != -1 || s.IndexOf("W") != -1)
                    {
                        for (int i = 0; i < checkForWeeks.Length; i++)
                            if (System.Text.RegularExpressions.Regex.IsMatch(s, checkForWeeks[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                s = s.Replace(checkForWeeks[i], "W");
                                isItAday = false;
                                break;
                            }
                    }
                    if (s.IndexOf('h') != -1 || s.IndexOf("H") != -1)
                    {
                        for (int i = 0; i < checkForHours.Length; i++)
                            if (System.Text.RegularExpressions.Regex.IsMatch(s, checkForHours[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                s = s.Replace(checkForHours[i], "H");
                                break;
                            }
                    }
                    if (s.IndexOf("m") != -1 || s.IndexOf("M") != -1)
                    {
                        for (int i = 0; i < checkForMinutes.Length; i++)
                            if (System.Text.RegularExpressions.Regex.IsMatch(s, checkForMinutes[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                s = s.Replace(checkForMinutes[i], "M");
                                break;
                            }
                    }
                    if (s.IndexOf("d") != -1 || s.IndexOf("D") != -1)
                    {
                        for (int i = 0; i < checkForDays.Length; i++)
                            if (System.Text.RegularExpressions.Regex.IsMatch(s, checkForDays[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                s = s.Replace(checkForDays[i], "D");
                                break;
                            }
                    }
                    if (s.IndexOf("s") != -1 || s.IndexOf("S") != -1)
                    {
                        for (int i = 0; i < checkforSeconds.Length; i++)
                            if (System.Text.RegularExpressions.Regex.IsMatch(s, checkforSeconds[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            {
                                s = s.Replace(checkforSeconds[i], "S");
                                break;
                            }

                    }
                }

                s = s.Replace(" ", "");
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

        public string convertTaskDetailToWriteIntoFile(TaskDetails objectDetailsOfTask)
        {
            try
            {
                return objectDetailsOfTask.getEventname() + ";" + objectDetailsOfTask.getLocation() + ";"
                   + objectDetailsOfTask.getStartDate() + ";" + objectDetailsOfTask.getEndDate() + ";" +
                     objectDetailsOfTask.getStartTime() + ";" + objectDetailsOfTask.getEndTime() + ";" +
                     objectDetailsOfTask.getDuration() + ";" + objectDetailsOfTask.getPriority() + ";" +
                     objectDetailsOfTask.getRemainderTime() + ";" + objectDetailsOfTask.getRepeat();
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }

        public List<Event> convertTaskdetailsIntoEvent(List<TaskDetails> tasks)
        {
            try
            {
                List<Event> events = new List<Event>();
                if (tasks == null)
                    return null;
                for (int i = 0; i < tasks.Count(); i++)
                {
                    string start = (tasks[i].getStartTime() == "null" ? " " : "," + tasks[i].getStartTime());
                    string end = (tasks[i].getEndTime() == "null" ? " " : "," + tasks[i].getEndTime());
                    string location = (tasks[i].getLocation() == "null" ? " " : " @ " + tasks[i].getLocation());
                    string id = (tasks[i].getID() == "null" ? (i + 1).ToString() : tasks[i].getID());
                    Event eventObj = new Event()
                    {
                        Event_ID = id,
                        Event_Name = (tasks[i].getEventname() == "null" ? " " : tasks[i].getEventname() + location),
                        Start = (tasks[i].getStartDate() == "null" ? " " : tasks[i].getStartDate() + start),
                        End = (tasks[i].getEndDate() == "null" ? " " : tasks[i].getEndDate() + end),
                        Priority = (tasks[i].getPriority() == "null" ? " " : tasks[i].getPriority()),
                        Remind_Me_Before = (tasks[i].getRemainderTime() == "0H" ? " " : tasks[i].getRemainderTime()),
                        //selectTask = false
                    };
                    events.Add(eventObj);
                }
                return events;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
        public /*List<TaskDetails>  */ TaskDetails convertEventsIntoTaskdetails(/*List<Event> */ Event events)
        {
            try
            {

                List<TaskDetails> tasks = new List<TaskDetails>();
                TaskDetails taskObj = new TaskDetails();
                // for (int i = 0; i < events.Count(); i++)
                {
                    string eventName = events.Event_Name;
                    string sDate = events.Start.Trim();
                    string eDate = events.End.Trim();
                    string sTime = "null", eTime = "null";
                    string location = "null";
                    if (events.Event_Name.IndexOf("@") != -1)
                    {
                        eventName = events.Event_Name.Split('@')[0];
                        location = events.Event_Name.Split('@')[1];
                    }
                    if (events.Start.IndexOf(',') != -1)
                    {
                        string[] start = events.Start.Split(',');
                        sDate = start[0].Trim();
                        sTime = start[1].Trim();
                    }
                    if (events.End.IndexOf(',') != -1)
                    {
                        string[] end = events.End.Split(',');
                        eDate = end[0].Trim();
                        eTime = end[1].Trim();
                    }
                    taskObj.setEventname(eventName);
                    taskObj.setlocation(location);
                    taskObj.setStartDate(sDate);
                    taskObj.setEndDate(eDate);
                    taskObj.setStartTime(sTime);
                    taskObj.setEndTime(eTime);
                    taskObj.setPriority(events.Priority);
                    taskObj.setReminder(events.Remind_Me_Before);
                    taskObj.setID(events.Event_ID);
                    tasks.Add(taskObj);
                }
                return taskObj;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }

        public List<TaskDetails> convertStringIntoTaskDetails(List<String> ListOfCompletedEvents)
        {
            try
            {
                List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();

                for (int j = 0; j < ListOfCompletedEvents.Count(); j++)
                {
                    String line = ListOfCompletedEvents[j];
                    int i = j + 1;
                    string[] eventParameters = line.Split(';');
                    TaskDetails task = new TaskDetails();
                    task.setEventname(eventParameters[0]);
                    task.setlocation(eventParameters[1]);
                    task.setStartDate(eventParameters[2]);
                    task.setEndDate(eventParameters[3]);
                    task.setStartTime(eventParameters[4]);
                    task.setEndTime(eventParameters[5]);
                    task.setDuration(eventParameters[6]);
                    task.setPriority(eventParameters[7]);
                    task.setReminder(eventParameters[8]);
                    task.setID(i.ToString());

                    listOfTasksInFile.Add(task);

                }
                return listOfTasksInFile;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }

        }
    }
}

