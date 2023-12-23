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
    public class BETimeZoneInfo : ObjectBase
    {
        #region TimeZone Fields
        private int _iTimeZoneID;
        private string _sTimeZoneID;
        private string _sTimeZoneName;
        private string _sTimeZoneDescription;
        private string _sOffsetGMT;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="oTimeZoneInfo"/> class.
        /// </summary>
        public BETimeZoneInfo()
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="oTimeZoneInfo"/> class.
        /// </summary>
        /// <param name="iTimeZoneID">Time zone ID.</param>
        /// <param name="sTimeZoneName">Name of the time zone.</param>
        /// <param name="sTimeZoneDescription">Time zone description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">Created by.</param>
        public BETimeZoneInfo(int iTimeZoneID, string sTimeZoneID, string sTimeZoneName, string sTimeZoneDescription, string sOffsetGMT, bool bDisabled, int iCreatedBy)
        {
            _iTimeZoneID = iTimeZoneID;
            _sTimeZoneID = sTimeZoneID;
            _sTimeZoneName = sTimeZoneName;
            _sTimeZoneDescription = sTimeZoneDescription;
            _sOffsetGMT = sOffsetGMT;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the time zone ID.
        /// </summary>
        /// <value>The time zone ID.</value>
        [DataMember]
        public int iTimeZoneID
        {
            get
            {
                return _iTimeZoneID;
            }
            set
            {
                _iTimeZoneID = value;
            }
        }

        [DataMember]
        public string sTimeZoneID
        {
            get
            {
                return _sTimeZoneID;
            }
            set
            {
                _sTimeZoneID = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the time zone.
        /// </summary>
        /// <value>The name of the time zone.</value>
        [DataMember]
        public string sTimeZoneName
        {
            get
            {
                return _sTimeZoneName;
            }
            set
            {
                _sTimeZoneName = value;
            }
        }

        /// <summary>
        /// Gets or sets the time zone description.
        /// </summary>
        /// <value>The time zone description.</value>
        [DataMember]
        public string sTimeZoneDescription
        {
            get
            {
                return _sTimeZoneDescription;
            }
            set
            {
                _sTimeZoneDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets the offset GMT.
        /// </summary>
        /// <value>offset GMT.</value>
        [DataMember]
        public string sOffsetGMT
        {
            get
            {
                return _sOffsetGMT;
            }
            set
            {
                _sOffsetGMT = value;
            }
        }
        #endregion
    }
}
