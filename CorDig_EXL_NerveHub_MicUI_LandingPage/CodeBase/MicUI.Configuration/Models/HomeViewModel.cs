using System.Web;

namespace MicUI.Configuration.Models
{
    public class HomeViewModel
    {
        private string _TenantName { get; set; }
        public string TenantName
        {
            get
            {
                string returntext = "";
                if (!string.IsNullOrEmpty(_TenantName))
                {
                    returntext = _TenantName;
                }
                return returntext;
            }
            set
            {
                _TenantName = value;
            }

        }

        private string _ClientName = "";
        public string ClientName
        {
            get
            {
                return HttpUtility.HtmlEncode(_ClientName.Replace(" ", "_"));
            }
            set
            {
                _ClientName = value;
            }
        }
        public IList<ModuleData> lactiveModule { get; set; }

        public IList<Module_Data> lactiveModuleAction { get; set; }

    }
}
