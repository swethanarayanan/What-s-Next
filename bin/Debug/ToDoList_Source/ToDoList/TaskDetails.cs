namespace ToDoList
{
    using System;

    internal class TaskDetails
    {
        private string duration;
        private string endDate;
        private string endtime;
        private string eventname;
        private string id;
        private string location;
        private string priority;
        private string reminder;
        private string repeat;
        private string startDate;
        private string startime;

        public TaskDetails()
        {
            this.eventname = this.location = this.startime = this.endtime = this.duration = this.endDate = "null";
            this.repeat = "0D";
            this.reminder = "1H";
            this.priority = "medium";
            this.repeat = "none";
            this.startDate = DateTime.Today.ToShortDateString();
        }

        public void CalculateDuration()
        {
            TimeSpan difference = new TimeSpan();
            difference = TimeSpan.Parse(this.endtime) - TimeSpan.Parse(this.startime);
            if (difference.Hours < 0)
            {
                difference = new TimeSpan(difference.Hours + 0x18, difference.Minutes, difference.Seconds);
            }
            this.duration = difference.ToString();
        }

        public void CalculateEndTime()
        {
            TimeSpan endTime = new TimeSpan();
            this.endtime = (TimeSpan.Parse(this.startime) + TimeSpan.Parse(this.duration)).ToString();
        }

        public string getDetails()
        {
            return ("ID-" + this.id + "\nEvent Name-" + this.eventname + "\nLocation-" + this.location + "\nStart Date-" + this.startDate + "\nEnd Date-" + this.endDate + "\nStart Time-" + this.startime + "\nEnd Time-" + this.endtime + "\nDuration-" + this.duration + "\nPriority-" + this.priority + "\nReminder-" + this.reminder + "\nRepeat-" + this.repeat + "\n");
        }

        public string getDuration()
        {
            return this.duration;
        }

        public string getEndDate()
        {
            if (this.endDate == "null")
            {
                this.setEndDate(this.startDate);
            }
            return this.endDate;
        }

        public string getEndTime()
        {
            return this.endtime;
        }

        public string getEventname()
        {
            return this.eventname;
        }

        public string getID()
        {
            return this.id;
        }

        public string getLocation()
        {
            return this.location;
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

        public string getStartDate()
        {
            return this.startDate;
        }

        public string getStartTime()
        {
            return this.startime;
        }

        public string getWord(int k)
        {
            switch (k)
            {
                case 1:
                    return this.id;

                case 2:
                    return this.eventname;

                case 3:
                    return this.location;

                case 4:
                    return this.startDate;

                case 5:
                    return this.endDate;

                case 6:
                    return this.startime;

                case 7:
                    return this.endtime;

                case 8:
                    return this.duration;

                case 9:
                    return this.priority;

                case 10:
                    return this.reminder;
            }
            return " ";
        }

        public void setDuration(string s)
        {
            if (s.CompareTo("null") == 0)
            {
                this.duration = s;
            }
            else
            {
                Conversions objectConversions = new Conversions();
                s = objectConversions.replaceWithAppropriateShortForms(s);
                this.duration = objectConversions.durationOrRemainderToTotalHours(s).ToString();
            }
        }

        public void setEndDate(string s)
        {
            this.endDate = s;
        }

        public void setEndTime(string s)
        {
            if (s.CompareTo("null") == 0)
            {
                this.endtime = s;
            }
            else
            {
                TimeSpan endtimeProperFormat = new Conversions().Convert12HourFormatTo24HourFormat(s);
                this.endtime = endtimeProperFormat.ToString();
            }
        }

        public void setEventname(string s)
        {
            this.eventname = s;
        }

        public void setID(string s)
        {
            this.id = s;
        }

        public void setlocation(string s)
        {
            this.location = s;
        }

        public void setPriority(string s)
        {
            this.priority = s;
        }

        public void setReminder(string s)
        {
            if (s.CompareTo("null") != 0)
            {
                s = new Conversions().replaceWithAppropriateShortForms(s);
                this.reminder = s;
            }
        }

        public void setRepeat(string s)
        {
            if (s.CompareTo("null") != 0)
            {
                this.repeat = s;
            }
        }

        public void setStartDate(string s)
        {
            this.startDate = s;
        }

        public void setStartTime(string s)
        {
            if (s.CompareTo("null") == 0)
            {
                this.startime = s;
            }
            else
            {
                TimeSpan startimeProperFormat = new Conversions().Convert12HourFormatTo24HourFormat(s);
                this.startime = startimeProperFormat.ToString();
            }
        }
    }
}

