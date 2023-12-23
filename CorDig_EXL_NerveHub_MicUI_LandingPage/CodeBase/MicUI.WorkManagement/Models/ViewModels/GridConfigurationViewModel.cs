namespace MicUI.WorkManagement.Models.ViewModels
{
    [Serializable]
    public class GridConfigurationViewModel
    {

        public string iClientID { get; set; }
        public string iProcessID { get; set; }
        public string iCampaignID { get; set; }

        public string iObjectID { get; set; }
        public string iObjProcessID { get; set; }
        public string iObjCampaignID { get; set; }
        List<Objectfive> _Objectfive = new List<Objectfive>();
        public List<Objectfive> lstObject { get { return _Objectfive; } set { _Objectfive = value; } }

        List<MappingObject> _MappingObject = new List<MappingObject>();
        public List<MappingObject> lstmappingObjects { get { return _MappingObject; } set { _MappingObject = value; } }

    }
    public class Objectfive
    {
        public int iDLinkCampaignID { get; set; }
        public string iObjectID { get; set; }
        public string iObjProcessID { get; set; }
        public string iObjCampaignID { get; set; }
        public bool bCPendingTransactions { get; set; }
        List<MappingObject> _OMappingObject = new List<MappingObject>();
        public List<MappingObject> lstObjectIN { get { return _OMappingObject; } set { _OMappingObject = value; } }
    }
    public class MappingObject
    {
        public int iMappID { get; set; }

        public int iDLinkCampaignID { get; set; }
        public string iObjectID { get; set; }
        public string iSourceObjID { get; set; }
        public string iDestination { get; set; }

        public bool bDesabled { get; set; }
        public string RowAction { get; set; }

    }
}
