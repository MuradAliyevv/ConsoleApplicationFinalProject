using System;

namespace EmployeeInfo.Models
{
    public class Worktime
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int WeekDay { get; set; }
        public int EntryHour { get; set; }
        public int EntryMinute { get; set; }
        public int DepartureHour { get; set; }
        public int DepartureMinute { get; set; }
        public DateTime Date { get; set; }

        public Employee Employee { get; set; }
    }
}
