using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.Security.BusinessEntity.ExtrernalRefre
{
    [Serializable]
    [DataContract]
    public class BEPendingApproval : ObjectBase
    {
        #region Fields
        /// <summary>
        /// Gets or sets a BatchApprovalID.
        /// </summary>
        /// <value>BatchApprovalID.</value>
        [DataMember]
        public int BatchApprovalID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ClientID.
        /// </summary>
        /// <value>The ClientID.</value>
        [DataMember]
        public int ClientID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ClientName.
        /// </summary>
        /// <value>The ClientName.</value>
        [DataMember]
        public string ClientName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ProcessID.
        /// </summary>
        /// <value>The ProcessID.</value>
        [DataMember]
        public int ProcessID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ProcessName.
        /// </summary>
        /// <value>The ProcessName.</value>
        [DataMember]
        public string ProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CampaignID.
        /// </summary>
        /// <value>The CampaignID.</value>
        [DataMember]
        public int CampaignID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CampignName.
        /// </summary>
        /// <value>The CampignName.</value>
        [DataMember]
        public string CampignName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the BatchCode.
        /// </summary>
        /// <value>The BatchCode.</value>
        [DataMember]
        public string BatchCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the FromDate.
        /// </summary>
        /// <value>The FromDate.</value>
        [DataMember]
        public string FromDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ToDate.
        /// </summary>
        /// <value>The s ToDate.</value>
        [DataMember]
        public string ToDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the i NoOfRecords.
        /// </summary>
        /// <value>The type of the i NoOfRecords.</value>
        [DataMember]
        public int NoOfRecords
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a s RequestBy.
        /// </summary>
        /// <value>RequestBy.</value>
        [DataMember]
        public string RequestBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value of i RequestByID.
        /// </summary>
        /// <value>RequestByID.</value>
        [DataMember]
        public int RequestByID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating s RequestDate.
        /// </summary>
        /// <value>RequestDate.</value>
        [DataMember]
        public string RequestDate
        {
            get;
            set;
        }

        #endregion
    }
}
