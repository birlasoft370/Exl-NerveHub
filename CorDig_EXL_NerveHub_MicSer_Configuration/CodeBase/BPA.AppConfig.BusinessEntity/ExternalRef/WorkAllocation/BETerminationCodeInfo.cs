using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    public class BETerminationCodeInfo : ObjectBase
    {
        private int _iTerminationCodeID;
        private string _sTermCodeName;
        private string _sTermCodeDesc;
        private bool _IsProductive;
        private bool _IsEnd;


        /// <summary>
        /// Initializes a new instance of the <see cref="BETerminationCodeInfo"/> class.
        /// </summary>
        public BETerminationCodeInfo()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BETerminationCodeInfo"/> class.
        /// </summary>
        /// <param name="iTerminationCodeID">The termination code ID.</param>
        /// <param name="sTermCodeName">Name of the term code.</param>
        /// <param name="sTermCodeDesc">The term code desc.</param>
        /// <param name="bDisabled">if set to <c>true</c> [disabled].</param>
        /// <param name="iCreatedBy">The created by.</param>
        /// <param name="dCreateDate">The create date.</param>
        public BETerminationCodeInfo(int iTerminationCodeID, string sTermCodeName,
            string sTermCodeDesc, bool bDisabled, int iCreatedBy, DateTime dCreateDate)
        {
            _iTerminationCodeID = iTerminationCodeID;
            _sTermCodeName = sTermCodeName;
            _sTermCodeDesc = sTermCodeDesc;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
            base.dCreateDate = dCreateDate;
        }

        /// <summary>
        /// Gets or sets the termination code ID.
        /// </summary>
        /// <value>The termination code ID.</value>
        public int iTerminationCodeID
        {
            get { return _iTerminationCodeID; }
            set { _iTerminationCodeID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the term code.
        /// </summary>
        /// <value>The name of the term code.</value>
        public string sTermCodeName
        {
            get { return _sTermCodeName; }
            set { _sTermCodeName = value; }
        }

        /// <summary>
        /// Gets or sets the term code desc.
        /// </summary>
        /// <value>The term code desc.</value>
        public string sTermCodeDesc
        {
            get { return _sTermCodeDesc; }
            set { _sTermCodeDesc = value; }
        }

        [DataMember]
        public bool bChecked
        {
            get;
            set;
        }

        public bool IsProductive
        {
            get { return _IsProductive; }
            set { _IsProductive = value; }
        }

        public bool IsEnd
        {
            get { return _IsEnd; }
            set { _IsEnd = value; }
        }

    }
}
