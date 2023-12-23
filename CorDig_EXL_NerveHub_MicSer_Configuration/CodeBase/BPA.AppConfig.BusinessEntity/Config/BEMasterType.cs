using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.Config
{
    [Serializable]
    [DataContract]
    public class BEMasterType : ObjectBase
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the FieldId.
        /// </summary>
        /// <value>The FieldId.</value>
        [DataMember]
        public int iFieldId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the FieldDescription.
        /// </summary>
        /// <value>The FieldDescription.</value>
        [DataMember]
        public string sFieldDescription
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the l master table.
        /// </summary>
        /// <value>The l master table.</value>
        [DataMember]
        public IList<BEMasterTable> lMasterTable
        {
            get; set;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BEMasterType"/> class.
        /// </summary>

        public BEMasterType()
        { lMasterTable = new List<BEMasterTable>(); }
        #endregion

    }
}
