/*********************************************************************************************************************************************
 * Author : Swetha Narayanan
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * Binds its objects to the datagrid corresponding to task display 
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoList
{

    public class Event 
    {
        public string Event_ID { get; set; }
        public string Event_Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Priority { get; set; }
        public string Remind_Me_Before { get; set; }

        public Event(string eventID, string eventName, string start, string end, string priority, string remindMeBefore)
        {
            Event_ID = eventID;
            Event_Name = eventName;
            Start = start;
            End = end;
            Priority = priority;
            Remind_Me_Before = remindMeBefore;
        }
        public Event()
        {

        }
    };
}


