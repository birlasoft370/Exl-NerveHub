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
    public class BELocation : ObjectBase
    {
        #region Field
        private int _iLocationID;
        private string _sLocationName;
        private string _sLocationDescription;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="oLocation"/> class.
        /// </summary>
        public BELocation()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="oLocationInfo"/> class.
        /// </summary>
        /// <param name="iLocationID">The Location ID.</param>
        /// <param name="sLocationName">Name of the Location.</param>
        /// <param name="sLocationDescription">The Location description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BELocation(int iLocationID, string sLocationName, string sLocationDescription, bool bDisabled, int iCreatedBy)
        {
            _iLocationID = iLocationID;
            _sLocationName = sLocationName;
            _sLocationDescription = sLocationDescription;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Location ID.
        /// </summary>
        /// <value>The Location ID.</value>
        [DataMember]
        public int iLocationID
        {
            get { return _iLocationID; }
            set { _iLocationID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Location.
        /// </summary>
        /// <value>The name of the Location.</value>
        [DataMember]
        public string sLocationName
        {
            get { return _sLocationName; }
            set { _sLocationName = value; }
        }

        /// <summary>
        /// Gets or sets the Location description.
        /// </summary>
        /// <value>The Location description.</value>
        [DataMember]
        public string sLocationDescription
        {
            get { return _sLocationDescription; }
            set { _sLocationDescription = value; }
        }
        #endregion
    }
}
