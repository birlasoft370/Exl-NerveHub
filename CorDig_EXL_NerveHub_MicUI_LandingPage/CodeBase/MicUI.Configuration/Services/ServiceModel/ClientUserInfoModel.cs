namespace MicUI.Configuration.Services.ServiceModel
{
    public class ClientUserInfoModel
    {
        public int UserId { get; set; }
        public int EmployeeID { get; set; }
        public bool LanID { get; set; }
        public bool ClientUser { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? LoginName { get; set; }
        public bool? Disabled { get; set; }
        public bool? bIsBot { get; set; }
        public int ClientID { get; set; }
        public string? Process { get; set; }
        public string? DeletedProcess { get; set; }
        public int RoleID { get; set; }
        public int RoleApprover { get; set; }
        public DateTime DOJ { get; set; }
        public int FacilityId { get; set; }
        public int SupervisorID { get; set; }
        public int LOBID { get; set; }
        public int SBUID { get; set; }
        public int JobID { get; set; }

    }
}
