namespace BPA.AppConfiguration.Models.Request
{
    public class SubProcessMasterModel
    {
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
        public int SubProcessID { get; set; }
        public string SubProcessName { get; set; }
        public string? Description { get; set; }
        public DateTime SubProcessStartDate { get; set; }
        public DateTime SubProcessEndDate { get; set; }
        public DateTime GoLiveDate { get; set; }
        public DateTime StabilizationStartDate { get; set; }
        public DateTime StabilizationEndDate { get; set; }
        public DateTime ProductionStartDate { get; set; }
        public DateTime ProductionEndDate { get; set; }
        public bool Disabled { get; set; }
    }
}
