namespace ToDoList
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Database
    {
        private string filename;

        public Database(string sFileName)
        {
            this.filename = sFileName;
        }

        public void ClearAllTheContentFromTheFile()
        {
            string tempFileName = "output.txt";
            using (new StreamWriter(tempFileName))
            {
                if (File.Exists(this.filename))
                {
                    File.Delete(this.filename);
                }
            }
            File.Move(tempFileName, this.filename);
            File.Delete(tempFileName);
        }

        private int LineNumberAssociatedWithTheText(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    return Convert.ToInt32(s[i]);
                }
            }
            return 1;
        }

        public List<TaskDetails> readFromFile()
        {
            List<TaskDetails> listOfTasksInFile = new List<TaskDetails>();
            using (StreamReader sr = new StreamReader(this.filename))
            {
                string line;
                int i = 1;
                int j = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!(line.Contains("*******") || (line.CompareTo("") == 0)))
                    {
                        j++;
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
                        i++;
                        listOfTasksInFile.Add(task);
                    }
                    else
                    {
                        listOfTasksInFile.RemoveRange(0, listOfTasksInFile.Count<TaskDetails>());
                    }
                }
                sr.Close();
            }
            return listOfTasksInFile;
        }

        public void writeIntoFile(string contentToBeWritten)
        {
            StreamWriter writer = new StreamWriter(this.filename, true);
            writer.Write(contentToBeWritten + Environment.NewLine);
            writer.Close();
        }
    }
}

