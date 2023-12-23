using System.Web;

namespace MicUI.Configuration.Models
{
    public class NavMenuViewModel
    {
        public NavMenuViewModel()
        {
            MenuViewModel = new List<MenuViewModel>();
        }
        private string _ClientName = "";
        private string _flag = "";
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

        public string flag
        {
            get
            {
                return _flag;
            }
            set
            {
                _flag = value;
            }

        }
        public string Area { get; set; }


        public IList<ModuleData> lactiveModule { get; set; }

        public string CurrentTimeZone { get; set; }

        public string Language { get; set; }

        public string TimeZone { get; set; }
        public string UserName { get; set; }

        public List<MenuViewModel> MenuViewModel { get; set; }

    }
}