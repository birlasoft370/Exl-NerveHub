using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.Configuration
{
    [Serializable]
    [DataContract]
    public class BESBUInfo : ObjectBase
    {
        #region Facility Fields
        private int _iSBUID;
        private int _iERPID;
        private int _iCLIENTID;
        private string _sName;
        private string _sDescription;
        private DataTable _dtClientSBUMap;
        private int _ClientSBUId;
        private bool _bIsClientSBU;


        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="oFacilityInfo"/> class.
        /// </summary>
        public BESBUInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BESBUInfo"/> class.
        /// </summary>
        /// <param name="iSBUID">The i SBUID.</param>
        /// <param name="iERPID">The i ERPID.</param>
        /// <param name="sName">Name of the s.</param>
        /// <param name="sDescription">The s description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        /// <param name="bIsClientSBU">if set to <c>true</c> [b is client SBU].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BESBUInfo(int iSBUID, int iERPID, string sName, string sDescription, bool bDisabled, bool bIsClientSBU, int iCreatedBy)
        {
            _iSBUID = iSBUID;
            _iERPID = iERPID;
            _sName = sName;
            _bIsClientSBU = bIsClientSBU;
            _sDescription = sDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BESBUInfo"/> class.
        /// </summary>
        /// <param name="ClientSBUId">The client SBU id.</param>
        /// <param name="iSBUID">The i SBUID.</param>
        /// <param name="iERPID">The i ERPID.</param>
        /// <param name="sName">Name of the s.</param>
        /// <param name="sDescription">The s description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        /// <param name="bIsClientSBU">if set to <c>true</c> [b is client SBU].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BESBUInfo(int ClientSBUId, int iSBUID, int iERPID, string sName, string sDescription, bool bDisabled, bool bIsClientSBU, int iCreatedBy)
        {
            _ClientSBUId = ClientSBUId;
            _iSBUID = iSBUID;
            _iERPID = iERPID;
            _sName = sName;
            _bIsClientSBU = bIsClientSBU;
            _sDescription = sDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the i CLIENTID.
        /// </summary>
        /// <value>The i CLIENTID.</value>
        [DataMember]
        public int iCLIENTID
        {
            get
            {
                return _iCLIENTID;
            }
            set
            {
                _iCLIENTID = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b is client SBU].
        /// </summary>
        /// <value><c>true</c> if [b is client SBU]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bIsClientSBU
        {
            get
            {
                return _bIsClientSBU;
            }
            set
            {
                _bIsClientSBU = value;
            }
        }



        /// <summary>
        /// Gets or sets the i SBUID.
        /// </summary>
        /// <value>The i SBUID.</value>
        [DataMember]
        public int iSBUID
        {
            get
            {
                return _iSBUID;
            }
            set
            {
                _iSBUID = value;
            }
        }

        /// <summary>
        /// Gets or sets the i ERPID.
        /// </summary>
        /// <value>The i ERPID.</value>
        [DataMember]
        public int iERPID
        {
            get
            {
                return _iERPID;
            }
            set
            {
                _iERPID = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the s.
        /// </summary>
        /// <value>The name of the s.</value>
        [DataMember]
        public string sName
        {
            get
            {
                return _sName;
            }
            set
            {
                _sName = value;
            }
        }

        /// <summary>
        /// Gets or sets the s description.
        /// </summary>
        /// <value>The s description.</value>
        [DataMember]
        public string sDescription
        {
            get
            {
                return _sDescription;
            }

            set
            {
                _sDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets the client SBU id.
        /// </summary>
        /// <value>The client SBU id.</value>
        [DataMember]
        public int ClientSBUId
        {
            get
            {
                return _ClientSBUId;
            }

            set
            {
                _ClientSBUId = value;
            }
        }
        /// <summary>
        /// Gets or sets the dt process break map.
        /// </summary>
        /// <value>The dt process break map.</value>
        [DataMember]
        public DataTable dtClientSBUMap
        {
            set
            {
                _dtClientSBUMap = value;
            }
            get
            {
                return _dtClientSBUMap;
            }
        }
        #endregion

    }
}
