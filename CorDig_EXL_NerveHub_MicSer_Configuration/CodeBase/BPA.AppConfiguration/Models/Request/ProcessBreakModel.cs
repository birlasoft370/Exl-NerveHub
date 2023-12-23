namespace BPA.AppConfiguration.Models.Request
{
    public class ProcessBreakModel
    {
        public ProcessBreakModel()
        {
            BreakList = new();
        }
        public int ClientID
        {
            get;
            set;
        }
        public int ProcessID
        {
            get;
            set;
        }
        public int BreakMapID
        {
            get;
            set;
        }

        public List<BreakList> BreakList { get; set; }
    }

    public class BreakList
    {
        public int BreakID { get; set; }
        public int Selected { get; set; }
        public string BreakName { get; set; }
        public int IsProductive { get; set; }
        public int IsScheduled { get; set; }
        public int Disabled { get; set; }

    }
}
