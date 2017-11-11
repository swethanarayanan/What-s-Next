/********************************************************************************************************
 * Authors : Nivetha Sathyarajan , Monika  Puhazhendhi
 * -----------------------------------Descripton--------------------------------------------------------- 
 * This class is used for storing the details of event.
 * id - stores the event id
 * eventname - stores the event description
 *location-stores the venue of event
 *startDate-stores the start date of event and the default value is the system date
 *endDate-stores the end date of event and the default value is the system date
 *startime-stores the start time of an event and the default value is "null"
 *endtime-stores the end time of an event and the default value is "null"
 *duration-stores the duration of a task and the default value is "null"
 *priority-stores the priority of a task and the default value is "medium"
 *reminder-stores the priority of a task and the default value is "medium"
 *repeat-stores the repeat of a task and the default value is "medium"
 *endRepeat-stores when a task is supposed to finish repeating and default value is "null"
 *CalculateEndTime() - calculates the end time with the help of start time and duration
 *CalculateDuration() - determines the duration of a task using start time and end time value
 *******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ToDoList
{
    class TaskDetails
    {
        private string id;
        private string eventname;
        private string location;
        private string startDate;
        private string endDate;
        private string startime;
        private string endtime;
        private string duration;
        private string priority;
        private string reminder;
        private string repeat;
        private string endRepeat;

        public TaskDetails()
        {
            id = eventname = location = startime = endtime = duration = endDate = endRepeat = "null";
            repeat = "0D";
            reminder = "1H";
            priority = "medium";
            repeat = "none";
            Conversions objectConversions = new Conversions();
            string SystemDate = objectConversions.ConvertSystemDateToddmmyyyy();
            startDate = SystemDate;
        }
        public string getDetails()
        {
            string content;
            content = "ID" + "-" + id + "\n" +
                      "Event Name" + "-" + eventname + "\n" +
                       "Location" + "-" + location + "\n" +
                        "Start Date" + "-" + startDate + "\n" +
                         "End Date" + "-" + endDate + "\n" +
                          "Start Time" + "-" + startime + "\n" +
                           "End Time" + "-" + endtime + "\n" +
                           "Duration" + "-" + duration + "\n" +
                           "Priority" + "-" + priority + "\n" +
                           "Reminder" + "-" + reminder + "\n" +
                           "Repeat" + "-" + repeat + "\n" +
                           "End Repeat" + "-" + endRepeat + "\n";


            return content;



        }
        public string getWord(int k)
        {
            switch (k)
            {
                case 1: return id;

                case 2: return eventname;

                case 3: return location;

                case 4: return startDate;

                case 5: return endDate;

                case 6: return startime;

                case 7: return endtime;

                case 8: return duration;

                case 9: return priority;

                case 10: return reminder;

                case 11: return repeat;

                case 12: return endRepeat;

                default: return " ";

            }


        }

        public void CalculateEndTime()
        {
            try
            {
                TimeSpan endTime = new TimeSpan();
                if (duration != "null" && startime != "null")
                {
                    endTime = TimeSpan.Parse(startime) + TimeSpan.Parse(duration);
                    this.endtime = endTime.ToString();
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }
        }
        public void CalculateDuration()
        {
            try
            {
                TimeSpan difference = new TimeSpan();
                if (endtime != "null" && startime != "null")
                {
                    TimeSpan t1, t2;
                    try
                    {
                        bool startTimeCheck = TimeSpan.TryParse(startime, out t1);
                        bool endTimeCheck = TimeSpan.TryParse(endtime, out t2);
                        if (startTimeCheck && endTimeCheck)
                        {
                            difference = TimeSpan.Parse(endtime) - TimeSpan.Parse(startime);
                            if (difference.Hours < 0)
                            {
                                difference = new TimeSpan(difference.Hours + 24, difference.Minutes, difference.Seconds);
                            }
                            duration = (difference.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                        loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                        loggingWriter.Close();
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
        public void setDuration(string s)
        {
            try
            {
                if (s.CompareTo("null") == 0)
                {
                    duration = s;
                    return;
                }
                Conversions objectConversions = new Conversions();
                s = objectConversions.replaceWithAppropriateShortForms(s);
                TimeSpan durationProperFormat = objectConversions.durationOrRemainderToTotalHours(s);
                duration = durationProperFormat.ToString();
                //duration = s;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }
        }
        public void setID(string s)
        {
            id = s;
        }
        public void setEventname(string s)
        {
            ErrorHandling objectErrorHandling = new ErrorHandling();
            if ((objectErrorHandling.CheckIfEventNameIsCorrect(s)) == null)
            {
                eventname = s;
            }
            else
            {
                eventname = "null";
            }
        }
        public void setlocation(string s)
        {
            location = s;
        }
        public void setStartDate(string s)
        {
            try
            {
                ErrorHandling objectErrorHandling = new ErrorHandling();
                string MessageOrDate = objectErrorHandling.CheckDate(s);
                if (MessageOrDate.CompareTo("start time value has been entered") == 0)
                    setStartTime(s);
                else
                {
                    startDate = MessageOrDate;
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }
        }
        public void setEndDate(string s)
        {
            try
            {
                Conversions objectConversions = new Conversions();
                ErrorHandling objectErrorHandling = new ErrorHandling();
                string MessageOrDate = objectErrorHandling.CheckDate(s);
                DateTime d1, d2;
                bool MessageOrDateCheck = DateTime.TryParse(MessageOrDate, out d1);
                bool startDateCheck = DateTime.TryParse(getStartDate(), out d2);

                if (startDateCheck && MessageOrDateCheck)
                {
                    if ((MessageOrDate.IndexOf('/') != -1))
                    {
                        try
                        {
                            if (objectConversions.ConvertStringToDate(MessageOrDate).CompareTo(objectConversions.ConvertStringToDate(getStartDate())) < 0)
                            {
                                MessageOrDate = getStartDate();// = objectErrorHandling.CheckDate(s); ;
                            }
                        }
                        catch (Exception e)
                        {
                            StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                            loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                            loggingWriter.Close();

                        }
                    }
                }
                if (MessageOrDate.CompareTo("start time value has been entered") == 0)
                    setEndTime(s);
                else
                {
                    endDate = MessageOrDate;
                }
            }

            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }

        }
        public void setStartTime(string s)
        {
            try
            {
                if (s.CompareTo("null") == 0)
                {
                    startime = s;
                    return;
                }

                //if (!(s.ElementAt(2) == ':' && s.ElementAt(5) == ':'))

                ErrorHandling objectErrorHandling = new ErrorHandling();
                string MessageOrDate = objectErrorHandling.checkTime(s);
                if (MessageOrDate.CompareTo("date value has been entered") == 0)
                {
                    setStartDate(s);
                    startime = "null";
                    return;
                }

                else if (MessageOrDate.CompareTo("invalid time value") == 0 || MessageOrDate.CompareTo("")==0)
                {
                    startime = MessageOrDate;
                    return;
                }
                else
                {
                    s = MessageOrDate;

                }


                Conversions objectConversions = new Conversions();
                TimeSpan startimeProperFormat = objectConversions.Convert12HourFormatTo24HourFormat(s);
                startime = startimeProperFormat.ToString();
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }
        }
        public void setEndTime(string s)
        {
            try
            {
                Conversions objectConversions = new Conversions();
                if (s.CompareTo("null") == 0)
                {
                    endtime = s;
                    return;
                }
                
                {
                    ErrorHandling objectErrorHandling = new ErrorHandling();
                    string MessageOrDate = objectErrorHandling.checkTime(s);
                    if (MessageOrDate.CompareTo("date value has been entered") == 0)
                    {
                        setEndDate(s);
                        endtime = "null";
                        return;
                    }
                    else if (MessageOrDate.CompareTo("invalid time value") == 0 || MessageOrDate.CompareTo("")==0)
                    {
                        endtime = "invalid time value";
                        return;
                    }
                    else
                    {
                        s = MessageOrDate;
                    }
                    TimeSpan endtimeProperFormat = objectConversions.Convert12HourFormatTo24HourFormat(s);
                    if (getStartTime() != "null")
                    {
                        DateTime d1;
                        DateTime d2;
                        try
                        {
                            bool startDateCheck = DateTime.TryParse(startDate, out d1);
                            bool endDateCheck = DateTime.TryParse(endDate, out d2);
                            if (startDateCheck && endDateCheck)
                            {
                                if ((objectConversions.ConvertStringToDate(startDate)).CompareTo(objectConversions.ConvertStringToDate(endDate)) == 0)
                                    if (TimeSpan.Parse(startime).CompareTo(endtimeProperFormat) < 0)
                                        endtime = endtimeProperFormat.ToString();
                            }
                        }
                        catch (FormatException f)
                        {

                            StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                            loggingWriter.WriteLine(System.DateTime.Now + f.Message + "\n");
                            loggingWriter.Close();
                        }
                    }
                    endtimeProperFormat = objectConversions.Convert12HourFormatTo24HourFormat(s);
                    endtime = endtimeProperFormat.ToString();
                }
            }
            catch (System.Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }


        }

        public void setPriority(string s)
        {
            ErrorHandling objectErrorHandling = new ErrorHandling();
            s = objectErrorHandling.CheckPriority(s);
            priority = s;
        }
        public void setReminder(string s)
        {
            if (s.CompareTo("null") == 0)
            {
                return;
            }
            Conversions objectConversions = new Conversions();
            s = objectConversions.replaceWithAppropriateShortForms(s);
            reminder = s;
        }
        public void setRepeat(string s)
        {
            //values that repeat can take = daily , weekly , biweekly , monthly,yearly
            if (s.CompareTo("null") == 0)
            {
                return;
            }
            repeat = s;
        }
        public void setEndRepeat(string s)
        {
            //endRepeat will hold a date 
            ErrorHandling objectErrorHandling = new ErrorHandling();
            string MessageOrDate = objectErrorHandling.CheckDate(s);
            if (s.CompareTo("null") == 0 || (MessageOrDate.CompareTo("start time value has been entered") == 0))
            {
                return;
            }
            else
            {
                endRepeat = MessageOrDate;
            }
        }
        public string getID()
        {
            return this.id;
        }
        public string getEventname()
        {
            return this.eventname;
        }
        public string getLocation()
        {
            return this.location;
        }
        public string getStartDate()
        {
            Conversions objectConversions = new Conversions();
            if (startDate.IndexOf('/') != -1)
            {
                return objectConversions.ConvertStringDateToddmmyyyy(this.startDate);
            }
            return startDate;
        }
        public string getEndDate()
        {
            if (endDate == "null")
                setEndDate(startDate);
            Conversions objectConversions = new Conversions();
            if (endDate.IndexOf('/') != -1)
            {
                return objectConversions.ConvertStringDateToddmmyyyy(this.endDate);
            }
            return endDate;
        }
        public string getStartTime()
        {
            return this.startime;
        }
        public string getEndTime()
        {
            return this.endtime;
        }
        public string getDuration()
        {
            return this.duration;
        }
        public string getPriority()
        {
            return this.priority;
        }
        public string getRemainderTime()
        {
            return this.reminder;
        }
        public string getRepeat()
        {
            return this.repeat;
        }
        public string getEndRepeat()
        {
            Conversions objectConversions = new Conversions();
            if (endRepeat.IndexOf('/') != -1)
            {
                return objectConversions.ConvertStringDateToddmmyyyy(this.endRepeat);
            }
            return endRepeat;
        }
    }
}
