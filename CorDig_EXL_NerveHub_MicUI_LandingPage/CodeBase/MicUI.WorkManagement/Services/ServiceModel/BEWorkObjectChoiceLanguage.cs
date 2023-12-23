using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEWorkObjectChoiceLanguage : ObjectBase
    {

        private int _iObjChoiceLanguageID;
        private int _iObjChoiceID;
        private int _iObjID;
        private int _iGroupID;
        private string _sChoiceValue;
        private bool _bDisabled;
        private int _iOrder;
        private int _iLanguageID;

        public BEWorkObjectChoiceLanguage()
        {

        }
        public BEWorkObjectChoiceLanguage(int iObjChoiceLanguageID, int iObjChoiceID, int iObjID, int iGroupID, string sChoiceValue, bool bDisabled, int iOrder, int iLanguageID)
        {
            _iObjChoiceLanguageID = iObjChoiceLanguageID;
            _iObjChoiceID = iObjChoiceID;
            _iObjID = iObjID;
            _sChoiceValue = sChoiceValue;
            _iGroupID = iGroupID;
            _bDisabled = bDisabled;
            _iOrder = iOrder;
            _iLanguageID = iLanguageID;

        }

        [DataMember]
        public int iObjChoiceLanguageID
        {
            get { return _iObjChoiceLanguageID; }
            set { _iObjChoiceLanguageID = value; }
        }
        [DataMember]
        public int iObjChoiceID
        {
            get { return _iObjChoiceID; }
            set { _iObjChoiceID = value; }
        }
        [DataMember]
        public int iObjID
        {
            get { return _iObjID; }
            set { _iObjID = value; }
        }
        [DataMember]
        public int iLanguageID
        {
            get { return _iLanguageID; }
            set { _iLanguageID = value; }
        }
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
