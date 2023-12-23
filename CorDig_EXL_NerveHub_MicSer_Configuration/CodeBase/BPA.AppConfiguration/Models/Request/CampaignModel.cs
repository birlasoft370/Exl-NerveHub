namespace BPA.AppConfiguration.Models.Request
{
    public class CampaignBase
    {
        public CampaignBase()
        {
            Purposeofcreationofcampaign = new();
        }
        public string? CampaignName { get; set; }
        public string Location { get; set; }
        public string ShiftWindow { get; set; }
        public string BusinessJustifications { get; set; }
        public string KeyBenefits { get; set; }
        public int? Q1 { get; set; }
        public int? Q2 { get; set; }
        public int? Q3 { get; set; }
        public int? Y1 { get; set; }
        public int? Y2 { get; set; }
        public int? Y3 { get; set; }
        public List<string> Purposeofcreationofcampaign { get; set; }
    }

    public class CampaignRequestDetails : CampaignBase
    {
        public int IsLevel { get; set; }
        public int ApprovelId { get; set; }
        public string Requestor { get; set; }
        public string ClientName { get; set; }
        public string ProcessName { get; set; }
        public string Purpose { get; set; }
        public string ChangeRequest { get; set; }
        public string ChangeRequestStatus { get; set; }
    }

    public class CampaignModel : CampaignBase
    {
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
        public int CampaignID { get; set; }
        public string? Description { get; set; }
        public List<string> Mode { get; set; }
        public DateTime? EndDate { get; set; }
        public int TimeZone { get; set; }
        public bool? NoFieldDataEntry { get; set; }

        public string? Email { get; set; }
        public int ThresholdForCompletion { get; set; }
        public int ThresholdForToOpen { get; set; }
        public double TargetEfficiency { get; set; }
        public bool? BillingSystem { get; set; }
        public bool? Disabled { get; set; }
        public int ApproverId { get; set; }
        public bool PurposesWM { get; set; }
        public bool PurposeTime { get; set; }
        public bool PurposeTrans { get; set; }

        /*
        public bool? BackOffice { get; set; }
        public bool? InBound { get; set; }
        public bool? OutBound { get; set; }

        public bool? WorkManagement { get; set; }
        public bool? TimeTracking { get; set; }
        public bool? TransactionsMonitoring { get; set; }

        public string TargetUsers { get; set; }
        public string BusinessApprover { get; set; }
      
        public string WorkDefinitionName { get; set; }
        public int IsLevel { get; set; }
       */
    }

    [Serializable]
    public class CampaignApproval
    {
        public int ApprovalId { get; set; }
        public int BusinessApproverId { get; set; }
        public int TechnologyApproverId { get; set; }
        public int CreatedBy { get; set; }
        public string ClientName { get; set; }
        public string ProcessName { get; set; }
        public string CampaignName { get; set; }
        public string KeyBenfits { get; set; }
        public string StoreName { get; set; }
        public string RequestCreater { get; set; }
        public string RequestedOn { get; set; }
        public string BusinessApprover { get; set; }
        public string TechApprover { get; set; }
        public string Status { get; set; }
        public string StatusToShowHideButtons { get; set; }
        public string ChangeRequest { get; set; }
        public bool? IsChecked { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        // public BETenant oTenant { get; set; }
    }

    public class CampaignRequestChange : CampaignBase
    {
        public int ApprovalId { get; set; }
        public bool PurposesWM { get; set; } = false;
        public bool PurposeTime { get; set; } = false;
        public bool PurposeTrans { get; set; } = false;
    }
}
