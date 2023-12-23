namespace MicUI.Configuration.Services.ServiceModel
{
    public class RoleApprovalModel
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
