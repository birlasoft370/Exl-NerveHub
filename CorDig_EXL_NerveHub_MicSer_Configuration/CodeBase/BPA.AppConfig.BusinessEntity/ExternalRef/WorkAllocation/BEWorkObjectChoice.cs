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
    public class BEWorkObjectChoice : ObjectBase
    {

        private int _iObjectChoiceID;
        private int _iObjID;
        private int _iGroupID;
        private string _sChoiceValue;
        private bool _bDisabled;
        private int _iOrder;
        private string _iUid;
        private string _iChoiceUid;
        private int _sCultureid { get; set; }
        private string _sLanguage { get; set; }
        private string _sConvertText { get; set; }

        private int _iChoiceLanguageID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkObjectChoice"/> class.
        /// </summary>
        public BEWorkObjectChoice()
        {

        }

        public BEWorkObjectChoice(int iObjectChoiceID, int iObjID, int iGroupID, string sChoiceValue, bool bDisabled, int iOrder)
        {
            _iObjectChoiceID = iObjectChoiceID;
            _iObjID = iObjID;
            _sChoiceValue = sChoiceValue;
            _iGroupID = iGroupID;
            _bDisabled = bDisabled;
            _iOrder = iOrder;
        }

        public BEWorkObjectChoice(int iObjectChoiceID, int iObjID, int iGroupID, string sChoiceValue, bool bDisabled, int iOrder, string iUid, int sCultureid, string sConvertText, int iChoiceLanguageID, string iChoiceUID)
        {
            _iObjectChoiceID = iObjectChoiceID;
            _iObjID = iObjID;
            _sChoiceValue = sChoiceValue;
            _iGroupID = iGroupID;
            _bDisabled = bDisabled;
            _iOrder = iOrder;
            _iUid = iUid;
            _iChoiceUid = iChoiceUID;
            _sCultureid = sCultureid;
            _sConvertText = sConvertText;
            _iChoiceLanguageID = iChoiceLanguageID;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkObjectChoice"/> class.
        /// </summary>
        /// <param name="iObjectChoiceID">The i object choice ID.</param>
        /// <param name="iObjID">The i obj ID.</param>
        /// <param name="sChoiceValue">The s choice value.</param>
        /// <param name="bDisabled">if set to <c>true</c> [b disabled].</param>
        public BEWorkObjectChoice(int iObjectChoiceID, int iObjID, int iGroupID, string sChoiceValue, bool bDisabled, int iOrder, string iUid, int sCultureid, string sConvertText, int iChoiceLanguageID)
        {
            _iObjectChoiceID = iObjectChoiceID;
            _iObjID = iObjID;
            _sChoiceValue = sChoiceValue;
            _iGroupID = iGroupID;
            _bDisabled = bDisabled;
            _iOrder = iOrder;
            _iUid = iUid;
            _sCultureid = sCultureid;
            _sConvertText = sConvertText;
            _iChoiceLanguageID = iChoiceLanguageID;
        }

        [DataMember]
        public int iChoiceLanguageID
        {
            get { return _iChoiceLanguageID; }
            set { _iChoiceLanguageID = value; }
        }
        [DataMember]
        public int sCultureid
        {
            get { return _sCultureid; }
            set { _sCultureid = value; }
        }
        [DataMember]
        public string sLanguage
        {
            get { return _sLanguage; }
            set { _sLanguage = value; }
        }
        [DataMember]
        public string sConvertText
        {
            get { return _sConvertText; }
            set { _sConvertText = value; }
        }
        [DataMember]
        public string iUid
        {
            get { return _iUid; }
            set { _iUid = value; }
        }
        /// <summary>
        /// Gets or sets the i object choice ID.
        /// </summary>
        /// <value>The i object choice ID.</value>
        [DataMember]
        public int iGroupID
        {
            get
            {
                return _iGroupID;
            }
            set
            {
                _iGroupID = value;
            }
        }

        /// <summary>
        /// Gets or sets the i object choice ID.
        /// </summary>
        /// <value>The i object choice ID.</value>
        [DataMember]
        public int iObjectChoiceID
        {
            get
            {
                return _iObjectChoiceID;
            }
            set
            {
                _iObjectChoiceID = value;
            }
        }


        /// <summary>
        /// Gets or sets the i obj ID.
        /// </summary>
        /// <value>The i obj ID.</value>
        [DataMember]
        public int iObjID
        {
            get
            {
                return _iObjID;
            }
            set
            {
                _iObjID = value;
            }
        }

        /// <summary>
        /// Gets or sets the s choice value.
        /// </summary>
        /// <value>The s choice value.</value>
        [DataMember]
        public string sChoiceValue
        {
            get { return _sChoiceValue; }
            set { _sChoiceValue = value; }
        }

        [DataMember]
        public bool bDisabled
        {
            get { return _bDisabled; }
            set { _bDisabled = value; }
        }

        [DataMember]
        public int iOrder
        {
            get
            {
                return _iOrder;
            }
            set
            {
                _iOrder = value;
            }
        }



    }
}
