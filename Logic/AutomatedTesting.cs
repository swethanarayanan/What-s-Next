/********************************************************************************************************
 * Author : Swetha Narayanan
 * -----------------------------------Descripton--------------------------------------------------------- 
 * This class does system testing
 * Various user inputs are listed in AutomatedTesting.txt
 * The expected output for every input in AutomatedTesting.txt is listed in AutomatedTestingOutput.txt
 * AutomatedEnter(input string) reads the AutomatedTesting.txt one line at a time and tries to match with the
 * expected output in AutomatedTestingOutput.txt
 * If the ouput and expected output do not match, then the error is logged in logging.txt
 *******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ToDoList
{
    public class AutomatedTesting
    {     

        public void StartTesting()
        {
            clearListOfTasksToDofile();
            StreamReader file1 = new StreamReader("AutomatedTesting.txt");
            Debug.Assert(file1 != null);
            StreamWriter file2 = new StreamWriter("AutomatedTestingOutput.txt");
            Debug.Assert(file2 != null);
            string line;
            while ((line = file1.ReadLine()) != null)
            {          
                
                file2.WriteLine(AutomatedEnter(line));
            }
            file2.Close();
            file1.Close();
        }
        private string AutomatedEnter(string line)
        {
           
            Parser objectTextUI = new Parser(line);
            objectTextUI.CheckIfCommandAndContentValid();
         
                
                 objectTextUI.taskToBePerformed();
                return objectTextUI.getDisplay();
            
        }
        private void clearListOfTasksToDofile()
        {

            StreamWriter file1 = new StreamWriter("ListOfTasksToDoFile.txt");
            Debug.Assert(file1 != null);
            file1.WriteLine(string.Empty);
            file1.Close();

        }
       
      

    }
}
