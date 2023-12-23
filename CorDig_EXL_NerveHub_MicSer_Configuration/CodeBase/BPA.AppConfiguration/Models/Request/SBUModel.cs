namespace BPA.AppConfiguration.Models.Request
{
    public class SBUModel
    {
        public int SBUID
        {
            get;
            set;
        }
        public string SBUName
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int ERPID
        { get; set; }

        public bool IsClientSBU
        {
            get;
            set;
        }

        public bool Disabled
        {
            get;
            set;
        }
        public int UserId { get; set; }
    }
}
