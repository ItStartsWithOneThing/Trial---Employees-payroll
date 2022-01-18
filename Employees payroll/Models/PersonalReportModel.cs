using System;
using System.Collections.Generic;
using System.Text;

namespace Employees_payroll.Models
{
    [Serializable]
    public class PersonalReportModel
    {
        DateTime date;
        int hours;
        string taskDescription;

        public DateTime Date { get { return date; } set { date = value; } }
        public int Hours { get { return hours; } set { hours = value; } }
        public string TaskDescription { get { return taskDescription; } set { taskDescription = value; } }

        public PersonalReportModel(DateTime currentDate, int currentHours, string currentDescription)
        {
            Date = currentDate;
            Hours = currentHours;
            TaskDescription = currentDescription;
        }
    }
}
