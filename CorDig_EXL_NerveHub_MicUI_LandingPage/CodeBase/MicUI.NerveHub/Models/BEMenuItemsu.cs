using System.Runtime.Serialization;

namespace MicUI.NerveHub.Models
{
    /// <summary>
    /// Main Menu Items Business Entity
    /// </summary>

    public class BEMenuItemsu : IDisposable
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
        public BEMenuItemsu()
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
        [DataMember]
        public string ToolTips { get; set; }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The role ID.</value>
        [DataMember]
        public int RoleID { get; set; }

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
        [DataMember]
        public int MenuStatus { get; set; }

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
        [DataMember]
        public int iFormType { get; set; }

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