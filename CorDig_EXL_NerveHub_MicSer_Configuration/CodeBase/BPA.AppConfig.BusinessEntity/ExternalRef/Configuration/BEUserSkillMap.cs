using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.Configuration
{
    [Serializable]
    [DataContract]
    public class BEUserSkillMap : ObjectBase
    {
        private int _iUserID;
        private string _sUserName;
        private string _sSkillID;
        private int _iCampaignID;
        private int _iSkillID;
        private string _sSkillName;

        public BEUserSkillMap()
        { }

        [DataMember]
        public int iUserID
        {
            get { return _iUserID; }
            set { _iUserID = value; }
        }

        [DataMember]
        public string sUserName
        {
            get { return _sUserName; }
            set { _sUserName = value; }
        }

        [DataMember]
        public string sSkillID
        {
            get { return _sSkillID; }
            set { _sSkillID = value; }
        }
        [DataMember]
        public int iCampaignID
        {
            get { return _iCampaignID; }
            set { _iCampaignID = value; }
        }

        [DataMember]
        public int iSkillID
        {
            get { return _iSkillID; }
            set { _iSkillID = value; }
        }

        [DataMember]
        public string sSkillName
        {
            get { return _sSkillName; }
            set { _sSkillName = value; }
        }
    }
}
