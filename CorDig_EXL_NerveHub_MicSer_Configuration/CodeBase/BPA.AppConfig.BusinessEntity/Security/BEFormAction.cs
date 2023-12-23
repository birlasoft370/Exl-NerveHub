using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Security
{
    [Serializable]
    [DataContract]
    public class BEFormAction : ObjectBase
    {
        private int _iFormID;
        private string _sFormName;
        private string _sDescription;
        private IList<BEActionInfo> _oAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="oFormAction"/> class.
        /// </summary>
        public BEFormAction()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="oFormAction"/> class.
        /// </summary>
        /// <param name="iFormID">The i form ID.</param>
        /// <param name="sFormName">Name of the s form.</param>
        /// <param name="sDescription">The s description.</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BEFormAction(int iFormID, string sFormName, string sDescription, bool bDisabled, int iCreatedBy)
        {
            _iFormID = iFormID;
            _sFormName = sFormName;
            _sDescription = sDescription;
            base.bDisabled = bDisabled;
        }


        /// <summary>
        /// Gets or sets the form ID.
        /// </summary>
        /// <value>form ID.</value>
        [DataMember]
        public int iFormID
        {
            get { return _iFormID; }
            set { _iFormID = value; }
        }
        /// <summary>
        /// Gets or sets the name of the form.
        /// </summary>
        /// <value>name of the form.</value>
        [DataMember]
        public string sFormName
        {
            get { return _sFormName; }
            set { _sFormName = value; }
        }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>description.</value>
        [DataMember]
        public string sDescription
        {
            get { return _sDescription; }
            set { _sDescription = value; }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>action.</value>
        [DataMember]
        public IList<BEActionInfo> oAction
        {
            get { return _oAction; }
            set { _oAction = value; }
        }

    }
}
