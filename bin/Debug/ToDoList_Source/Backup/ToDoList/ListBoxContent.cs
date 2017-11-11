namespace ToDoList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ListBoxContent
    {
        public List<string> addItemToListBox(string s)
        {
            List<string> listOfwords = new List<string>();
            s = s.Replace("\"", " ");
            s = s.Trim();
            listOfwords = this.searchForsubString(s);
            if (listOfwords.Count<string>() == 0)
            {
                listOfwords.Add(s);
            }
            return listOfwords;
        }

        private List<string> searchForsubString(string subString)
        {
            List<string> listOfWordsReadfromFile = new List<string>();
            listOfWordsReadfromFile = new IntellisenseDatabase().readFromFile();
            List<string> results = new List<string>();
            for (int i = 0; i < listOfWordsReadfromFile.Count<string>(); i++)
            {
                if (listOfWordsReadfromFile[i].IndexOf(subString, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(listOfWordsReadfromFile[i]);
                }
            }
            return results;
        }
    }
}

