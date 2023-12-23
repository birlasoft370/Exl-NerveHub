namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class WorkDefinitionModel
    {
        public int StoreId { get; set; }
        public int CampId { get; set; }

        public string WorkDefinitionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool DisableWork { get; set; }
        public bool IsForStaging { get; set; }
        public int NoOfRows { get; set; }
        public int NoOfColumns { get; set; }
        public bool GenerateLetter { get; set; }
        public bool DistributionBot { get; set; }
        public bool IsRunTimeUploadRequired { get; set; }
        public int IncreaseSearch { get; set; }
        public bool ErrorSnapshot { get; set; }
        public int ErrorDuration { get; set; }
        public bool TABMapping { get; set; }
        public bool IsGridConfiguration { get; set; }
        public string? GridObjectMultiName { get; set; }
        public BusinessJustificatonModel BusJustData { get; set; } = new();
        public List<WorkObjectModel> WorkDefinition { get; set; } = new List<WorkObjectModel>();
        public List<TabMasterModel> WorkObjectTABList { get; set; } = new List<TabMasterModel>();
    }
    public class BusinessJustificatonModel
    {
        public bool Purposeswm { get; set; }
        public bool Purposetime { get; set; }
        public bool Purposetrans { get; set; }
        public string? Locations { get; set; }
        public string? Shiftwindows { get; set; }
        public string? BusinessJustifications { get; set; }
        public string? Targetq1 { get; set; }
        public string? Targetq2 { get; set; }
        public string? Targetq3 { get; set; }
        public string? Targety1 { get; set; }
        public string? Targety2 { get; set; }
        public string? Targety3 { get; set; }
        public string? keybenifits { get; set; }
        public int BuisnessID { get; set; }
        public int TechID { get; set; }
        public string? Status { get; set; }
    }
    public class WorkObjectModel
    {
        public string? DataType { get; set; }
        public int Length { get; set; }
        public int ObjectID { get; set; }
        public int StoreID { get; set; }
        public string? ObjectName { get; set; }
        public bool WorkID { get; set; }
        public string ObjectDescription { get; set; } = string.Empty;
        public string? ObjectLabel { get; set; }
        public int ObjectType { get; set; }

        public int ValidationID { get; set; }
        public bool Visible { get; set; }
        public bool Search { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public bool Disabled { get; set; }
        public bool UniqueID { get; set; }
        public bool EmpIdLanId { get; set; }
        public bool TransactionType { get; set; }
        public bool IsUpload { get; set; }
        public bool IsReport { get; set; }
        public bool CustomerIdentifier { get; set; }
        public int ReportOrder { get; set; }
        public int Row_No { get; set; }
        public int Column_No { get; set; }
        public int Column_Span { get; set; }
        public string? TAB_ID { get; set; }
        public string? GridControlID { get; set; }
        public bool SearchableSearch { get; set; }
        public string? ReportsOrderSearch { get; set; }
        public bool IsTranslate { get; set; }
        public string GridObjectMultiName { get; set; } = "";
        public List<WorkObjectChoiceModel> Choice { get; set; } = new List<WorkObjectChoiceModel>();
        public List<WorkObjectTranslateModel> TranslateList { get; set; } = new List<WorkObjectTranslateModel>();
    }
    public class WorkObjectChoiceModel
    {
        public int ObjectChoiceID { get; set; }
        public string? ChoiceValue { get; set; }
        public bool Disabled { get; set; }
        public int Order { get; set; }
        public int GroupID { get; set; }
        public int LanguageId { get; set; }
        public string? Language { get; set; }
        public int ChoiceLanguageID { get; set; }
    }
    public class WorkObjectTranslateModel
    {
        public int SDOBJLanID { get; set; }
        public int LanguageID { get; set; }
        public string ConvertText { get; set; }
    }
    public class TabMasterModel
    {
        public int ObjectChoiceID { get; set; }
        public int ObjID { get; set; }
        public string? ChoiceValue { get; set; }
        public string? TABNameValue { get; set; }
        public bool Disabled { get; set; }
        public int Order { get; set; }
    }
}
