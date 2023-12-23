using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    public class BEShiftInfo : ObjectBase
    {
        private int _iShiftID;
        private string _sShiftName;
        private string _sShiftDescription;
        private string _sStartTime;
        private string _sEndTime;

        /// <summary>
        /// Initializes a new instance of  <see cref="oShiftInfo"/> class.
        /// </summary>
        public BEShiftInfo()
        { }

        /// <summary>
        /// Initializes a new instance of  <see cref="oShiftInfo"/> class.
        /// </summary>
        /// <param name="iShiftID"> shift ID.</param>
        /// <param name="sShiftName">Name of shift.</param>
        /// <param name="sShiftDescription"> shift description.</param>
        /// <param name="sStartTime"> start time.</param>
        /// <param name="sEndTime"> end time.</param>
        /// <param name="iCreatedBy">created by.</param>
        public BEShiftInfo(int iShiftID, string sShiftName, string sShiftDescription, string sStartTime, string sEndTime, int iCreatedBy, Boolean Disabled)
        {
            _iShiftID = iShiftID;
            _sShiftName = sShiftName;
            _sShiftDescription = sShiftDescription;
            _sStartTime = sStartTime;
            _sEndTime = sEndTime;
            base.bDisabled = Disabled;
            base.iCreatedBy = iCreatedBy;
        }


        /// <summary>
        /// Gets or sets  shift ID.
        /// </summary>
        /// <value> shift ID.</value>
        public int iShiftID
        {
            get
            {
                return _iShiftID;
            }
            set
            {
                _iShiftID = value;
            }
        }

        /// <summary>
        /// Gets or sets  name of  shift.
        /// </summary>
        /// <value> name of shift.</value>
        public string sShiftName
        {
            get
            {
                return _sShiftName;
            }
            set
            {
                _sShiftName = value;
            }
        }

        /// <summary>
        /// Gets or sets  shift description.
        /// </summary>
        /// <value> shift description.</value>
        public string sShiftDescription
        {
            get
            {
                return _sShiftDescription;
            }
            set
            {
                _sShiftDescription = value;
            }
        }
        /// <summary>
        /// Gets or sets  start time.
        /// </summary>
        /// <value> start time.</value>
        public string sStartTime
        {
            get
            {
                return _sStartTime;
            }
            set
            {
                _sStartTime = value;
            }
        }
        /// <summary>
        /// Gets or sets  end time.
        /// </summary>
        /// <value> end time.</value>
        public string sEndTime
        {
            get
            {
                return _sEndTime;
            }
            set
            {
                _sEndTime = value;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                if (_sStartTime != null && _sEndTime != null)
                    return _sShiftName + " (" + _sStartTime + "-" + _sEndTime + ")";
                else
                    return _sShiftName;
            }
        }
    }
}
