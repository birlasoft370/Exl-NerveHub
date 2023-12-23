using System.Runtime.Serialization;
using MicUI.Configuration.Models;

namespace MicUI.Configuration.Services.ServiceModel
{
    [Serializable]
    [DataContract]
    public class BEVerticalInfo : ObjectBase
    {
        private int _iVerticalID;
        private string _sVerticalName;
        private int _iERPID;
        private string _sDescription;

        /// <summary>
        /// Initializes a new instance of the <see cref="BEVerticalInfo"/> class.
        /// </summary>
        public BEVerticalInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="BEVerticalInfo"/> class.
        /// </summary>
        /// <param name="iVerticalID">The i defect ID.</param>
        /// <param name="iERPID">The i ERPID.</param>
        /// <param name="sVerticalName">Name of the s defect.</param>
        /// <param name="sDescription">The s description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [bdisabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEVerticalInfo(int iVerticalID, int iERPID, string sVerticalName, string sDescription, bool bDisabled, int iCreatedBy)
        {
            _iVerticalID = iVerticalID;
            _iERPID = iERPID;
            _sVerticalName = sVerticalName;
            _sDescription = sDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        /// <summary>
        /// Gets or sets  Vertical ID.
        /// </summary>
        /// <value> Vertical ID.</value>
        [DataMember]
        public int iVerticalID
        {
            get
            {
                return _iVerticalID;
            }
            set
            {
                _iVerticalID = value;
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
        /// Gets or sets  Name.
        /// </summary>
        /// <value> Name.</value>       
        public string Name
        {
            get
            {
                string str = _iERPID.ToString() + " : " + _sVerticalName;
                if (bDisabled)
                {
                    str += " (Disabled)";
                }
                return str + "";
            }
        }

        /// <summary>
        /// Gets or sets the name of the s vertical.
        /// </summary>
        /// <value>The name of the s vertical.</value>
        [DataMember]
        public string sVerticalName
        {
            get
            {
                return _sVerticalName;
            }
            set
            {
                _sVerticalName = value;
            }
        }

        /// <summary>
        /// Gets or sets  Vertical Decription.
        /// </summary>
        /// <value> Vertical Description.</value>
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
