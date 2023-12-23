/* Copyright © 2012 ExlService (I) Pvt. Ltd.
 * project Name                 :   
 * Class Name                   :   
 * Namespace                    :   
 * Purpose                      :
 * Description                  :
 * Dependency                   :   
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :   
 * Created on                   :   
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace BPA.Security.BusinessEntity.Security
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

    /// <summary>
    /// ERP JOB and Role Mapping
    /// </summary>
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
    public class ErpJobRoleMap
    {
        public int iMappedOn { get; set; }
        public int RoleId { get; set; }
        public int jobId { get; set; }
        public string RoleName { get; set; }
        public string jobDescription { get; set; }
        public int ApproverId { get; set; }
        public string iMode { get; set; }
        public int CreatedBy { get; set; }
        public bool bDefaultRole { get; set; }
        public bool bDisabled { get; set; }
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


}
