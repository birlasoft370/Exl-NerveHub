using System.Runtime.Serialization;

namespace MicUI.Configuration.Services.ServiceModel
{
    /// <summary>
    /// Job Code 
    /// </summary>
    [Serializable]
    [DataContract]
    public class BEJobCodeInfo : ObjectBase
    {
        /// <summary>
        /// Gets or sets the JOBID.
        /// </summary>
        /// <value>The JOBID.</value>
        [DataMember]
        public int iJOBID { get; set; }

        /// <summary>
        /// Gets or sets the ERP job code.
        /// </summary>
        /// <value>The ERP job code.</value>
        [DataMember]
        public int iJobCode { get; set; }

        /// <summary>
        /// Gets or sets the ERP job desc.
        /// </summary>
        /// <value>The ERP job desc.</value>
        [DataMember]
        public string sJobDesc { get; set; }

    }

  
    [Serializable]
    [DataContract]
    public class RoleFormAccessModel
    {
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string FormId { get; set; }
        [DataMember]
        public string FormName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string View { get; set; }
        [DataMember]
        public string Modify { get; set; }
        [DataMember]
        public string Delete { get; set; }
        [DataMember]
        public string Approve { get; set; }
        [DataMember]
        public string Add { get; set; }
    }
    public class ErpJobRoleMap
    { 
        public int iMappedOn { get; set; }
        public int RoleId { get; set; }
        public int jobId { get; set; }
        public string RoleName { get; set; }
        public string jobDescription { get; set; }
        public int ApproverId { get; set; }
        public string iMode { get; set; }
        public int CreatedBy { get; set;}
        public bool bDefaultRole { get; set; }
        public bool bDisabled {  get; set; }
    }


}
