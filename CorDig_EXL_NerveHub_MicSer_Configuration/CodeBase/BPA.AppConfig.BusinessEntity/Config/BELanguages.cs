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
    public class BELanguages : ObjectBase
    {
        private int _iLanguageID;
        private string _sLanguage;
        private string _sCulture;
        public BELanguages()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BELanguages"/> class.
        /// </summary>
        /// <param name="iLanguage">The i Language ID.</param>
        /// <param name="sLanguage">Name of the s Language.</param>
        /// <param name="bDisabled">if set to <c>true</c> [bdisabled].</param>
        /// <param name="iCreatedBy">The i created by.</param>
        public BELanguages(int iLanguageID, string sLanguage, string sCulture, bool bDisabled, int iCreatedBy)
        {
            _iLanguageID = iLanguageID;
            _sLanguage = sLanguage;
            _sCulture = sCulture;
            base.bDisabled = bDisabled;
            base.iCreatedBy = iCreatedBy;
        }

        [DataMember]
        public string ApiKey { get; set; }
        [DataMember]
        public string ApiUrl { get; set; }
        /// <summary>
        /// Gets or sets  Language ID.
        /// </summary>
        /// <value> LanguageID.</value>
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
        /// Gets or sets  Language Name.
        /// </summary>
        /// <value> Language.</value>
        [DataMember]
        public string sLanguage
        {
            get
            {
                return _sLanguage;
            }
            set
            {
                _sLanguage = value;
            }
        }

        /// <summary>
        /// Gets or sets  Culture Name.
        /// </summary>
        /// <value> Culture.</value>
        [DataMember]
        public string sCulture
        {
            get
            {
                return _sCulture;
            }
            set
            {
                _sCulture = value;
            }
        }

        [DataMember]
        public DateTime dReceivedDateTime { get; set; }

        [DataMember]
        public int LanguageConfigID { get; set; }

        [DataMember]
        public int CampaignID { get; set; }

        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string LanguageFilePath { get; set; }

    }
}
