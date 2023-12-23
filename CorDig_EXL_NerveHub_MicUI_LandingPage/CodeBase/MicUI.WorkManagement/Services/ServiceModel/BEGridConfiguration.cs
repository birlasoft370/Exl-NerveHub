using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEGridConfiguration : ObjectBase, IDisposable
    {
        public BEGridConfiguration()
        {

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        [DataMember]
        public string iClientID { get; set; }
        [DataMember]
        public string iProcessID { get; set; }
        [DataMember]
        public string iCampaignID { get; set; }
        [DataMember]
        public string iObjectID { get; set; }
        [DataMember]
        public string iObjProcessID { get; set; }
        [DataMember]
        public string iObjCampaignID { get; set; }
        [DataMember]
        public bool bDisabled { get; set; }
        List<ObjectfiveSD> _Objectfive = new List<ObjectfiveSD>();
        public List<ObjectfiveSD> lstObject { get { return _Objectfive; } set { _Objectfive = value; } }

        List<MappingObjectSD> _MappingObject = new List<MappingObjectSD>();
        public List<MappingObjectSD> lstmappingObjects { get { return _MappingObject; } set { _MappingObject = value; } }
    }
    [Serializable]
    public class ObjectfiveSD
    {
        [DataMember]
        public int iDLinkCampaignID { get; set; }
        [DataMember]
        public string iObjectID { get; set; }
        [DataMember]
        public string iObjProcessID { get; set; }
        [DataMember]

        public string iObjCampaignID { get; set; }

        [DataMember]

        public string sDCampaignName { get; set; }
        [DataMember]
        public bool bCPendingTransactions { get; set; }

        List<MappingObjectSD> _OMappingObject = new List<MappingObjectSD>();
        public List<MappingObjectSD> lstObjectIN { get { return _OMappingObject; } set { _OMappingObject = value; } }
    }
    [Serializable]
    public class MappingObjectSD
    {
        [DataMember]
        public int iMappID { get; set; }
        [DataMember]
        public string iObjectID { get; set; }
        [DataMember]
        public string iSourceObjID { get; set; }
        [DataMember]
        public string iDestination { get; set; }
        [DataMember]
        public bool bDesabled { get; set; }

        [DataMember]
        public string RowAction { get; set; }

        [DataMember]
        public string sColumnName { get; set; }

        [DataMember]
        public string sRowNumTab { get; set; }


    }
}
