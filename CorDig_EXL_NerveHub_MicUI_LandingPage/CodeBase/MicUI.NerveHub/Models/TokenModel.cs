using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Web;

namespace MicUI.NerveHub.Models
{
    public class TokenModel
    {
        public TokenModel()
        {
            DetailsList = new List<Details>();
        }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("developerMessage")]
        public string DeveloperMessage { get; set; }

        [JsonProperty("details")]
        public List<Details> DetailsList { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        public class Details
        {
            [JsonProperty("fieldName")]
            public string FieldName { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
        public class AuthenticationResponse
        {
            public string UserName { get; set; }
            public string Language { get; set; }
            public string RoleName { get; set; }
            public int RoleId { get; set; }
            public string JwtToken { get; set; }
            public int ExpiresIn { get; set; }
        }

    }
    public class Module_Data
    {
        public string ModuleName { get; set; }
        public int DisplayOrder { get; set; }

        public string ModuleAction { get; set; }

        public bool Module_Action { get; set; }

        public string ModuleText { get; set; }
    }
    public class ModuleData
    {
        public string ModuleName { get; set; }
        public int DisplayOrder { get; set; }
    }
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
    public class RootObject
    {
        public string success { get; set; }
        public List<BELandingPageMenu> LandingPageMenu { get; set; }
    }
    public class BELandingPageMenu
    {
        public string text { get; set; }
        public string modulename { get; set; }
        public bool readaction { get; set; }




    }
    [Serializable]
    [DataContract]
    public class BEMenuItems : IDisposable
    {

        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        { }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="oMenuItems"/> class.
        /// </summary>
        public BEMenuItems()
        { }

        /// <summary>
        /// Gets or sets the node ID.
        /// </summary>
        /// <value>The node ID.</value>
        [DataMember]
        public int NodeID { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>The name of the node.</value>
        [DataMember]
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        /// <value>The parent ID.</value>
        [DataMember]
        public int ParentID { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [DataMember]
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        [DataMember]
        public string Flag { get; set; }

        /// <summary>
        /// Gets or sets the tool tips.
        /// </summary>
        /// <value>The tool tips.</value>
        //[DataMember]
        //public string ToolTips { get; set; }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
        //[DataMember]
        //public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the menu order.
        /// </summary>
        /// <value>The menu order.</value>
        [DataMember]
        public int MenuOrder { get; set; }

        /// <summary>
        /// Gets or sets the menu status.
        /// </summary>
        /// <value>The menu status.</value>
        //[DataMember]
        //public int MenuStatus { get; set; }

        /// <summary>
        /// Gets or sets the i form ID.
        /// </summary>
        /// <value>The i form ID.</value>
        [DataMember]
        public int iFormID { get; set; }

        /// <summary>
        /// Gets or sets the type of the i form.
        /// </summary>
        /// <value>The type of the i form.</value>
        //[DataMember]
        //public int iFormType { get; set; }

        /// <summary>
        /// Gets or sets the b has permission.
        /// </summary>
        /// <value>The b has permission.</value>
        [DataMember]
        public bool bHasPermission { get; set; }

        /// <summary>
        /// Gets or sets the ModuleName.
        /// </summary>
        [DataMember]
        public string sModuleName { get; set; }

        [DataMember]
        public string sController { get; set; }

        [DataMember]
        public string sAction { get; set; }
        [DataMember]
        public string sIconClass { get; set; }
    }

}


