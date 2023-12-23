namespace MicUI.Configuration.Services.ServiceModel
{
    public class UserRequestStatus
    {
        public int RequestId { get; set; }
        public string? RequestedFor { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public string? Approver { get; set; }
        public string? RequestStatus { get; set; }
        public int Cancelable { get; set; }
        public string? RequestDesc { get; set; }
    }
}
