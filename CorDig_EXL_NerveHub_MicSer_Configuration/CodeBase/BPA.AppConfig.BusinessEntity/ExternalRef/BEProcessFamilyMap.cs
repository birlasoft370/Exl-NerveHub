using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AppConfig.BusinessEntity.ExternalRef
{
    [Serializable]
    public class BEProcessFamilyMap : ObjectBase
    {

        /// <summary>
        /// Gets or sets Process Family ID.
        /// </summary>
        public int ProcessFamilyId { get; set; }

        /// <summary>
        /// Gets or sets Process ID.
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Gets or sets Campaign ID.
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Gets or sets Process Family Name.
        /// </summary>
        public string ProcessFamilyName { get; set; }

        /// <summary>
        /// Gets or sets Client ID.
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// Gets or sets Process Name.
        /// </summary>
        public string ProcessName { get; set; }
    }

    [Serializable]
    public class BEReportMasterMap : ObjectBase
    {
        /// <summary>
        /// Gets or sets Report ID.
        /// </summary>
        public int iReportId { get; set; }

        /// <summary>
        /// Gets or sets Campaign ID.
        /// </summary>
        public int iCampaignId { get; set; }

        public int iProcessFamilyId { get; set; }

        /// <summary>
        /// Gets or sets Report Name.
        /// </summary>
        public string sReportName { get; set; }

        /// <summary>
        /// Gets or sets Campaign ID.
        /// </summary>
        public int iFieldId { get; set; }

        /// <summary>
        /// Gets or sets Report Name.
        /// </summary>
        public bool bGroupBy { get; set; }

        /// <summary>
        /// Gets or sets Report Name.
        /// </summary>
        public bool bSumOnRowLevel { get; set; }

        /// <summary>
        /// Gets or sets Report Name.
        /// </summary>
        public bool bSumOnColumnLevel { get; set; }
    }

    [Serializable]
    public class BEReportFieldType : ObjectBase
    {
        /// <summary>
        /// Gets or sets Field Type ID.
        /// </summary>
        public int FieldTypeID { get; set; }

        /// <summary>
        /// Gets or sets Field Type Name.
        /// </summary>
        public string FieldTypeName { get; set; }
    }

    [Serializable]
    public class BEReportFieldMaster : ObjectBase
    {
        private DateTime _dtEffStartDate;
        private DateTime _dtEffEndDate;
        /// <summary>
        /// Gets or sets Field ID.
        /// </summary>
        public int iFieldID { get; set; }

        /// <summary>
        /// Gets or sets Campaign ID.
        /// </summary>
        public int iCampaignId { get; set; }

        /// <summary>
        /// Gets or sets Field Type ID.
        /// </summary>
        public int iFieldTypeID { get; set; }

        /// <summary>
        /// Gets or sets Field Name.
        /// </summary>
        public string sFieldName { get; set; }

        /// <summary>
        /// Gets or sets Thread Level.
        /// </summary>
        public bool blIsThreadLevel { get; set; }

        /// <summary>
        /// Gets or sets Thread Id.
        /// </summary>
        public int iThreadId { get; set; }

        /// <summary>
        /// Gets or sets Campaign Level.
        /// </summary>
        public bool blIsCampaignLevel { get; set; }

        /// <summary>
        /// Gets or sets Is Default Value.
        /// </summary>
        public bool blIsDefaultValueField { get; set; }

        /// <summary>
        /// Gets or sets Default Value.
        /// </summary>
        public float flDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets Is Input Parameter.
        /// </summary>
        public bool blIsInputParameter { get; set; }

        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int iId { get; set; }

        /// <summary>
        /// Gets or sets Effective Start Date.
        /// </summary>
        public DateTime dtEffStartDate
        {
            get { return _dtEffStartDate; }
            set { _dtEffStartDate = value; }
        }

        /// <summary>
        /// Gets or sets Effective End Date.
        /// </summary>
        public DateTime dtEffEndDate
        {
            get { return _dtEffEndDate; }
            set { _dtEffEndDate = value; }
        }

        /// <summary>
        /// Gets or sets Formula.
        /// </summary>
        public string sFormula { get; set; }

        /// <summary>
        /// Gets or sets Formula to be displayed.
        /// </summary>
        public string sFormulaDisplay { get; set; }

        /// <summary>
        /// Gets or sets Format String Id.
        /// </summary>
        public int iFormatStringId { get; set; }


        public bool blIsDaily { get; set; }

        public bool blIsWeekly { get; set; }

        public bool blIsMonthly { get; set; }

        public bool blIsTopLevel { get; set; }
    }

    [Serializable]
    public class BEReportFieldCriteria : ObjectBase
    {
        /// <summary>
        /// Gets or sets Criteria ID.
        /// </summary>
        public int iCriteriaId { get; set; }

        /// <summary>
        /// Gets or sets Field ID.
        /// </summary>
        public int iFieldId { get; set; }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        public string sValue { get; set; }
    }

    [Serializable]
    public class BEThreadMaster : ObjectBase
    {
        /// <summary>
        /// Gets or sets Field ID.
        /// </summary>
        public int iThreadID { get; set; }

        /// <summary>
        /// Gets or sets Campaign ID.
        /// </summary>
        public int iCampaignId { get; set; }

        /// <summary>
        /// Gets or sets Thread Name.
        /// </summary>
        public string sThreadName { get; set; }

    }

    [Serializable]
    public class BEReportProcessFamilyFormula : ObjectBase
    {
        private DateTime _dtEffStartDate;
        private DateTime _dtEffEndDate;

        /// <summary>
        /// Gets or sets Field ID.
        /// </summary>
        public int iFamilyFieldID { get; set; }

        /// <summary>
        /// Gets or sets Process Family ID.
        /// </summary>
        public int iProcessFamilyId { get; set; }

        /// <summary>
        /// Gets or sets Field Name.
        /// </summary>
        public string sFieldName { get; set; }

        /// <summary>
        /// Gets or sets Effective Start Date.
        /// </summary>
        public DateTime dtEffStartDate
        {
            get { return _dtEffStartDate; }
            set { _dtEffStartDate = value; }
        }

        /// <summary>
        /// Gets or sets Effective End Date.
        /// </summary>
        public DateTime dtEffEndDate
        {
            get { return _dtEffEndDate; }
            set { _dtEffEndDate = value; }
        }

        /// <summary>
        /// Gets or sets Formula.
        /// </summary>
        public string sFormula { get; set; }

        /// <summary>
        /// Gets or sets Formula to be displayed.
        /// </summary>
        public string sFormulaDisplay { get; set; }

        /// <summary>
        /// Gets or sets Format String Id.
        /// </summary>
        public int iFormatStringId { get; set; }
    }

    [Serializable]
    public class BEReportProcessFormula : ObjectBase
    {
        private DateTime _dtEffStartDate;


        private DateTime _dtEffEndDate;
        /// <summary>
        /// Gets or sets Field ID.
        /// </summary>
        public int iProcessFieldID { get; set; }

        /// <summary>
        /// Gets or sets Process Family ID.
        /// </summary>
        public int iProcessId { get; set; }

        /// <summary>
        /// Gets or sets Field Name.
        /// </summary>
        public string sFieldName { get; set; }

        /// <summary>
        /// Gets or sets Effective Start Date.
        /// </summary>
        public DateTime dtEffStartDate
        {
            get { return _dtEffStartDate; }
            set { _dtEffStartDate = value; }
        }


        /// <summary>
        /// Gets or sets Effective End Date.
        /// </summary>
        public DateTime dtEffEndDate
        {
            get { return _dtEffEndDate; }
            set { _dtEffEndDate = value; }
        }

        /// <summary>
        /// Gets or sets Formula.
        /// </summary>
        public string sFormula { get; set; }

        /// <summary>
        /// Gets or sets Formula to be displayed.
        /// </summary>
        public string sFormulaDisplay { get; set; }

        /// <summary>
        /// Gets or sets Format String Id.
        /// </summary>
        public int iFormatStringId { get; set; }
    }
}
