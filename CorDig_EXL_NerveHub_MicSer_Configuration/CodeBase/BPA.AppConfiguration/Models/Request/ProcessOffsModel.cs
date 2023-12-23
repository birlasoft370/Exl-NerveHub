using BPA.AppConfig.BusinessEntity.ExternalRef.BIReports;

namespace BPA.AppConfiguration.Models.Request
{
    public class ProcessOffsModel
    {
        public ProcessOffsModel()
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

    [Serializable]
    public class GridList
    {
        public int IsSelected { get; set; }
        public string DateName { get; set; }
        public string dayName { get; set; }
    }

    public class ProcessOffModel
    {
       
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

      

       
    }
}
