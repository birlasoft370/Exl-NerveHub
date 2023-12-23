namespace BPA.AppConfiguration.Models.Request
{
    public class CampTermMappingModel
    {
        public CampTermMappingModel()
        {
            GridTermCodeMapList = new();
        }
        public int ClientID
        {
            get;
            set;
        }
        public int ProcessID
        {
            get;
            set;
        }

        public int CampaignTermMapID
        {
            get;
            set;
        }
        public int CampaignID
        {
            get;
            set;
        }

        public List<TermCodeList> GridTermCodeMapList
        {
            get;
            set;
        }
    }

    [Serializable]
    public class TermCodeList
    {
        public int TerminationID { get; set; }
        public int Selected { get; set; }
        public string TerminatioName { get; set; }
        public int IsProductive { get; set; }
        public int IsEnd { get; set; }
        public int Disabled { get; set; }
    }
}
