namespace ToDoList
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class IntellisenseDatabase
    {
        private string filename = "intellisense.txt";

        public void addWordToIntellisenseList(string wordToBeAdded)
        {
            List<string> listOfWords = this.createListWithWordInsertedAtCorrectPosition(wordToBeAdded);
            this.clearTheFile();
            for (int i = 0; i < listOfWords.Count<string>(); i++)
            {
                this.writeIntoFile(listOfWords[i]);
            }
        }

        private void clearTheFile()
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

        private List<string> createListWithWordInsertedAtCorrectPosition(string word)
        {
            List<string> line = new List<string>();
            line = this.readFromFile();
            int pos = 0;
            for (int i = 0; i < line.Count<string>(); i++)
            {
                if (string.Compare(line[i].ToUpper(), word.ToUpper()) == -1)
                {
                    pos = i + 1;
                }
                else if (string.Compare(line[i].ToUpper(), word.ToUpper()) == 0)
                {
                    line.RemoveAt(i);
                    pos = i;
                }
                else
                {
                    break;
                }
            }
            line.Insert(pos, word);
            return line;
        }

        public List<string> readFromFile()
        {
            List<string> listOfWords = new List<string>();
            using (StreamReader sr = new StreamReader(this.filename))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    listOfWords.Add(line);
                }
            }
            return listOfWords;
        }

        private void writeIntoFile(string contentToBeWritten)
        {
            StreamWriter writer = new StreamWriter(this.filename, true);
            writer.Write(contentToBeWritten + Environment.NewLine);
            writer.Close();
        }
    }
}

