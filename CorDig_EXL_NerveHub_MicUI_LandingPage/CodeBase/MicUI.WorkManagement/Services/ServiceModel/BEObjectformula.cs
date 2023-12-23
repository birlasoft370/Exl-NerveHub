using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    [DataContract]
    public class BEObjectformula : ObjectBase
    {
        private DateTime _CreatedOn;

        private DateTime _ModifiedOn;

        [DataMember]
        public int iClientID { get; set; }

        [DataMember]
        public int iProcessID { get; set; }

        [DataMember]
        public int iCampaignID { get; set; }

        [DataMember]
        public int iDObjID { get; set; }

        [DataMember]
        public int iDSObjID { get; set; }

        [DataMember]
        public string sDObjEvent { get; set; }

        [DataMember]
        public string sDformula { get; set; }

        [DataMember]
        public string sDisplayFormula { get; set; }

        [DataMember]
        public int iWorkObjectID { get; set; }

        [DataMember]
        public string sWorkObjectName { get; set; }

        [DataMember]
        public string sWorkObjectDataType { get; set; }

        [DataMember]
        public string sObjControleType { get; set; }

        [DataMember]
        public string iDsObjectID { get; set; }

        [DataMember]
        public string sObjNameFormula { get; set; }

        [DataMember]
        public string sObjValueFormula { get; set; }

        [DataMember]
        public bool bDisabled { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        [DataMember]
        public DateTime ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = value; }
        }

        [DataMember]
        public int ModifiedBy { get; set; }

        [DataMember]
        public string sSavePointXML { get; set; }
    }
}
