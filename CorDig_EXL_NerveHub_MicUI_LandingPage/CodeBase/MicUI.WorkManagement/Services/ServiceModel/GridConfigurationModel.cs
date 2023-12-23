namespace MicUI.WorkManagement.Services.ServiceModel
{
    public class GridConfigurationModel
    {
        public int StoreID { get; set; }
        public int CampaignID { get; set; }
        public int ProcessID { get; set; }
        public GridWorkObjectModel GridObject { get; set; } = new();
    }
    public class GridWorkObjectModel
    {
        public int GridObjectID { get; set; }
        public int StoreID { get; set; }
        public string? GridObjectName { get; set; }
        public bool GridEditable { get; set; }
        public bool Disabled { get; set; }
        public List<GridWorkObjectControlModel> Grid_LstValues { get; set; } = new List<GridWorkObjectControlModel>();
    }
    public class GridWorkObjectControlModel
    {
        public string? DataType { get; set; }
        public int Length { get; set; }
        public int ObjectID { get; set; }
        public int StoreID { get; set; }
        public string? ObjectName { get; set; }
        public string ObjectDescription { get; set; } = string.Empty;
        public string? ObjectLabel { get; set; }
        public int ObjectType { get; set; }
        public List<WorkObjectChoiceModel> Choice { get; set; } = new List<WorkObjectChoiceModel>();
        public int ValidationID { get; set; }
        public bool Visible { get; set; }
        public bool Search { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public bool Disabled { get; set; }
        public int Row_No { get; set; }
        public int Column_No { get; set; }
        public int Column_Span { get; set; }
        public string? ReportsOrderSearch { get; set; }
    }
}
