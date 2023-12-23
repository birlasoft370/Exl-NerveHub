using BPA.GlobalResources.UI.AppConfiguration;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;
using MicUI.WorkManagement.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class CalendarDataModel : Calendarweek
    {

        public IList<BECalendarInfo> mCalendarList = new List<BECalendarInfo>();

        public string Description { get; set; }
        public string CalendarName { get; set; }
        public int MaxWeek { get; set; }

        public int iCalID { get; set; }

        public int iCalendarID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredCalendar")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayCalendar")]
        public string mCalendarDate
        {
            get;
            set;
        }

        //[Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredCalendar", sControlKeyCollection = "btnSearch")]
        public string mCalendarDateSearch
        {
            get;
            set;
        }

        //[Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredMonth")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayMonth")]
        public string mMonth
        {
            get;
            set;
        }

        //[Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredMonth", sControlKeyCollection = "btnSearch")]
        public string mMonthSearch
        {
            get;
            set;
        }


        //[Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredYear")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayYear")]
        public string mYear
        {
            get;
            set;
        }

        //[Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredYear", sControlKeyCollection = "btnSearch")]
        public string mYearSearch
        {
            get;
            set;
        }


        [Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredWeekStartDay")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayWeekStartDay")]
        public string mWeekStartDay
        {
            get;
            set;
        }





        [Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredStartDateofMonth")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayStartDateofMonth")]
        public string mStartDateofMonth
        {
            get;
            set;
        }


        [Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredEndDateofMonth")]
        [Display(ResourceType = typeof(Resources_Calender), Name = "displayEndDateofMonth")]
        public string mEndDateofMonth
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resources_Calender), Name = "displayDisable")]
        public bool Disable
        {
            get;
            set;

        }

        [Required(ErrorMessageResourceType = typeof(Resources_Calender), ErrorMessageResourceName = "requiredWeekList")]
        public IList<Calendarweek> WeekList = new List<Calendarweek>();

        public int WeekCounter { get; set; }

        public DataTable DTWeek
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resources_Calender), Name = "displayMonth")]
        public string mMonthYear { get; set; }
    }

    [Serializable]
    public class Calendarweek : ViewModelValidationHelper
    {
        public Calendarweek()
        { }

        public int miWeek { get; set; }

        [DataType(DataType.Date)] // making data type as date     
        public DateTime DisplayEndDate { get; set; }

        [DataType(DataType.Date)] // making data type as date   
        public DateTime DisplayStartDate { get; set; }

        // making data type as date     
        public string StrDisplayEndDate { get; set; }

        // making data type as date   
        public string StrDisplayStartDate { get; set; }

        public string? msRowState { get; set; }

        public int miCalendarId { get; set; }

        //  public IEnumerable<object> data { get; set; }
    }

    public class CalendarDataDetails
    {
        public int CalID { get; set; }

        public int? CalendarID { get; set; }
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
        public string? StartDateofMonth
        {
            get;
            set;
        }
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
        public List<Calendarweek> WeekList { get; set; }
    }




}