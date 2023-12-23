namespace MicUI.Configuration.Services.ServiceModel
{
    public class InsertUserMappingForApprovalModel
    {
        public InsertUserMappingForApprovalModel()
        {
            RoleInfo = new();
            UserInfo = new();
            ClientInfo = new List<UserMappingForApprovalClientInfo>();
            ProcessInfo = new List<UserMappingForApprovalProcessInfo>();
            CampaignInfo = new List<UserMappingForApprovalCampaignInfo>();
        }
        public int MappedOn { get; set; }
        public bool Disabled { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? DeletedNodes { get; set; }
        public int iCreatedBy { get; set; }
        public UserMappingForApprovalRoleInfo RoleInfo { get; set; }
        public UserMappingForApprovalUserInfo UserInfo { get; set; }
        public List<UserMappingForApprovalClientInfo> ClientInfo { get; set; }
        public List<UserMappingForApprovalProcessInfo> ProcessInfo { get; set; }
        public List<UserMappingForApprovalCampaignInfo> CampaignInfo { get; set; }
    }
    public class UserMappingForApprovalRoleInfo
    {
        public int RoleID { get; set; }
    }
    public class UserMappingForApprovalUserInfo
    {
        public int UserID { get; set; }
    }
    public class UserMappingForApprovalClientInfo
    {
        public int ClientID { get; set; }
    }
    public class UserMappingForApprovalProcessInfo
    {
        public int ProcessID { get; set; }
    }
    public class UserMappingForApprovalCampaignInfo
    {
        public int CampaignID { get; set; }
    }


    public class UserRequestType
    {
        public string? RequestTypeDetails { get; set; }
        public int RequestType { get; set; }
        public int Id { get; set; }
        public string? RequestName { get; set; }
        public int RequestTypeId { get; set; }
    }

    public class UserMappingApproverModel
    {
        public List<UserMappingApprover> UserMappingApprovers { get; set; } = new();
    }
    public class UserMappingApprover
    {
        public int RequestId { get; set; }
        public int RequestType { get; set; }
        public int RequestTypeId { get; set; }
        public int Approver1Id { get; set; }
        public string Approver2Id { get; set; } = "";
    }
}
