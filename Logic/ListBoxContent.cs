/*********************************************************************************************************************************
 * Author : Nivetha Sathyarajan
 * -----------------------------------Descripton---------------------------------------------------------------------------------- 
 * searchForsubString() 
 * Parameters taken : takes in the sequence of character following the last input space character in the command box.
 * It then interacts with the intellisense database to retrieve all the list of words that contain the parameter as a substring
 * addItemToListBox()
 * Parameter taken : takes in the sequence of character following the last input space character in the command box.
 * Checks if the parameter exists as a substring/string in the database
 * Returns the list of strings that contain parameter otherwise adds the parameter to the intellisense database
 *********************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ToDoList
{
    class ListBoxContent
    {
        private List<string> searchForsubString(string subString)
        {
            try
            {
                List<string> listOfWordsReadfromFile = new List<string>();
                IntellisenseDatabase objectIntellisenseDatabase = new IntellisenseDatabase();
                listOfWordsReadfromFile = objectIntellisenseDatabase.readFromFile();
                List<string> results = new List<string>(); //stores all strings that contain the subString in ascending order
                for (int i = 0; i < listOfWordsReadfromFile.Count(); i++)
                {
                    if (listOfWordsReadfromFile[i].IndexOf(subString, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(listOfWordsReadfromFile[i]);
                    }
                }
                return results;
            }
            catch (Exception e)
            {
                StreamWriter loggingWriter = new StreamWriter("Logging.txt");
                loggingWriter.WriteLine(System.DateTime.Now + e.Message + "\n");
                loggingWriter.Close();
                return null;

            }
        }

        // parameter 's' has the sequence of characters entered by user in the textbox.
        //the function calls searchForsubString() to retrieve the list of words that contain the value in s
        public List<string> addItemToListBox(string s)
        {
            try
            {
                List<string> listOfwords = new List<string>();  //stores the list of words that are to appear in the listbox
                s = s.Replace("\"", " ");
                s = s.Trim();
                listOfwords = searchForsubString(s);
                if (listOfwords.Count() == 0)
                {
                    listOfwords.Add(s);
                }
                return listOfwords;
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
