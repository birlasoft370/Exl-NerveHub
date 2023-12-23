namespace BPA.AppConfiguration.Models.Request
{
    public class BreakMasterModel
    {
        public int BreakID
        {
            get;
            set;
        }
        public string BreakName
        { get; set; }

        public string Description
        { get; set; }
        public bool Disabled
        {
            get;
            set;
        }
        public int UserId { get; set; }
    }
}
