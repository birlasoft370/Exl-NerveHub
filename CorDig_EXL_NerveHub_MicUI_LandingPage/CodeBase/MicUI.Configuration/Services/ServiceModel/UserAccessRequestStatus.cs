namespace MicUI.Configuration.Services.ServiceModel
{
    public class UserAccessRequestStatus
    {
        public string RequestId { get; set; }
        public string RequestedFor { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedOn { get; set; }
        public string Approver { get; set; }
        public string RequestStatus { get; set; }
        public string RequestDesc { get; set; }
    }
}
