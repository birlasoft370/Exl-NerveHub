namespace MicUI.Configuration.Services.ServiceModel
{
    public class ProcessOwnerApprovalReport
    {
        public int RequestId { get; set; }
        public string? ClientName { get; set; }
        public string? ProcessName { get; set; }
        public string? Creater { get; set; }
        public string? Approver { get; set; }
        public DateTime CreateDate { get; set; }
        public string Foruser { get; set; } = string.Empty;
        public string? TransStatus { get; set; }
    }
}
