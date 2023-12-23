using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    [DataContract]
    public class BEERPProcess : ObjectBase
    {
        #region Member Fields
        private int _iERPProcessID;
        private int _iERPCode;
        private string _sClient;
        private string _sName;
        private string _sDescription;
        private BELocation _oLocation;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BEERPProcessMaster"/> class.
        /// </summary>
        public BEERPProcess()
        {
            oLocation = new BELocation();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEERPProcessMaster"/> class.
        /// </summary>
        /// <param name="iERPProcessID">ERPProcessID.</param>
        /// <param name="sName">Name ERPProcess.</param>
        /// <param name="sDescription">Description.</param>
        /// <param name="iERPCode">iERPCode</param>
        /// <param name="oLocation">Location object</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        /// <param name="iCreatedBy">created by.</param>
        public BEERPProcess(int iERPProcessID, string sName, string sDescription, int iERPCode, BELocation oLocation, bool bDisabled, int iCreatedBy)
        {
            _iERPProcessID = iERPProcessID;
            _sName = sName;
            _sDescription = sDescription;
            _iERPCode = iERPCode;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
            _oLocation = oLocation;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the ERPProcessID.
        /// </summary>
        /// <value>The ERPProcessID.</value>
        [DataMember]
        public int iERPProcessID
        {
            get
            {
                return _iERPProcessID;
            }
            set
            {
                _iERPProcessID = value;
            }
        }

        /// <summary>
        /// Gets or sets the s client.
        /// </summary>
        /// <value>The s client.</value>
        [DataMember]
        public string sClient
        {
            get
            {
                return _sClient;
            }
            set
            {
                _sClient = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the  ERPProcess.
        /// </summary>
        /// <value>The name of the  ERPProcess.</value>
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
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
        /// Gets or sets the  ERPCode 
        /// </summary>
        /// <value>The ERPCode .</value>
        [DataMember]
        public int iERPCode
        {
            get
            {
                return _iERPCode;
            }
            set
            {
                _iERPCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        [DataMember]
        public BELocation oLocation { get { return _oLocation; } set { _oLocation = value; } }

        #endregion
    }
}
