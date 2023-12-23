using System.Data;
using System.Runtime.Serialization;

namespace BPA.AppConfig.BusinessEntity.ExternalRef.WorkAllocation
{
    [Serializable]
    [DataContract]
    public class BEWorkObject : ObjectBase
    {
        #region Fields

        /// <summary>
        /// Gets or sets a value indicating whether [b visible].
        /// </summary>
        /// <value><c>true</c> if [b visible]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bVisible
        {
            get;
            set;
        }

        [DataMember]
        public List<BEWorkObjectTranslateList> oTranslateLanguage
        {
            get;
            set;
        }
        [DataMember]
        public bool bIsTranslate { get; set; }

        /// <summary>
        /// Gets or sets the i row number.
        /// </summary>
        /// <value>The i row number.</value>
        [DataMember]
        public int iRowNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the i column number.
        /// </summary>
        /// <value>The i column number.</value>
        [DataMember]
        public int iColumnNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the i col span.
        /// </summary>
        /// <value>The i col span.</value>
        [DataMember]
        public int iColSpan
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the i validation ID.
        /// </summary>
        /// <value>The i validation ID.</value>
        [DataMember]
        public int iValidationID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the i object ID.
        /// </summary>
        /// <value>The i object ID.</value>
        [DataMember]
        public int iObjectID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the i store ID.
        /// </summary>
        /// <value>The i store ID.</value>
        [DataMember]
        public int iStoreID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the s object.
        /// </summary>
        /// <value>The name of the s object.</value>
        [DataMember]
        public string sObjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the s object decription.
        /// </summary>
        /// <value>The s object decription.</value>
        [DataMember]
        public string sObjectDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the s object label.
        /// </summary>
        /// <value>The s object label.</value>
        [DataMember]
        public string sObjectLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the i object.
        /// </summary>
        /// <value>The type of the i object.</value>
        [DataMember]
        public int iObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b editable].
        /// </summary>
        /// <value><c>true</c> if [b editable]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bEditable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b required].
        /// </summary>
        /// <value><c>true</c> if [b required]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b read only].
        /// </summary>
        /// <value><c>true</c> if [b read only]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b work ID].
        /// </summary>
        /// <value><c>true</c> if [b work ID]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bWorkID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the s data.
        /// </summary>
        /// <value>The type of the s data.</value>
        [DataMember]
        public string sDataType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the s data type validation.
        /// </summary>
        /// <value>The s data type validation.</value>
        [DataMember]
        public string sDataTypeValidation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the o choice set.
        /// </summary>
        /// <value>The o choice set.</value>
        [DataMember]
        public string oChoiceSet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the i.
        /// </summary>
        /// <value>The length of the i.</value>
        [DataMember]
        public int iLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dt choice table.
        /// </summary>
        /// <value>The dt choice table.</value>
        [DataMember]
        public DataTable dtChoiceTable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the o choice.
        /// </summary>
        /// <value>The o choice.</value>
        [DataMember]
        public List<BEWorkObjectChoice> oChoice
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the o choice.
        /// </summary>
        /// <value>The o choice.</value>
        //[DataMember]
        //public List<BEWorkObjectTranslateList> oTranslateLanguage
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// Gets or sets a value indicating whether [b search].
        /// </summary>
        /// <value><c>true</c> if [b search]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bSearch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b change version].
        /// </summary>
        /// <value><c>true</c> if [b change version]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bChangeVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(EmitDefaultValue = true)]
        public string ObjectType_IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object data type_ is enabled.
        /// </summary>
        /// <value>The object data type_ is enabled.</value>
        [DataMember(EmitDefaultValue = true)]
        public string ObjectDataType_IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lenght_ is enabled.
        /// </summary>
        /// <value>The lenght_ is enabled.</value>
        [DataMember(EmitDefaultValue = true)]
        public string Lenght_IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the validation_ is enabled.
        /// </summary>
        /// <value>The validation_ is enabled.</value>
        [DataMember(EmitDefaultValue = true)]
        public string Validation_IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b TAT].
        /// </summary>
        /// <value><c>true</c> if [b TAT]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bTATComparison
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the s TAT.
        /// </summary>
        /// <value>The type of the s TAT.</value>
        [DataMember]
        public string sTATType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b TAT comparison].
        /// </summary>
        /// <value><c>true</c> if [b TAT comparison]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bTargetTAT { get; set; }

        /// <summary>
        /// Gets or sets the TAT type_ is enabled.
        /// </summary>
        /// <value>The TAT type_ is enabled.</value>
        [DataMember(EmitDefaultValue = true)]
        public string TATType_IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the TAT comparison_ is enabled.
        /// </summary>
        /// <value>The TAT comparison_ is enabled.</value>
        [DataMember(EmitDefaultValue = true)]
        public string TATComparison_IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [b unique].
        /// </summary>
        /// <value><c>true</c> if [b unique]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool bUniqueID
        {
            get;
            set;
        }

        [DataMember]
        public bool bTransactionType { get; set; }

        [DataMember]
        public bool bIsUpload { get; set; }

        [DataMember]
        public bool bIsReport { get; set; }
        [DataMember]
        public Int32 iIsReportOrder { get; set; }

        //  [DataMember]
        // public bool bIsTranslate { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <value><c>true</c> set row  <c>false</c>.</value>
        [DataMember]
        public int irow_No
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <value><c>true</c> set Column  <c>false</c>.</value>
        [DataMember]
        public int icolumn_No
        {
            get;
            set;
        }
        /// <summary>
        ///
        /// </summary>
        /// <value><c>true</c> set Column Span  <c>false</c>.</value>
        [DataMember]
        public int icolumn_Span
        {
            get;
            set;
        }
        [DataMember]
        public string iTAB_ID
        {
            get;
            set;
        }
        [DataMember]
        public string iGridObjectMappID
        {
            get;
            set;
        }
        [DataMember]
        public string sGridControlID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <value><c>true</c> set Column Span  <c>false</c>.</value>
        [DataMember]
        public bool isSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the i Process Step ID.
        /// </summary>
        /// <value>The i Process Step ID.</value>
        [DataMember]
        public int iProcessStepId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the s Process Step Name.
        /// </summary>
        /// <value>The s Process Step Name.</value>
        [DataMember]
        public string sProcessStepName
        {
            get;
            set;
        }

        [DataMember]
        public int irowNo
        {
            get;
            set;
        }
        [DataMember]
        public int icolumnNo
        {
            get;
            set;
        }
        [DataMember]
        public int icolumnSpan
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool bCustomerIdentifier
        {
            get;
            set;
        }
        [DataMember]
        public Int32 RowID
        {
            get;
            set;
        }

        BEControlTypeInfo _selectControlType = new BEControlTypeInfo() { sControlType = "--Select--", iControlTypeID = 0 };
        public BEControlTypeInfo selectControlType
        {
            get { return _selectControlType; }
            set
            {
                _selectControlType = value;

            }
        }

        [DataMember]
        public bool bEmailCampaign
        {
            get;
            set;
        }

        [DataMember]
        public bool bEmpIdLanId
        {
            get;
            set;
        }

        [DataMember]
        public bool bSystemField
        {
            get;
            set;
        }

        [DataMember]
        public bool bIsRunTimeUpload { get; set; }


        [DataMember]
        public int iGridID { get; set; }
        [DataMember]
        public int sGridName { get; set; }
        [DataMember]
        public IList<BEStoreInfo> lstGRDWorkObject { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string WorkTransID { get; set; }
        [DataMember]
        public bool bSearchableSearch
        {
            get;
            set;
        }
        [DataMember]
        public string iReportsOrderSearch { get; set; }
        #endregion Fields
    }
}
