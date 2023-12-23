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
    public class BEWorkObjectTranslateList : ObjectBase
    {
        private int _iSDOBJLanID;
        private int _iObjID;
        private int _iLanguageID;
        private string _sCulture;
        private string _sLanguage;
        private string _sConvertText;



        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkObjectChoice"/> class.
        /// </summary>
        public BEWorkObjectTranslateList()
        {

        }



        public BEWorkObjectTranslateList(int iSDOBJLanID, int iObjID, int iLanguageID, string sCulture, string sLanguage, string sConvertText)
        {
            _iSDOBJLanID = iSDOBJLanID;
            _iObjID = iObjID;
            _iLanguageID = iLanguageID;
            _sCulture = sCulture;
            _sLanguage = sLanguage;
            _sConvertText = sConvertText;

        }
        [DataMember]
        public int iSDOBJLanID
        {
            get
            {
                return _iSDOBJLanID;
            }
            set
            {
                _iSDOBJLanID = value;
            }
        }
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
        /// Gets or sets the i object choice ID.
        /// </summary>
        /// <value>The i object choice ID.</value>
        [DataMember]
        public int iLanguageID
        {
            get
            {
                return _iLanguageID;
            }
            set
            {
                _iLanguageID = value;
            }
        }

        /// <summary>
        /// Gets or sets the s choice value.
        /// </summary>
        /// <value>The s choice value.</value>
        [DataMember]
        public string sCulture
        {
            get { return _sCulture; }
            set { _sCulture = value; }
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
            get
            {
                return _sConvertText;
            }
            set
            {
                _sConvertText = value;
            }
        }



    }
}
