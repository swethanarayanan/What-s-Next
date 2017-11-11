namespace ToDoList
{
    using System;
    using System.Runtime.CompilerServices;

    public class Event
    {
        public bool delete { get; set; }

        public bool edit { get; set; }

        public string EndDate { get; set; }

        public string endTime { get; set; }

        public string EventName { get; set; }

        public string ID { get; set; }

        public string priority { get; set; }

        public string reminderBefore { get; set; }

        public string StartDate { get; set; }

        public string startTime { get; set; }
    }
}

