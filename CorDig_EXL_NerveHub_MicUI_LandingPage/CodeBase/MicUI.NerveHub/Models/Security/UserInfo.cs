namespace MicUI.NerveHub.Models.Security
{
    public class UserInfo
    {
        public int userid { get; set; }
        public int empid { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string loginname { get; set; }
        public string password { get; set; }
        public string userlevelid { get; set; }
        public bool disabled { get; set; }
        public bool islaniduser { get; set; }
        public int createdby { get; set; }
        public int facilityid { get; set; }
        public string emailid { get; set; }
        public int timezoneid { get; set; }
        public string timezonename { get; set; }
        public string language { get; set; }
        public string servertimezone { get; set; }
        public int processmap { get; set; }
        public int maxsessionid { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
    }

}