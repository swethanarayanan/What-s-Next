/*********************************************************************************************************************************************
 * Author : Nivetha Sathyarajan,Swetha Narayanan
 * -----------------------------------Descripton---------------------------------------------------------------------------------------------
 * Stores all the details of a task in the file
 * Each task detail is separated by a semi-colon(;)
 * Task details are stored in the following order : event;at;start;end;from;to;duration;priority;reminder;repeat
 * In case of undo/redo, every snapshot begins with a pattern - "*******".Thereby, this pattern helps to separate every snapshot in the file
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ToDoList
{
    class Database
    {
        private string filename;

        public Database(string sFileName)
        {
            filename = sFileName;
        }
        private int LineNumberAssociatedWithTheText(string s)
        {
            int LineNumber = 1;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    LineNumber = Convert.ToInt32(s[i]);
                    break;
                }
            }
            return LineNumber;
        }
        public void ClearAllTheContentFromTheFile()
        {
            string tempFileName = "output.txt";
            using (StreamWriter writer = new StreamWriter(tempFileName))
                Debug.Assert(writer != null);
                if (File.Exists(filename))
                    File.Delete(filename);
            File.Move(tempFileName, filename);
            File.Delete(tempFileName);
        }
        public void writeIntoFile(string contentToBeWritten)
        {
            StreamWriter writer = new StreamWriter(filename, true);
            Debug.Assert(writer != null);
            writer.Write(contentToBeWritten + Environment.NewLine);
            writer.Close();
        }
        public List<TaskDetails> readFromFile()
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();

            using (StreamReader sr = new StreamReader(filename))
            {
                Debug.Assert(sr != null);
                String line;
                int i = 1;
                int j = 1;
                // Read and display lines from the file until the end of
                // the file is reached.

                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    if (!(line.Contains("*******")) && line.CompareTo("")!=0)
                    {
                        j++;
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
                        i++;
                        listOfTasksInFile.Add(task);
                    }
                    else
                    {
                        listOfTasksInFile.RemoveRange(0, listOfTasksInFile.Count());
                    }
                }
                sr.Close();
            }
            return listOfTasksInFile;

        }
    }
}

