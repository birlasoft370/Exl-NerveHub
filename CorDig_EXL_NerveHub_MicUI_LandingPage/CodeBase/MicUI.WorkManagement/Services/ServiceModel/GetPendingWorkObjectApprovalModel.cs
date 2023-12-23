namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class GetPendingWorkObjectApprovalModel
    {
        public int ApprovalId { get; set; }
        public int BusinessApproverId { get; set; }
        public int TechnologyApproverId { get; set; }
        public int CreatedBy { get; set; }
        public string? ClientName { get; set; }
        public string? ProcessName { get; set; }
        public string? CampaignName { get; set; }
        public string? KeyBenfits { get; set; }
        public string? StoreName { get; set; }
        public string? RequestCreator { get; set; }
        public string? RequestedOn { get; set; }
        public string? BusinessApprover { get; set; }
        public string? TechApprover { get; set; }
        public string? Status { get; set; }
        public string? StatusToShowHideButtons { get; set; }
        public string? ChangeRequest { get; set; }
        public string? TechApprovers { get; set; }
    }
}
