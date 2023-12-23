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
    public class BEFacility : ObjectBase
    {
        #region Facility Fields
        private int _iFacilityID;
        private int _iLocationID;
        private string _sFacilityName;
        private string _sFacilityDescription;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="oFacilityInfo"/> class.
        /// </summary>
        public BEFacility()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="oFacilityInfo"/> class.
        /// </summary>
        /// <param name="iFacilityID">The Facility ID.</param>
        /// <param name="iLocationID">The i location ID.</param>
        /// <param name="sFacilityName">Name of the Facility.</param>
        /// <param name="sFacilityDescription">The Facility description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [ disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEFacility(int iFacilityID, int iLocationID, string sFacilityName, string sFacilityDescription, bool bDisabled, int iCreatedBy)
        {
            _iFacilityID = iFacilityID;
            _iLocationID = iLocationID;
            _sFacilityName = sFacilityName;
            _sFacilityDescription = sFacilityDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets Unique Facility ID
        /// </summary>
        /// <value>The Facility ID.</value>
        [DataMember]
        public int iFacilityID
        {
            get
            {
                return _iFacilityID;
            }
            set
            {
                _iFacilityID = value;
            }
        }



        /// <summary>
        /// Gets or sets the Location ID.
        /// </summary>
        /// <value>Location ID.</value>
        [DataMember]
        public int iLocationID
        {
            get
            {
                return _iLocationID;
            }
            set
            {
                _iLocationID = value;
            }
        }
        /// <summary>
        /// Gets or sets Facility Name
        /// </summary>
        /// <value>The name of the Facility.</value>
        [DataMember]
        public string sFacilityName
        {
            get
            {
                return _sFacilityName;
            }
            set
            {
                _sFacilityName = value;
            }
        }

        /// <summary>
        /// Gets or sets Facility description.
        /// </summary>
        /// <value>The Facility description.</value>
        [DataMember]
        public string sFacilityDescription
        {
            get
            {
                return _sFacilityDescription;
            }
            set
            {
                _sFacilityDescription = value;
            }
        }
        #endregion
    }
}
