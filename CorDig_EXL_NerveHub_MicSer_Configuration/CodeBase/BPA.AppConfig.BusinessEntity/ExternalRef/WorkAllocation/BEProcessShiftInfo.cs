using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    public class BEProcessShiftInfo : ObjectBase
    {
        private int _iProcessID;
        private int _iProcessShiftID;
        private IList<BEShiftInfo> _oShift;

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessShiftInfo"/> class.
        /// </summary>
        public BEProcessShiftInfo()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessShiftInfo"/> class.
        /// </summary>
        /// <param name="iProcessID">The i process ID.</param>
        public BEProcessShiftInfo(int iProcessID)
        {
            _iProcessID = iProcessID;

            base.iCreatedBy = iCreatedBy;
        }

        /// <summary>
        /// Gets or sets the i process ID.
        /// </summary>
        /// <value>The i process ID.</value>
        public int iProcessID
        {
            get { return _iProcessID; }
            set { _iProcessID = value; }
        }



        /// <summary>
        /// Gets or sets the i shift ID.
        /// </summary>
        /// <value>The i shift ID.</value>
        public IList<BEShiftInfo> oShift
        {
            get { return _oShift; }
            set { _oShift = value; }
        }


    }
}
