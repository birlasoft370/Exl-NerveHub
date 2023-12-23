namespace MicUI.Configuration.Services.ServiceModel
{
    public class AccessRequestApprovalList
    {
        public int RequestId { get; set; }
        public int RequestTypeID { get; set; }
        public int RequestType { get; set; }
        public int ApprovalLevel { get; set; }
        public string RquestedBy { get; set; } = string.Empty;
        public DateTime RequestedOn { get; set; }
        public string RequestDesc { get; set; } = string.Empty;
    }
}
