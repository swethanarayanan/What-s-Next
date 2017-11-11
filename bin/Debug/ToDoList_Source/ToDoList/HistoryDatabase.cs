namespace ToDoList
{
    using System;
    using System.IO;
    using System.Windows;

    internal class HistoryDatabase
    {
        private string CompletedFileList = "Completed.txt";

        public void ClearCompletedFileList()
        {
            new StreamWriter(this.CompletedFileList).Close();
        }

        public int UpdateCompletedFileList(string fileName)
        {
            int NumberOfEventsStoredInHistory = 0;
            MessageBox.Show("entered update()");
            StreamReader reader = new StreamReader(fileName);
            StreamWriter writer = new StreamWriter(this.CompletedFileList, true);
            string EventReadFromArgumentFile = reader.ReadLine();
            while (EventReadFromArgumentFile != null)
            {
                writer.WriteLine(EventReadFromArgumentFile);
                EventReadFromArgumentFile = reader.ReadLine();
                NumberOfEventsStoredInHistory++;
            }
            reader.Close();
            writer.Close();
            return NumberOfEventsStoredInHistory;
        }

        public void ViewCompletedEvents()
        {
            History HistoryWindow = new History();
            StreamReader reader = new StreamReader("Completed.txt");
            for (string CompletedEvent = reader.ReadLine(); CompletedEvent != null; CompletedEvent = reader.ReadLine())
            {
                HistoryWindow.SetEventDetails(CompletedEvent);
            }
            reader.Close();
            HistoryWindow.Show();
        }
    }
}

