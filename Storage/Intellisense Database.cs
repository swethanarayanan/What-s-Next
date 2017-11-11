/*********************************************************************************************************************************************
 * Author : Nivetha Sathyarajan
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * Stores all the list of words that a user has entered/used in this application 
 * Besides storing the words entered by user, it also stores the KEYWORDS(add,priority,etc) of the application so that the user can choose 
 * the keywords from the listbox , instead of trying to remember the keywords
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ToDoList
{
    class IntellisenseDatabase
    {
        private string filename;

        public IntellisenseDatabase()
        {
            filename = "intellisense.txt";
        }
        
        private void clearTheFile()
        {
            string tempFileName = "output.txt";
            using (StreamWriter writer1 = new StreamWriter(tempFileName))
                Debug.Assert(writer1 != null);
                if (File.Exists(filename))
                    File.Delete(filename);
            File.Move(tempFileName, filename);
            File.Delete(tempFileName);

        }

        private List<string> createListWithWordInsertedAtCorrectPosition(string word)
        {
            List<string> line = new List<string>();
            line = readFromFile();
            int pos = 0;
            for (int i = 0; i < line.Count(); i++)
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

        private void writeIntoFile(string contentToBeWritten)
        {
            StreamWriter writer = new StreamWriter(filename, true);
            Debug.Assert(writer != null);
            writer.Write(contentToBeWritten + Environment.NewLine);
            writer.Close();
        }

        public void addWordToIntellisenseList(string wordToBeAdded)
        {
            List<string> listOfWords = createListWithWordInsertedAtCorrectPosition(wordToBeAdded);
            clearTheFile();
            for (int i = 0; i < listOfWords.Count(); i++)
            {
                writeIntoFile(listOfWords[i]);
            }
        }

        public List<string> readFromFile()
        {
            List<string> listOfWords = new List<string>();
            using (StreamReader sr = new StreamReader(filename))
            {
                Debug.Assert(sr != null);
                String line = null;

                while ((line = sr.ReadLine()) != null)
                {
                    listOfWords.Add(line);

                }
            }
            return listOfWords;
        }
    }
}
