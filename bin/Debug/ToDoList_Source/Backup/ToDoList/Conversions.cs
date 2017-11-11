namespace ToDoList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;

    internal class Conversions
    {
        public TimeSpan Convert12HourFormatTo24HourFormat(string s)
        {
            int indexOfAM = s.IndexOf("am");
            int indexOfam = s.IndexOf("AM");
            int indexOfPM = s.IndexOf("pm");
            int indexOfpm = s.IndexOf("PM");
            string[] HrMinSec = null;
            int HourMin = 0;
            s = s.Replace(" ", "");
            if ((indexOfPM != -1) || (indexOfpm != -1))
            {
                if (indexOfPM != -1)
                {
                    s = s.Replace("pm", null);
                }
                else
                {
                    s = s.Replace("PM", null);
                }
                HrMinSec = s.Split(new char[] { '.', ':' });
                if (Convert.ToInt32(HrMinSec[0]) < 12)
                {
                    HrMinSec[0] = (Convert.ToInt32(HrMinSec[0]) + 12).ToString();
                }
                HourMin = HrMinSec.Length;
            }
            else if ((indexOfAM != -1) || (indexOfam != -1))
            {
                if (indexOfAM != -1)
                {
                    s = s.Replace("am", null);
                }
                else
                {
                    s = s.Replace("AM", null);
                }
                HrMinSec = s.Split(new char[] { '.', ':' });
                if (Convert.ToInt32(HrMinSec[0]) == 12)
                {
                    HrMinSec[0] = Convert.ToInt32("0").ToString();
                }
                HourMin = HrMinSec.Length;
            }
            if ((((indexOfAM == -1) && (indexOfPM == -1)) && (indexOfam == -1)) && (indexOfpm == -1))
            {
                HrMinSec = s.Split(new char[] { '.', ':' });
                HourMin = HrMinSec.Length;
            }
            if (((HourMin <= 1) && (HrMinSec[0] != string.Empty)) && (HrMinSec[0] != null))
            {
                return new TimeSpan(Convert.ToInt32(HrMinSec[0]), 0, 0);
            }
            if (HourMin == 2)
            {
                return new TimeSpan(Convert.ToInt32(HrMinSec[0]), Convert.ToInt32(HrMinSec[1]), 0);
            }
            if (HourMin == 3)
            {
                return new TimeSpan(Convert.ToInt32(HrMinSec[0]), Convert.ToInt32(HrMinSec[1]), Convert.ToInt32(HrMinSec[2]));
            }
            return new TimeSpan(0, 0, 0);
        }

        public string Convert24HourFormatTo12HourFormat(TimeSpan s)
        {
            string timeLine = s.Hours.ToString() + ":" + s.Minutes.ToString() + " am";
            if (s.Hours == 12)
            {
                return (s.Hours.ToString() + ":" + s.Minutes.ToString() + " pm");
            }
            if ((s.Hours == 0) || (s.Hours == 0x18))
            {
                int CS$0$0001 = s.Hours + 12;
                return (CS$0$0001.ToString() + ":" + s.Minutes.ToString() + " am");
            }
            if (s.Hours > 12)
            {
                timeLine = ((s.Hours - 12)).ToString() + ":" + s.Minutes.ToString() + " pm";
            }
            return timeLine;
        }

        public string ConvertDateTo_ddmmyyFormat(string _date)
        {
            string[] dateformat = _date.Split(new char[] { '/' });
            _date = dateformat[1] + "/" + dateformat[0] + "/" + dateformat[2];
            return _date;
        }

        public double ConvertDaysToHours(string days)
        {
            return TimeSpan.FromDays(Convert.ToDouble(days)).TotalHours;
        }

        public double ConvertMinutesToHours(string minutes)
        {
            return TimeSpan.FromMinutes(Convert.ToDouble(minutes)).TotalHours;
        }

        public double ConvertSecondsToHours(string seconds)
        {
            return TimeSpan.FromSeconds(Convert.ToDouble(seconds)).TotalHours;
        }

        public List<TaskDetails> convertStringIntoTaskDetails(List<string> ListOfCompletedEvents)
        {
            List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
            for (int j = 0; j < ListOfCompletedEvents.Count<string>(); j++)
            {
                string line = ListOfCompletedEvents[j];
                int i = j + 1;
                string[] eventParameters = line.Split(new char[] { ';' });
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

        public DateTime ConvertStringToDate(string s)
        {
            try
            {
                if (s != " ")
                {
                    string[] mmddyy = s.Split(new char[] { '/' });
                    if (mmddyy != null)
                    {
                        s = mmddyy[0] + "/" + mmddyy[1] + "/" + mmddyy[2];
                    }
                    return DateTime.Parse(s);
                }
                return DateTime.Now;
            }
            catch (Exception e)
            {
                if (((e is ArgumentNullException) || (e is ArgumentException)) || (e is ArgumentOutOfRangeException))
                {
                    MessageBox.Show("Error!");
                }
                return DateTime.Today;
            }
        }

        public List<Event> convertTaskdetailsIntoEvent(List<TaskDetails> tasks)
        {
            List<Event> events = new List<Event>();
            if (tasks == null)
            {
                return null;
            }
            for (int i = 0; i < tasks.Count<TaskDetails>(); i++)
            {
                string location = (tasks[i].getLocation() == "null") ? null : (" @ " + tasks[i].getLocation());
                Event <>g__initLocal0 = new Event();
                <>g__initLocal0.ID = (i + 1).ToString();
                <>g__initLocal0.EventName = (tasks[i].getEventname() == "null") ? " " : (tasks[i].getEventname() + location);
                <>g__initLocal0.StartDate = (tasks[i].getStartDate() == "null") ? " " : tasks[i].getStartDate();
                <>g__initLocal0.EndDate = (tasks[i].getEndDate() == "null") ? " " : tasks[i].getEndDate();
                <>g__initLocal0.startTime = (tasks[i].getStartTime() == "null") ? " " : tasks[i].getStartTime();
                <>g__initLocal0.endTime = (tasks[i].getEndTime() == "null") ? " " : tasks[i].getEndTime();
                <>g__initLocal0.priority = (tasks[i].getPriority() == "null") ? " " : tasks[i].getPriority();
                <>g__initLocal0.reminderBefore = (tasks[i].getRemainderTime() == "null") ? " " : tasks[i].getRemainderTime();
                Event eventObj = <>g__initLocal0;
                events.Add(eventObj);
            }
            return events;
        }

        public string convertTaskDetailToWriteIntoFile(TaskDetails objectDetailsOfTask)
        {
            return (objectDetailsOfTask.getEventname() + ";" + objectDetailsOfTask.getLocation() + ";" + objectDetailsOfTask.getStartDate() + ";" + objectDetailsOfTask.getEndDate() + ";" + objectDetailsOfTask.getStartTime() + ";" + objectDetailsOfTask.getEndTime() + ";" + objectDetailsOfTask.getDuration() + ";" + objectDetailsOfTask.getPriority() + ";" + objectDetailsOfTask.getRemainderTime() + ";" + objectDetailsOfTask.getRepeat());
        }

        public TimeSpan durationOrRemainderToTotalHours(string s)
        {
            string[] splitDuration;
            string[] calculateHours = new string[] { "0", "0", "0", "0" };
            if (s.IndexOf("H") != -1)
            {
                splitDuration = s.Split(new char[] { 'H' });
                calculateHours[1] = splitDuration[0];
                s = splitDuration[1];
            }
            if (s.IndexOf("M") != -1)
            {
                splitDuration = s.Split(new char[] { 'M' });
                calculateHours[2] = splitDuration[0];
                s = splitDuration[1];
            }
            if (s.IndexOf("S") != -1)
            {
                splitDuration = s.Split(new char[] { 'S' });
                calculateHours[3] = splitDuration[0];
                s = splitDuration[1];
            }
            if (s.Split(new char[] { ':' }).Count<string>() == 3)
            {
                splitDuration = s.Split(new char[] { ':' });
                int Hours = Convert.ToInt32(splitDuration[0]);
                if (Hours < 0)
                {
                    splitDuration[0] = (0x18 + Hours).ToString();
                }
                return new TimeSpan(Convert.ToInt32(splitDuration[0]), Convert.ToInt32(splitDuration[1]), Convert.ToInt32(splitDuration[2]));
            }
            return new TimeSpan(Convert.ToInt32(calculateHours[1]), Convert.ToInt32(calculateHours[2]), Convert.ToInt32(calculateHours[3]));
        }

        public int interpretRepeatAsNumberOfDays(string s, string date)
        {
            int days = 0;
            DateTime dateOfTheEvent = this.ConvertStringToDate(date);
            if (string.Compare(s, "daily", true) == 0)
            {
                return 1;
            }
            if (string.Compare(s, "weekly", true) == 0)
            {
                return 7;
            }
            if (string.Compare(s, "biweekly", true) == 0)
            {
                return 14;
            }
            if (string.Compare(s, "monthly", true) == 0)
            {
                return DateTime.DaysInMonth(dateOfTheEvent.Year, dateOfTheEvent.Month);
            }
            if (string.Compare(s, "yearly", true) != 0)
            {
                return days;
            }
            if (DateTime.IsLeapYear(dateOfTheEvent.Year))
            {
                return 0x16e;
            }
            return 0x16d;
        }

        public string replaceWithAppropriateShortForms(string s)
        {
            string[] checkForHours = new string[] { "hours", "hour", "hrs", "hr", "h" };
            string[] checkForWeeks = new string[] { "weeks", "week", "w" };
            string[] checkForMinutes = new string[] { "minutes", "mins", "min", "m" };
            string[] checkForDays = new string[] { "days", "day", "d" };
            string[] checkforSeconds = new string[] { "seconds", "secs", "sec", "s" };
            bool isItAday = false;
            if (!isItAday)
            {
                int i;
                if ((s.IndexOf('w') != -1) || (s.IndexOf("W") != -1))
                {
                    for (i = 0; i < checkForWeeks.Length; i++)
                    {
                        if (Regex.IsMatch(s, checkForWeeks[i], RegexOptions.IgnoreCase))
                        {
                            s = s.Replace(checkForWeeks[i], "W");
                            isItAday = false;
                            break;
                        }
                    }
                }
                if ((s.IndexOf('h') != -1) || (s.IndexOf("H") != -1))
                {
                    for (i = 0; i < checkForHours.Length; i++)
                    {
                        if (Regex.IsMatch(s, checkForHours[i], RegexOptions.IgnoreCase))
                        {
                            s = s.Replace(checkForHours[i], "H");
                            break;
                        }
                    }
                }
                if ((s.IndexOf("m") != -1) || (s.IndexOf("M") != -1))
                {
                    for (i = 0; i < checkForMinutes.Length; i++)
                    {
                        if (Regex.IsMatch(s, checkForMinutes[i], RegexOptions.IgnoreCase))
                        {
                            s = s.Replace(checkForMinutes[i], "M");
                            break;
                        }
                    }
                }
                if ((s.IndexOf("d") != -1) || (s.IndexOf("D") != -1))
                {
                    for (i = 0; i < checkForDays.Length; i++)
                    {
                        if (Regex.IsMatch(s, checkForDays[i], RegexOptions.IgnoreCase))
                        {
                            s = s.Replace(checkForDays[i], "D");
                            break;
                        }
                    }
                }
                if ((s.IndexOf("s") != -1) || (s.IndexOf("S") != -1))
                {
                    for (i = 0; i < checkforSeconds.Length; i++)
                    {
                        if (Regex.IsMatch(s, checkforSeconds[i], RegexOptions.IgnoreCase))
                        {
                            s = s.Replace(checkforSeconds[i], "S");
                            break;
                        }
                    }
                }
            }
            s = s.Replace(" ", "");
            return s;
        }
    }
}

