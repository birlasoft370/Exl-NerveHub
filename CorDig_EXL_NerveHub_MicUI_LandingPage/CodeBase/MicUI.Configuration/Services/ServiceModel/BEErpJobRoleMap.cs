using System.Runtime.Serialization;

namespace MicUI.Configuration.Services.ServiceModel
{
    [Serializable]
    [DataContract]
    public class BEErpJobRoleMap : ObjectBase
    {
        /// <summary>
        /// Gets or sets the role job ID.
        /// </summary>
        /// <value>The role job ID.</value>
        [DataMember]
        public int iRoleJobID { get; set; }


        /// <summary>
        /// Gets or sets the object role.
        /// </summary>
        /// <value>The object role.</value>
        [DataMember]
        public BERoleInfo oRole { get; set; }

        /// <summary>
        /// Gets or sets the object job.
        /// </summary>
        /// <value>The object job.</value>
        [DataMember]
        public BEJobCodeInfo oJob { get; set; }

        /// <summary>
        /// Gets the role job.
        /// </summary>
        /// <value>The role job.</value>
        public string RoleJob
        {
            get
            {
                string str = "";
                if (oJob != null)
                    str = "ERP :" + oJob.sJobDesc + " - ";
                if (oRole != null)
                    str += "Application :" + oRole.sRoleName;

                if (base.bDisabled) str += " (Disabled)";
                return str;
            }
        }

        public string JobDesc
        {
            get
            {
                string str = "";
                if (oJob != null)
                    str = "ERP :" + oJob.sJobDesc;
                if (base.bDisabled) str += "- (Disabled)";
                return str;
            }
        }
        public string RoleName
        {
            get
            {
                string str = "";
                if (oRole != null)
                    str += "Application :" + oRole.sRoleName;
                if (base.bDisabled) str += " (Disabled)";
                return str;
            }
        }

        /// <summary>
        /// Gets or sets the mapped on.
        /// Client is 1
        /// Process is 2
        /// Campaign is 3
        /// </summary>
        /// <value>The mapped on.</value>
        [DataMember]
        public int iMappedOn { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [b default role].
        /// </summary>
        /// <value><c>true</c> if [b default role]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bDefaultRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [b desable].
        /// </summary>
        /// <value><c>true</c> if [b desable]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bDisable { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int iApprover { get; set; }




        [DataMember]
        public int RequestId { get; set; }


        [DataMember]
        public string RequestedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime RequestedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Approver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RequestStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Cancelable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RequestDesc { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string iMode { get; set; }



    }
}
