﻿
using MicUI.Configuration.Services.ServiceModel;
using System.Runtime.Serialization;

namespace MicUI.Configuration.Models.MasterValue
{

    [Serializable]
    [DataContract]
    public class BEMasterTable : ObjectBase
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the master id.
        /// </summary>
        /// <value>The master id.</value>
        [DataMember]
        public int iMasterId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the s temp master id.
        /// </summary>
        /// <value>The s temp master id.</value>
        [DataMember]
        public string sTempMasterId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember]
        public string sValue
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the field id.
        /// </summary>
        /// <value>The field id.</value>
        [DataMember]
        public int iFieldId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state of the o row.
        /// </summary>
        /// <value>The state of the o row.</value>
        [DataMember]
        public RowState oRowState
        {
            get;
            set;
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BEMasterTable"/> class.
        /// </summary>

        public BEMasterTable()
        { }


        public BEMasterTable(int iMasterId, string sValue)
        {
            this.iMasterId = iMasterId;
            this.sValue = sValue;
        }

        #endregion

    }
}