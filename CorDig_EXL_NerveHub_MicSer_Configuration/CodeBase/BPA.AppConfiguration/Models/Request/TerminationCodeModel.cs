namespace BPA.AppConfiguration.Models.Request
{
    public class TerminationCodeModel
    {
        public int TerminationCodeID
        {
            get;
            set;
        }
        public string TerminationCodeName
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
