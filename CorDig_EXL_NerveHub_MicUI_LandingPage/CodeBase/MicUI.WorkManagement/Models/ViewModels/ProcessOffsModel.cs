using BPA.GlobalResources.UI.WorkManagement;
using BPA.GlobalResources.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.AccessControl;
using System.Xml.Linq;
using MicUI.WorkManagement.Services.ServiceModel;
using MicUI.WorkManagement.Helper.CustomValidationAttributes;

namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class ProcessOffsModel : ViewModelValidationHelper
    {

        public IList<BEProcessOff> BEProcessOffList = new List<BEProcessOff>();
        public bool IsEditMode
        {
            get;
            set;
        }
        public int iprocessOffId
        {
            get;
            set;
        }
        public string SubmitMode
        {
            get;
            set;
        }
        //Client 

        [Required(ErrorMessageResourceType = typeof(Resources_common), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Client")]
        public string mClientID { get; set; }

        //Process
        [Required(ErrorMessageResourceType = typeof(Resources_common), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Process")]
        public string mProcessID { get; set; }

        // Search ClientIDSearch 
        [Required(ErrorMessageResourceType = typeof(Resources_common), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Client")]
        public string ClientIDSearch { get; set; }

        // Search ProcessIDSearch 
        [Required(ErrorMessageResourceType = typeof(Resources_common), ErrorMessageResourceName = "required_Process")]
        [Display(ResourceType = typeof(Resources_common), Name = "display_Process")]
        public string ProcessIDSearch { get; set; }

        [Display(ResourceType = typeof(Resources_ProcessOff), Name = "display_Month")]
        public string MonthSearch
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(Resources_ProcessOff), Name = "display_Year")]
        //public string mYear { get; set; }
        public string YearSearch
        {
            get;
            set;
        }

        // End of View Search Entity
        public string mDescription
        {
            get;
            set;
        }


        //Month
        [RequiredField(ErrorMessageResourceType = typeof(Resources_ProcessOff), ErrorMessageResourceName = "required_Month", sControlKeyCollection = "btnGenerate")]
        [Display(ResourceType = typeof(Resources_ProcessOff), Name = "display_Month")]
        public string mMonth { get; set; }

        //Year
        [RequiredField(ErrorMessageResourceType = typeof(Resources_ProcessOff), ErrorMessageResourceName = "required_Year", sControlKeyCollection = "btnGenerate")]
        [Display(ResourceType = typeof(Resources_ProcessOff), Name = "display_Year")]
        public string mYear { get; set; }

        public DataTable DaysList
        { get; set; }

        public SelectList monthlist;

        public List<GridList> GridListView = new List<GridList>();

        public int btnAction { get; set; }
        public int btnGrid { get; set; }
        //DataTable DayList
        //{
        //    get;
        //    set;
        //}
        public string Flag { get; set; }

        //Month
        [Display(ResourceType = typeof(Resources_ProcessOff), Name = "display_Month")]
        public string mMonthYear { get; set; }
    }

    // Flag
    [Serializable]
    public class DayList
    {
        //int IsSelected
        //{
        //    get;
        //    set;
        //}

        //string Date
        //{
        //    get;
        //    set;
        //}

        //string Weekday
        //{
        //    get;
        //    set;
        //}


    }
    [Serializable]
    public class GridList
    {

        public int IsSelected { get; set; }

        public string DateName { get; set; }

        public string dayName { get; set; }



    }
    public class ProcessOffDisplayDetail
    {
        public ProcessOffDisplayDetail()
        {
            DaysList = new List<GridList>();
        }
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
        public int ProcessOffId
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }
        public int Month
        {
            get;
            set;
        }

        public string? MonthYear { get; set; }
        public string? Description { get; set; }
        public string[] DaysNameList { get; set; }

        public List<GridList> DaysList { get; set; }

        public IList<BEProcessOff> BEProcessOffList { get; set; } = new List<BEProcessOff>();

    }
}