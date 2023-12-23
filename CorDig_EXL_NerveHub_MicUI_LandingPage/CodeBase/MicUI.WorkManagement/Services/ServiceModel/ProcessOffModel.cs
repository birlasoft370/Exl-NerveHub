using MicUI.WorkManagement.Models.ViewModels;

namespace MicUI.WorkManagement.Services.ServiceModel
{
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

        public string MonthYear { get; set; }
        public string? Description { get; set; }
        public string[] DaysNameList { get; set; }
    


    }
}
