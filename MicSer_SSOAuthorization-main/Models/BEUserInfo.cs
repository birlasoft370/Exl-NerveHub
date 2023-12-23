namespace MicSer.SSOAuthorization.Models
{
    public class BEUserInfo
    {
        public int USERID { get; set; }
        public int EMPID { get; set; }
        public string FIRSTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string LASTNAME { get; set; }
        public string LOGINNAME { get; set; }
        public string PASSWORD { get; set; }
        public string USERLEVELID { get; set; }
        public bool DISABLED { get; set; }
        public bool ISLANIDUSER { get; set; }
        public int CREATEDBY { get; set; }
        public int FACILITYID { get; set; }
        public string EMAILID { get; set; }
        public int TIMEZONEID { get; set; }
        public string TIMEZONENAME { get; set; }
        public string LANGUAGE { get; set; }
        public string ConnectionString { get; set; }
        public string ServerTimeZone { get; set; }
        public int ProcessMap { get; set; }
        public int maxSessionID { get; set; }
    }        

    public class BERoleInfo
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
    public class BELanguages
    {
        public int iLanguageID { get; set; }
        public string sLanguage { get; set; }
        public string sCulture { get; set; }
    }
}
