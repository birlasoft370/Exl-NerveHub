using System.Runtime.Serialization;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    [DataContract]
    public class BEStoreInfo : ObjectBase
    {

        /// <summary>
        /// Gets or sets the i rows.
        /// </summary>
        /// <value>The i rows.</value>
        [DataMember]
        public int iRows { get; set; }

        [DataMember]
        public bool bDistributionBot { get; set; }
        /// <summary>
        /// Gets or sets the i columns.
        /// </summary>
        /// <value>The i columns.</value>
        [DataMember]
        public int iColumns { get; set; }

        /// <summary>
        /// Gets or sets the i store id.
        /// </summary>
        /// <value>The i store id.</value>
        [DataMember]
        public int iStoreId { get; set; }

        /// <summary>
        /// Gets or sets the bIsMail.
        /// </summary>
        /// <value>The bIsMail.</value>
        [DataMember]
        public bool bIsEmail { get; set; }

        /// <summary>
        /// Gets or sets the name of the s store.
        /// </summary>
        /// <value>The name of the s store.</value>
        [DataMember]
        public string sStoreName { get; set; }

        /// <summary>
        /// Gets or sets the s store description.
        /// </summary>
        /// <value>The s store description.</value>
        [DataMember]
        public string sStoreDescription { get; set; }



        /// <summary>
        /// Gets or sets the i campaign id.
        /// </summary>
        /// <value>The i campaign id.</value>
        [DataMember]
        public int iCampId { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [b use client database].
        /// </summary>
        /// <value><c>true</c> if [b use client database]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bUseClientDatabase { get; set; }

        /// <summary>
        /// Gets or sets the name of the s access DB.
        /// </summary>
        /// <value>The name of the s access DB.</value>
        [DataMember]
        public string sAccessDBName { get; set; }

        /// <summary>
        /// Gets or sets the s shared location for back up.
        /// </summary>
        /// <value>The s shared location for back up.</value>
        [DataMember]
        public string sSharedLocationForBackUp { get; set; }

        /// <summary>
        /// Gets or sets the s shared location for file.
        /// </summary>
        /// <value>The s shared location for file.</value>
        [DataMember]
        public string sSharedLocationForFile { get; set; }


        /// <summary>
        /// Gets or sets the lastbackup date.
        /// </summary>
        /// <value>The lastbackup date.</value>
        [DataMember]
        public string dLastBackUPDate { get; set; }


        /// <summary>
        /// Gets or sets the object work object.
        /// </summary>
        /// <value>The object work object.</value>
        [DataMember]
        public List<BEWorkObject> oWorkObject { get; set; }

        [DataMember]
        public bool bGenerateLetter { get; set; }
        [DataMember]
        public string sLetterConfiguration { get; set; }

        [DataMember]
        public bool bErrorSnapshot { get; set; }

        [DataMember]
        public int iErrorDuration { get; set; }

        [DataMember]
        public string sPDSubProcessIDName { get; set; }


        [DataMember]
        public bool bDisableWork { get; set; }

        [DataMember]
        public bool DisableGridObject { get; set; }

        [DataMember]
        public bool bRunTimeUploadRequired { get; set; }
        [DataMember]
        public int iIncreaseSearch { get; set; }

        [DataMember]
        public bool bTABMapping { get; set; }

        [DataMember]
        public bool bGridObject { get; set; }
        [DataMember]
        public List<BEWorkObjectTAB> oWorkObjectTAB { get; set; }
        [DataMember]
        public int iGridObject { get; set; }
        [DataMember]
        public string sGridObjectName { get; set; }

        [DataMember]
        public List<BEWorkObject> oWorkObjectGRD { get; set; }
        [DataMember]
        public List<BEWorkObjectGrid> oWorkObjectGrid { get; set; }
        [DataMember]
        public int iObjectid { get; set; }
        [DataMember]
        public int iObjectTypeid { get; set; }
        [DataMember]
        public int iTAB_ID { get; set; }
        [DataMember]
        public bool bGridIsEditable { get; set; }

        public string sGridObjectMultiName { get; set; }
        public bool bIsForStaging { get; set; }
    }
}
