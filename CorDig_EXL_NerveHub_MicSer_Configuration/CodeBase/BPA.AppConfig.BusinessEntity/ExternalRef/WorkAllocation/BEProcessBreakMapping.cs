using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    public class BEProcessBreakMapping : ObjectBase
    {

        private int _iProcessId;
        private int _iClientId;
        private DataTable _dtProcessBreakMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessBreakMapping"/> class.
        /// </summary>
        public BEProcessBreakMapping()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEProcessBreakMapping"/> class.
        /// </summary>
        /// <param name="iProcessId">The i process id.</param>
        public BEProcessBreakMapping(int iClientID, int iProcessId, bool bDisabled, int iCreatedBy)
        {
            _iProcessId = iProcessId;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;

        }

        /// <summary>
        /// Gets or sets the i process id.
        /// </summary>
        /// <value>The i process id.</value>
        public int iProcessId
        {
            set
            {
                _iProcessId = value;
            }
            get
            {
                return _iProcessId;
            }
        }

        /// <summary>
        /// Gets or sets the i client ID.
        /// </summary>
        /// <value>The i client ID.</value>
        public int iClientID
        {
            set
            {
                _iClientId = value;
            }
            get
            {
                return _iClientId;
            }
        }

        /// <summary>
        /// Gets or sets the dt process break map.
        /// </summary>
        /// <value>The dt process break map.</value>
        public DataTable dtProcessBreakMap
        {
            set
            {
                _dtProcessBreakMap = value;
            }
            get
            {
                return _dtProcessBreakMap;
            }
        }

    }
}
