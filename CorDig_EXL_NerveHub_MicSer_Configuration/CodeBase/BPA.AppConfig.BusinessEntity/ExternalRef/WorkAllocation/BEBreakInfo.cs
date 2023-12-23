using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    [DataContract]
    public class BEBreakInfo : ObjectBase
    {
        private int _iBreakID;
        private string _sBreakName;
        private string _sBreakDescription;
        private bool _IsProductive;
        private bool _IsSchedule;
        private bool _IsChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakInfo"/> class.
        /// </summary>
        public BEBreakInfo()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakInfo"/> class.
        /// </summary>
        /// <param name="iBreakID">The break ID.</param>
        /// <param name="sBreakName">Name of the break.</param>
        /// <param name="sBreakDescription">The break description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The created by.</param>
        /// <param name="dCreateDate">The create date.</param>
        public BEBreakInfo(int iBreakID, string sBreakName, string sBreakDescription,
             bool bDisabled, int iCreatedBy, DateTime dCreateDate, bool IsChecked)
        {
            _iBreakID = iBreakID;
            _sBreakName = sBreakName;
            _sBreakDescription = sBreakDescription;
            base.bDisabled = bDisabled;
            base.dCreateDate = dCreateDate;
            base.iCreatedBy = iCreatedBy;
            _IsChecked = IsChecked;
        }

        /// <summary>
        /// Gets or sets the break ID.
        /// </summary>
        /// <value>The break ID.</value>
        [DataMember]
        public int iBreakID
        {
            get { return _iBreakID; }
            set { _iBreakID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the break.
        /// </summary>
        /// <value>The name of the break.</value>
        [DataMember]
        public string sBreakName
        {
            get { return _sBreakName; }
            set { _sBreakName = value; }
        }
        /// <summary>
        /// Gets or sets the break description.
        /// </summary>
        /// <value>The break description.</value>
        [DataMember]
        public string sBreakDescription
        {
            get { return _sBreakDescription; }
            set { _sBreakDescription = value; }
        }

        [DataMember]
        public bool bChecked
        {
            get;
            set;
        }
        [DataMember]
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; }
        }
        public bool IsProductive
        {
            get { return _IsProductive; }
            set { _IsProductive = value; }
        }

        public bool IsSchedule
        {
            get { return _IsSchedule; }
            set { _IsSchedule = value; }
        }
    }
}
