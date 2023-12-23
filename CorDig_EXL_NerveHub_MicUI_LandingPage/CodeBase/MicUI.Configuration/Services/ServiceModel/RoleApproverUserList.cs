using System.ComponentModel.DataAnnotations;

namespace MicUI.Configuration.Services.ServiceModel
{
    public class RoleApproverUserList
    {
        public int UserId { get; set; }

        public string? Agent { get; set; }
    }
}
