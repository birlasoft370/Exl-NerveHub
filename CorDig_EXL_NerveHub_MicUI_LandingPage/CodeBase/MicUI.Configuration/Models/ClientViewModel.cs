using BPA.GlobalResources.UI.AppConfiguration;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicUI.Configuration.Helper.CustomValidationAttributes;
using MicUI.Configuration.Services.ServiceModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MicUI.Configuration.Models
{
    [Serializable]
    public class ClientViewModel : ViewModelValidationHelper
    {
        public ClientViewModel()
        {
            ClientList = new List<BEClientInfo>();
            BESBUInfoList = new List<BESBUInfo>();
            ERPClientList = new List<ERPClientList>();
            SBUList = new List<Models.SBUList>();
        }

        public int iClientID
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }

        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ClientInfo), ErrorMessageResourceName = "required_Client")]
        [Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_Client_Name")]

        public string ClientName
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceType = typeof(BPA.GlobalResources.UI.AppConfiguration.Resources_ClientInfo), ErrorMessageResourceName = "required_Vertical")]
        [Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_Vertical")]
        public int VerticalID
        {
            get;
            set;
        }


        [Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_Description")]
        public string Description
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_EXL_Specific_Client")]
        public bool EXLSpecificClient
        {
            get;
            set;
        }
        [Display(ResourceType = typeof(Resources_ClientInfo), Name = "display_Disable")]
        public bool Disabled
        {
            get;
            set;
        }

        public int Flag { get; set; }
        public DataTable listClient { get; set; }
        public DataTable ShowlistClient { get; set; }



        public string SEARCHNAME
        {
            get;
            set;
        }

        public DataTable Mydata
        {
            get;
            set;
        }
        public List<BEClientInfo> ClientList { get; set; }
        public List<BESBUInfo> BESBUInfoList { get; set; }
        public IEnumerable<SelectListItem> listVertical { get; set; }
        public List<ERPClientList> ERPClientList { get; set; }
        public List<SBUList> SBUList { get; set; }
    }
    [Serializable]
    public class ERPClientList : IDisposable
    {
        public int SELECTED { get; set; }
        public string ERPCLIENTID { get; set; }
        public string ERPCLIENTNAME { get; set; }
        public int ISMAPPED { get; set; }

        public void Dispose()
        {

        }
    }

    [Serializable]
    public class ERPClient : IDisposable
    {
        public int SELECTED { get; set; }
        public int ERPCLIENTID { get; set; }
        public string ERPCLIENTNAME { get; set; }
        public int ISMAPPED { get; set; }

        public void Dispose()
        {

        }
    }
    [Serializable]
    public class SBUList : IDisposable
    {
        public void Dispose()
        {

        }
        public int SELECTED { get; set; }
        public int SBUID { get; set; }
        public string SBUNAME { get; set; }
        public int DISABLED { get; set; }
    }

}
