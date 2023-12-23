using BPA.AppConfig.BusinessEntity.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.Security
{
    [Serializable]
    [DataContract]
    public class BETeamInfo : ObjectBase
    {
        #region Field
        private int _iTeamID;
        private string _sTeamName;
        private int _iProcessID;
        private string _sTeamDesc;
        private IList<BEUserInfo> _iUserID;
        private bool _bClientLevel;
        private int _iClientID;
        private string _sClientName;
        private string _sProcessName;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BETeamInfo"/> class.
        /// </summary>
        public BETeamInfo()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BETeamInfo"/> class.
        /// </summary>
        /// <param name="iTeamID">The Team ID.</param>
        /// <param name="sTeamName">Name of the Team.</param>
        /// <param name="sTeamDesc">The Team desc.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedby">The createdby.</param>
        public BETeamInfo(int iTeamID, string sTeamName, string sTeamDesc, int iProcessID,
            bool bDisabled, int iCreatedby, string sClientName, string sProcessName)
        {
            _iTeamID = iTeamID;
            _sTeamName = sTeamName;
            _sTeamDesc = sTeamDesc;
            _iProcessID = iProcessID;
            base.iCreatedBy = iCreatedby;
            base.bDisabled = bDisabled;
            _sClientName = sClientName;
            _sProcessName = sProcessName;
        }


        public BETeamInfo(int iTeamID, string sTeamName, string sTeamDesc, int iProcessID, bool bDisabled, int iCreatedby, bool bClientLevel, int iClientID)
        {
            _iTeamID = iTeamID;
            _sTeamName = sTeamName;
            _sTeamDesc = sTeamDesc;
            _iProcessID = iProcessID;
            _bClientLevel = bClientLevel;
            base.iCreatedBy = iCreatedby;
            base.bDisabled = bDisabled;
            _iClientID = iClientID;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Team ID.
        /// </summary>
        /// <value>The Team ID.</value>
        [DataMember]
        public int iTeamID
        {
            get { return _iTeamID; }
            set { _iTeamID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Team.
        /// </summary>
        /// <value>The name of the Team.</value>
        [DataMember]
        public string sTeamName
        {
            get { return _sTeamName; }
            set { _sTeamName = value; }
        }

        /// <summary>
        /// Gets or sets the Team desc.
        /// </summary>
        /// <value>The Team desc.</value>
        [DataMember]
        public string sTeamDesc
        {
            get { return _sTeamDesc; }
            set { _sTeamDesc = value; }
        }

        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        /// <value>The process ID.</value>
        [DataMember]
        public int iProcessID
        {
            get { return _iProcessID; }
            set { _iProcessID = value; }
        }
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        [DataMember]
        public IList<BEUserInfo> iUserID
        {
            get { return _iUserID; }
            set { _iUserID = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [disabled].
        /// </summary>
        /// <value><c>true</c> if [disabled]; otherwise, <c>false</c>.</value>
        /// 
        [DataMember]
        public bool bClientLevel
        {
            get { return _bClientLevel; }
            set { _bClientLevel = value; }
        }

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

        [DataMember]
        public string sClientName
        {
            get { return _sClientName; }
            set { _sClientName = value; }
        }

        [DataMember]
        public string sProcessName
        {
            get { return _sProcessName; }
            set { _sProcessName = value; }
        }
        #endregion
    }


    [Serializable]
    [DataContract]
    public class BESLAStepInfo : ObjectBase
    {
        public BESLAStepInfo()
        { }
        private int _iProcessStepId;
        private string _sProcessStepName;
        public BESLAStepInfo(int iProcessStepId, string sProcessStepName)
        {
            _iProcessStepId = iProcessStepId;
            _sProcessStepName = sProcessStepName;
        }
        [DataMember]
        public int iProcessStepId
        {
            get { return _iProcessStepId; }
            set { _iProcessStepId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Team.
        /// </summary>
        /// <value>The name of the Team.</value>
        [DataMember]
        public string sProcessStepName
        {
            get { return _sProcessStepName; }
            set { _sProcessStepName = value; }
        }

    }
}
