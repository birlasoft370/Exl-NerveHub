using System.ComponentModel.DataAnnotations;

namespace BPA.AppConfiguration.Models.Request
{
    public class CalendarDataModel
    {
        public int CalID { get; set; }

        public int CalendarID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthYear { get; set; }
        public string CalendarName { get; set; }
        public string Description { get; set; }
        public string WeekStartDay
        {
            get;
            set;
        }
        public string StartDateofMonth
        {
            get;
            set;
        }
        public string EndDateofMonth
        {
            get;
            set;
        }
        public bool Disabled
        {
            get;
            set;
        }
        public List<Calendarweek> WeekList { get; set; }
    }


    public class Calendarweek
    {
        public int? miWeek { get; set; }

        public DateTime DisplayEndDate { get; set; }

        public DateTime DisplayStartDate { get; set; }

        public string? msRowState { get; set; } = "";

        public int? miCalendarId { get; set; }
        public string StrDisplayEndDate { get; set; }

        // making data type as date   
        public string StrDisplayStartDate { get; set; }

    }
    public class CalendarDataDetails
    {
        public int CalID { get; set; }

        public int CalendarID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string? MonthYear { get; set; }
        public string? CalendarName { get; set; }
        public string? Description { get; set; }
        public string? WeekStartDay
        {
            get;
            set;
        }
        public string StartDateofMonth
        {
            get;
            set;
        } = "";
        public string? EndDateofMonth
        {
            get;
            set;
        }
        public bool Disabled
        {
            get;
            set;
        }
        public List<Calendarweek> WeekList { get; set; } = new();
    }
}
