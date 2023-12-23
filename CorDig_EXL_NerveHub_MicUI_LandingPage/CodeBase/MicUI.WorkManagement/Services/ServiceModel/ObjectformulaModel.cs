namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class ObjectformulaModel
    {
        public int CampaignID { get; set; }
        public int DSObjID { get; set; }
        public int DObjID { get; set; }
        public string? DObjEvent { get; set; }
        public string? Dformula { get; set; }
        public string? DisplayFormula { get; set; }
        public bool Disabled { get; set; }
        public string? SavePointXML { get; set; }
    }
}
