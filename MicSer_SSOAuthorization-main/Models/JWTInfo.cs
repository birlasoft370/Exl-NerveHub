namespace MicSer.SSOAuthorization.Models
{
    public class JWTInfo
    {
        public string UserInfo { get; set; }
        public string Role { get; set; }
        public string RoleID { get; set; }
        public string ConString { get; set; }
        public string UserId { get; set; }
        public string EmpId { get; set; }
        public string TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
        public string Language { get; set; }
        public string ServerTimeZone { get; set; }
        public string Email { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
    }
}
