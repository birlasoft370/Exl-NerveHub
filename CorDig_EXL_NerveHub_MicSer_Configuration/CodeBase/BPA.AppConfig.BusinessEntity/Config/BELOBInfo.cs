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
    public class BELOBInfo : ObjectBase
    {
        private int _iLOBID;
        private string _sLOBName;
        private int _iERPID;
        private string _sDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="BELOBInfo"/> class.
        /// </summary>
        public BELOBInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BELOBInfo"/> class.
        /// </summary>
        /// <param name="iLOBID">The i defect ID.</param>
        /// <param name="iERPID">The i ERPID.</param>
        /// <param name="sLOBName">Name of the s defect.</param>
        /// <param name="sDescription">The s description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [bdisabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BELOBInfo(int iLOBID, int iERPID, string sLOBName, string sDescription, bool bDisabled, int iCreatedBy)
        {
            _iLOBID = iLOBID;
            _iERPID = iERPID;
            _sLOBName = sLOBName;
            _sDescription = sDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        /// <summary>
        /// Gets or sets  LOB ID.
        /// </summary>
        /// <value> LOB ID.</value>
        [DataMember]
        public int iLOBID
        {
            get
            {
                return _iLOBID;
            }
            set
            {
                _iLOBID = value;
            }
        }
        /// <summary>
        /// Gets or sets  ERP ID.
        /// </summary>
        /// <value> ERP ID.</value>
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
        /// Gets or sets  LOB Name.
        /// </summary>
        /// <value> LOB Name.</value>
        [DataMember]
        public string sLOBName
        {
            get
            {
                return _sLOBName;
            }
            set
            {
                _sLOBName = value;
            }
        }

        /// <summary>
        /// Gets or sets  LOB Decription.
        /// </summary>
        /// <value> LOB Description.</value>
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


    }
}
