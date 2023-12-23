using BPA.EmailManagement.BusinessEntity.ExternalRef.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.EmailManagement.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    [DataContract]
    public class BEWorkObjectList : ObjectBase, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BEWorkObjectList"/> class.
        /// </summary>
        public BEWorkObjectList()
        {

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        [DataMember]
        public bool IsPGTCampaign { get; set; }

        [DataMember]
        public bool IsMailCampaign { get; set; }

        [DataMember]
        public bool IsDistributionBot { get; set; }
        /// <summary>
        /// Gets or sets the LST work object choice.
        /// </summary>
        /// <value>The LST work object choice.</value>
        [DataMember]
        public IList<BEWorkObjectChoice> lstWorkObjectChoice { get; set; }

        /// <summary>
        /// Gets or sets the LST rule.
        /// </summary>
        /// <value>The LST rule.</value>
        [DataMember]
        public IList<BEWorkRule> lstRule { get; set; }

        /// <summary>
        /// Gets or sets the table name and version.
        /// </summary>
        /// <value>The table name and version.</value>
        [DataMember]
        public string TableNameAndVersion { get; set; }

        /// <summary>
        /// Gets or sets the LST work object.
        /// </summary>
        /// <value>The LST work object.</value>
        [DataMember]
        public IList<BEWorkObject> lstWorkObject { get; set; }

        /// <summary>
        /// Gets or sets the LST Search work object.
        /// </summary>
        /// <value>The LST search work object.</value>
        [DataMember]
        public IList<BEWorkObject> lstSearchWorkObject { get; set; }

        /// <summary>
        /// Gets or sets the LST Termination code.
        /// </summary>
        /// <value>The LST termination code.</value>
        [DataMember]
        public IList<BETerminationCodeInfo> lstTerminationCode { get; set; }

        /// <summary>
        /// Gets or sets the LST break code.
        /// </summary>
        /// <value>The LST break code.</value>
        [DataMember]
        public IList<BEBreakInfo> lstBreakCode { set; get; }

        /// <summary>
        /// Gets or sets the LST delay code.
        /// </summary>
        /// <value>The LST delay code.</value>
        [DataMember]
        public IList<BEMasterTable> lstDelayCode { set; get; }

        // Gets or sets the Control formula.
        /// </summary>
        /// <value>The Formula.</value>
        [DataMember]
        public DataTable WorkObjectFormulaEvent { set; get; }
        [DataMember]
        public List<BEWorkObjectTAB> lstTABMaster { set; get; }

        [DataMember]
        public string sCulture { get; set; }

        [DataMember]
        public Boolean bIsTabConfiguration { get; set; }

        [DataMember]
        public int iIncreaseSearch { get; set; }
        [DataMember]
        public bool bIsGridConfiguration { get; set; }

        [DataMember]
        public IList<BEStoreInfo> lstGRDWorkObject { get; set; }
        [DataMember]
        public bool bIsLinkCampaign { get; set; }

        [DataMember]
        public List<BEGridConfiguration> lstLinkCampaignData { get; set; }

        [DataMember]
        public bool IsCheckWorkCode { get; set; }
    }
}
