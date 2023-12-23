using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEWorkObjectApprover : ObjectBase
    {
        [DataMember]
        public string sLocations { get; set; }

        [DataMember]
        public string sShiftwindows { get; set; }

        [DataMember]
        public bool sPurposeswm { get; set; }

        [DataMember]
        public bool sPurposetime { get; set; }

        [DataMember]
        public bool sPurposetrans { get; set; }

        [DataMember]
        public string sBusinessJustifications { get; set; }

        [DataMember]
        public string sTargetq1 { get; set; }

        [DataMember]
        public string sTargetq2 { get; set; }
        [DataMember]
        public string sTargetq3 { get; set; }

        [DataMember]
        public string sTargety1 { get; set; }
        [DataMember]
        public string sTargety2 { get; set; }
        [DataMember]
        public string sTargety3 { get; set; }

        [DataMember]
        public string skeybenifits { get; set; }

        [DataMember]
        public int iBuisnessID { get; set; }

        [DataMember]
        public int iTechID { get; set; }

        [DataMember]
        public string sStatus { get; set; }

        [DataMember]
        public int iApprovalId { get; set; }
        public List<string> PurposeofcreationofWork { get; set; }
    }
}
