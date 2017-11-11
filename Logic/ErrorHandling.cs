/********************************************************************************************************
 * Author : Nivetha Sathyarajan , Monika  Puhazhendhi
 * -----------------------------------Descripton--------------------------------------------------------- 
 * This class is meant for handling errors in case the user enters any invalid parameter 
 *******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;

namespace ToDoList
{
    class ErrorHandling
    {
        private string errorMessage;
        private const string INVALIDDAY = "invalid day";
        private const string INVALIDMONTH = "invalid month";
        private const string INVALIDVALUE = "invalid value";
        private const string STARTTIME = "start time value has been entered";
        private const string VALIDID = "Please provide valid ID";
        private const string DATEFORMAT = "date value has been entered";
        private const string INVALIDTIME = "invalid time value";
        private const string INVALIDEVENT = "Please enter a valid Event Name";

        public ErrorHandling()
        {
            errorMessage = null;
        }
        public string CheckIfEventNameIsCorrect(string s)
        {
            try
            {
                int count = s.Trim().Length - s.Trim().Replace("\"", "").Length - s.Trim().Replace(" ", "").Length;
                if (count == 0)
                {
                    errorMessage = INVALIDEVENT;
                }
                return errorMessage;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                errorMessage = "error!";
                return errorMessage;
            }
        }
        public List<int> HandleInvalidID(List<int> id, int maxValue)
        {
            try
            {

                for (int i = 0; i < id.Count(); i++)
                {
                    if (id[i] < 1 || id[i] > maxValue)
                    {
                        id.RemoveAt(i);
                        i--;
                    }
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
        public string CheckIfANumber(string[] s, int maxValue)
        {
            try
            {
                List<string> ids = s.ToList<string>();
                for (int i = 0; i < ids.Count(); i++)
                {
                    if (ids[i].Trim() == "")
                    {
                        ids.RemoveAt(i);
                        i--;
                    }
                    else if (Convert.ToInt32(ids[i].Trim()) < 1 || Convert.ToInt32(ids[i].Trim()) > maxValue)
                    {
                        ids.RemoveAt(i);
                        i--;
                    }
                }
                if (ids.Count() == 0)
                {
                    errorMessage = VALIDID;
                    return errorMessage;
                }
                return null;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                errorMessage = "error!";
                return errorMessage;
            }
        }
        public string CheckIfDeleteParameterInProperFormat(string s)
        {
            //replace '-' '.' ',' and '\' with '/'
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (!char.IsDigit(s[i]))
                    {
                        s = s.Replace(s[i].ToString(), " ");
                    }
                }
                return s;
                // return null;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;
            }
        }
        public void checkIfTimeFormat(string s)
        {
            try
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
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }

        }

        public string CheckDate(string s)
        {
            try
            {
                checkIfTimeFormat(s);
                if (errorMessage != null)
                {
                    return errorMessage;
                }
                //replace '-' '.' ',' and '\' with '/'

                for (int i = 0; i < s.Trim().Length; i++)
                {
                    if (!char.IsDigit(s[i]))
                    {
                        s = s.Replace(s[i].ToString(), "/");
                    }
                }
                //remove extra special characters..example // or .. etc
                string[] s_date = s.Split('/');
                string s_temp = null;
                if ((s_date[s_date.Count() - 1]) == "")
                {
                    s_date[s_date.Count() - 1] = System.DateTime.Today.Year.ToString();
                }
                else if (s_date.Count() == 2)
                {
                    s += "/" + System.DateTime.Today.Year.ToString();
                    s_date = s.Trim().Split('/');
                }
                if (s_date.Count() != 3)
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
                s = CheckForValidDateAndMonth(s);
                Conversions objectConversions = new Conversions();
                if (errorMessage != null)
                {
                    return errorMessage;
                }
                if (objectConversions.ConvertStringToDate(s).CompareTo((objectConversions.ConvertStringToDate(objectConversions.ConvertSystemDateToddmmyyyy()))) < 0)
                {
                    s = objectConversions.ConvertSystemDateToddmmyyyy();
                }
                return s;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                errorMessage = "error!";
                return errorMessage;
            }
        }

        private string CheckForValidDateAndMonth(string s)
        {
            //DateTime date = objectConversions.ConvertStringToDate(s);
            int d,m,y;
            try
            {

                string[] date = {"0","0","0"};
                string[] temp_date = s.Trim().Split('/');
                int j = 0;
                for (int i = 0; i < temp_date.Count(); i++)
                {
                    if (!(temp_date[i].CompareTo("") == 0) && j<3)
                    {
                        date[j] = temp_date[i];
                        j++;
                    }
                }
                    try
                    {

                        bool yearvalueCheck = Int32.TryParse(date[2], out y);
                        bool monthvalueCheck = Int32.TryParse(date[1], out m);
                        bool datevalueCheck = Int32.TryParse(date[0], out d);
                        if (yearvalueCheck && monthvalueCheck && datevalueCheck)
                        {
                            if (!(Convert.ToInt32(date[2]) >= System.DateTime.Now.Year && Convert.ToInt32(date[2]) /10000 == 0))
                                date[2] = "" + System.DateTime.Now.Year; ;
                            s = date[0] + "/" + date[1] + "/" + date[2];
                            int dd = Convert.ToInt32(date[0]);
                            int mm = Convert.ToInt32(date[1]);
                            int yy = Convert.ToInt32(date[2]);

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
                            else if (mm < 1 || mm > 12)
                            {
                                errorMessage = INVALIDMONTH;
                            }

                        }

                        return s;
                    }
                    catch (Exception i)
                    {
                        StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                        loggingWriter.WriteLine(System.DateTime.Now + i.Message + "\n");
                        loggingWriter.Close();
                        errorMessage = "error!";
                        return errorMessage;
                    }
                
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                errorMessage = "error!";
                return errorMessage;
            }

        }
        public void checkIfDateFormat(string s)
        {
            try
            {
                if (s.IndexOf("AM") == -1 && s.IndexOf("am") == -1 && s.IndexOf("PM") == -1 & s.IndexOf("pm") == -1)
                    if (s.IndexOf('/') != -1)
                        errorMessage = DATEFORMAT;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }

        }
        public string checkTime(string s)
        {
            try
            {
                //check for date values
                checkIfDateFormat(s);
                if (errorMessage != null)
                {
                    return errorMessage;
                }
                //replace all special characters with ' '
                string temp = s;
                for (int i = 0; i < s.Trim().Length; i++)
                {
                    if ((!char.IsDigit(s[i])) && s[i] != '.' && s[i] != ':')
                    {
                        if (i + 1 < s.Trim().Length)
                        {
                            string checkIfAMorPM = "" + s[i] + s[i + 1];
                            if (checkIfAMorPM.CompareTo("AM") != 0 && checkIfAMorPM.CompareTo("am") != 0 && checkIfAMorPM.CompareTo("PM") != 0 && checkIfAMorPM.CompareTo("pm") != 0)
                                temp = temp.Replace(s[i].ToString(), "");

                            else
                                i++;
                        }
                        else
                            temp = temp.Replace(s[i].ToString(), "");

                    }

                }
                s = temp;
                //replace extra "." and ":" with ":"
                s = s.Replace('.', ':');
                int ExtraSymbolCount = 0;
                for (int i = 0; i < s.Trim().Length; i++)
                {
                    if (s[i] == ':')
                    {
                        ExtraSymbolCount++;

                        if (ExtraSymbolCount >= 3 || (s[i] == ':' && s[i - 1] == ':'))
                        {
                            s = s.Remove(i, 1);
                            if ((s[i] == ':' && s[i - 1] == ':'))
                                i--;
                        }

                    }
                }

                //check for valid time values
                string TimeArgForValidation = "";
                int j;
                for (j = 0; j < s.Length; j++)
                {
                    if (s[j] == 'a' || s[j] == 'p' || s[j] == 'm')
                        continue;
                    TimeArgForValidation += s[j];

                }

                if (checkValidTime(TimeArgForValidation) != null)
                    return errorMessage;


                return s;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                errorMessage = "error!";
                return errorMessage;
            }
        }
        public string checkValidTime(string s)
        {
            try
            {
                if (s != "")
                {
                    string[] time = { "0", "0", "0" };
                    string[] temp_time = s.Trim().Split(':');
                    int j = 0;
                    for (int i = 0; i < temp_time.Count(); i++)
                    {
                        if (!(temp_time[i].CompareTo("") == 0) && j < 3)
                        {
                            time[j] = temp_time[i];
                            j++;
                        }
                    }
                   
                    int hr = 0, min = 0, sec = 0;
                    for (int i = 0; i < time.Count(); i++)
                    {
                      
                            int second,m,h;
                            try
                            {

                                bool secCheck = Int32.TryParse(time[0], out second);
                                bool minCheck = Int32.TryParse(time[1], out m);
                                bool hrCheck = Int32.TryParse(time[2], out h);
                                if (secCheck && minCheck && hrCheck)
                                {
                                    switch (i)
                                    {
                                        case 0: hr = Convert.ToInt32(time[0]);
                                            break;
                                        case 1: min = Convert.ToInt32(time[1]);
                                            break;
                                        case 2: sec = Convert.ToInt32(time[2]);
                                            break;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                                loggingWriter.Close();
                                errorMessage = "error!";
                                return errorMessage;
                            }
                    }

                    if (!(hr >= 0 && hr <= 23))
                    {
                        errorMessage = INVALIDTIME;
                        return errorMessage;
                    }
                    else if (!(min >= 0 && min <= 59))
                    {
                        errorMessage = INVALIDTIME;
                        return errorMessage;
                    }
                    else if (!(sec >= 0 && sec <= 59))
                    {
                        errorMessage = INVALIDTIME;
                        return errorMessage;
                    }


                }
                return null;
            }

            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                errorMessage = "error!";
                return errorMessage;
            }
        }

        public void checkEndTimeWithStartTime(string start, ref string end, ref string startime, ref string endtime)
        {
            try
            {

                string TimeArgForValidation = "";
                int j;
                for (j = 0; j < start.Length; j++)
                {
                    if (start[j] == 'a' || start[j] == 'p' || start[j] == 'm')
                        continue;
                    TimeArgForValidation += start[j];

                }
                string[] time = { "0", "0", "0" };
                string[] Starttime = TimeArgForValidation.Split(':');
                int k = 0;
                for (int i = 0; i < Starttime.Count(); i++)
                {
                    if (!(Starttime[i].CompareTo("") == 0) && k < 3)
                    {
                        time[k] = Starttime[i];
                        k++;
                    }
                }

           
                int Starthr = 0, Startmin = 0, Startsec = 0;
                int h, m, second;
                for (int i = 0; i < Starttime.Count(); i++)
                {
                    bool hrCheck = Int32.TryParse(time[0], out h);
                    bool minCheck = Int32.TryParse(time[1], out m);
                    bool secCheck = Int32.TryParse(time[2], out second);
                    if (secCheck && minCheck && hrCheck)
                    {

                        switch (i)
                        {
                            case 0:
                                Starthr = Convert.ToInt32(Starttime[0]);
                                break;
                            case 1: Startmin = Convert.ToInt32(Starttime[1]);
                                break;
                            case 2: Startsec = Convert.ToInt32(Starttime[2]);
                                break;
                        }
                    }
                }
                TimeArgForValidation = "";
                for (j = 0; j < end.Length; j++)
                {
                    if (end[j] == 'a' || end[j] == 'p' || end[j] == 'm')
                        continue;
                    TimeArgForValidation += end[j];

                }

                string[] Endtime = TimeArgForValidation.Split(':');
                int Endhr = 0, Endmin = 0, Endsec = 0;
                for (int i = 0; i < Endtime.Count(); i++)
                {
                    switch (i)
                    {
                        case 0: Endhr = Convert.ToInt32(Endtime[0]);
                            break;
                        case 1: Endmin = Convert.ToInt32(Endtime[1]);
                            break;
                        case 2: Endsec = Convert.ToInt32(Endtime[2]);
                            break;
                    }
                }
                TimeSpan t1 = new TimeSpan(Starthr, Startmin, Startsec);
                TimeSpan t2 = new TimeSpan(Endhr, Endmin, Endsec);

                if (TimeSpan.Compare(t1, t2) > 0)
                {

                    TimeSpan newEndTime = t1 + new TimeSpan(1, 0, 0);
                    end = newEndTime.ToString();
                }
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();

            }



        }





        public string CheckPriority(string s)
        {
            try
            {
                if (s.IndexOf('h') != -1 || s.IndexOf('g') != -1 || s.IndexOf('H') != -1 || s.IndexOf('G') != -1)
                {
                    s = "high";
                }
                else if (s.IndexOf('m') != -1 || s.IndexOf('M') != -1 || s.IndexOf('d') != -1 || s.IndexOf('D') != -1 || s.IndexOf('u') != -1 || s.IndexOf('U') != -1)
                {
                    s = "medium";
                }
                else if (s.IndexOf('l') != -1 || s.IndexOf('L') != -1 || s.IndexOf('o') != -1 || s.IndexOf('O') != -1 || s.IndexOf('w') != -1 || s.IndexOf('W') != -1)
                {
                    s = "low";
                }
                else if (string.Compare(s, "high") != 0 || string.Compare(s, "medium") != 0 || string.Compare(s, "low") != 0)
                {
                    s = "medium";
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
    }
}


 