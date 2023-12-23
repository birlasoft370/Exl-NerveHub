namespace MicUI.Configuration.Services.ServiceModel
{
    public class PowerUserInfo
    {
        public PowerUserInfo()
        {
            Roles = new List<PowerRoleInfo>();
        }
        public int EmployeeID { get; set; }
        public bool LanID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LoginName { get; set; } = string.Empty;
        public bool Disabled { get; set; }
        public int CreatedBy { get; set; }
        public List<PowerRoleInfo> Roles { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? RoleApprover { get; set; }
        public int? Approver { get; set; }
    }

    public class PowerRoleInfo
    {
        public int CreatedBy { get; set; }
        public int RoleID { get; set; }
    }
}
