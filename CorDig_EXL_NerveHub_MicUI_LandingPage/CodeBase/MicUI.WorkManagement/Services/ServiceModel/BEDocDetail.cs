using System.Runtime.Serialization;

namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEDocDetail : ObjectBase
    {


        private int _iTemplateId;
        private int _iClientId;
        private int _iProcessId;
        private string _sTemplateName;
        private string _sDescription;
        private string _sXmlColumn;
        private bool _bDisabled;
        private Byte[] _bFileContent;

        private string _sPdfFilePath;

        public string sPdfFilePath
        {
            get { return _sPdfFilePath; }
            set { _sPdfFilePath = value; }
        }

        public int iTemplateId
        {
            get { return _iTemplateId; }
            set { _iTemplateId = value; }
        }
        [DataMember]
        public int iClientId
        {
            get { return _iClientId; }
            set { _iClientId = value; }
        }
        [DataMember]
        public int iProcessId
        {
            get { return _iProcessId; }
            set { _iProcessId = value; }
        }
        [DataMember]
        public string sTemplateName
        {
            get { return _sTemplateName; }
            set { _sTemplateName = value; }
        }
        [DataMember]
        public string sDescription
        {
            get { return sPdfFilePath; }
            set { sPdfFilePath = value; }
        }

        [DataMember]
        public string sXmlColumn
        {
            get { return _sXmlColumn; }
            set { _sXmlColumn = value; }
        }

        [DataMember]
        public bool bDisabled
        {
            get { return _bDisabled; }
            set { _bDisabled = value; }
        }
        [DataMember]
        public Byte[] bFileContent
        {
            get { return _bFileContent; }
            set { _bFileContent = value; }
        }

        public BEDocDetail()
        { }

        public BEDocDetail(int iTemplateId, int iProcessId, string sTemplateName, string sDescription, bool bDisabled)
        {
            _iTemplateId = iTemplateId;
            _iProcessId = iProcessId;
            _sTemplateName = sTemplateName;
            sPdfFilePath = sDescription;
            base.bDisabled = bDisabled;
        }

        public BEDocDetail(int iTemplateId, string sTemplateName)
        {
            _iTemplateId = iTemplateId;
            _sTemplateName = sTemplateName;

        }
    }
}
