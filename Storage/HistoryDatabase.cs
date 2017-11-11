/*********************************************************************************************************************************************
 * Author : Monika  Puhazhendhi
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * Stores all the details of completed tasks in the file
 * Each task detail is separated by a semi-colon(;)
 * Task details are stored in the following order : event;at;start;end;from;to;duration;priority;reminder
 * The file gets refreshed(cleared) every month
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace ToDoList
{
    class HistoryDatabase
    {
        private String CompletedFileList;


        public HistoryDatabase()
        {
            CompletedFileList = @"Completed.txt";
        }

        public int UpdateCompletedFileList()
        {
            int NumberOfEventsStoredInHistory = 0;
            StreamReader reader = new StreamReader(CompletedFileList);
            Debug.Assert(reader != null);
            String EventReadFromArgumentFile = reader.ReadLine();
            while (EventReadFromArgumentFile != null)
            {
                NumberOfEventsStoredInHistory++;
                EventReadFromArgumentFile = reader.ReadLine();

            }
            reader.Close();

            return NumberOfEventsStoredInHistory;
        }

        public void ClearCompletedFileList()
        {
            StreamWriter writer = new StreamWriter(CompletedFileList);
            Debug.Assert(writer != null);
            writer.Close();
        }

        public List<TaskDetails> ViewCompletedEvents()
        {
            List<TaskDetails> listOfTaskDetails;
            List<string> listOfCompletedEvents = new List<string>();
            if (UpdateCompletedFileList() == 0)
            {
                return null;
            }
            StreamReader reader = new StreamReader("Completed.txt");
            Debug.Assert(reader != null);
            String CompletedEvent = reader.ReadLine();
            while (CompletedEvent != null)
            {
                listOfCompletedEvents.Add(CompletedEvent);
                CompletedEvent = reader.ReadLine();
            }
            Conversions objectConversions = new Conversions();
            listOfTaskDetails = objectConversions.convertStringIntoTaskDetails(listOfCompletedEvents);

            reader.Close();
            if (UpdateCompletedFileList() > 10)
                ClearCompletedFileList();
            return listOfTaskDetails;
        }
        public int EventCountForCurrentMonth()
        {
            StreamReader reader = new StreamReader(CompletedFileList);
            Debug.Assert(reader != null);
            int i = 0, CompletedEventsForCurrentMonth = 0;
            List<String> CompletedEvents = new List<String>();
            String EventReadFromFile = reader.ReadLine();
            while (EventReadFromFile != null)
            {
                CompletedEvents.Add(EventReadFromFile);
                i++;
                EventReadFromFile = reader.ReadLine();
            }
            reader.Close();
            List<TaskDetails> TaskDetailsobj = new Conversions().convertStringIntoTaskDetails(CompletedEvents);
            for (i = 0; i < CompletedEvents.Count; i++)
            {
                String EventEndDateInStringFormat = TaskDetailsobj[i].getEndDate();
                CompletedEventsForCurrentMonth++;
            }
            return CompletedEventsForCurrentMonth;
        }
    }
}
