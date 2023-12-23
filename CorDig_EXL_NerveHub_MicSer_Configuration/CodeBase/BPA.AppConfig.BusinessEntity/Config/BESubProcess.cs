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
    public class BESubProcess : ObjectBase
    {
        private DateTime _dSubProcessStartDate;
        private DateTime _dSubProcessEndDate;
        private DateTime _dGoLiveDate;
        private DateTime _dStabilizationStartDate;
        private DateTime _dStabilizationEndDate;
        private DateTime _dProductionStartDate;
        private DateTime _dProductionEndDate;

        [DataMember]
        public int iClientID { get; set; }
        /// <summary>
        /// Gets or sets the sub process ID.
        /// </summary>
        /// <value>The sub process ID.</value>
        [DataMember]
        public int iSubProcessID { get; set; }

        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        /// <value>The process ID.</value>
        [DataMember]
        public int iProcessID { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub process.
        /// </summary>
        /// <value>The name of the sub process.</value>
        [DataMember]
        public string sSubProcessName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string sDescription { get; set; }

        /// <summary>
        /// Gets or sets the sub process start date.
        /// </summary>
        /// <value>The sub process start date.</value>
        [DataMember]
        public DateTime dSubProcessStartDate
        {
            get
            {
                return _dSubProcessStartDate;
            }
            set
            {
                _dSubProcessStartDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the sub process endate.
        /// </summary>
        /// <value>The sub process endate.</value>
        [DataMember]
        public DateTime dSubProcessEndDate
        {
            get
            {
                return _dSubProcessEndDate;
            }
            set
            {
                _dSubProcessEndDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the go live date.
        /// </summary>
        /// <value>The go live date.</value>
        [DataMember]
        public DateTime dGoLiveDate
        {
            get
            {
                return _dGoLiveDate;
            }
            set
            {
                _dGoLiveDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the stabilization start date.
        /// </summary>
        /// <value>The stabilization start date.</value>
        [DataMember]
        public DateTime dStabilizationStartDate
        {
            get
            {
                return _dStabilizationStartDate;
            }
            set
            {
                _dStabilizationStartDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the stabilization endate.
        /// </summary>
        /// <value>The stabilization endate.</value>
        [DataMember]
        public DateTime dStabilizationEndDate
        {
            get
            {
                return _dStabilizationEndDate;
            }
            set
            {
                _dStabilizationEndDate = value;
            }
        }


        /// <summary>
        /// Gets or sets the production start date.
        /// </summary>
        /// <value>The production start date.</value>
        [DataMember]
        public DateTime dProductionStartDate
        {
            get
            {
                return _dProductionStartDate;
            }
            set
            {
                _dProductionStartDate = value;
            }
        }
        /// <summary>
        /// Gets or sets the production endate.
        /// </summary>
        /// <value>The production endate.</value>
        [DataMember]
        public DateTime dProductionEndDate
        {
            get
            {
                return _dProductionEndDate;
            }
            set
            {
                _dProductionEndDate = value;
            }
        }

        [DataMember]
        public Int64 iPDSubProcessID { get; set; }

        [DataMember]
        public string sPDSubProcessName { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>        
        public string Name
        {
            get
            {
                string str = sSubProcessName;
                if (bDisabled)
                { str += " (Disabled)"; }
                return str;
            }
        }
    }
}
