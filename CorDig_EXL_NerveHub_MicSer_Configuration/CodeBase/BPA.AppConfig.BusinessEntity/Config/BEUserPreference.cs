namespace BPA.AppConfig.BusinessEntity.Config
{
    public class BEUserPreference : ObjectBase
    {
        private int _iUserID;
        private BETimeZoneInfo _oTimezone;
        private int _iTimeZoneID;
        private string _sLanguage;
        private bool _bDisable;

        public BEUserPreference()
        { }

        public int iUserID
        {
            get
            {
                return _iUserID;
            }
            set
            {
                _iUserID = value;
            }
        }

        public BETimeZoneInfo oTimezone
        {
            get
            {
                return _oTimezone;
            }
            set
            {
                _oTimezone = value;
            }
        }

        public int iTimeZoneID
        {
            get
            {
                return _iTimeZoneID;
            }
            set
            {
                _iTimeZoneID = value;
            }
        }

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
        public bool bDisable
        {
            get
            {
                return _bDisable;
            }
            set
            {
                _bDisable = value;
            }
        }

        public BEUserPreference(int iUserID, int TimeZoneID, string sLanguage, bool bDisable)
        {
            _iUserID = iUserID;
            _sLanguage = sLanguage;
            _iTimeZoneID = TimeZoneID;
            _bDisable = bDisable;
        }
    }
}
