namespace MicUI.WorkManagement.Services.ServiceModel
{
    [Serializable]
    public class BEProcessGroup : ObjectBase
    {
        public BEProcessGroup()
        {
            oERPProcess = new BEERPProcess();
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets the process group ID.
        /// </summary>
        /// <value>The process group ID.</value>
        public int iProcessGroupID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ERP process object.
        /// </summary>
        /// <value>The ERP process.</value>
        public BEERPProcess oERPProcess { get; set; }

        /// <summary>
        /// Gets or sets the state of the row.
        /// </summary>
        /// <value>The state of the row.</value>
        public RowState oRowState { get; set; }

        #endregion
    }
}
