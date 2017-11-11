using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ToDoList
{
    class ErrorHandling
    {
        private string errorMessage;
        private const string INVALIDDAY = "invalid day";
        private const string INVALIDMONTH = "invalid month";
        private const string INVALIDVALUE = "invlaid value";
        private const string STARTTIME = "starttime value ";

        public ErrorHandling()
        {
            errorMessage = null;
        }
        public void checkIfTimeFormat(string s)
        {
            int indexOfAM = s.IndexOf("am");
            int indexOfam = s.IndexOf("AM");
            int indexOfPM = s.IndexOf("pm");
            int indexOfpm = s.IndexOf("PM");

            Conversions objectConversions = new Conversions();
            if (indexOfam != -1 || indexOfAM != -1 || indexOfpm != -1 || indexOfPM != -1)
            {
                TimeSpan startimeProperFormat = objectConversions.Convert12HourFormatTo24HourFormat(s);
                errorMessage = STARTTIME;
            }
           
        }
        public string CheckDate(string s)
        {
            checkIfTimeFormat(s);
            if (errorMessage != null)
            {
                return errorMessage;
            }
            //replace '-' '.' ',' and '\' with '/'
            
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i]))
                {
                    s = s.Replace(s[i].ToString(), "/");
                }
            }
            //remove extra special characters..example // or .. etc
            string[] s_date = s.Split('/');
            string s_temp = null;
            if ((s_date[s_date.Count()-1]) == "")
            {
                s_date[s_date.Count() - 1] = System.DateTime.Today.Year.ToString();
            }
            else if (s_date.Count() == 2)
            {
                s += "/" + System.DateTime.Today.Year.ToString();
                s_date = s.Trim().Split('/');
            }
            else if (s_date.Count() != 3)
            {
                errorMessage = INVALIDVALUE;
                return errorMessage;
            }
            int k = 0;
            for (int i = 0; i < s_date.Count(); i++)
            {
                if (s_date[i] == "")
                { 
                  errorMessage = INVALIDVALUE;
                  return errorMessage;
                }
                s_temp += s_date[i];
                k++;
                if (k <= 2)
                  s_temp += "/";
                
            }
            if (s_temp != null)
            {
                s = s_temp;
            }
            //convert into mmddyyyy if entered in ddmmyyyy format
            string[] pattern = { "M/dd/yyyy", "M/d/yyyy", "M/dd/yy", "M/d/yy", "MM/dd/yyyy","MM/d/yyyy","MM/dd/yy","MM/d/yy", 
                                 "dd/MM/yyyy","dd/MM/yy", "dd/M/yyyy","dd/M/yy", "d/M/yyyy", "d/M/yy","d/MM/yyyy","d/MM/yy" };
            DateTime parsedDate;
            bool check = DateTime.TryParseExact(s, pattern, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            if (check)
                s = parsedDate.ToShortDateString();
            //check if date is less than 31 and check if month is less than 12
            CheckForValidDateAndMonth(s);
            if (errorMessage != null)
            {
                return errorMessage;
            }
            if (DateTime.Parse(s).CompareTo((DateTime.Parse(DateTime.Today.ToShortDateString()))) < 0)
            {
                s = DateTime.Today.ToShortDateString();
            }
            return s;
        }

        private void CheckForValidDateAndMonth(string s)
        {
            //DateTime date = DateTime.Parse(s);
           
            string[] date = s.Trim().Split('/');
            
            int dd = Convert.ToInt32(date[0]);
            int mm = Convert.ToInt32(date[1]);
            int yy = Convert.ToInt32(date[2]); 

            string[] pattern = { "MM/dd/yyyy" };
            DateTime parsedDate;
            bool check = DateTime.TryParseExact(DateTime.Today.ToShortDateString(), pattern, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            if (check) // if check is false system date is in mmddyyyy format
            {
                mm = Convert.ToInt32(date[0]);
                dd = Convert.ToInt32(date[1]);
            }
           
                
            int[] month = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (yy % 4 == 0)
                month[2] += 1;
            if (mm >= 1 && mm <= 12)
            {
                if (!(dd >= 1 && dd <= month[mm]))
                {
                    errorMessage = INVALIDDAY;
                }
            }
            else
            {
                errorMessage = INVALIDMONTH;
            }
            
        }
    }
}

