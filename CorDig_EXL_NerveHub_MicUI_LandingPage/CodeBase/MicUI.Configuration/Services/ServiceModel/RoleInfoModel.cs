namespace MicUI.Configuration.Services.ServiceModel
{
    public class RoleInfoModel
    {
        public RoleInfoModel()
        {
            FormActions = new();
        }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public bool Disabled { get; set; }
        public bool IsClientRole { get; set; }
        public int LevelID { get; set; }
        public string? MailBodyContent { get; set; }
        public List<FormAction> FormActions { get; set; }
    }

    public class FormAction
    {
        public string? ModuleName { get; set; }
        public long FormID { get; set; }
        public string? FormName { get; set; }
        public string? Description { get; set; }
        public string?  View { get; set; }
        public string?  Modify { get; set; }
        public string?  Delete { get; set; }
        public string?  Approve { get; set; }
        public string?  Add { get; set; }
        public string?  ChildID { get; set; }

    }

    public class RoleApprovalRequestModel
    {
        public int RequestId { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public string Approver { get; set; }
        public string RequestStatus { get; set; }
        public string Cancelable { get; set; }
        public string RequestDesc { get; set; }
    }
}
