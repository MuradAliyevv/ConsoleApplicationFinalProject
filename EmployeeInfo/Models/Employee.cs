using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeInfo.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime JobStartDate { get; set; }
        public string Position { get; set; }
        public double WageRate { get; set; }
        public int MonthlyTotalTime { get; set; }

        public override string ToString()
        {
            return $"{this.EmployeeId}    {this.Name} {this.Surname}    {this.JobStartDate.ToShortDateString()}    {this.Position}    {this.WageRate}";
        }
    }
}
