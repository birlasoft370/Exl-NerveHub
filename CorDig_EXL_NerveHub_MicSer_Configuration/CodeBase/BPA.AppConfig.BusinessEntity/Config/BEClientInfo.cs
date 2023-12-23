using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    [DataContract]
    public class BEClientInfo : ObjectBase
    {
        #region Field
        private int _iClientID;
        private int _iVerticalID;
        private string _sClientName;
        private string _sClientDescription;
        private DateTime _dtEndDate;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="oClientInfo"/> class.
        /// </summary>
        public BEClientInfo()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="oClientInfo"/> class.
        /// </summary>
        /// <param name="iClientID">The client ID.</param>
        /// <param name="sClientName">Name of the client.</param>
        /// <param name="sClientDescription">The client description.</param>
        /// <param name="dtEndDate">The dt end date.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEClientInfo(int iClientID, string sClientName, string sClientDescription, DateTime dtEndDate, bool bDisabled, int iCreatedBy)
        {
            _iClientID = iClientID;
            _sClientName = sClientName;
            _sClientDescription = sClientDescription;
            _dtEndDate = dtEndDate;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="oClientInfo"/> class.
        /// </summary>
        /// <param name="iClientID">The client ID.</param>
        /// <param name="iVerticalID">The i vertical ID.</param>
        /// <param name="sClientName">Name of the client.</param>
        /// <param name="sClientDescription">The client description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEClientInfo(int iClientID, int iVerticalID, string sClientName, string sClientDescription, DateTime dtEndDate, bool bDisabled, int iCreatedBy)
        {
            _iClientID = iClientID;
            _iVerticalID = iVerticalID;
            _sClientName = sClientName;
            _sClientDescription = sClientDescription;
            _dtEndDate = dtEndDate;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        /// <value>The client ID.</value>
        [DataMember]
        public int iClientID
        {
            get { return _iClientID; }
            set { _iClientID = value; }
        }
        /// <summary>
        /// Gets or sets the Vertical ID.
        /// </summary>
        /// <value>The Vertical ID.</value>
        [DataMember]
        public int iVerticalID
        {
            get { return _iVerticalID; }
            set { _iVerticalID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>The name of the client.</value>
        [DataMember]
        public string sClientName
        {
            get { return _sClientName; }
            set { _sClientName = value; }
        }

        /// <summary>
        /// Gets or sets the clinet description.
        /// </summary>
        /// <value>The clinet description.</value>
        [DataMember]
        public string sClientDescription
        {
            get { return _sClientDescription; }
            set { _sClientDescription = value; }
        }

        /// <summary>
        /// Gets or sets the dt end date.
        /// </summary>
        /// <value>The dt end date.</value>
        [DataMember]
        public DateTime dtEndDate
        {
            get { return _dtEndDate; }
            set { _dtEndDate = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b EXL specific].
        /// </summary>
        /// <value><c>true</c> if [b EXL specific]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bEXLSpecific
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the i approved access.
        /// </summary>
        /// <value>The i approved access.</value>
        [DataMember]
        public int iApprovedAccess { get; set; }

        /// <summary>
        /// Gets or sets the s ERP client mapped.
        /// </summary>
        /// <value>The s ERP client mapped.</value>
        [DataMember]
        public string sERPClientMapped
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the s ERP client deleted.
        /// </summary>
        /// <value>The s ERP client deleted.</value>
        [DataMember]
        public string sERPClientDeleted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dt ERP client.
        /// </summary>
        /// <value>The dt ERP client.</value>
        [DataMember]
        public DataTable dtERPClient
        {
            get;
            set;
        }
        #endregion
    }
}
