using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.SharePointConfiguration
{
    [Serializable]
    [DataContract]
    public class BESharePointConfiguration : ObjectBase
    {

        #region Public Properties

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        [DataMember]
        public string sUserID { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [DataMember]
        public string sPassword { get; set; }

        /// <summary>
        /// Gets or sets the Scheduler ID.
        /// </summary>
        /// <value>The Scheduler ID.</value>
        [DataMember]
        public int iSchedulerID { get; set; }

        /// <summary>
        /// Gets or sets the Client ID.
        /// </summary>
        /// <value>The Client ID.</value>
        [DataMember]
        public int iClientID { get; set; }


        /// <summary>
        /// Gets or sets the Process ID.
        /// </summary>
        /// <value>The Process ID.</value>
        [DataMember]
        public int iProcessID { get; set; }

        /// <summary>
        /// Gets or sets the Campaign ID.
        /// </summary>
        /// <value>Campaign ID.</value>
        [DataMember]
        public int iCampaignID { get; set; }

        /// <summary>
        /// Gets or sets the Campaign Name.
        /// </summary>
        /// <value>Campaign Name.</value>
        [DataMember]
        public string sCampaignName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Share Point Path.
        /// </summary>
        /// <value>The name of the Share Point Path.</value>
        [DataMember]
        public string sSharePointPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the Root Folder Name.
        /// </summary>
        /// <value>The name of the Root Folder Name.</value>
        [DataMember]
        public string sRootFolderName { get; set; }

        /// <summary>
        /// Gets or sets the name of the Data Folder Location.
        /// </summary>
        /// <value>The name of the Data Folder Location.</value>
        [DataMember]
        public string sDataFolderLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the Error Folder Location.
        /// </summary>
        /// <value>The name of the Error Folder Location.</value>
        [DataMember]
        public string sErrorFolderLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the Archival Folder Location.
        /// </summary>
        /// <value>The name of the Archival Folder Location.</value>
        [DataMember]
        public string sArchivalFolderLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the Duration.
        /// </summary>
        /// <value>The name of the Duration.</value>
        [DataMember]
        public int iDuration { get; set; }

        /// <summary>
        /// Gets or sets the name of the File Extention.
        /// </summary>
        /// <value>The name of the File Extention.</value>
        [DataMember]
        public string sFrequency { get; set; }

        /// <summary>
        /// Gets or sets the name of the File Extention.
        /// </summary>
        /// <value>The name of the File Extention.</value>
        [DataMember]
        public string sFileExtention { get; set; }

        /// <summary>
        /// Gets or sets the name of the Delimiter.
        /// </summary>
        /// <value>The name of the Delimiter.</value>
        [DataMember]
        public string sDelimiter { get; set; }

        [DataMember]
        public string sSharePointName { get; set; }

        [DataMember]
        public bool bDisabled { get; set; }

        #endregion
    }
}
