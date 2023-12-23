using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    public class BEProcessFTE : ObjectBase
    {
        #region Member Fields
        private int _iProcessFTEID;
        //private int _iFTE;
        //private int _iQCACount;
        private float _iFTE;
        private float _iQCACount;
        private DateTime _dtEffectiveEndDate;
        private DateTime _dtEffectiveStartDate;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessFTEMaster"/> class.
        /// </summary>
        public BEProcessFTE()
        { oLocation = new BELocation(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessFTEMaster"/> class.
        /// </summary>
        /// <param name="iProcessFTEID">The ERPProcessID.</param>
        /// <param name="iProcessID">ProcessID</param>
        /// <param name="iLocationId">LocationId</param>
        /// <param name="iFTE">FTE.</param>
        /// <param name="iQCACount">QCA count.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">created by.</param>
        public BEProcessFTE(int iProcessFTEID, int iProcessID, int iLocationId, float iFTE, float iQCACount, bool bDisabled, int iCreatedBy)
        {
            _iProcessFTEID = iProcessFTEID;
            _iFTE = iFTE;
            _iQCACount = iQCACount;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ERPProcessID.
        /// </summary>
        /// <value>The ERPProcessID.</value>
        public int iProcessFTEID
        {
            get
            {
                return _iProcessFTEID;
            }
            set
            {
                _iProcessFTEID = value;
            }
        }


        /// <summary>
        /// Gets or sets the i FTE.
        /// </summary>
        /// <value>The i FTE.</value>
        public float iFTE
        {
            get
            {
                return _iFTE;
            }
            set
            {
                _iFTE = value;
            }
        }


        /// <summary>
        /// Gets or sets the i QCA count.
        /// </summary>
        /// <value>The i QCA count.</value>
        public float iQCACount
        {
            get
            {
                return _iQCACount;
            }
            set
            {
                _iQCACount = value;
            }
        }

        /// <summary>
        /// Gets or sets the location object
        /// </summary>
        /// <value>location object.</value>
        public BELocation oLocation { get; set; }

        /// <summary>
        /// Gets or sets the state of the row.
        /// </summary>
        /// <value>The state of the row.</value>
        public RowState oRowState { get; set; }

        /// <summary>
        /// Gets or sets the dt effective start date.
        /// </summary>
        /// <value>The dt effective start date.</value>
        public DateTime dtEffectiveStartDate
        {
            get { return _dtEffectiveStartDate; }
            set { _dtEffectiveStartDate = value; }
        }
        /// <summary>
        /// Gets or sets the dt effective end date.
        /// </summary>
        /// <value>The dt effective end date.</value>
        public DateTime dtEffectiveEndDate
        {
            get { return _dtEffectiveEndDate; }
            set { _dtEffectiveEndDate = value; }
        }
        #endregion
    }
}
