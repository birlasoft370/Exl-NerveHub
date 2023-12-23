using System.ComponentModel.DataAnnotations;

namespace MicSer.SSOAuthorization.Models
{
    public class AuthenticationRequest
    {
        [Required(ErrorMessage = "Not Empty")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Not Empty")]
        public string TenantName { get; set; }

        //[Required(ErrorMessage = "Not Empty")]
        //public string Password { get; set; }

        [Required(ErrorMessage = "Not Empty")]
        public string HostName { get; set; }

        [Required(ErrorMessage = "Not Empty")]
        public string IPAddress { get; set; }
        [Required]
        public string SystemSessionID { get; set; }
    }
}
