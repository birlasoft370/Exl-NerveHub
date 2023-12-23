namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class LinkCampaignGridConfigurationModel
    {
        public int ClientID { get; set; }
        public int ProcessID { get; set; }
        public int CampaignID { get; set; }
        public List<ObjectfiveSDGrid> lstObject { get; set; } = new List<ObjectfiveSDGrid>();
    }
    public class ObjectfiveSDGrid
    {
        public int ObjectID { get; set; }
        public int ProcessID { get; set; }
        public int CampaignID { get; set; }
        //  public int DLinkCampaignID { get; set; }
        // public int DCampaignId { get; set; }
        //public int RowNum { get; set; }
        public bool CPendingTransactions { get; set; }
        public List<MappingbjectSDGrid> lstObjectIN { get; set; } = new List<MappingbjectSDGrid>();
    }
    public class MappingbjectSDGrid
    {
        public int MaxLength { get; set; }
        public int MappID { get; set; }
        //public int DGrdConfigID { get; set; }
        public int SourceObjectID { get; set; }
        public int DestinationObjectID { get; set; }
        public bool Disabled { get; set; }
    }
}
